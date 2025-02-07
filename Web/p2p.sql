DECLARE @svrname AS sysname
SELECT @svrname = @@SERVERNAME
DECLARE @cnt INT
SELECT @cnt=COUNT(*) FROM sys.databases WHERE name = 'distribution'
IF @cnt>0
BEGIN
PRINT 'distributor already exists'
END
ELSE
BEGIN
EXEC sp_adddistributor @distributor=@svrname
EXEC sp_adddistributiondb @database='distribution',@security_mode=1
END
go

DECLARE @svrname AS sysname
SELECT @svrname = @@SERVERNAME

EXEC sp_adddistpublisher @publisher =@svrname,
	@distribution_db = N'distribution',
	@security_mode = 1,
	@publisher_type = N'MSSQLSERVER'

DECLARE @dbname VARCHAR(30)
DECLARE @dbcheck SQL_VARIANT

SELECT @dbname = 'GameTime' /***** Set Your Database Name Here**************/
SELECT @dbcheck = DATABASEPROPERTYEX(@dbname,'ispublished')
SELECT @dbcheck
IF @dbcheck = 0
BEGIN
PRINT 'Database not set as Publisher. Setting as Publisher'

EXEC sp_replicationdboption @dbname=@dbname,
@optname='publish',
@value='true'
END
ELSE
BEGIN
PRINT 'Database already set as Publisher'
END

DECLARE @pubname VARCHAR(300)
SELECT @pubname = 'p2p' /********Add Publication Name Here***********/

EXEC sp_addpublication @publication=@pubname,
@restricted='false',
@sync_method='native',
@repl_freq='continuous',
@allow_push='true',
@allow_pull='true',
@immediate_sync='true',
@allow_sync_tran='false',
@autogen_sync_procs='false',
@retention=60,
@independent_agent='true',
@enabled_for_p2p='true',
@status='active',
@allow_initialize_from_backup='true'
GO
/* Add articles for this Publication*/
/**********This step needs to be executed for each article in the publication***************/
DECLARE @artname VARCHAR(300)
DECLARE @artins VARCHAR(300)
DECLARE @artdel VARCHAR(300)
DECLARE @artupd VARCHAR(300)
SELECT @artname = 'Games'/**********Add Article Name Here***************/
SELECT @artins = 'CALL [sp_MSins_'+@artname+']'
SELECT @artdel = 'CALL [sp_MSdel_'+@artname+']'
SELECT @artupd = 'CALL [sp_MSupd_'+@artname+']'

DECLARE @pubname VARCHAR(300)
SELECT @pubname = 'p2p'/***********Add your publication name here*************/
EXEC sp_addarticle @publication = @pubname,
@article = @artname,
@source_owner = N'dbo',
@source_object = @artname,
@type = N'logbased',
@description = N'',
@creation_script = N'',
@pre_creation_cmd = N'none',
@schema_option = 0x00000000000044F7,
@identityrangemanagementoption = N'manual',
@destination_table = @artname,
@destination_owner = N'dbo',
@status = 16,
@vertical_partition = N'false',
@ins_cmd = @artins,
@del_cmd = @artdel,
@upd_cmd = @artupd
GO

DECLARE @artname VARCHAR(300)
DECLARE @artins VARCHAR(300)
DECLARE @artdel VARCHAR(300)
DECLARE @artupd VARCHAR(300)
SELECT @artname = 'Reviews'/**********Add Article Name Here***************/
SELECT @artins = 'CALL [sp_MSins_'+@artname+']'
SELECT @artdel = 'CALL [sp_MSdel_'+@artname+']'
SELECT @artupd = 'CALL [sp_MSupd_'+@artname+']'

DECLARE @pubname VARCHAR(300)
SELECT @pubname = 'p2p'/***********Add your publication name here*************/
EXEC sp_addarticle @publication = @pubname,
@article = @artname,
@source_owner = N'dbo',
@source_object = @artname,
@type = N'logbased',
@description = N'',
@creation_script = N'',
@pre_creation_cmd = N'none',
@schema_option = 0x00000000000044F7,
@identityrangemanagementoption = N'manual',
@destination_table = @artname,
@destination_owner = N'dbo',
@status = 16,
@vertical_partition = N'false',
@ins_cmd = @artins,
@del_cmd = @artdel,
@upd_cmd = @artupd
GO

