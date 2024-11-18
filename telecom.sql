--start of 2.1 a
create database Telecom_Team_32;
GO
USE Telecom_Team_32;
--end of 2.1 a
GO

--start of 2.1 b
CREATE PROCEDURE createAllTables
AS
BEGIN
    CREATE TABLE Customer_Profile (
        nationalID INT PRIMARY KEY,  --not identity
        first_name VARCHAR(50),
        last_name VARCHAR(50),
        email VARCHAR(50),
        address VARCHAR(50),
        date_of_birth DATE
    );
    
    CREATE TABLE Customer_Account (
        mobileNo CHAR(11) PRIMARY KEY, --not identity
        pass VARCHAR(50),
        balance DECIMAL(10,1),
        account_type VARCHAR(50),
        start_date DATE,
        status VARCHAR(50),
        point INT DEFAULT 0,
        nationalID INT,
        constraint acc_status_chk check (status in('active','onhold')),
        constraint type_chk check (account_type in('post paid','prepaid','pay_as_you_go')),
        constraint belongs_to FOREIGN KEY (nationalID) REFERENCES Customer_Profile(nationalID)
    );
    
    CREATE TABLE Service_Plan (
        planID INT PRIMARY KEY IDentity(1,1),
        SMS_offered INT,
        minutes_offered INT,
        data_offered INT,
        name VARCHAR(50),
        price INT,
        description VARCHAR(50)
    );
    
    CREATE TABLE Subscription (
        mobileNo CHAR(11),
        planID INT,
        subscription_date DATE,
        status VARCHAR(50),
        PRIMARY KEY(mobileNo,planID),
        constraint sub_status_chk check(status in('active','onhold')),
        constraint acc_sub FOREIGN KEY (mobileNo) REFERENCES 

Customer_Account(mobileNo),
        constraint service_sub FOREIGN KEY (planID) REFERENCES Service_Plan(planID)
    );
    
    CREATE TABLE Plan_Usage (
        usageID INT PRIMARY KEY IDENTITY(1,1),
        start_date DATE,
        end_date DATE,
        data_consumption INT,
        minutes_used INT,
        SMS_sent INT,
        mobileNo CHAR(11),
        planID INT,
        constraint plan_acc FOREIGN KEY (mobileNo) REFERENCES Customer_Account(mobileNo),
        constraint plan_serv_1 FOREIGN KEY (planID) REFERENCES Service_Plan(planID)
    );
    
    CREATE TABLE Payment (
        paymentID INT PRIMARY KEY IDENTITY(1,1),
        amount DECIMAL(10,1),
        date_of_payment DATE,
        payment_method VARCHAR(50),
        status VARCHAR(50),
        mobileNo CHAR(11),
        constraint pay_status check (status in('successful','pending','rejected')),
        constraint pay_acc FOREIGN KEY (mobileNo) REFERENCES Customer_Account(mobileNo)
    );
    
   /* CREATE TABLE Process_Payment ( –this is false. suspected solution uses views??
        paymentID INT PRIMARY KEY,
        planID INT,
        remaining_balance as 
            (CASE WHEN ((SELECT top 1 p.amount FROM payment p WHERE p.paymentID = paymentID ) < (SELECT top 1 s.price FROM servicePlan s WHERE s.planID = planID )) THEN 
                (SELECT top 1 s.price FROM servicePlan s WHERE s.planID = planID) - (SELECT top 1 p.amount FROM payment p WHERE p.paymentID = paymentID)
              ELSE
                0
              END
             ),
        additional_amounts as 
            (CASE WHEN ((SELECT top 1 p.amount FROM payment p WHERE p.paymentID = paymentID ) > (SELECT top 1 s.price FROM servicePlan s WHERE s.planID = planID )) THEN 
                (SELECT top 1 p.amount FROM payment p WHERE p.paymentID = paymentID) - (SELECT top 1 s.price FROM servicePlan s WHERE s.planID = planID)
              ELSE
                0
              END
             )
    );*/
    
    
    CREATE TABLE Wallet (
        walletID INT PRIMARY KEY IDENTITY(1,1),
        current_balance DECIMAL(10,2),
        currency VARCHAR(50),
        last_modified_date DATE,
        nationalID INT,
        mobileNo CHAR(11),
        constraint wall_profile FOREIGN KEY (nationalID) REFERENCES Customer_Profile(nationalID)
    );
    
    CREATE TABLE Transfer_money (
        transfer_id INT PRIMARY KEY IDENTITY(1,1),
        walletID1 INT,
        walletID2 INT,
        amount DECIMAL(10,2),
        transfer_date DATE,
        constraint sender_wallet FOREIGN KEY (walletID1) REFERENCES Wallet(walletID),
        constraint receive_wallet FOREIGN KEY (walletID2) REFERENCES Wallet(walletID)
    );
    
    CREATE TABLE Benefits (
        benefitID INT PRIMARY KEY IDENTITY(1,1),
        description VARCHAR(50),
        validity_date DATE,
        status VARCHAR(50),
        mobileNo CHAR(11),
        constraint benefit_status_chk check (status in ('active','expired')),
        constraint benefits_acc FOREIGN KEY (mobileNo) REFERENCES Customer_Account(mobileNo)
    );
    
    CREATE TABLE Points_Group (
        pointID INT PRIMARY KEY IDENTITY(1,1),
        benefitID INT,
        pointsAmount INT,
        paymentID INT,
        constraint points_benefits FOREIGN KEY (benefitID) REFERENCES Benefits(benefitID),
        constraint points_payment FOREIGN KEY (paymentID) REFERENCES Payment(paymentID)
    );
    
    CREATE TABLE Exclusive_Offer (
        offerID INT PRIMARY KEY IDENTITY(1,1),
        benefitID INT,
        internet_offered INT,
        SMS_offered INT,
        minutes_offered INT,
        constraint offer_benefit FOREIGN KEY (benefitID) REFERENCES Benefits(benefitID)
    );
    
    CREATE TABLE Cashback (--10% of payment 
        CashbackID INT PRIMARY KEY IDENTITY(1,1),
        benefitID INT,
        walletID INT,
        amount INT,
        credit_date DATE,
        constraint cashback_benefit FOREIGN KEY (benefitID) REFERENCES Benefits(benefitID),
        constraint cashback_wallet FOREIGN KEY (walletID) REFERENCES Wallet(walletID)
    );
    
    CREATE TABLE Plan_Provides_Benefits (
        benefitID INT,
        planID INT,
        PRIMARY KEY (benefitID, planID),
        constraint plan_benefits FOREIGN KEY (benefitID) REFERENCES Benefits(benefitID),
        constraint plan_serv_2 FOREIGN KEY (planID) REFERENCES Service_Plan(planID)
    );
    
    CREATE TABLE Shop (
        shopID INT PRIMARY KEY IDENTITY(1,1),
        name VARCHAR(50),
        category VARCHAR(50) NOT NULL
    );
    
    CREATE TABLE Physical_Shop (
        shopID INT PRIMARY KEY,
        address VARCHAR(50),
        working_hours VARCHAR(50),
        constraint phys_shop FOREIGN KEY (shopID) REFERENCES Shop(shopID)
    );
    
    CREATE TABLE E_shop (
        shopID INT PRIMARY KEY,
        URL VARCHAR(50),
        rating INT,
        constraint electronic_shop FOREIGN KEY (shopID) REFERENCES Shop(shopID)
    );
    CREATE TABLE Voucher (
        voucherID INT PRIMARY KEY,
        value INT ,
        expiry_date DATE,
        points INT,
        mobileNo CHAR(11),
        shopID INT,
        redeem_date DATE,
        CONSTRAINT mobile_FK FOREIGN KEY (mobileNo) REFERENCES Customer_Account(mobileNo),
        CONSTRAINT shopID_FK FOREIGN KEY (shopID) REFERENCES Shop(shopID)
    );
    CREATE TABLE Technical_Support_Ticket (
        ticketID INT PRIMARY KEY IDENTITY(1,1),
        mobileNo CHAR(11),
        Issue_description VARCHAR(50),
        priority_level INT,
        status VARCHAR(50),
        constraint ticket_stat_chk check (status in('open','in progress','resolved')),
        constraint ticket_acc FOREIGN KEY (mobileNo) REFERENCES Customer_Account(mobileNo)
     );
 END
