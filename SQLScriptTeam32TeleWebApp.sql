create database Milestone2DB_24

use  Milestone2DB_24

go
CREATE PROCEDURE [createAllTables]

AS

create table customer_profile(
nationalID int primary key,
first_name varchar(50) not null, 
last_name varchar(50) not null,
email varchar(50),
address varchar(70) not null, 
date_of_birth date
)


create table customer_account(
mobileNo char(11) primary key,
pass varchar(50),
balance decimal(10,1),
account_type varchar(50) check(account_type = 'postpaid' or account_type = 'prepaid' or account_type = 'pay-as-you-go' ),
start_date date not null,
status varchar(50) check(status = 'active' or status = 'onhold'),
points int default 0,
nationalID int,
FOREIGN KEY (nationalID) REFERENCES customer_profile (nationalID)
)

create table Service_plan(
planID int identity primary key,
name varchar(50) not null,
price int not null,
SMS_offered int not null,
minutes_offered int not null,
data_offered int not null,
description varchar(50)
)

create table Subscription(
mobileNo Char(11),
planID int,
subscription_date date,
status varchar(50) check(status='active' or status='onhold'),
constraint pk_subscription primary key (mobileNo,planID),
FOREIGN KEY (mobileNo) REFERENCES customer_account (mobileNo),
FOREIGN KEY (planID) REFERENCES Service_plan (planID)
)

create table Plan_Usage(
usageID int identity primary key,
start_date date not null,
end_date date not null,
data_consumption int,
minutes_used int ,
SMS_sent int  , 
mobileNo Char(11) , 
planID int,
FOREIGN KEY (mobileNo) REFERENCES customer_account (mobileNo),
FOREIGN KEY (planID) REFERENCES Service_plan (planID)
)


create table Payment(
paymentID int identity primary key,
amount decimal(10,1) not null,
date_of_payment date not null,
payment_method varchar(50) check(payment_method ='cash' or payment_method ='credit'),
status varchar(50) check(status ='successful' or status='rejected' or status='pending'),
mobileNo Char(11),
FOREIGN KEY (mobileNo) REFERENCES customer_account (mobileNo)
)




create table process_payment(
paymentID int,
planID int,
FOREIGN KEY (paymentID) REFERENCES Payment (paymentID),
FOREIGN KEY (planID) REFERENCES Service_plan (planID),

remaining_amount as(dbo.function_remaining_amount(paymentID, planID)),
extra_amount as (dbo.function_extra_amount(paymentID, planID)),

constraint pk_process_payment primary key (paymentID) 
)

create table Wallet
(
walletID int identity primary key,
current_balance decimal(10,2) default 0,
currency varchar(50) default 'egp',
last_modified_date date ,
nationalID int,
mobileNo char(11),
FOREIGN KEY (nationalID) REFERENCES customer_profile (nationalID)
)

create table transfer_money(
walletID1 int, 
walletID2 int, 
transfer_id int identity,
amount decimal (10,2),
transfer_date date, 
constraint pk_transfer_money primary key (walletID1,walletID2,transfer_id),
FOREIGN KEY (walletID1) REFERENCES Wallet(walletID),
FOREIGN KEY (walletID2) REFERENCES Wallet (walletID)
)

create table Benefits (
benefitID int primary key identity, 
description varchar(50),
validity_date date, 
status varchar (50) check(status='active' or status ='expired'),
mobileNo char(11), 
FOREIGN KEY (mobileNo) REFERENCES customer_account(mobileNo)
)

create table Points_group(
pointId int identity,
benefitID int, 
pointsAmount int,
paymentId int,
FOREIGN KEY (paymentId) REFERENCES Payment(paymentID),
FOREIGN KEY (benefitID) REFERENCES Benefits(benefitID),
constraint pk_Points_group primary key (pointId,benefitId)
)

create table Exclusive_offer (
offerID int identity, 
benefitID int, 
internet_offered int ,
SMS_offered int,
minutes_offered int,
FOREIGN KEY (benefitID) REFERENCES Benefits(benefitID),
constraint pk_Exclusive_offer primary key (offerID,benefitId)

)

