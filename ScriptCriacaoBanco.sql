create database pocFramework
go
use pocFramework
go
create table pessoas (
	id int identity primary key,
	nome varchar(55) not null
)
go