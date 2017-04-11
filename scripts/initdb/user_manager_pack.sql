set serveroutput on;

create or replace package user_manager as
  user_not_found exception;
  email_exists exception;
  pragma exception_init(user_not_found, -20002);
  pragma exception_init(email_exists, -20001);
  
  function emailExists(email_ varchar2) return integer;
  procedure addUser(firstName varchar, lastName varchar,
  email varchar, cellphone varchar, passwd varchar);
  function updateUser(user_id number, first_name varchar, last_name varchar,
  email varchar, phone_nr varchar, passwd varchar) return varchar;
  function removeUser(user_id number) return varchar;
  function login(email varchar, passwd varchar) return integer;
  function getUserId(email varchar) return number;
end user_manager;
/
create or replace package body user_manager as
  function emailExists(email_ varchar2) return integer as
    cnt integer := 0;
  begin
    cnt := 0;
    select count(*) into cnt from users_ u where u.email=email_;
    if (cnt >= 1) then
      cnt := 1;
    end if;
    return cnt;
  end;
  
  function getUserId(email varchar) return number is
    user_id number(10, 0) := 0;
    cnt integer;
  begin
    if length(email) = 0 then
      return 0;
    end if;
    select nvl(count(*), 0) into cnt from users_ where users_.email = email;
    if(cnt = 0) then
      return 0;
    end if;
    select userid into user_id from users_ where email=users_.email;
    return user_id;
  end;
  
  procedure addUser(firstName varchar, lastName varchar,
  email varchar, cellphone varchar, passwd varchar) as
  email_exists exception;
  pragma exception_init(email_exists, -20001);
    ret_msg varchar(255);
    v_temp integer;
  begin
    v_temp := emailExists(email);
    if(v_temp >= 1) then
--      raise user_manager.email_exists;
        raise_application_error(-20001, 'Email already exists');
    end if;
    
    INSERT into users_ (users_.firstName, USERS_.LASTNAME, users_.email,
    USERS_.CELLPHONE, USERS_.PASSWORD) values (firstname, lastname,
    email, cellphone, passwd);
    ret_msg := 'OK';
--    return ret_msg;
--  Exception 
--  exception
--  when user_manager.email_exists then
--    ret_msg := 'Email already exists';
--    return ret_msg;
  end;
  
  function updateUser(user_id number, first_name varchar, last_name varchar,
  email varchar, phone_nr varchar, passwd varchar) return varchar Is 
  ret_msg varchar(255);
    cnt integer;
  begin
    select nvl(count(*), 0) into cnt from users_ where user_id=users_.userid;
    if(cnt = 0) then
      raise user_manager.user_not_found;
    end if;
    
    cnt := user_manager.emailExists(email);
    if(cnt >= 1) then
      raise user_manager.email_exists;
    end if;
    
    update users_ set
    users_.firstname = first_name,
    users_.lastname = last_name,
    users_.email = email,
    users_.cellphone = phone_nr,
    users_.password = passwd
    where users_.userid=user_id;
    
    ret_msg := 'OK';
    return ret_msg;
  exception 
    when user_manager.user_not_found then
      ret_msg := 'User not found';    
    return ret_msg;
    when user_manager.email_exists then
      ret_msg := 'Email already exists';
    return ret_msg;
  end;
  
  function removeUser(user_id number) return varchar as 
    ret_msg varchar(255);
    cnt integer;
  begin
    select nvl(count(*), 0) into cnt from users_ where user_id=users_.userid;
    if(cnt < 1) then
      raise user_manager.user_not_found;
    end if;
    delete from users_ where users_.userid=user_id;
    ret_msg := 'OK';
    return ret_msg;
  exception
  when user_manager.user_not_found then
    ret_msg := 'User not found';
    return ret_msg;
  end;
  
  function login(email varchar, passwd varchar) return integer is
    cnt integer := 0;
  begin
    select nvl(count(*), 0) into cnt from users_ where email=users_.email;
    if(cnt >= 1) then
      cnt := 1;
    end if;
    return 0;
  end;
end user_manager;
/

BEGIN
  DBMS_OUTPUT.PUT_LINE('AA' || user_manager.emailExists('gheorghe.balan@yahoo.com'));
END;