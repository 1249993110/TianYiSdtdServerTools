PRAGMA FOREIGN_KEYS = ON;			--启用外键

--积分信息
CREATE TABLE IF NOT EXISTS ScoreInfo(
	UUID TEXT PRIMARY KEY,			--唯一ID, Universally Unique Identifier
	CreateDateTime TEXT,			--创建日期
	PlayerName TEXT,				--玩家昵称
	SteamID TEXT UNIQUE NOT NULL,	--SteamID
	ScoreOwned INTEGER,				--拥有积分
	LastSignDate INTEGER);			--上次签到天数
--创建索引
CREATE UNIQUE INDEX IF NOT EXISTS ScoreInfo_index ON ScoreInfo(SteamID);

--公共回城点
CREATE TABLE IF NOT EXISTS CityPosition(
	UUID TEXT PRIMARY KEY,			--唯一ID
	CreateDateTime TEXT,			--创建日期
	CityName TEXT,					--城市名称
	TeleCmd TEXT UNIQUE NOT NULL,	--传送命令
	Pos TEXT);						--三维坐标
--创建索引
CREATE UNIQUE INDEX IF NOT EXISTS CityPosition_index ON CityPosition(TeleCmd);

--私人回城点配置
CREATE TABLE IF NOT EXISTS HomePositionConfig(
	UUID TEXT PRIMARY KEY,			--唯一ID
	CreateDateTime TEXT,			--创建日期
	HomeName TEXT UNIQUE NOT NULL,	--Home名称
	TeleCmd TEXT UNIQUE NOT NULL,	--传送命令
	TeleNeedScore INTEGER,			--每次使用扣除积分
	SetHomeCmd TEXT UNIQUE NOT NULL,--设置家命令
	SetHomeNeedScore INTEGER);		--每次设定扣除积分	
--创建索引
CREATE UNIQUE INDEX IF NOT EXISTS HomePositionConfig_index ON HomePositionConfig(TeleCmd);
CREATE UNIQUE INDEX IF NOT EXISTS HomePositionConfig_index1 ON HomePositionConfig(SetHomeCmd);

--私人回城点
CREATE TABLE IF NOT EXISTS HomePosition(
	UUID TEXT PRIMARY KEY,			--唯一ID
	CreateDateTime TEXT,			--创建日期
	HomeName TEXT UNIQUE NOT NULL,	--Home名称
	PlayerName TEXT,				--玩家昵称
	SteamID TEXT NOT NULL,			--SteamID	
	Pos TEXT,						--三维坐标
	FOREIGN KEY(HomeName) REFERENCES HomePositionConfig(HomeName) ON DELETE RESTRICT ON UPDATE RESTRICT);						
--创建索引
CREATE UNIQUE INDEX IF NOT EXISTS HomePosition_index ON HomePosition(SteamID);