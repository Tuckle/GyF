set serveroutput on;

create or replace package user_manager ass
  function emailExists(email varchar) return integer;
  function addUser(firstName varchar, lastName varchar,
  email varchar, cellphone varchar, passwd varchar) return integer;
  function updateUser(user_id number, first_name varchar, last_name varchar,
  email varchar, phone_nr varchar, passwd varchar) return integer;
  function removeUser(user_id number) return integer;
  function login(email varchar, passwd varchar) return integer;
  function getUserId(email varchar) return number;
end user_manager;
/
create or replace package body user_manager as
  function emailExists(email varchar) return integer as
    cnt integer := 0;
  begin
    select nvl(count(*), 0) into cnt from users_ where users_.email=email;
    if (cnt >= 1) then
      cnt := 1;
    end if;
    return cnt;
  end;
  
  function getUserId(email varchar) return number is
    user_id number(10, 0) := 0;
    cnt integer;
  begin
    select nvl(count(*), 0) into cnt from users_ where users_.email = email;
    if(cnt = 0) then
      return 0;
    end if;
    select userid into user_id from users_ where email=users_.email;
    return user_id;
  end;
  
  function addUser(firstName varchar, lastName varchar,
  email varchar, cellphone varchar, passwd varchar) return integer as 
    v_temp integer;
  begin
    v_temp := emailExists(email);
    if(v_temp = 0) then
      return 0;
    end if;
    
    INSERT into users_ (users_.firstName, USERS_.LASTNAME, users_.email,
    USERS_.CELLPHONE, USERS_.PASSWORD) values (firstname, lastname,
    email, cellphone, passwd);
    return 1;
--  Exception 
  end;
  
  function updateUser(user_id number, first_name varchar, last_name varchar,
  email varchar, phone_nr varchar, passwd varchar) return integer Is 
    cnt integer;
  begin
    select nvl(count(*), 0) into cnt from users_ where user_id=users_.userid;
    if(cnt = 0) then
      return 0;
    end if;
    
    cnt := user_manager.emailExists(email);
    if(cnt >= 1) then
      return -1;
    end if;
    
    update users_ set
    users_.firstname = first_name,
    users_.lastname = last_name,
    users_.email = email,
    users_.cellphone = phone_nr,
    users_.password = passwd
    where users_.userid=user_id;
    
    return 1;
  end;
  
  function removeUser(user_id number) return integer as 
    cnt integer;
  begin
    select nvl(count(*), 0) into cnt from users_ where user_id=users_.userid;
    if(cnt < 1) then
      return 0;
    end if;
    delete from users_ where users_.userid=user_id;
    return 1;
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