create table Cashback(
cashbackID int identity, 
benefitID int, 
walletID int, 
amount int,
credit_date date,FOREIGN KEY (benefitID) REFERENCES Benefits(benefitID),
FOREIGN KEY (walletid) REFERENCES Wallet(walletid),
constraint pk_Cashback primary key (cashbackID,benefitId)
)

create table plan_provides_benefits(
benefitid int, 
planID int, 
FOREIGN KEY (benefitID) REFERENCES Benefits(benefitID),
FOREIGN KEY (planID) REFERENCES service_plan(planID),
constraint pk_plan_provides_benefits primary key (planID,benefitId)
)

create table shop (
shopID int identity primary key,
name varchar(50),
Category varchar(50)
)
create table Physical_shop (
shopID int primary key, 
address varchar(50),
working_hours varchar(50),
FOREIGN KEY (shopID) REFERENCES shop(shopID),
)

create table E_shop (
shopID int primary key , 
URL varchar(50) not null,
rating int,
FOREIGN KEY (shopID) REFERENCES shop(shopID),
)

create table Voucher (
voucherID int identity primary key,
value int,
expiry_date date,
points int, 
mobileNo char(11),
redeem_date date,
shopid int, 
FOREIGN KEY (shopID) REFERENCES shop(shopID),
FOREIGN KEY (mobileNo) REFERENCES customer_account(mobileNo)
)



create table Technical_support_ticket (
ticketID int identity,
mobileNo char(11), 
issue_description varchar(50),
priority_level int,
status varchar(50) check (status in ('Open','In progress','Resolved'))
FOREIGN KEY (mobileNo) REFERENCES customer_account(mobileNo),
constraint pk_Technical_support_ticket primary key (ticketID,mobileNo)
)

GO

-------------------------------------------------------------------------------------
------------------------remaining function--------------------------------------------
go
CREATE FUNCTION [function_remaining_amount]
(@paymentId int, @planId int) --Define Function Inputs
RETURNS int -- Define Function Output
AS

Begin

declare @amount int

If (SELECT payment.amount FROM payment WHERE payment.paymentid=@paymentId)  < (SELECT Service_plan.price FROM Service_plan
WHERE Service_plan.planid=@planid)

Set @amount =  (SELECT Service_plan.price FROM Service_plan WHERE Service_plan.planid=@planid) - (SELECT payment.amount FROM payment
WHERE payment.paymentid=@paymentId)

Else
Set @amount = 0

Return @amount

END
go

-------------------------------------------------------------------------------------------
---------------------------------extra function--------------------------------------------
go
CREATE FUNCTION [function_extra_amount]
(@paymentId int, @planId int) --Define Function Inputs
RETURNS int -- Define Function Output
AS

Begin

declare @amount int

If (SELECT payment.amount FROM payment WHERE payment.paymentid=@paymentId)  > (SELECT Service_plan.price FROM Service_plan
WHERE Service_plan.planid=@planid)

Set @amount = (SELECT payment.amount FROM payment WHERE payment.paymentid=@paymentId) - (SELECT Service_plan.price FROM Service_plan WHERE Service_plan.planid=@planid)

Else
Set @amount = 0

Return @amount

END

go

Exec createAllTables

GO
CREATE VIEW [allCustomerAccounts] AS
SELECT p.*,a.mobileNo,a.account_type,a.status,a.start_date,a.balance,a.points from customer_profile p
inner join customer_account a 
on p.nationalID = a.nationalID
where a.status = 'active'

GO

GO
CREATE VIEW [allServicePlans] AS
select * from Service_plan
GO

GO
CREATE VIEW [allBenefits] AS
select * from Benefits
where status = 'active'
GO



GO
CREATE VIEW [AccountPayments] AS
select * from Payment p

GO

GO
CREATE VIEW [allShops] AS
select * from shop 
GO

GO
CREATE VIEW [allResolvedTickets] AS
select * from Technical_support_ticket
where status = 'Resolved'
GO