--end of 2.1 b
 GO

 --start of 2.1 c
 CREATE PROCEDURE dropAllTables 
 AS
 BEGIN
ALTER TABLE Customer_Account
DROP CONSTRAINT belongs_to;
ALTER TABLE Subscription
DROP CONSTRAINT acc_sub;
ALTER TABLE Subscription
DROP CONSTRAINT service_sub;
ALTER TABLE Plan_Usage
DROP CONSTRAINT plan_acc;
ALTER TABLE Plan_Usage
DROP CONSTRAINT plan_serv_1;
ALTER TABLE Payment
DROP CONSTRAINT pay_acc;
ALTER TABLE Process_Payment
DROP CONSTRAINT proc_pay;
ALTER TABLE Process_Payment
DROP CONSTRAINT proc_serv;
ALTER TABLE Wallet
DROP CONSTRAINT wall_profile;
ALTER TABLE Transfer_money
DROP CONSTRAINT sender_wallet;
ALTER TABLE Transfer_money
DROP CONSTRAINT receive_wallet;
ALTER TABLE Benefits
DROP CONSTRAINT benefits_acc;
ALTER TABLE Points_Group
DROP CONSTRAINT points_benefits;
ALTER TABLE Points_Group
DROP CONSTRAINT points_payment;
ALTER TABLE Exclusive_Offer
DROP CONSTRAINT offer_benefit;
ALTER TABLE Cashback
DROP CONSTRAINT cashback_benefit;
ALTER TABLE Cashback
DROP CONSTRAINT cashback_wallet;
ALTER TABLE Plan_Provides_Benefits
DROP CONSTRAINT plan_benefits;
ALTER TABLE Plan_Provides_Benefits
DROP CONSTRAINT plan_serv_2;
ALTER TABLE Physical_Shop
DROP CONSTRAINT phys_shop;
ALTER TABLE E_shop
DROP CONSTRAINT electronic_shop;
ALTER TABLE Voucher
DROP CONSTRAINT mobile_FK;
ALTER TABLE Voucher
DROP CONSTRAINT shopID_FK;
ALTER TABLE Technical_Support_Ticket
DROP CONSTRAINT ticket_acc;

