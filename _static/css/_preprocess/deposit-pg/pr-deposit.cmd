SET base_dir=D:\Kabanina\7\SDT\Roulette

cd %base_dir%

SET proc=%base_dir%\_static\preprocess.py
SET prefix=_static\css\deposit-pg

SET src=%prefix%\deposit.css
SET targ=%prefix%\deposit.out.css
SET var=%prefix%\var.txt

python %proc% -s %src% -t %targ% -v %var%