GO
CREATE VIEW [CustomerWallet] AS
select w.*,p.first_name,p.last_name from Wallet w
inner join customer_profile p
on w.nationalID = p.nationalID

GO

GO
CREATE VIEW [E_shopVouchers] AS
select e.*, v.voucherID,v.value from E_shop e
inner join Voucher v
on e.shopID = v.shopid

GO
GO
CREATE VIEW [PhysicalStoreVouchers] AS
select p.*, v.voucherID,v.value from Physical_shop p
inner join Voucher v
on p.shopID = v.shopid

GO
GO
CREATE VIEW [Num_of_cashback] AS
select walletID,count(*)as 'cashbackCount' from Cashback
group by walletID

GO


go
create Procedure [Account_Plan]
AS
Select customer_account.*, Service_plan.* from customer_account inner join Subscription
on customer_account.mobileNo = Subscription.mobileNo inner join Service_plan on Subscription.planID = Service_plan.planID
GO

go
CREATE FUNCTION [Account_Plan_date]
(@sub_date date, @plan_id int) --Define Function Inputs
RETURNS Table -- Define Function Output
AS
Return (Select customer_account.mobileNo, Service_plan.planID, Service_plan.name from customer_account inner join Subscription
on customer_account.mobileNo = Subscription.mobileNo inner join Service_plan on Subscription.planID = Service_plan.planID
WHERE Subscription.subscription_date = @sub_date AND Service_plan.planID = @plan_id)
go
go
create FUNCTION [Account_Usage_Plan]
(@mobile_num char(11), @start_date date) --Define Function Inputs
RETURNS Table -- Define Function Output
AS
Return (Select Plan_Usage.planID, sum(Plan_Usage.data_consumption) as 'total_data' ,sum(Plan_Usage.minutes_used) as 'total_mins',sum(Plan_Usage.SMS_sent) as 'total_SMS'
from Plan_Usage
where  Plan_Usage.mobileNo = @mobile_num and Plan_Usage.start_date >= @start_date
group by Plan_Usage.planID)
go
go
CREATE PROCEDURE [Benefits_Account]
@mobile_num char(11), @plan_id int

AS
update Benefits
set mobileNo = null
from  Benefits B inner join plan_provides_benefits pb
on B.benefitID = pb.benefitid 
where B.mobileNo = @mobile_num and pb.planID = @plan_id
go
go
CREATE FUNCTION [Account_SMS_Offers]
(@mobile_num char(11)) --Define Function Inputs
RETURNS Table -- Define Function Output
AS
Return(Select o.* from Exclusive_offer o inner join Benefits b 
on o.benefitID = b.benefitID 
where b.mobileNo = @mobile_num and o.SMS_offered >0 )
go 

go
CREATE PROCEDURE [Account_Payment_Points]
@mobile_num char(11)

AS
select count(p.paymentID) as 'transactions', isnull(sum(pb.pointsAmount),0) as 'points' from Payment P
inner join Points_group pb 
on p.paymentID = pb.paymentId
where P.mobileNo = @mobile_num and (year(current_timestamp) - year(p.date_of_payment)=1 ) 
and P.status = 'successful'
go
go
CREATE FUNCTION [Wallet_Cashback_Amount]
(@walletID int, @planID int) --Define Function Inputs
RETURNS int -- Define Function Output
AS
Begin
declare @amount int

set @amount = (Select sum(c.amount) from Cashback c 
inner join plan_provides_benefits pb 
on c.benefitID = pb.benefitid
where c.walletID = @walletID and pb.planID = @planID)

return @amount
END
go


go
CREATE FUNCTION [Wallet_Transfer_Amount]
(@walletID int, @start_date date, @end_date date) --Define Function Inputs
RETURNS int -- Define Function Output
AS
Begin
declare @avg int

set @avg = (Select avg(t.amount) from transfer_money t
where t.walletID1 = @walletID and t.transfer_date between @start_date and @end_date)

return @avg
END
go