DROP TABLE Customer_profile;
DROP TABLE Customer_Account;
DROP TABLE Service_Plan;
DROP TABLE Subscription;
DROP TABLE Plan_Usage;
DROP TABLE Payment;
/*DROP TABLE Process_Payment;*/
DROP TABLE Wallet;
DROP TABLE Transfer_money;
DROP TABLE Benefits;
DROP TABLE Points_Group;
DROP TABLE Exclusive_Offer;
DROP TABLE Cashback;
DROP TABLE Plan_Provides_Benefits;
DROP TABLE Shop;
DROP TABLE Physical_Shop ;
DROP TABLE E_shop;
DROP TABLE Voucher;
DROP TABLE Technical_Support_Ticket;

 END
 --end of 2.1 c
 Go
 --start of 2.1 e
 CREATE PROCEDURE clearAllTables
 AS
 BEGIN
    ALTER TABLE Customer_Account
   DROP CONSTRAINT belongs_to;
   ALTER TABLE Subscription
   DROP CONSTRAINT acc_sub;
   ALTER TABLE Subscription
   DROP CONSTRAINT service_sub;
   ALTER TABLE Plan_Usage
   DROP CONSTRAINT plan_acc;
   ALTER TABLE Plan_Usage
   DROP CONSTRAINT plan_serv_1;
   ALTER TABLE Payment
   DROP CONSTRAINT pay_acc;
   /*ALTER TABLE Process_Payment
   DROP CONSTRAINT proc_pay;
   ALTER TABLE Process_Payment
   DROP CONSTRAINT proc_serv;*/
   ALTER TABLE Wallet
   DROP CONSTRAINT wall_profile;
   ALTER TABLE Transfer_money
   DROP CONSTRAINT sender_wallet;
   ALTER TABLE Transfer_money
   DROP CONSTRAINT receive_wallet;
   ALTER TABLE Benefits
   DROP CONSTRAINT benefits_acc;
   ALTER TABLE Points_Group
   DROP CONSTRAINT points_benefits;
   ALTER TABLE Points_Group
   DROP CONSTRAINT points_payment;
   ALTER TABLE Exclusive_Offer
   DROP CONSTRAINT offer_benefit;
   ALTER TABLE Cashback
   DROP CONSTRAINT cashback_benefit;
   ALTER TABLE Cashback
   DROP CONSTRAINT cashback_wallet;
   ALTER TABLE Plan_Provides_Benefits
   DROP CONSTRAINT plan_benefits;
   ALTER TABLE Plan_Provides_Benefits
   DROP CONSTRAINT plan_serv_2;
   ALTER TABLE Physical_Shop
   DROP CONSTRAINT phys_shop;
   ALTER TABLE E_shop
   DROP CONSTRAINT electronic_shop;
   ALTER TABLE Voucher
   DROP CONSTRAINT mobile_FK;
   ALTER TABLE Voucher
   DROP CONSTRAINT shopID_FK;
   ALTER TABLE Technical_Support_Ticket
   DROP CONSTRAINT ticket_acc;


    TRUNCATE TABLE Customer_Profile;
    TRUNCATE TABLE Customer_Account;
    TRUNCATE TABLE Service_Plan;
    TRUNCATE TABLE Subscription;
    TRUNCATE TABLE Plan_Usage;
    TRUNCATE TABLE Payment;
  --TRUNCATE TABLE Process_Payment;
    TRUNCATE TABLE Wallet;
    TRUNCATE TABLE Transfer_money;
    TRUNCATE TABLE Benefits;
    TRUNCATE TABLE Points_Group;
    TRUNCATE TABLE Exclusive_Offer;
    TRUNCATE TABLE Cashback;
    TRUNCATE TABLE Plan_Provides_Benefits;
    TRUNCATE TABLE Shop;
    TRUNCATE TABLE Physical_Shop ;
    TRUNCATE TABLE E_shop;
    TRUNCATE TABLE Voucher;
    TRUNCATE TABLE Technical_Support_Ticket;
 END
 --end of 2.1 e
 GO
 --start of 2.2 a
 CREATE VIEW allCustomerAccounts AS
    Select p.*,a.mobileNo,a.pass,a.balance,a.account_type,a.start_date,a.status,a.point,a.nationalID as account_national_id FROM
    Customer_Profile p 
    join Customer_Account a on a.nationalID = p.nationalID
    where a.status='active';
 --end of 2.2 a
 GO
 --start of 2.2 b
 CREATE VIEW allServicePlans AS
    Select * 
    FROM Service_Plan

 --end of 2.2 b
 GO
 --start of 2.2 c
 CREATE VIEW allBenefits AS 
    SELECT * 
    FROM Benefits b
    WHERE b.status = 'active'
 --end of 2.2 c
 --start of 2.2 d
 GO
 CREATE VIEW AccountPayments AS 
 SELECT a.*, p.paymentID, p.amount, p.date_of_payment, p.payment_method, p.status as 'payment_status'
 FROM Payment p INNER JOIN Customer_Account a ON p.mobileNo = a.mobileNo
 --end of 2.2 d
 --start of 2.2 e
 GO 
 CREATE VIEW allShops AS
 SELECT * FROM Shop
 --end of 2.2 e
 --start of 2.2 f
 GO 
 CREATE VIEW allResolvedTickets AS
 SELECT * FROM Technical_Support_Ticket
 WHERE status = 'resolved'
 --end of 2.2 f
 GO
  --start of 2.2 g
  CREATE VIEW CustomerWallet AS
