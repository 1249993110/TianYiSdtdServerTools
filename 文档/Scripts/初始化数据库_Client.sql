--PRAGMA FOREIGN_KEYS = ON;			--启用外键
--积分信息
CREATE TABLE IF NOT EXISTS ScoreInfo(
	GUID TEXT PRIMARY KEY,			--唯一ID, Universally Unique Identifier
	CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	PlayerName TEXT,				--玩家昵称
	SteamID TEXT NOT NULL,			--SteamID
	ScoreOwned INTEGER,				--拥有积分
	LastSignDate INTEGER			--上次签到天数
);
--创建索引
CREATE UNIQUE INDEX IF NOT EXISTS Index_ScoreInfo ON ScoreInfo(SteamID);

--公共回城点
CREATE TABLE IF NOT EXISTS CityPosition(
	GUID TEXT PRIMARY KEY,			--唯一ID	
	CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	CityName TEXT,					--城镇名称
	TeleCmd TEXT NOT NULL,			--传送命令
	TeleNeedScore INTEGER,			--传送需要积分
	Pos TEXT						--三维坐标
);
--创建索引
CREATE UNIQUE INDEX IF NOT EXISTS Index_CityPosition ON CityPosition(TeleCmd);

--私人回城点
CREATE TABLE IF NOT EXISTS HomePosition(
	GUID TEXT PRIMARY KEY,			--唯一ID
	CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	HomeName TEXT NOT NULL,			--Home名称
	PlayerName TEXT,				--玩家昵称
	SteamID TEXT NOT NULL,			--SteamID
	Pos TEXT						--三维坐标
);
--创建索引
CREATE INDEX IF NOT EXISTS Index_HomePosition ON HomePosition(SteamID);

--传送记录
CREATE TABLE IF NOT EXISTS TeleRecord(
	GUID TEXT PRIMARY KEY,			--唯一ID	
	CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	SteamID TEXT NOT NULL,			--SteamID
	LastTeleDateTime TEXT			--上次传送日期
);
--创建索引
CREATE UNIQUE INDEX IF NOT EXISTS Index_TeleRecord ON TeleRecord(SteamID);

--商品
CREATE TABLE IF NOT EXISTS Goods(
	GUID TEXT PRIMARY KEY,			--唯一ID	
	CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	GoodsName TEXT NOT NULL,		--商品名称
	BuyCmd TEXT NOT NULL,			--购买命令
	Content TEXT NOT NULL,			--内容 物品/方块/实体/指令
	Amount INTEGER,					--数量
	Quality INTEGER,				--品质
	Price INTEGER,					--售价
	GoodsType TEXT NOT NULL			--商品类型
);
--创建索引
CREATE UNIQUE INDEX IF NOT EXISTS Index_Goods ON Goods(BuyCmd);

--奖品
CREATE TABLE IF NOT EXISTS Lottery(
	GUID TEXT PRIMARY KEY,			--唯一ID	
	CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	LotteryName TEXT NOT NULL,		--奖品名称
	Content TEXT NOT NULL,			--内容 物品/方块/实体/指令/积分
	Amount INTEGER,					--数量
	Quality INTEGER,				--品质
	LotteryType TEXT NOT NULL		--奖品类型
);