go
CREATE FUNCTION [Wallet_MobileNo]
(@mobile_num char(11)) --Define Function Inputs
RETURNS bit -- Define Function Output
AS
Begin
declare @output bit
IF exists((Select w.walletID from Wallet w
where w.mobileNo = @mobile_num ))
set @output = 1
else 
set @output = 0

return @output
END
go

go
CREATE PROCEDURE [Total_Points_Account]
@mobile_num char(11)  

AS
declare @sum int
select @sum =  sum(pg.pointsAmount) from Points_group pg
inner join Payment p
on pg.paymentId = p.paymentID
where p.mobileNo = @mobile_num

update customer_account  
set points = @sum
where mobileNo = @mobile_num

delete from Points_group
where pointId in  (select pg.pointId from Points_group pg
inner join Payment p on pg.paymentId = p.paymentID
where p.mobileNo = @mobile_num)
go

go
CREATE FUNCTION [AccountLoginValidation]
(@mobile_num char(11), @pass varchar(50)) --Define Function Inputs
RETURNS bit -- Define Function Output
AS
Begin
declare @output bit
IF exists((Select a.mobileNo from customer_account a
where a.mobileNo = @mobile_num and a.pass = @pass ))
set @output = 1
else 
set @output = 0

return @output
END
go

go
CREATE FUNCTION [Consumption]
(@Plan_name varchar(50), @start_date date, @end_date date) --Define Function Inputs
RETURNS Table -- Define Function Output
AS
Return(Select p.data_consumption,p.minutes_used,p.SMS_sent from Plan_Usage p 
inner join Service_plan s on s.planID = p.planID
where s.name = @Plan_name and p.start_date >= @start_date and p.end_date <= @end_date)
go 
go
CREATE PROCEDURE [Unsubscribed_Plans]
@mobile_num char(11)

AS
select  sp.* from  Service_plan sp 
where sp.planID not in (
select s.planID  from Subscription s
where s.mobileNo = @mobile_num)
go


go
CREATE FUNCTION [Usage_Plan_CurrentMonth]
(@mobile_num char(11)) --Define Function Inputs
RETURNS Table -- Define Function Output
AS
Return(select p.data_consumption, p.minutes_used, p.SMS_sent from Plan_Usage p
inner join Subscription s 
on p.planID = s.planID and p.mobileNo = s.mobileNo
where p.mobileNo = @mobile_num and s.status = 'active' 
and month(p.start_date)= month(current_timestamp) or month(p.end_date)= month(current_timestamp) and year(p.start_date)= year(current_timestamp) or year(p.end_date)= year(current_timestamp))
go 


go
CREATE FUNCTION [Cashback_Wallet_Customer]
(@NID int) --Define Function Inputs
RETURNS Table -- Define Function Output
AS
Return(select c.* from Cashback c 
inner join Wallet w 
on c.walletID = w.walletID 
where w.nationalID = @NID)
go 




go
CREATE PROCEDURE [Ticket_Account_Customer]
@NID int 

AS
select count(t.ticketID) as 'Value' from Technical_support_ticket t
inner join customer_account a 
on t.mobileNo = a.mobileNo
where t.status <> 'resolved' and a.nationalID = @NID
go
EXEC Ticket_Account_Customer @NID =101



go
CREATE PROCEDURE [Account_Highest_Voucher]
@mobile_num char(11)  

AS
declare @max int
select @max =  max(v.value) from Voucher v 
where v.mobileNo = @mobile_num 

select v.voucherID from voucher v
where v.mobileNo = @mobile_num and v.value = @max

go


go
CREATE FUNCTION [Remaining_plan_amount]
(@mobile_num char(11), @plan_name varchar(50)) --Define Function Inputs
RETURNS int -- Define Function Output
AS
Begin
declare @output int, @plan_id int, @payment_id int
Select @plan_id = s.planID, @payment_id= p.paymentID from Service_plan s inner join process_payment pp
on s.planID = pp.planID inner join payment p 
on pp.paymentID = p.paymentID
where s.name = @plan_name and p.mobileNo = @mobile_num and p.status='successful'

