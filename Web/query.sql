create database GameTime COLLATE Cyrillic_General_CI_AS;
use GameTime;
go
drop table if exists Games;
create table Games
(
	id int primary key identity(1, 1),
	Title nvarchar(200) unique,
	ReleaseDate date,
	Developer nvarchar(200),
	Publisher nvarchar(200),
	[Platform] nvarchar(200)
);
drop table if exists Reviews;
create table Reviews
(
	GameId int,
	UserLogin nvarchar(200),
	[Text] nvarchar(1000),
	Score int,
	PublicationDate date,
	primary key(GameId, UserLogin)
);
drop table if exists TimeRecords;
create table TimeRecords
(
	TimeId int primary key identity(1, 1),
	GameId int,
	UserLogin nvarchar(200),
	[Hours] int,
	[Minutes] int,
	[Type] int,
);
drop table if exists Users;
create table Users
(
	[Login] nvarchar(200) primary key,
	[Password] nvarchar(200)
);

create login [AuthorizedUser] with password = 'UserPassword1'
create user AU for login [AuthorizedUser]
create role [AuthorizedUserRole]
alter role [AuthorizedUserRole] add member AU
deny delete on Reviews to [AuthorizedUserRole];
deny update on Reviews to [AuthorizedUserRole];
deny delete on TimeRecords to [AuthorizedUserRole];
deny update on TimeRecords to [AuthorizedUserRole];
deny insert on Games to [AuthorizedUserRole];
deny delete on Games to [AuthorizedUserRole];
deny update on Games to [AuthorizedUserRole];
deny delete on Users to [AuthorizedUserRole];
deny update on Users to [AuthorizedUserRole];
deny alter to [AuthorizedUserRole];
grant insert on Reviews to [AuthorizedUserRole];
grant insert on TimeRecords to [AuthorizedUserRole];
grant insert on Users to [AuthorizedUserRole];
grant select on Users to [AuthorizedUserRole];
grant select on Games to [AuthorizedUserRole];
grant select on Reviews to [AuthorizedUserRole];
grant select on TimeRecords to [AuthorizedUserRole];

create login [GuestUser] with password = 'GuestPassword1'
create user GU for login [GuestUser]
create role [GuestUserRole]
alter role [GuestUserRole] add member GU
deny delete on Reviews to [GuestUserRole];
deny update on Reviews to [GuestUserRole];
deny delete on TimeRecords to [GuestUserRole];
deny update on TimeRecords to [GuestUserRole];
deny insert on Games to [GuestUserRole];
deny delete on Games to [GuestUserRole];
deny update on Games to [GuestUserRole];
deny delete on Users to [GuestUserRole];
deny update on Users to [GuestUserRole];
deny alter to [GuestUserRole];
deny insert on Reviews to [GuestUserRole];
deny insert on TimeRecords to [GuestUserRole];
deny insert on Users to [GuestUserRole];
grant select on Users to [GuestUserRole];
grant select on Games to [GuestUserRole];
grant select on Reviews to [GuestUserRole];
grant select on TimeRecords to [GuestUserRole];

select * from Games
select * from TimeRecords
select * from Reviews
select * from Users


insert Games values ('Game1', '10-10-2010', 'Developer', 'Publisher', 'plstform')

insert Users values('admin', 'admin')
delete from Games where 0 = 0
delete from TimeRecords where 0 = 0
delete from Reviews where 0 = 0
delete from Users where 0 = 0
go

use master;
declare @distributor as SYSNAME;
declare @distributorLogin as SYSNAME;
declare @distributorPassword as SYSNAME;
declare @server as SYSNAME;

select @server = @@SERVERNAME;

set @distributor = @server;
set @distributorLogin = N'sa';
set @distributorPassword = N'Password_1';

exec sp_adddistributor @distributor = @distributor;

exec sp_adddistributiondb @database = N'distribution'
	,@log_file_size = 2
	,@deletebatchsize_xact = 5000
	,@deletebatchsize_cmd = 2000
	,@security_mode = 0
	,@login = @distributorLogin
	,@password = @distributorPassword;
go

use distribution;
go

declare @snapshotdirectory as NVARCHAR(500);
set @snapshotdirectory = N'/var/opt/mssql/ReplData/';
if (not exists (select * from sysobjects where name = 'UIProperties' and type = 'U'))
	create table UIProperties(id int);
if (exists (select * from::fn_listextendedproperty('SnapshotFolder', 'user', 'dbo', 'table', 'UIProperties', null, null)))
	exec sp_updateextendedproperty N'SnapshotFolder'
	,@snapshotdirectory
	,'user'
	,dbo
	,'table'
	,'UIProperties'
else
	exec sp_addextendedproperty N'SnapshotFolder'
	,@snapshotdirectory
	,'user'
	,dbo
	,'table'
	,'UIProperties'
go

USE distribution;
GO
 
DECLARE @publisher AS SYSNAME;
DECLARE @distributorlogin AS SYSNAME;
DECLARE @distributorpassword AS SYSNAME;
DECLARE @Server SYSNAME;
 
SELECT @Server = @@SERVERNAME;
 
SET @publisher = @Server;
SET @distributorlogin = N'sa';
SET @distributorpassword = N'Password_1';
 
EXEC sp_adddistpublisher @publisher = @publisher
    ,@distribution_db = N'distribution'
    ,@security_mode = 0
    ,@login = @distributorlogin
    ,@password = @distributorpassword
    ,@working_directory = N'/var/opt/mssql/ReplData'
    ,@trusted = N'false'
    ,@thirdparty_flag = 0
    ,@publisher_type = N'MSSQLSERVER';