SELECT w.walletID, w.mobileNo, w.current_balance AS wallet_balance, w.currency, w.last_modified_date, w.nationalID, p.first_name AS customer_first_name, p.last_name AS customer_last_name
FROM 
    Wallet w
JOIN 
    Customer_Account a ON w.mobileNo = a.mobileNo
JOIN 
    Customer_Profile p ON a.nationalID = p.nationalID;
 --end of 2.2 g
  --start of 2.2 h
  GO
 CREATE VIEW E_shopVouchers AS
 SELECT s.name, v.voucherID,v.value
 FROM E_shop e, Shop s, Voucher v 
 where e.shopID=s.shopID and v.shopID=e.shopID and v.redeem_date is not null
 GO
 --end of 2.2 h
 --start of 2.2 f
 GO CREATE VIEW allResolvedTickets AS
 SELECT * FROM Technical_Support_Ticket
 WHERE status = 'resolved'
  GO
 --end of 2.2 f

  --start of 2.2 i
  GO
 CREATE VIEW PhysicalStoreVouchers AS
 SELECT s.name, v.voucherID,v.value
 FROM Physical_Shop p, Shop s, Voucher v 
 where p.shopID=s.shopID and v.shopID=p.shopID and v.redeem_date is not null
  GO
 --end of 2.2 i
 
  --start of 2.2 j
  GO
 CREATE VIEW Num_of_cashback AS
 SELECT w.mobileNo ,count(c.CashbackID) AS cashback_count
 FROM Wallet w left outer join Cashback c on c.walletID=w.walletID
 group by w.mobileNo
  GO
 --end of 2.2 j
 
 --start of 2.3 a
 CREATE PROCEDURE Account_Plan
 AS
 BEGIN                         --when listing an account is mobile number enough or all acc attributes need to be submitted?
    Select mobileNo,planID FROM Subscription
 END
 --end of 2.3 a