set @output = dbo.function_remaining_amount(@payment_id,@plan_id)
return @output
END
go

go
CREATE FUNCTION [Extra_plan_amount]
(@mobile_num char(11), @plan_name varchar(50)) --Define Function Inputs
RETURNS int -- Define Function Output
AS
Begin
declare @output int, @plan_id int, @payment_id int
Select @plan_id = s.planID, @payment_id= p.paymentID from Service_plan s inner join process_payment pp
on s.planID = pp.planID inner join payment p 
on pp.paymentID = p.paymentID
where s.name = @plan_name and p.mobileNo = @mobile_num and p.status='successful'

set @output = dbo.function_extra_amount(@payment_id,@plan_id)
return @output
END
go

go
CREATE PROCEDURE [Top_Successful_Payments]
@mobile_num char(11)  

AS
select top 10 p.* from Payment p 
where p.mobileNo = @mobile_num
and p.status = 'successful'
order by p.amount desc
go


go
CREATE FUNCTION [Subscribed_plans_5_Months]
(@MobileNo char(11)) --Define Function Inputs
RETURNS Table -- Define Function Output
AS
Return(Select sp.* from Service_plan sp 
inner join Subscription s 
on sp.planID = s.planID
where s.mobileNo = @MobileNo and 
s.subscription_date >= DATEADD(month,-5,CURRENT_TIMESTAMP))
go 


go
CREATE PROCEDURE [Initiate_plan_payment]
@mobile_num char(11), @amount decimal(10,1), @payment_method varchar(50),
@plan_id int 

AS
declare @payment_id int
Insert into Payment (amount,date_of_payment,payment_method,status,mobileNo)
values(@amount,CURRENT_TIMESTAMP,@payment_method,'successful',@mobile_num)
SELECT @payment_id = p.paymentID from Payment p    
where p.mobileNo = @mobile_num and p.amount = @amount and p.date_of_payment = CAST(CURRENT_TIMESTAMP AS DATE)
and p.payment_method = @payment_method and p.status = 'successful'
Insert into process_payment(paymentID, planID) values(@payment_id, @plan_id)
if(select remaining_amount from process_payment where planID = @plan_id and paymentID = @payment_id) = 0 
update Subscription
set status = 'active'
where planID = @plan_id and mobileNo = @mobile_num
else
update Subscription
set status = 'onhold'
where planID = @plan_id and mobileNo = @mobile_num

go

go
CREATE PROCEDURE [Payment_wallet_cashback]
@mobile_num char(11), @payment_id int, @benefit_id int 

AS
declare @amount int, @cash_amount int, @wallet_id int 
select @amount = p.amount  from Payment p
where p.paymentID = @payment_id and p.status = 'successful'
set @cash_amount = 0.1 * @amount
select @wallet_id = w.walletID from Wallet w
inner join customer_account a on
w.nationalID = a.nationalID 
where a.mobileNo = @mobile_num

Insert into Cashback(benefitID,walletID,amount,credit_date)
values(@benefit_id,@wallet_id,@cash_amount,current_timestamp)

update Wallet
set current_balance = current_balance + @cash_amount,
last_modified_date = current_timestamp
where walletID = @wallet_id

go

go
CREATE PROCEDURE [Initiate_balance_payment]
@mobile_num char(11), @amount decimal(10,1), @payment_method varchar(50)

as
Insert into Payment (amount,date_of_payment,payment_method,status,mobileNo)
values(@amount,CURRENT_TIMESTAMP,@payment_method,'successful',@mobile_num)

update customer_account
set balance = balance + @amount
where mobileNo = @mobile_num

go

go
CREATE PROCEDURE [Redeem_voucher_points]
@mobile_num char(11), @voucher_id int 

AS
If (Select v.points from Voucher v 
where v.voucherID = @voucher_id and v.expiry_date >CURRENT_TIMESTAMP ) <= (Select a.points from customer_account a 
where a.mobileNo = @mobile_num) 
begin 
declare @voucher_points int 
select @voucher_points = points from Voucher
where voucherID = @voucher_id
update Voucher
set mobileNo = @mobile_num , redeem_date = current_timestamp 
where voucherID = @voucher_id 

