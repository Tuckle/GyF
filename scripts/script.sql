drop table Users_;
drop table Companies;
drop table Owners;
drop table Services;
drop table Resources;
drop table Certificates;
drop table Requests;

create table Users_ (
userId number(10) primary key not null,  
firstName varchar2(50) not null, 
lastName varchar2(50) not null, 
email varchar2(100) not null, 
cellphone varchar2(25), 
password varchar2(32) not null
);

create table Companies (
companyId number(10) primary key not null, 
name varchar2(100) not null, 
description varchar2(3000),
location varchar2(100) not null, 
website varchar2(100),
certificateId number(10) not null
);

create table Owners (
userId number(10) not null, 
companyId number(10) not null, 
primary key (userId, companyId)
);

create table Services (
serviceId number(10) primary key not null, 
companyId number(10) not null,
name varchar2(100) not null, 
description varchar2(3000)
);

create table Resources (
resourceId number(10) primary key not null, 
companyId number(10) not null, 
name varchar2(100) not null, 
description varchar2(3000) not null,
cost number(10) not null
);

create table Requests(
requestId number(10) not null primary key, 
serviceId number(10) not null,
resourceId number(10) not null,
companyId number(10) not null, 
status varchar2(10) not null
);

create table Certificates(
certificateId number(10) not null primary key,
userId number(10) not null,
companyId number(10) not null,
authority varchar2(100) not null,
verified NUMBER(1) not null
);

DROP SEQUENCE usersSequence;
DROP SEQUENCE companiesSequence;
DROP SEQUENCE servicesSequence;
DROP SEQUENCE requestsSequence;
DROP SEQUENCE resourcesSequence;
DROP SEQUENCE certificatesSequence;

CREATE SEQUENCE usersSequence START WITH 1;
CREATE SEQUENCE companiesSequence START WITH 1;
CREATE SEQUENCE servicesSequence START WITH 1;
CREATE SEQUENCE requestsSequence START WITH 1;
CREATE SEQUENCE resourcesSequence START WITH 1;
CREATE SEQUENCE certificatesSequence START WITH 1;

CREATE OR REPLACE TRIGGER usersBI BEFORE INSERT ON Users_ FOR EACH ROW BEGIN SELECT usersSequence.NEXTVAL INTO :new.userId FROM dual; END; 
/
CREATE OR REPLACE TRIGGER companiesBI BEFORE INSERT ON Companies FOR EACH ROW BEGIN SELECT companiesSequence.NEXTVAL INTO :new.companyId FROM dual; END;
/
CREATE OR REPLACE TRIGGER servicesBI BEFORE INSERT ON Services FOR EACH ROW BEGIN SELECT servicesSequence.NEXTVAL INTO :new.serviceId FROM dual; END;
/
CREATE OR REPLACE TRIGGER resourcesBI BEFORE INSERT ON Resources FOR EACH ROW BEGIN SELECT resourcesSequence.NEXTVAL INTO :new.resourceId FROM dual; END;
/
CREATE OR REPLACE TRIGGER requestsBI BEFORE INSERT ON Requests FOR EACH ROW BEGIN SELECT requestsSequence.NEXTVAL INTO :new.requestId FROM dual; END;
/
CREATE OR REPLACE TRIGGER certificatesBI BEFORE INSERT ON Certificates FOR EACH ROW BEGIN SELECT certificatesSequence.NEXTVAL INTO :new.certificateId FROM dual; END;
/

DECLARE
  CURSOR userList IS SELECT name, username from users;
  v_std_name users.name%TYPE;
  v_std_username users.username%TYPE;
BEGIN
  OPEN userList;
  LOOP
    FETCH userList into v_std_name, v_std_username;
    INSERT INTO Users_ (FIRSTNAME, LASTNAME, EMAIL, PASSWORD) VALUES (v_std_name, v_std_username, v_std_username||'@yahoo.com', 'gigica');
    EXIT WHEN userList%NOTFOUND;
  END LOOP;
END;
/