-- start of 2.3b
go
create function Account_Plan_date
(@Subscription_Date date, @Plan_id int)
returns Table
as
return (
select acc.mobileNo, sub.planID, serv_plan.name
from Customer_Account acc inner join Subscription sub
on sub.subscription_date=@Subscription_Date and acc.mobileNo=sub.mobileNo and sub.planID=@Plan_id 
inner join Service_Plan serv_plan
on serv_plan.planID=@Plan_id
)
go

-- endof 2.3b

-- start of 2.3c
go
create function Account_Usage_Plan
(@mobileNo char(11), @from_date date)
returns Table
as
return (
select usage.planID, usage.data_consumption, usage.minutes_used, usage.SMS_sent
from Customer_Account acc inner join Plan_Usage usage
on acc.mobileNo=@mobileNo and usage.start_date=@from_date and usage.mobileNo=acc.mobileNo
)
go

-- end of 2.3c

 GO
 --start of 2.3 d
CREATE PROCEDURE Benefits_Account
  @mobileNo char(11),
  @plan_ID int
AS
BEGIN
WITH Filtered_Plan_provides_benefit AS (
    SELECT * 
    FROM plan_provides_benefits
    WHERE plan_provides_benefits.planID = @plan_ID
)
DELETE FROM Benefits WHERE mobileNo = @mobileNo
    AND EXISTS (
        SELECT 1
        FROM Filtered_Plan_Provides_Benefit
        WHERE Filtered_Plan_Provides_Benefit.benefit_ID = Benefits.benefit_ID
    ); 
END
 --end of 2.3 d
 GO

-- start of 2.3e
go
create function Account_SMS_Offers
(@mobileNo char(11))
returns Table
as
return (
select sms_offers.offerID, sms_offers.internet_offered , sms_offers.SMS_offered, sms_offers.minutes_offered
from Customer_Account acc inner join Exclusive_Offer sms_offers
on acc.mobileNo=@mobileNo and sms_offers.SMS_offered>0 and sms_offers.SMS_offered is not null  inner join Benefits benefit
on benefit.mobileNo=acc.mobileNo and benefit.benefitID=sms_offers.benefitID
)
go

-- start of 2.3e

 --start of 2.3 f
 CREATE PROCEDURE Account_Payment_Points
 @mobileNO char(11),
 @total_number_of_transactions int OUTPUT,--could it be null?
 @total_amount_of_points int OUTPUT --could it be null?
 AS
 BEGIN
  WITH account_payments as(
    select *
    FROM Payment
    WHERE @mobileNo = Payment.mobileNo AND Payment.status = 'successful' AND  date_of_payment >= DATEADD(YEAR, -1, CURRENT_TIMESTAMP)
  )

  select @total_amount_of_points = sum(Points_Group.pointsAmount)
  FROM account_payments
  JOIN Points_Group on Points_Group.paymentID = account_payments.paymentID

 SELECT @total_number_of_transactions = count(*) FROM Payment
    WHERE @mobileNo = Payment.mobileNo AND Payment.status = 'successful' AND  date_of_payment >= DATEADD(YEAR, -1, CURRENT_TIMESTAMP)
 END
 --end of 2.3 f