update customer_account
set points = points - @voucher_points
where mobileNo = @mobile_num
end 
else 
print 'no enough points to redeem voucher'

go 
INSERT INTO Customer_Profile (nationalID, first_name, last_name, email, address, date_of_birth)
VALUES (1, 'John', 'Doe', 'john.doe@example.com', '123 Elm Street', '1990-01-01'),
       (2, 'Jane', 'Smith', 'jane.smith@example.com', '456 Oak Avenue', '1985-05-15'),
       (3, 'Robert', 'Brown', 'robert.brown@example.com', '789 Pine Street', '1988-11-22'),
       (4, 'Emily', 'Davis', 'emily.davis@example.com', '101 Maple Drive', '1995-04-14'),
       (5, 'Michael', 'Johnson', 'michael.johnson@example.com', '202 Birch Lane', '1992-07-30'),
       (6, 'Sarah', 'Wilson', 'sarah.wilson@example.com', '303 Cedar Ave', '1990-10-05'),
       (7, 'David', 'Lee', 'david.lee@example.com', '404 Spruce Road', '1993-03-18');
      
SET IDENTITY_INSERT Service_Plan ON;
INSERT INTO Service_Plan (planID, SMS_offered, minutes_offered, data_offered, name, price, description)
VALUES (101, 100, 500, 2048, 'Basic Plan', 20, 'Affordable basic plan'),
       (102, 200, 1000, 4096, 'Premium Plan', 50, 'High-end plan with more data'),
       (103, 50, 300, 1024, 'Starter Plan', 10, 'Beginner-level plan'),
       (104, 300, 1500, 8192, 'Ultra Plan', 70, 'Best value for heavy users'),
       (105, 150, 800, 3072, 'Mid-Range Plan', 30, 'Mid-tier data offering'),
       (106, 500, 2500, 12288, 'Enterprise Plan', 100, 'Business-level plan'),
       (107, 75, 400, 1536, 'Budget Plan', 15, 'Low-cost option');
SET IDENTITY_INSERT Service_Plan OFF;

SET IDENTITY_INSERT Shop ON;
INSERT INTO Shop (shopID, name, category)
VALUES (1, 'Tech Store', 'Electronics'),
       (2, 'Fashion Hub', 'Clothing'),
       (3, 'Grocery Mart', 'Groceries'),
       (4, 'Sports Arena', 'Sports Equipment'),
       (5, 'Book Haven', 'Books'),
       (6, 'Music World', 'Music'),
       (7, 'Home Essentials', 'Furniture');
SET IDENTITY_INSERT Shop OFF;

INSERT INTO Customer_Account (mobileNo, pass, balance, account_type, start_date, status, points, nationalID)
VALUES ('01234567890', 'password123', 100.0, 'prepaid', '2023-01-01', 'active', 50, 1),
       ('09876543210', 'securepass', 200.0, 'postpaid', '2023-02-01', 'active', 75, 2),
       ('01122334455', 'mypassword', 150.0, 'prepaid', '2023-03-01', 'active', 30, 3),
       ('02233445566', 'mypin123', 250.0, 'postpaid', '2023-04-01', 'onhold', 60, 4),
       ('03344556677', 'safepass', 120.0, 'prepaid', '2023-05-01', 'active', 40, 5),
       ('04455667788', 'simplepass', 80.0, 'postpaid', '2023-06-01', 'active', 55, 6),
       ('05566778899', 'supersecure', 300.0, 'prepaid', '2023-07-01', 'active', 90, 7);
       SET IDENTITY_INSERT Wallet ON;
