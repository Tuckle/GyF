create or replace trigger triggerOnCompanyDelete 
after delete
on companies
for each row
begin
  delete from resources where companyid = :OLD.companyid;
  delete from requests where companyid = :OLD.companyid;
  delete from services where companyid = :OLD.companyid;
  delete from owners where companyid = :OLD.companyid;
end;
/

create or replace trigger triggerOnUserDelete
after delete
on users_
for each row
begin 
  delete from companies where companyid in (select companyid from owners where userid = :old.userid);
  delete from owners where userid = :old.userid;
end;
/

create or replace trigger triggerOnResourceDelete
after delete
on resources
for each row
begin
  delete from requests where resourceid = :old.resourceid;
end;
/

create or replace trigger triggerOnServiceDelete
after delete
on services
for each row
begin 
  delete from services where serviceid = :old.serviceid;
end;
/

