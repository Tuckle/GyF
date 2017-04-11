set serveroutput on;

create or replace package resourcePack is
  type resIdType is table of number index by pls_integer;
  
  rname_exists exception;
  rid_not_found exception;
  pragma exception_init(rname_exists, -21356);
  pragma exception_init(rid_not_found, -21357);

  function updateResource(resId number, rname varchar, description varchar, cost number) return varchar;
  function recommendResources(company_id number) return resIdType;
end resourcePack;
/

create or replace package body resourcePack is
  function updateResource(resId number, rname varchar, description varchar, cost number) return varchar as
    ret_msg varchar(255);
    cnt integer;
  begin
    select nvl(count(*), 0) into cnt from resources where resources.resourceId=resId;
    if(cnt < 1) then
      raise resourcePack.rid_not_found;
    end if;
    
    update resources set
    resources.name = rname,
    resources.description = description,
    resources.cost = cost
    where resources.resourceId=resId;
    ret_msg := 'OK';
    return ret_msg;
  
  exception 
  when resourcePack.rid_not_found then
    ret_msg := 'Resource id not found';
    return ret_msg;
  when no_data_found then
    ret_msg := 'No data found';
    return ret_msg;
  end;

  function recommendResources(company_id number) return resIdType is
    resIdType id_list;
  begin
    
    
    return id_list;
  
  exception
  when no_data_found then
    return null;
  end;

end resourcePack;
/