INSERT INTO Wallet (walletID, current_balance, currency, last_modified_date, nationalID, mobileNo)
VALUES (1, 150.50, 'USD', '2023-10-01', 1, '01234567890'),
       (2, 300.75, 'USD', '2023-10-02', 2, '09876543210'),
       (3, 180.00, 'USD', '2023-10-03', 3, '01122334455'),
       (4, 225.25, 'USD', '2023-10-04', 4, '02233445566'),
       (5, 110.10, 'USD', '2023-10-05', 5, '03344556677'),
       (6, 95.75, 'USD', '2023-10-06', 6, '04455667788'),
       (7, 400.00, 'USD', '2023-10-07', 7, '05566778899');
SET IDENTITY_INSERT Wallet OFF;
SET IDENTITY_INSERT Plan_Usage ON;
INSERT INTO Plan_Usage (usageID, start_date, end_date, data_consumption, minutes_used, SMS_sent, mobileNo, planID)
VALUES (1, '2023-01-01', '2023-01-31', 1500, 400, 80, '01234567890', 101),
       (2, '2023-02-01', '2023-02-28', 3500, 800, 150, '09876543210', 102),
       (3, '2023-03-01', '2023-03-31', 800, 200, 40, '01122334455', 103),
       (4, '2023-04-01', '2023-04-30', 6000, 1300, 300, '02233445566', 104),
       (5, '2023-05-01', '2023-05-31', 2500, 900, 120, '03344556677', 105),
       (6, '2023-06-01', '2023-06-30', 12000, 2400, 500, '04455667788', 106),
       (7, '2023-07-01', '2023-07-31', 1200, 350, 60, '05566778899', 107);
SET IDENTITY_INSERT Plan_Usage OFF;
SET IDENTITY_INSERT Payment ON;
INSERT INTO Payment (paymentID, amount, date_of_payment, payment_method, status, mobileNo)
VALUES (1, 20.0, '2023-01-01', 'credit', 'successful', '01234567890'),
       (2, 50.0, '2023-02-01', 'credit', 'successful', '09876543210'),
       (3, 15.0, '2023-03-01', 'cash', 'pending', '01122334455'),
       (4, 70.0, '2023-04-01', 'credit', 'successful', '02233445566'),
       (5, 30.0, '2023-05-01', 'cash', 'successful', '03344556677'),
       (6, 100.0, '2023-06-01', 'credit', 'successful', '04455667788'),
       (7, 25.0, '2023-07-01', 'cash', 'successful', '05566778899');
SET IDENTITY_INSERT Payment OFF;
SET IDENTITY_INSERT Benefits ON;
INSERT INTO Benefits (benefitID, description, validity_date, status, mobileNo)
VALUES (1, 'Free extra data', '2024-01-01', 'active', '01234567890'),
       (2, 'Discounted SMS', '2024-02-01', 'active', '09876543210'),
       (3, 'Bonus minutes', '2024-03-01', 'expired', '01122334455'),
       (4, 'Promotional Offer', '2024-04-01', 'active', '02233445566'),
       (5, 'Limited Time cashback', '2024-05-01', 'expired', '03344556677'),
       (6, 'Holiday Special', '2024-06-01', 'active', '04455667788'),
       (7, 'Birthday Reward', '2024-07-01', 'active', '05566778899');
SET IDENTITY_INSERT Benefits OFF;
SET IDENTITY_INSERT Voucher ON;
INSERT INTO Voucher (voucherID, value, expiry_date, points, mobileNo, shopID, redeem_date)
VALUES (1, 10, '2024-01-01', 20, '01234567890', 1, '2023-12-01'),
       (2, 15, '2024-01-15', 30, '09876543210', 2, '2023-12-10'),
       (3, 20, '2024-02-01', 40, '01122334455', 3, '2023-12-20'),
       (4, 25, '2024-03-01', 50, '02233445566', 4, '2024-01-01'),
       (5, 30, '2024-04-01', 60, '03344556677', 5, '2024-01-10'),
       (6, 35, '2024-05-01', 70, '04455667788', 6, '2024-01-20'),
       (7, 40, '2024-06-01', 80, '05566778899', 7, '2024-02-01');
