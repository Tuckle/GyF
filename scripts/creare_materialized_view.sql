drop materialized view resourcesMaterializedView;
create materialized view resourcesMaterializedView build immediate refresh force on demand for update as select * from resources;
/
drop table ResourceLogTable;
create table ResourceLogTable(dateOfModif timeStamp,executionString varchar2(3000),stateI varchar2(300));
/
create or replace trigger resourceMVInsert 
before insert on resourcesMaterializedView
for each row
BEGIN SELECT resourcesSequence.NEXTVAL INTO :new.resourceId FROM dual; END;
/
create or replace trigger resourceMVTrigger 
after
  INSERT OR
  UPDATE OR
  DELETE
on resourcesMaterializedView
for each row
declare
  executionState varchar2(3000);
begin
  case 
    when inserting then
--      dbms_output.put_line('Inserting');
      executionState := 'insert into resources values(' || :NEW.resourceid ||','||:NEW.companyid||', ''' ||  :NEW.name || ''','''||:NEW.description || ''',' || :new.cost || ')';
--      dbms_output.put_line('Statement: '|| executionState);
      insert into ResourceLogTable values (CURRENT_TIMESTAMP, executionState, 'inserting');
    when updating then
--      dbms_output.put_line('Updating');
      executionState := 'update resources set resourceid = '|| :NEW.resourceid || ', companyid = '||:NEW.companyid||', name = ''' ||  :NEW.name|| ''', description = '''||:NEW.description || ''', cost = ' || :new.cost || ' where resourceid = ' || :OLD.resourceid || ' and companyid = ' ||  :OLD.companyid || ' and name = '''||:OLD.name || ''' and description = ''' || :OLD.description || ''' and cost = ' || :OLD.cost;
--      dbms_output.put_line('Statement: '|| executionState);
      insert into ResourceLogTable values (CURRENT_TIMESTAMP, executionState, 'updating');
    when deleting then
--      dbms_output.put_line('Deleting');
      executionState := 'delete from resources where resourceid = ' || :OLD.resourceid || ' and companyid = ' ||  :OLD.companyid || ' and name = '''||:OLD.name || ''' and description = ''' || :OLD.description || ''' and cost = ' || :OLD.cost;
--      dbms_output.put_line('Statement: '|| executionState);
      insert into ResourceLogTable values (CURRENT_TIMESTAMP, executionState, 'deleting');
  end case;
end;
/

create or replace procedure syncronizeStuff as
pCursor integer;
ignore integer;
rowType ResourceLogTable.executionstring%type;
cursor syncDB is select executionstring from ResourceLogTable order by dateofmodif;
BEGIN
  open syncDB;
  loop
    fetch syncDB into rowType;
    exit when syncDB%NOTFOUND;
    pCursor := dbms_sql.open_cursor;
    DBMS_OUTPUT.PUT_LINE(rowType);
    dbms_sql.parse(pCursor, rowType, dbms_sql.native);
    ignore := dbms_sql.execute(pCursor);
    dbms_sql.close_cursor(pCursor);
  end loop;
  DBMS_MVIEW.refresh('resourcesMaterializedView');
  delete from ResourceLogTable;
END;
/