--start of 2.3g
go
create function Wallet_Cashback_Amount
(@Wallet_id int, @plan_id int)
returns int
as
begin
declare @res int
select @res=sum(cashback.amount)
from Cashback cashback inner join Wallet wallet
on cashback.walletID=wallet.walletID and cashback.walletID=@Wallet_id inner join Benefits 
on benefit.benefitID=cashback.benefitID inner join Plan_Provides_Benefits plan_benef
on benefit.benefitID=plan_benef.benefitID and plan_benef.planID=@plan_id

return @res
end
go

--end of 2.3g

--start of 2.3h
go
create function Wallet_Transfer_Amount
(@Wallet_id int, @start_date date, @end_date date)
returns int
as
begin
declare @res int
select @res=avg(transfer.amount)
from Transfer_money transfer 
where transfer.walletID1=@Wallet_id and transfer.transfer_date between @start_date and @end_date
return @res
end
go

--end of 2.3h

--start of 2.3i
go
create function Wallet_MobileNo
(@MobileNo char(11))
returns bit
as
begin
declare @res bit
if exists(select* from Wallet wallet where wallet.mobileNo=@MobileNo)
	set @res='1'
else
	set @res='0'

return @res
end
go 

--end of 2.3i

 --start of 2.3 j
CREATE PROCEDURE Total_Points_Account
@MobileNo char(11),
@totalPoints int OUTPUT --could it be null?
AS
BEGIN
    WITH account_payments as(
        select *
        FROM Payment
        WHERE @mobileNo = Payment.mobileNo AND Payment.status = 'successful'
    )
    
    select @totalPoints = sum(Points_Group.pointsAmount)
    FROM account_payments
    JOIN Points_Group on Points_Group.paymentID = account_payments.paymentID 
    
    UPDATE Customer_Account  
    set point = @totalPoints
    WHERE Customer_Account.mobileNo = @MobileNo
END
--end of 2.3 j
GO
--start of 2.4 c
CREATE PROCEDURE Unsubscribed_Plans
@mobileNO char(11)
AS
BEGIN
    select  * 
    FROM Service_Plan 
    EXCEPT 
    (Select s.planID,s.SMS_offered,s.minutes_offered,s.data_offered,s.name,s.price,s.description FROM Service_Plan s
    join Subscription on Subscription.plan_ID  = service_Plan.planID
    WHERE Subscription.mobileNo = @mobileNO
    )
END
--end of 2.4 c
GO
--start of 2.4 f
CREATE PROCEDURE Ticket_Account_Customer
@NationalID int,
@Technical_Support_tickets_count int OUTPUT  --does it include in_progress tickets as well since they are not resolved? it likely does lol
AS
BEGIN
    select @Technical_Support_tickets_count = count(*)
    FROM Customer_Account a
    join Technical_Support_Ticket t on a.mobileNo = t.mobileNo
    WHERE a.nationalID = @NationalID AND t.status <> 'resolved'
END
--end of 2.4 f
GO
--start of 2.4 g
CREATE PROCEDURE Account_Highest_Voucher
@Mobile_NO char(11),
@Voucher_id int OUTPUT
AS
BEGIN
    SELECT TOP 1 @Voucher_id = v.VoucherID
    FROM Voucher v
    WHERE v.mobileNo = @Mobile_NO
    ORDER BY v.value DESC;
END
--end of 2.4 g

-- start of 2.4 j 
GO
CREATE PROCEDURE Top_Successful_Payments
@MobileNo char(11)
AS
BEGIN
    SELECT TOP 10 * 
    FROM Payments p
    WHERE p.mobileNo = @MobileNo AND status='successful'
    ORDER BY p.amount DESC
END 
-- end of 2.4 j

