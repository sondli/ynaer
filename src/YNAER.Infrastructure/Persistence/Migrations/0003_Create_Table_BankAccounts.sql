start transaction;

set search_path to ynaer;

create table "BankAccounts"
(
    "Id"        uuid      not null,
    "UserId"    uuid      not null,
    "Name"      character varying(256),
    "CreatedOn" timestamp not null,
    "UpdatedOn" timestamp,
    constraint "PK_BankAccounts" primary key ("Id"),
    constraint "FK_BankAccounts_AspNetUsers" foreign key ("UserId") references "AspNetUsers" ("Id")
        on delete cascade
);

create index "IX_BankAccounts_UserId" on "BankAccounts" ("UserId");

commit;



