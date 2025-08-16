create schema if not exists ynaer authorization "ynaer-migrator";

revoke all on schema ynaer from public;

grant usage on schema ynaer to "ynaer-api";

alter default privileges in schema ynaer
grant select, insert, update, delete on tables to "ynaer-api";

alter default privileges in schema ynaer
grant usage, select on sequences to "ynaer-api";
