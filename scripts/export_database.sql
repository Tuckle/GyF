set serveroutput on;

CREATE OR REPLACE DIRECTORY CTEST AS 'C:\Faculty\sgbd';
GRANT READ ON DIRECTORY CTEST TO PUBLIC; 
GRANT EXECUTE ON SYS.utl_file TO PUBLIC;

declare
  cursor customCursor is select dbms_metadata.GET_DDL(u.object_type,u.object_name) from user_objects u where 
--  u.object_type in ('TABLE', 'VIEW', 'TRIGGER', 'PROCEDURE', 'FUNCTION') and 
  lower(u.object_name) in ('companies', 'resources', 'services', 'views', 'owners', 'requests', 'users_', 'certificates', 'orderbycostindex', 'ownerscompanyindex',
  'ownerssecondjoinindex', 'ownersuseridindex', 'requestscompanyindex', 'resourcescompanyindex', 'servicescompanyindex', 
--  'resourcepack', 'user_manager', 
  'syncronizestuff', 
  'cellphonetrigger', 'certificatesbi', 'companiesbi', 'requestsbi', 'resourcemvinsert', 'resourcemvtrigger', 'resourcesbi', 'servicesbi', 'triggeroncompanydelete', 
  'triggeronresourcedelete', 'triggeronservicedelete', 'triggeronuserdelete', 'usersbi', 'certificatessequence', 'companiessequence', 'requestssequence', 'resourcessequence', 
  'servicessequence', 'userssequence', 
--  'resourcesmaterializedview', 
  'resourcelogtable')
  order by u.object_type;
  cursor getTables is select object_name from user_objects where object_type = 'TABLE' and lower(object_name) in ('companies', 'resources', 'services', 'views', 'owners', 'requests', 'users_', 'certificates', 'resourcelogtable');
  tempService varchar2(2000);
  ignore integer;
  cursorid integer;
  tempSelectStatement varchar2(2000);
  tempSelectCursor integer;
  columnType varchar2(300);
  columnName varchar2(300);
  out_File UTL_FILE.FILE_TYPE;
  finalSelectStatement varchar2(5000);
  finalSelectCursor integer;
  
  toOutputString varchar2(5000);
  toOutputCursor integer;
  
  TYPE CustomArray IS TABLE OF VARCHAR2(300) INDEX BY pls_integer;
  typeArray CustomArray;
  columnNameArray CustomArray;
  counter integer;
  step_counter integer;
begin
  out_File := UTL_FILE.FOPEN('CTEST', 'script_test.sql' , 'W');
  OPEN customCursor;
    LOOP
        FETCH customCursor INTO tempService;
        EXIT WHEN customCursor%NOTFOUND;
          UTL_FILE.PUT_LINE(out_File, tempService);
          UTL_FILE.PUT_LINE(out_File, '/');
    END LOOP;
  CLOSE customCursor;  
    OPEN getTables;
    LOOP
      FETCH getTables INTO tempService;
      EXIT WHEN getTables%NOTFOUND;
        counter := 0;
        tempSelectStatement := 'select data_type, column_name from user_tab_columns where table_name='''|| tempService || '''';
        tempSelectCursor := dbms_sql.open_cursor;
        dbms_sql.parse(tempSelectCursor, tempSelectStatement, dbms_sql.native);
        dbms_sql.define_column(tempSelectCursor, 1, columnType, 300);
        dbms_sql.define_column(tempSelectCursor, 2, columnName, 300);
        ignore := dbms_sql.execute(tempSelectCursor);
        Loop
          if dbms_sql.fetch_rows(tempSelectCursor) > 0 then
            dbms_sql.column_value(tempSelectCursor, 1, columnType);
            dbms_sql.column_value(tempSelectCursor, 2, columnName);
            typeArray(counter) := columnType;
            columnNameArray(counter) := columnName;
            counter := counter + 1;
          else
            exit;
          end if;
        end loop;
        finalSelectStatement := 'select ''insert into '|| tempService ||' values (''';
        step_counter := 0;
        while step_counter < counter loop
          if (typeArray(step_counter) = 'VARCHAR2') then
            if (step_counter != 0) then
              finalSelectStatement := finalSelectStatement || ''',''''''||' || columnNameArray(step_counter) || '||'''''; 
            else
              finalSelectStatement := finalSelectStatement || '''''||' || columnNameArray(step_counter) || '||'''''; 
            end if;
          else
            if (step_counter != 0) then
              finalSelectStatement := finalSelectStatement || ''',''||' || columnNameArray(step_counter) || '||';
            else
              finalSelectStatement := finalSelectStatement || '||' || columnNameArray(step_counter) || '||';
            end if;
          end if;
          step_counter := step_counter + 1;
        end loop;
        finalSelectStatement := finalSelectStatement || ''');'' from ' || tempService;
        dbms_output.put_line(finalSelectStatement);
        toOutputCursor := dbms_sql.open_cursor;
        dbms_sql.parse(toOutputCursor, finalSelectStatement, dbms_sql.native);
        dbms_sql.define_column(toOutputCursor, 1, toOutputString, 5000);
        ignore := dbms_sql.execute(toOutputCursor);
        Loop
          if dbms_sql.fetch_rows(toOutputCursor) > 0 then
            dbms_sql.column_value(toOutputCursor, 1, toOutputString);
            UTL_FILE.PUT_LINE(out_File, toOutputString);
          else
            exit;
          end if;
        end loop;
        dbms_sql.close_cursor(toOutputCursor);        
    end loop;
    close getTables;
    UTL_FILE.FCLOSE(out_file);
end;
