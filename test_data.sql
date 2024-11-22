USE Telecom_Team_32;
--start 2.3D test data
Insert INTO Customer_Profile VALUES (1234,'fady','bassem','fady@gmail.com','dokki','2005-2-8');
 Insert INTO Customer_Account VALUES ('01234567890','pass',2000.0,'prepaid','2024-11-9','active',0,1234);
 Insert INTO Customer_Account VALUES ('01234567891','pass',3000.0,'prepaid','2024-11-10','active',0,1234);
 Insert INTO benefits VALUES ('test desc1','2027-1-1','active','01234567890');
 Insert INTO benefits VALUES ('test desc2','2027-1-2','active','01234567890');
 Insert INTO benefits VALUES ('test desc3','2027-1-3','active','01234567891');
 Insert INTO Service_Plan VALUES (5,5,5,'koko',5,'varchar');
 Insert INTO Service_Plan VALUES (6,6,6,'koko',6,'varchar');
 Insert INTO plan_provides_benefits VALUES(1,1);
 Insert INTO plan_provides_benefits VALUES(1,2);
 Insert INTO plan_provides_benefits VALUES(3,1);
 Insert INTO plan_provides_benefits VALUES(2,1);
 Insert INTO plan_provides_benefits VALUES(2,2);
 Insert INTO plan_provides_benefits VALUES(3,2);
 select * from Customer_Profile
 select * from Customer_Account
 select * from benefits
 select * from Service_Plan
 select * from plan_provides_benefits;
 EXEC dbo.Benefits_Account '01234567890', 1;
 select * from Customer_Profile
 select * from Customer_Account
 select * from benefits
 select * from Service_Plan
 select * from plan_provides_benefits;
 EXEC clearAllTables;
 --end 2.3 D 

 --start 2.3F
 Insert INTO Customer_Profile VALUES (1234,'fady','bassem','fady@gmail.com','dokki','2005-2-8');
 Insert INTO Customer_Account VALUES ('01234567890','pass',2000.0,'prepaid','2024-11-9','active',0,1234);
 Insert INTO Customer_Account VALUES ('01234567891','pass',3000.0,'prepaid','2024-11-10','active',0,1234);
 
 INSERT INTO Payment (amount, date_of_payment, payment_method, status, mobileNo)
VALUES
(100.0, DATEADD(DAY, -30, GETDATE()), 'Credit', 'successful', '01234567890'),
(150.5, DATEADD(DAY, -200, GETDATE()), 'cash', 'successful', '01234567890'),
(50.0, DATEADD(DAY, -60, GETDATE()), 'cash', 'successful', '01234567891'),
(200.0, '2022-05-15', 'Credit', 'successful', '01234567890'),
(300.0, '2021-11-20', 'cash', 'successful', '01234567890'),
(75.5, '2020-09-10', 'credit', 'successful', '01234567891');
Insert INTO benefits VALUES ('test desc1','2027-1-1','active','01234567890');
 Insert INTO benefits VALUES ('test desc2','2027-1-2','active','01234567890');
 Insert INTO benefits VALUES ('test desc3','2027-1-3','active','01234567891');
INSERT INTO Points_Group 
VALUES 
(1,160,1),
(2, 100, 2),  
(3, 75, 3),   
(1, 120, 4),  
(2, 90, 5),   
(3, 30, 6);
select * FROM Payment;
select * from Points_Group;
select * from benefits;
DECLARE @totalPoints INT;
DECLARE @totalTransactions INT;
exec Account_Payment_Points '01234567890',@totalTransactions OUTPUT,@totalPoints OUTPUT;
print @totalTransactions;
print @totalPoints;



 --end 2.3 f

