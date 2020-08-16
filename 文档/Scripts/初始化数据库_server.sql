USE [LuoShuiTianYi_sdtd]

GO
--用户表
CREATE TABLE T_User(
	GUID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),	--GUID
	CreatedDate DATETIME DEFAULT GETDATE(),				--创建日期
	UserID VARCHAR(20) UNIQUE NOT NULL CHECK(LEN(UserID) BETWEEN 6 AND 20),	--用户ID
	DisplayName VARCHAR(24),							--显示名称
	LastLoginTime DATETIME,								--上次登录时间
	LastLoginIP VARCHAR(40),							--上次登录IP
);
--创建索引
CREATE UNIQUE INDEX index_User ON T_User(UserID);

GO
--普通账户表
CREATE TABLE T_StandardAccount(
	GUID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),	--GUID
	CreatedDate DATETIME DEFAULT GETDATE(),				--创建日期
	Fk_UserID VARCHAR(20) FOREIGN KEY REFERENCES T_User(UserID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,								--用户ID
	PasswordHash VARCHAR(32),							--密码哈希值
);
--创建索引
CREATE UNIQUE INDEX index_StandardAccount ON T_StandardAccount(Fk_UserID);

GO
--QQ账户表
CREATE TABLE T_QQAccount(
	GUID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),	--GUID
	CreatedDate DATETIME DEFAULT GETDATE(),				--创建日期
	Fk_UserID VARCHAR(20) FOREIGN KEY REFERENCES T_User(UserID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,								--用户ID
	OpenID VARCHAR(40) UNIQUE NOT NULL,					--对应QQ用户身份的标识
);
--创建索引
CREATE UNIQUE INDEX index_QQAccount ON T_QQAccount(Fk_UserID);
CREATE UNIQUE INDEX index1_QQAccount ON T_QQAccount(OpenID);

GO
--角色表
CREATE TABLE T_Role(
	GUID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),	--GUID
	CreatedDate DATETIME DEFAULT GETDATE(),				--创建日期
	RoleID INT DEFAULT(-1) UNIQUE NOT NULL,				--角色ID
	RoleName NVARCHAR(8),								--角色名称
);
--创建索引
CREATE UNIQUE INDEX index_Role ON T_Role(RoleID);
GO
INSERT INTO T_Role(RoleID,RoleName) VALUES(-1,'黑名单用户');
INSERT INTO T_Role(RoleID,RoleName) VALUES(0,'超级管理员');
INSERT INTO T_Role(RoleID,RoleName) VALUES(1,'管理员');
INSERT INTO T_Role(RoleID,RoleName) VALUES(2,'优惠会员');
INSERT INTO T_Role(RoleID,RoleName) VALUES(3,'普通会员');
INSERT INTO T_Role(RoleID,RoleName) VALUES(4,'普通用户');

GO
--用户角色表
CREATE TABLE T_UserRole(
	GUID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),	--GUID
	CreatedDate DATETIME DEFAULT GETDATE(),				--创建日期
	Fk_UserID VARCHAR(20) FOREIGN KEY REFERENCES T_User(UserID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,								--用户ID
	Fk_RoleID INT FOREIGN KEY REFERENCES T_Role(RoleID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,								--用户角色ID
);
--创建索引
CREATE UNIQUE INDEX index_UserRole ON T_UserRole(Fk_UserID);

GO
--工具使用期限表
CREATE TABLE T_ST_Expiry(
	GUID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),	--GUID
	CreatedDate DATETIME DEFAULT GETDATE(),				--创建日期
	Fk_UserID VARCHAR(20) FOREIGN KEY REFERENCES T_User(UserID)
		ON DELETE CASCADE
		ON UPDATE CASCADE,								--用户ID
	ExpiryTime DATETIME DEFAULT GETDATE(),				--使用期限
);
--创建索引
CREATE UNIQUE INDEX index_ST_Expiry ON T_ST_Expiry(Fk_UserID);

GO
--在线用户表
CREATE TABLE T_OnlineUser(
	GUID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),	--GUID
	CreatedDate DATETIME DEFAULT GETDATE(),				--创建日期
	Fk_UserID VARCHAR(20) FOREIGN KEY REFERENCES T_User(UserID)
		ON UPDATE CASCADE,								--用户ID
	DisplayName VARCHAR(24),							--显示名称
	LoginTime DATETIME,									--登录时间
	IPAddress VARCHAR(40),								--IP地址
	ExpiryTime DATETIME DEFAULT GETDATE(),				--使用期限
	Fk_RoleID INT FOREIGN KEY REFERENCES T_Role(RoleID)
		ON UPDATE CASCADE,								--用户角色ID
	RoleName NVARCHAR(8),								--角色名称
);
--创建索引
CREATE INDEX index_OnlineUser ON T_OnlineUser(Fk_UserID);

GO
--创建视图
CREATE VIEW V_User AS
SELECT u.UserID,u.DisplayName,u.LastLoginTime,u.LastLoginIP,
rol.RoleID,rol.RoleName,expiry.ExpiryTime FROM T_User u
LEFT JOIN T_UserRole AS urole ON u.UserID=urole.Fk_UserID
LEFT JOIN T_Role AS rol ON urole.Fk_RoleID=rol.RoleID
LEFT JOIN T_ST_Expiry AS expiry ON u.UserID=expiry.Fk_UserID;

GO
--存储过程，插入多条与标准账户相关的数据
CREATE PROCEDURE SP_InsertStandardAccount
	@UserID VARCHAR(20),		--用户ID
	@DisplayName VARCHAR(24),	--显示名称
	@LastLoginTime DATETIME,	--上次登录时间
	@LastLoginIP VARCHAR(40),	--上次登录IP
	@PasswordHash VARCHAR(32),	--密码哈希值
	@RoleID INT,				--用户角色ID
	@ExpiryTime DATETIME		--使用期限
AS
BEGIN TRANSACTION
    DECLARE @nror INT
	    SET @nror=0
	DECLARE @rowcount INT
        SET @rowcount=0

	INSERT INTO T_User(UserID,DisplayName,LastLoginTime,LastLoginIP)
	VALUES(@UserID,@DisplayName,@LastLoginTime,@LastLoginIP)      
	SET @nror += @@error
	SET @rowcount += @@rowcount
 
	INSERT INTO T_UserRole(Fk_UserID,Fk_RoleID)
	VALUES(@UserID,@RoleID)
	SET @nror += @@error
	SET @rowcount += @@rowcount
	
	INSERT INTO T_StandardAccount(Fk_UserID,PasswordHash)
	VALUES(@UserID,@PasswordHash)
	SET @nror += @@error
	SET @rowcount += @@rowcount
	
	INSERT INTO T_ST_Expiry(Fk_UserID,ExpiryTime)
	VALUES(@UserID,@ExpiryTime)
	SET @nror += @@error
	SET @rowcount += @@rowcount
 
	IF (@nror != 0 OR @rowcount != 4)
	    BEGIN
			ROLLBACK TRANSACTION
			RETURN 0;	--执行失败
		END
    ELSE
         
	    BEGIN
			COMMIT TRANSACTION
			RETURN 1;	--执行成功
	    END
;
