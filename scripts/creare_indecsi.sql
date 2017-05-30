drop index resourcesCompanyIndex;
drop index servicesCompanyIndex;
drop index requestsCompanyIndex;
--drop index ownersCompanyIndex;
drop index ownersUserIdIndex;
drop index companiesCompanyIndex;
drop index user_UserIdIndex;
drop index ownersJoinIndex;
--drop index ownersSecondJoinIndex;
drop index orderByCostIndex;

create index resourcesCompanyIndex on resources(companyid); -- checked in getStats (join)
create index servicesCompanyIndex on services(companyid); -- checked in getStats
create index requestsCompanyIndex on requests(companyid); -- checked in getStats
--create index ownersCompanyIndex on owners(companyid); --not implemented yet, for checking if a company has more than one owner
create index ownersUserIdIndex on owners(userid); -- checked in getStats (select simplu)
create index companiesCompanyIndex on companies(companyid); -- already indexed by system
create index user_UserIdIndex on users_(userid); -- already indexed by system
create index ownersJoinIndex on owners (userid, companyid); -- already indexed by system
--create index ownersSecondJoinIndex on owners (companyid, userid); -- useless, our mistake
create index orderByCostIndex on resources(cost); -- custom sort (order by)

