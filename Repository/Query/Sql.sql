drop table if exists t_account;
drop table if exists t_money;
drop table if exists t_stats;

create table t_account
(
    id              bigint auto_increment
        primary key,
    email           varchar(50) charset utf8mb3        not null,
    password        varchar(250) charset utf8mb3       not null,
    last_login_time datetime default (now())           null,
    create_time     datetime default CURRENT_TIMESTAMP not null,
    constraint t_account_pk
        unique (email)
);

create table t_money
(
    id          bigint                             not null
        primary key,
    aid         bigint                             not null,
    money_type  tinyint                            null,
    value       bigint   default 0                 not null,
    update_time datetime default CURRENT_TIMESTAMP null on update CURRENT_TIMESTAMP
);

create index t_money_aid_index
    on t_money (aid);

create table t_stats
(
    id          bigint                             not null
        primary key,
    aid         bigint                             not null,
    stat_type   tinyint                            null,
    level       int      default 1                 not null,
    update_time datetime default CURRENT_TIMESTAMP null on update CURRENT_TIMESTAMP
);

create index t_stats_aid_index
    on t_stats (aid);

