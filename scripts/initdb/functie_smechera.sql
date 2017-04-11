create or replace procedure getUserRanking(user_id number, rank_ OUT number, rank_nr OUT number, total OUT number) as
  comp_nr integer;  
  res_nr integer;
  req_nr integer;
  ser_nr integer;
  temp_comp_nr integer;
  temp_res_nr integer;
  temp_req_nr integer;
  temp_ser_nr integer;
  compute_rank integer;
  
  cursor uzrs is
  select * from users_ where users_.userid<>user_id;
begin
  select count(*) into total from users_;
  if total > 0 then
    total := total - 1;
  end if;
  rank_nr := total;
  
  
  select count(*) into comp_nr from owners where owners.userid=user_id;
  select count(*) into res_nr from resources re join owners o on o.COMPANYID=re.COMPANYID
  where o.userid=user_id;
  select count(*) into req_nr from requests re join owners o on o.COMPANYID=re.COMPANYID
  where o.userid=user_id;
  select count(*) into ser_nr from services se join owners o on o.COMPANYID=se.COMPANYID
  where o.userid=user_id;
  req_nr := req_nr + 1;
  rank_ := comp_nr*0.1 + (res_nr + ser_nr)/req_nr * 10;
  select count(*) into temp_res_nr from resources;
  res_nr := round(res_nr/temp_res_nr);
  select count(*) into temp_ser_nr from services;
  ser_nr := round(ser_nr/temp_ser_nr);
  select count(*) into temp_req_nr from requests;
  req_nr := round(req_nr/temp_req_nr);
  
  for uz in uzrs loop
    select count(*) into temp_comp_nr from owners where owners.userid=user_id;
    select count(*) into temp_res_nr from resources re join owners o on o.COMPANYID=re.COMPANYID
    where o.userid=user_id;
    select count(*) into temp_req_nr from requests re join owners o on o.COMPANYID=re.COMPANYID
    where o.userid=user_id;
    select count(*) into temp_ser_nr from services se join owners o on o.COMPANYID=se.COMPANYID
    where o.userid=user_id;
    temp_req_nr := temp_req_nr + 1;
    compute_rank := temp_comp_nr*0.1 + (temp_res_nr + temp_ser_nr)/temp_req_nr * 10;
    if rank_ > compute_rank then
      rank_nr := rank_nr - 1;
    end if;
    rank_ := round((comp_nr * 0.1 + res_nr * 3 + ser_nr * 5 + req_nr * 0.1) * 100);
  end loop;
  
  
end;