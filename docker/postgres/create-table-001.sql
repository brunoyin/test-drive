create schema yin;

create yin.college(
    id varchar(12) not null primary key,
    name varchar(128) not null,
    city varchar(36) not null,
    state char(2) not null,
    zip varchar(16) not null,
    region int,
    latitude numeric,
    longitude numeric,
    adm_rate numeric,
    sat_avg numeric,
    act_avg numeric,
    earnings numeric,
    cost numeric,
    enrollments numeric
);