SET IDENTITY_INSERT Voucher OFF;
SET IDENTITY_INSERT Exclusive_Offer ON;
INSERT INTO Exclusive_Offer (offerID, benefitID, internet_offered, SMS_offered, minutes_offered)
VALUES (1, 1, 500, 50, 100),
       (2, 2, 1000, 100, 200),
       (3, 3, 250, 25, 50),
       (4, 4, 800, 75, 150),
       (5, 5, 1200, 150, 300),
       (6, 6, 400, 30, 60),
       (7, 7, 600, 45, 90);
SET IDENTITY_INSERT Exclusive_Offer OFF;
SET IDENTITY_INSERT cashback ON;
INSERT INTO cashback (cashbackID, benefitID, walletID, amount, credit_date)
VALUES (1, 1, 1, 5, '2023-12-01'),
       (2, 2, 2, 10, '2023-12-05'),
       (3, 3, 3, 15, '2023-12-10'),
       (4, 4, 4, 20, '2023-12-15'),
       (5, 5, 5, 25, '2023-12-20'),
       (6, 6, 6, 30, '2023-12-25'),
       (7, 7, 7, 35, '2023-12-30');
SET IDENTITY_INSERT cashback OFF;
INSERT INTO Plan_Provides_Benefits (benefitID, planID)
VALUES (1, 101),
       (2, 102),
       (3, 103),
       (4, 104),
       (5, 105),
       (6, 106),
       (7, 107);
INSERT INTO Process_Payment (paymentID, planID)
VALUES (1, 101),
       (2, 102),
       (3, 103),
       (4, 104),
       (5, 105),
       (6, 106),
       (7, 107);

SET IDENTITY_INSERT Transfer_Money ON;
INSERT INTO Transfer_Money (walletID1, walletID2, transfer_id, amount, transfer_date)
VALUES (1, 2, 1, 50.0, '2023-12-01'),
       (2, 3, 2, 75.0, '2023-12-05'),
       (3, 4, 3, 30.0, '2023-12-10'),
       (4, 5, 4, 100.0, '2023-12-15'),
       (5, 6, 5, 20.0, '2023-12-20'),
       (6, 7, 6, 45.0, '2023-12-25'),
       (7, 1, 7, 15.0, '2023-12-30');
SET IDENTITY_INSERT Transfer_Money OFF;
SET IDENTITY_INSERT Technical_Support_Ticket ON;
INSERT INTO Technical_Support_Ticket (ticketID, mobileNo, Issue_description, priority_level, status)
VALUES (1, '01234567890', 'Network issue', 1, 'Resolved'),
       (2, '09876543210', 'Billing query', 2, 'Open'),
       (3, '01122334455', 'Slow connection', 3, 'In progress'),
       (4, '02233445566', 'Account locked', 1, 'Resolved'),
       (5, '03344556677', 'Incorrect charges', 2, 'Open'),
       (6, '04455667788', 'Unable to recharge', 3, 'In progress'),
       (7, '05566778899', 'Service disruption', 1, 'Resolved');
SET IDENTITY_INSERT Technical_Support_Ticket OFF;
SET IDENTITY_INSERT Points_Group ON;
INSERT INTO Points_Group (pointID, benefitID, pointsAmount, PaymentID)
VALUES 
    (1, 1, 50, 1),
    (2, 2, 75, 2),
    (3, 3, 30, 3),
    (4, 4, 60, 4),
    (5, 5, 40, 5),
    (6, 6, 55, 6),
    (7, 7, 90, 7);
SET IDENTITY_INSERT Points_Group OFF;
INSERT INTO Physical_Shop (shopID, address, working_hours)
VALUES 
    (1, '123 Elm Street', '9 AM - 9 PM'),
    (3, '789 Pine Street', '8 AM - 10 PM'),
    (4, '404 Spruce Road', '10 AM - 8 PM'),
    (7, '567 Maple Drive', '11 AM - 7 PM');
INSERT INTO E_Shop (shopID, URL, rating)
VALUES 
    (2, 'https://fashionhub.com', 4),
    (5, 'https://bookhaven.com', 5),
    (6, 'https://musicworld.com', 3);