GO

USE GameTime;
GO
 
DECLARE @replicationdb AS SYSNAME;
DECLARE @publisherlogin AS SYSNAME;
DECLARE @publisherpassword AS SYSNAME;
 
SET @replicationdb = N'GameTime';
SET @publisherlogin = N'sa';
SET @publisherpassword = N'Password_1'

EXEC sp_replicationdboption @dbname = N'GameTime'
    ,@optname = N'publish'
    ,@value = N'true';
 
EXEC sp_addpublication @publication = N'SnapshotRepl'
    ,@description = N'Snapshot publication of database ''GameTime'' from Publisher ''''.'
    ,@retention = 0
    ,@allow_push = N'true'
    ,@repl_freq = N'snapshot'
    ,@status = N'active'
    ,@independent_agent = N'true';
 

EXEC sp_addpublication_snapshot @publication = N'SnapshotRepl'
    ,@frequency_type = 4
    ,@frequency_interval = 1
    ,@frequency_relative_interval = 1
    ,@frequency_recurrence_factor = 0
    ,@frequency_subday = 8
    ,@frequency_subday_interval = 1
    ,@active_start_time_of_day = 0
    ,@active_end_time_of_day = 235959
    ,@active_start_date = 0
    ,@active_end_date = 0
    ,@publisher_security_mode = 0
    ,@publisher_login = @publisherlogin
    ,@publisher_password = @publisherpassword;
GO
 
EXEC sp_addarticle @publication = N'SnapshotRepl'
    ,@article = N'Games'
    ,@source_owner = N'dbo'
    ,@source_object = N'Games'
    ,@type = N'logbased'
    ,@description = NULL
    ,@creation_script = NULL
    ,@pre_creation_cmd = N'drop'
    ,@schema_option = 0x000000000803509D
    ,@identityrangemanagementoption = N'manual'
    ,@destination_table = N'Games'
    ,@destination_owner = N'dbo'
    ,@vertical_partition = N'false';

EXEC sp_addarticle @publication = N'SnapshotRepl'
    ,@article = N'Reviews'
    ,@source_owner = N'dbo'
    ,@source_object = N'Reviews'
    ,@type = N'logbased'
    ,@description = NULL
    ,@creation_script = NULL
    ,@pre_creation_cmd = N'drop'
    ,@schema_option = 0x000000000803509D
    ,@identityrangemanagementoption = N'manual'
    ,@destination_table = N'Reviews'
    ,@destination_owner = N'dbo'
    ,@vertical_partition = N'false';

EXEC sp_addarticle @publication = N'SnapshotRepl'
    ,@article = N'TimeRecords'
    ,@source_owner = N'dbo'
    ,@source_object = N'TimeRecords'
    ,@type = N'logbased'
    ,@description = NULL
    ,@creation_script = NULL
    ,@pre_creation_cmd = N'drop'
    ,@schema_option = 0x000000000803509D
    ,@identityrangemanagementoption = N'manual'
    ,@destination_table = N'TimeRecords'
    ,@destination_owner = N'dbo'
    ,@vertical_partition = N'false';

EXEC sp_addarticle @publication = N'SnapshotRepl'
    ,@article = N'Users'
    ,@source_owner = N'dbo'
    ,@source_object = N'Users'
    ,@type = N'logbased'
    ,@description = NULL
    ,@creation_script = NULL
    ,@pre_creation_cmd = N'drop'
    ,@schema_option = 0x000000000803509D
    ,@identityrangemanagementoption = N'manual'
    ,@destination_table = N'Users'
    ,@destination_owner = N'dbo'
    ,@vertical_partition = N'false';
GO

DECLARE @subscriber AS SYSNAME
DECLARE @subscriber_db AS SYSNAME
DECLARE @subscriberLogin AS SYSNAME
DECLARE @subscriberPassword AS SYSNAME
 
SET @subscriber = N'replica'
SET @subscriber_db = N'GameTime'
SET @subscriberLogin = N'sa'
SET @subscriberPassword = N'Password_1'

EXEC sp_addsubscription @publication = N'SnapshotRepl'
    ,@subscriber = @subscriber
    ,@destination_db = @subscriber_db
    ,@subscription_type = N'Push'
    ,@sync_type = N'automatic'
    ,@article = N'all'
    ,@update_mode = N'read only'
    ,@subscriber_type = 0;


exec sp_addpushsubscription_agent 
    @publication = N'SnapshotRepl', 
    @subscriber = @subscriber, 
    @subscriber_db = @subscriber_db, 
    @subscriber_login = @subscriberLogin,
    @subscriber_password = @subscriberPassword,
    @subscriber_security_mode = 0, 
    @frequency_type = 4, 
    @frequency_interval = 1, 
    @frequency_relative_interval = 0, 
    @frequency_recurrence_factor = 0, 
    @frequency_subday = 8, 
    @frequency_subday_interval = 1, 
    @active_start_time_of_day = 0, 
    @active_end_time_of_day = 235959, 
    @active_start_date = 0, 
    @active_end_date = 99991231, 
    @dts_package_location = N'Distributor'
GO

use msdb;
go

DECLARE @job1 SYSNAME;
SELECT @job1 = name FROM msdb.dbo.sysjobs
WHERE name LIKE '%-GameTime-SnapshotRepl-1'
EXEC msdb.dbo.sp_start_job @job1
go

DECLARE @job2 SYSNAME;
SELECT @job2 = name FROM msdb.dbo.sysjobs
WHERE name LIKE '%-GameTime-SnapshotRepl-replica-1'
EXEC msdb.dbo.sp_start_job @job2
go

