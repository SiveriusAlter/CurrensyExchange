Запросы создания таблиц:
create table currencies (id serial primary key, code varchar(400), fullname varchar(400), sign varchar(400));

create table exchangerates (id serial primary key, 
basecurrencyid int references currencies(id), 
targetcurrencyid int references currencies(id), 
rate real);

Запросы добавления тестовых танных:
Insert into currencies as c (code, fullname, sign) 
values ('USD', 'US Dollar', '$'),
('RUB', 'Russian Ruble', '₽');
insert into exchangerates as e (basecurrencyid, targetcurrencyid, rate) 
values ('1', '2', 0.012), 
('2', '1', 82.25);

Добавление уникальных индексов:
create unique index ix_code on currencies (code);
create unique index ix_currencyids on exchangerates (basecurrencyid, targetcurrencyid);