INSERT INTO COMPANIES (name, description, location, website, certificateId) VALUES ('Bitdefender', 'Security Stuff', 'Iasi', 'bitdefender.com', 1);
INSERT INTO COMPANIES (name, description, location, website, certificateId) VALUES ('Bitdefender', 'Security Stuff', 'Bucuresti', 'bitdefender.com', 1);
INSERT INTO COMPANIES (name, description, location, website, certificateId) VALUES ('Amazon', 'Web Stuff', 'Iasi', 'amazon.com', 2);
INSERT INTO COMPANIES (name, description, location, website, certificateId) VALUES ('Continental', 'Cars Stuff', 'Iasi', 'continental.com', 3);
INSERT INTO COMPANIES (name, description, location, website, certificateId) VALUES ('Endava', 'Java Stuff', 'Iasi', 'endava.com', 4);
INSERT INTO COMPANIES (name, description, location, website, certificateId) VALUES ('CECBank', 'Money Stuff', 'Iasi', 'cecbank.com', 5);
INSERT INTO COMPANIES (name, description, location, website, certificateId) VALUES ('BRD', 'Money Stuff', 'Iasi', 'brd.com', 6);

INSERT INTO Owners (userId, companyId) VALUES(324, 3);
INSERT INTO Owners (userId, companyId) VALUES(324, 4);
INSERT INTO Owners (userId, companyId) VALUES(324, 5);
INSERT INTO Owners (userId, companyId) VALUES(324, 6);
INSERT INTO Owners (userId, companyId) VALUES(122, 3);
INSERT INTO Owners (userId, companyId) VALUES(789, 4);
INSERT INTO Owners (userId, companyId) VALUES(312, 5);
INSERT INTO Owners (userId, companyId) VALUES(346, 6);

DECLARE
  CURSOR questionsList IS SELECT question from questions;
  v_std_question questions.question%TYPE;
BEGIN
  OPEN questionsList;
  LOOP
    FETCH questionsList into v_std_question;
    INSERT INTO Services (companyId, name, description) VALUES (DBMS_RANDOM.VALUE(3,6), SUBSTR(v_std_question, 4, 13), v_std_question);
    EXIT WHEN questionsList%NOTFOUND;
  END LOOP;
END;
/

DECLARE
  CURSOR answersList IS SELECT answer from answers;
  v_std_answer answers.answer%TYPE;
BEGIN
  OPEN answersList;
  LOOP
    FETCH answersList into v_std_answer;
    INSERT INTO Resources (companyId, name, description, cost) VALUES (DBMS_RANDOM.VALUE(3,6), SUBSTR(v_std_answer, 4, 13), v_std_answer, DBMS_RANDOM.VALUE(1,1000));
    EXIT WHEN answersList%NOTFOUND;
  END LOOP;
END;
/

DECLARE
  v_max_resourceId resources.resourceId%TYPE;
  v_max_serviceId services.serviceId%TYPE;
BEGIN
  SELECT max(resourceId) into v_max_resourceId from Resources;
  SELECT max(serviceId) into v_max_serviceId from Services;
  FOR loopCounter in 1..15001
  LOOP
    INSERT INTO Requests(serviceId, resourceId, companyId, status) values (DBMS_RANDOM.value(1, v_max_serviceId), DBMS_RANDOM.value(1, v_max_resourceId), DBMS_RANDOM.value(3, 6), 'N\A');
  END LOOP;
END;
/
INSERT INTO Certificates (userId, companyId, authority, verified) VALUES (0, 1, 'Bitdefender SRL', 1);
INSERT INTO Certificates (userId, companyId, authority, verified) VALUES (0, 2, 'Amazon SRL', 1);
INSERT INTO Certificates (userId, companyId, authority, verified) VALUES (0, 3, 'Continental', 1);
INSERT INTO Certificates (userId, companyId, authority, verified) VALUES (0, 4, 'Endava', 1);
INSERT INTO Certificates (userId, companyId, authority, verified) VALUES (0, 5, 'CECBank', 1);
INSERT INTO Certificates (userId, companyId, authority, verified) VALUES (0, 6, 'BCR', 0);

COMMIT;