﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Roulette.Context;
using Roulette.Models;

namespace Roulette.Controllers
{
    public class DepositController : Controller
    {
        private static string PricesURL = "https://api.csgofast.com/price/all";
        private readonly AppDbContext AppDbContext;
        private readonly HomeModel HomeModel;
        public static Dictionary<string, float> PricesDictionary = new Dictionary<string, float>();

        public DepositController(AppDbContext context, HomeModel homeModel)
        {
            AppDbContext = context;
            HomeModel = homeModel;
        }

        public async Task<IActionResult> Index()
        {
            var steamID = User.Claims.Where(c => c.Type == "steamID").Select(c => c.Value).SingleOrDefault();
            var user =
                await AppDbContext.SteamUsers.FirstOrDefaultAsync(x => x.SteamID == steamID);
            HomeModel.User = user;
            return View(HomeModel);
        }

        public static async Task GetPriceList()
        {
            HttpClient cli = new HttpClient();
            var data = await cli.GetStringAsync(PricesURL).ConfigureAwait(false);
            PricesDictionary = JsonConvert.DeserializeObject<Dictionary<string, float>>(data);
        }


        public class Item
        {
            public ulong id { get; set; }
            public ulong classid { get; set; }
            public ItemDescription Description { get; set; }
        }

        public class ItemDescription
        {
            public string name { get; set; }
            public string type { get; set; }
            public bool tradable { get; set; }
            public bool marketable { get; set; }
            public string iconURL { get; set; }
            public string exterior { get; set; }

            public dynamic metadata { get; set; }
        }


        private static List<Item> ProceedInventory(string data)
        {
            dynamic invResponse = JsonConvert.DeserializeObject(data);
            if (invResponse.success == false)
            {
                return null;
            }

            List<Item> items = new List<Item>();
            Dictionary<ulong, ItemDescription> descriptions = new Dictionary<ulong, ItemDescription>();
            foreach (var item in invResponse.rgInventory)
            {
                foreach (var description in invResponse.rgDescriptions)
                {
                    foreach (var classid_instanceid in description)
                    {
                        var marketname = (string) classid_instanceid.market_name;
                        descriptions.TryAdd((ulong) classid_instanceid.classid, new ItemDescription
                        {
                            name = classid_instanceid.name,
                            type = classid_instanceid.type,
                            marketable = (bool) classid_instanceid.marketable,
                            tradable = (bool) classid_instanceid.marketable,
                            metadata = classid_instanceid.descriptions        ,
                            iconURL = classid_instanceid.icon_url ,
                            exterior = marketname.Contains('(') ? marketname.Split('(')[1].Replace(")","") : string.Empty
                        });
                        break;
                    }
                }

                foreach (var itemId in item)
                {
                    items.Add(new Item
                        {id = itemId.id, classid = itemId.classid, Description = descriptions[(ulong) itemId.classid]});
                }
            }

            return items;
        }

        public static async Task<List<Item>> LoadInventory(string SteamID)
        {
            if (!Directory.Exists("cache"))
            {
                Directory.CreateDirectory("cache");
            }

            if (string.IsNullOrEmpty(SteamID)) return null;

            string path = Path.Combine("cache", SteamID);

            string data;
            if ((DateTime.UtcNow - System.IO.File.GetLastWriteTimeUtc(path)).Minutes > 15 ||
                !System.IO.File.Exists(path))
            {
                data = await new HttpClient()
                    .GetStringAsync("http://steamcommunity.com/profiles/" + SteamID +
                                    "/inventory/json/730/2?trading=1").ConfigureAwait(false);
                await System.IO.File.WriteAllTextAsync(path, data);
            }
            else
            {
                data = await System.IO.File.ReadAllTextAsync(path);
            }

            return ProceedInventory(data);
        }
    }
}