--start of 2.4 L
GO
CREATE PROCEDURE Initiate_plan_payment
@MobileNo char(11),
@amount decimal(10,1),
@payment_method varchar(50),
@planID int
AS
BEGIN
    DECLARE @plan_price INT;
    Declare @today DATE;
    DECLARE @paymentID INT;
    SELECT @today = CONVERT(DATE, GETDATE());
    select @plan_price = sp.price FROM service_plan sp WHERE sp.planID = @planID
    if (@plan_price <= @amount) 
        BEGIN
            insert into Payment VALUES (@amount,@today,@payment_method,'successful',@MobileNo);
            Update Subscription
            SET status = 'active'
            WHERE Subscription.mobileNo=@MobileNo and Subscription.planID=@planID
       END
    ELSE
    BEGIN
       insert into Payment VALUES (@amount,@today,@payment_method,'rejected',@MobileNo)
    END    
    SELECT @paymentID = MAX(p.paymentID) 
    FROM Payment p;
    INSERT INTO Process_Payment (paymentID, planID)
    VALUES (@paymentID, @planID)
END

--end of 2.4 L
--start of 2.4 m
GO
CREATE PROCEDURE Payment_wallet_cashback
@MobileNo char(11), 
@payment_id int,
@benefit_id int
AS
BEGIN
    DECLARE @oldBalance decimal(10,2);
    DECLARE @cashback decimal(10,2);
    DECLARE @payment decimal(10,2);
    DECLARE @newBalance decimal(10,2);
    DECLARE @wallet_id INT;
    
    SELECT @payment = p.amount FROM Payment p WHERE p.paymentID = @payment_id 
    SET @cashback = @payment*0.1
    
    SELECT @oldBalance = w.current_balance FROM Wallet w 
    WHERE w.mobileNo = @MobileNo
    SET @newBalance = @oldBalance + @cashback
    
    UPDATE Wallet  SET Wallet.current_balance = @newBalance 
    WHERE w.mobileNo = @MobileNo
    Select @wallet_id = w.walletID From Wallet w WHERE @MobileNo = w.mobileNo;

    INSERT INTO Cashback VALUES(@benefit_id,@wallet_id,@cashback,CONVERT(DATE, GETDATE()))
    
END

--end of 2.4 m

--start of 2.4 n
GO 
CREATE PROCEDURE Initiate_balance_payment
@MobileNo char(11),
@amount decimal(10,1),
@payment_method varchar(50)
AS
BEGIN 
    Insert Into payment values (@amount,CONVERT(DATE,GETDATE()),@payment_method,'successful',@MobileNo)
    
    UPDATE Wallet 
    Set current_balance = current_balance + @amount
    WHERE Wallet.mobileNo  =@MobileNo

END

--end of 2.4 n

--start of 2.4 o
GO
CREATE PROCEDURE Redeem_voucher_points
@MobileNo char(11),
@voucher_id int
AS
BEGIN
DECLARE @addPoints int;
DECLARE @oldPoints int;
DECLARE @newPoints int;

SELECT @addPoints = v.value FROM Voucher v WHERE v.voucherID = @voucher_id
SELECT @oldPoints = point FROM Customer_Account WHERE mobileNo = @MobileNo
SET @newPoints = @oldPoints + @addPoints

UPDATE Customer_Account SET point = @newPoints WHERE mobileNo = @MobileNo
UPDATE Voucher SET redeem_date = CONVERT(DATE,GETDATE()) 
WHERE voucherID = @voucher_id


END
--end of 2.4 o


-- 2.4 Functions --

-- 2.4 a) [Start]
GO
CREATE FUNCTION [AccountLoginValidation](
    @MobileNo CHAR(11),
    @password VARCHAR(50)
)
RETURNS BIT
AS
BEGIN
    DECLARE @Success BIT;
   
    IF EXISTS (SELECT * FROM Customer_Account WHERE mobileNo = @MobileNo AND pass = @password)
        BEGIN 
            SET @Success = 1;
        END
    ELSE 
        BEGIN 
            SET @Success = 0;
        END

    RETURN @Success;
END;
-- 2.4 a) [END]

