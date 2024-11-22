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