DECLARE @artname VARCHAR(300)
DECLARE @artins VARCHAR(300)
DECLARE @artdel VARCHAR(300)
DECLARE @artupd VARCHAR(300)
SELECT @artname = 'TimeRecords'/**********Add Article Name Here***************/
SELECT @artins = 'CALL [sp_MSins_'+@artname+']'
SELECT @artdel = 'CALL [sp_MSdel_'+@artname+']'
SELECT @artupd = 'CALL [sp_MSupd_'+@artname+']'

DECLARE @pubname VARCHAR(300)
SELECT @pubname = 'p2p'/***********Add your publication name here*************/
EXEC sp_addarticle @publication = @pubname,
@article = @artname,
@source_owner = N'dbo',
@source_object = @artname,
@type = N'logbased',
@description = N'',
@creation_script = N'',
@pre_creation_cmd = N'none',
@schema_option = 0x00000000000044F7,
@identityrangemanagementoption = N'manual',
@destination_table = @artname,
@destination_owner = N'dbo',
@status = 16,
@vertical_partition = N'false',
@ins_cmd = @artins,
@del_cmd = @artdel,
@upd_cmd = @artupd
GO

DECLARE @artname VARCHAR(300)
DECLARE @artins VARCHAR(300)
DECLARE @artdel VARCHAR(300)
DECLARE @artupd VARCHAR(300)
SELECT @artname = 'Users'/**********Add Article Name Here***************/
SELECT @artins = 'CALL [sp_MSins_'+@artname+']'
SELECT @artdel = 'CALL [sp_MSdel_'+@artname+']'
SELECT @artupd = 'CALL [sp_MSupd_'+@artname+']'

DECLARE @pubname VARCHAR(300)
SELECT @pubname = 'p2p'/***********Add your publication name here*************/
EXEC sp_addarticle @publication = @pubname,
@article = @artname,
@source_owner = N'dbo',
@source_object = @artname,
@type = N'logbased',
@description = N'',
@creation_script = N'',
@pre_creation_cmd = N'none',
@schema_option = 0x00000000000044F7,
@identityrangemanagementoption = N'manual',
@destination_table = @artname,
@destination_owner = N'dbo',
@status = 16,
@vertical_partition = N'false',
@ins_cmd = @artins,
@del_cmd = @artdel,
@upd_cmd = @artupd
GO

DECLARE @pubname VARCHAR(300)
SELECT @pubname = 'p2p'/***********Add your publication name here*************/
DECLARE @dbname VARCHAR(30)
SELECT @dbname = 'GameTime'/***********Add your Subscriber database name here*************/
DECLARE @subname VARCHAR(30)
SELECT @subname = '172.22.0.3'/***********Add your Subscriber Server name here*************/
EXEC sp_addsubscription @publication = @pubname,
@subscriber = @subname,
@destination_db = @dbname,
@sync_type = 'replication support only'
GO

DECLARE @pubname VARCHAR(300)
SELECT @pubname = 'p2p'/***********Add your publication name here*************/
DECLARE @dbname VARCHAR(30)
SELECT @dbname = 'GameTime'/***********Add your Subscriber database name here*************/
DECLARE @subname VARCHAR(30)
SELECT @subname = '172.22.0.3'/***********Add your Subscriber Server name here*************/
EXEC sys.sp_addpushsubscription_agent
@publication = @pubname,
@subscriber = @subname,
@subscriber_db = @dbname,
@subscriber_security_mode = 0,
@subscriber_login = @subscriberLogin,
@subscriber_password = @subscriberPassword,
@frequency_type = 64,
@frequency_interval = 1,
@frequency_relative_interval = 1,
@frequency_recurrence_factor = 0,
@frequency_subday = 4,
@frequency_subday_interval = 5,
@active_start_time_of_day = 0,
@active_end_time_of_day = 235959,
@active_start_date = 0,
@active_end_date = 0,
@dts_package_location = N'Distributor'