-- 2.4 b) [Start]
GO
CREATE FUNCTION [Consumption](
    @Plan_name varchar(50),
    @start_date date,
    @end_date date
)
RETURNS TABLE
AS
RETURN 
(
    SELECT 
        SUM(pu.data_consumption) AS [Data consumption],
        SUM(pu.minutes_used) AS [Minutes Used],
        SUM(pu.SMS_sent) AS [SMS sent]
    FROM 
        Plan_Usage pu JOIN Service_Plan sp ON pu.PlanID =sp.planID
    WHERE 
        sp.name = @Plan_name AND pu.start_date BETWEEN @start_date AND @end_date And pu.end_date <= @end_date
);
-- 2.4 b) END 

-- 2.4 c) NOT a function question  

-- 2.4 d) [START]
GO 
CREATE FUNCTION [Usage_Plan_CurrentMonth](
    @MobileNo char(11)
)
RETURNS TABLE 
AS 
RETURN
(
    SELECT 
        pu.data_consumption,
        pu.minutes_used,
        pu.SMS_sent
    FROM 
        Plan_Usage pu
        INNER JOIN Customer_Account c ON pu.mobileNo = c.mobileNo
        INNER JOIN Subscription s ON pu.mobileNo = s.mobileNo
    WHERE
        pu.mobileNo = @MobileNo
        AND s.status = 'active'
        AND MONTH(pu.start_date) = MONTH(Getdate())
        AND YEAR(pu.start_date) = YEAR(Getdate())
);
GO 
-- 2.4 d) END



-- 2.4 e) [Start]
GO 
CREATE FUNCTION [Cashback_Wallet_Customer](
    @NationalID int
)
Returns TABLE 
AS 
RETURN
(
    SELECT 
        c.CashbackID,
        c.amount AS [Cashback Amount],
        c.credit_date AS [Credit Date],
        w.current_balance AS [Wallet Balance],
        w.currency AS [Currency]
    FROM 
        Cashback c 
        JOIN Wallet w ON c.walletID = w.walletID
    WHERE 
        w.nationalID = @NationalID
);
-- 2.4 e) [END]




--start of 2.4f
--end of 2.4f

--start of 2.4g
--end of 2.4g

--start of 2.4h
GO 
CREATE FUNCTION [Remaining_plan_amount](
    @MobileNo char(11),
    @plan_name varchar(50)
)
RETURNS DECIMAL 
AS
BEGIN
    DECLARE @plan_price DECIMAL(10,1);
    DECLARE @payment_amount DECIMAL(10,1);
    DECLARE @remaining_amount Decimal(10,1);

    SELECT @plan_price = price 
    FROM Service_Plan
    WHERE name = @plan_name;

    SET @payment_amount = 0;
    SELECT @payment_amount = SUM (amount)
    FROM Payment 
    WHERE mobileNO = @MobileNO;
    
    SET @remaining_amount = @plan_price-@payment_amount;

    RETURN @remaining_amount;
END;
GO
--end of 2.4h

--start of 2.4i
GO
CREATE FUNCTION Extra_plan_amount
(@MobileNo char(11), @plan_name varchar(50))
RETURNS DECIMAL
AS
BEGIN
DECLARE @extra_amount DECIMAL

SELECT @extra_amount = pp.extra_amount 
FROM Subscription s INNER JOIN Service_Plan sp ON s.planID = sp.planID
INNER JOIN Process_Payment pp ON s.planID = pp.planID 
WHERE s.mobileNo = @MobileNo AND sp.name = @plan_name 

RETURN @extra_amount
END
GO
--end of 2.4i

--start of 2.4j
--end of 2.4j

--start of 2.4k
go
create function Subscribed_plans_5_Months
(@MobileNo char(11))
returns table
as

return
(
select serv_plan.planID,serv_plan.data_offered, serv_plan.description, serv_plan.minutes_offered, serv_plan.name,  serv_plan.price, serv_plan.SMS_offered 
from Service_Plan serv_plan inner join Subscription sub 
on serv_plan.planID=sub.planID and sub.mobileNo=@MobileNo and sub.subscription_date>= DATEADD(MONTH, -5, CURRENT_TIMESTAMP)
)
go
--end of 2.4k




	




