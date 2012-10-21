if exists (select * from dbo.sysobjects where id = object_id(N'dbo.[SubscriptionSaga]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.[SubscriptionSaga]
if exists (select * from dbo.sysobjects where id = object_id(N'dbo.[SubscriptionClientSaga]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.[SubscriptionClientSaga]
create table dbo.[SubscriptionSaga] (
  CorrelationId UNIQUEIDENTIFIER not null,
   ClientId UNIQUEIDENTIFIER null,
   MessageCorrelationId NVARCHAR(255) null,
   EndpointUri NVARCHAR(255) null,
   MessageName NVARCHAR(255) null,
   SequenceNumber BIGINT null,
   SubscriptionId UNIQUEIDENTIFIER null,
   CurrentState NVARCHAR(255) null,
   primary key (CorrelationId)
)
create table dbo.[SubscriptionClientSaga] (
  CorrelationId UNIQUEIDENTIFIER not null,
   CurrentState NVARCHAR(255) null,
   ControlUri NVARCHAR(255) null,
   DataUri NVARCHAR(255) null,
   primary key (CorrelationId)
)

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.[HealthSaga]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.[HealthSaga]
create table dbo.[HealthSaga] (
  CorrelationId UNIQUEIDENTIFIER not null,
   CurrentState NVARCHAR(255) null,
   ControlUri NVARCHAR(255) null,
   DataUri NVARCHAR(255) null,
   LastHeartbeat DATETIME null,
   HeartbeatIntervalInSeconds INT null,
   primary key (CorrelationId)
)

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.[TimeoutSaga]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table dbo.[TimeoutSaga]
create table dbo.[TimeoutSaga] (
  CorrelationId UNIQUEIDENTIFIER not null,
   Tag INT not null,
   CurrentState NVARCHAR(255) null,
   TimeoutAt DATETIME null,
   primary key (CorrelationId, Tag)
)
