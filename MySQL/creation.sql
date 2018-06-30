DROP SCHEMA fill;
CREATE SCHEMA fill;
USE fill;

CREATE TABLE User(
	UserId int AUTO_INCREMENT,
	DeviceId int NOT NULL,
	Email char(100),
	Pwd char(255), -- hashed value
	UserName char(100),
	PRIMARY KEY (UserId)
);

CREATE TABLE Category(
	CName char(50),
	PRIMARY KEY (CName)
);

-- perhaps add DEFAULT for colors as well
CREATE TABLE MapMeta(
	GameId int AUTO_INCREMENT,
	GName char(100),
	CName char(50),
	Creater int,
	NumOfVertices int,
	NumOfHoles int,
	LineColorRGB int, -- 9 digit number: rrrgggbbb = r:0.rrr g:0.ggg b:0.bbb
	LineColorA float DEFAULT 1.0,
	BGColorRGB int,
	BGColorA float DEFAULT 1.0,
	GuardBasicColorRGB int,
	GuardBasicColorA float DEFAULT 1.0,
	GuardSelectedColorRGB int,
	GuardSelectedColorA float DEFAULT 1.0,
	VGColorRGB int,
	VGColorA float DEFAULT 1.0,
	FOREIGN KEY (CName)
		REFERENCES Category(CName)
		ON UPDATE CASCADE ON DELETE NO ACTION,
	FOREIGN KEY (Creater)
		REFERENCES User(UserId)
		ON UPDATE CASCADE ON DELETE NO ACTION,
	PRIMARY KEY (GameId)
);

CREATE TABLE Map(
	GameId int,
	JsonFile mediumtext,
	ImageFile blob,
	FOREIGN KEY (GameId)
		REFERENCES MapMeta(GameId)
		ON UPDATE CASCADE ON DELETE NO ACTION,
	PRIMARY KEY (GameId)
);

CREATE TABLE PlayResult(
	GameId int,
	UserId int,
	Submission int,
	NumOfGuards int NOT NULL,
	GuardLocation varchar(2000) NOT NULL, -- json formatted information
	GameHash char(255) NOT NULL, -- hash created about the game w/o GameInfo, to validate user's play
	Score float NOT NULL,
	FOREIGN KEY (GameId)
		REFERENCES MapMeta(GameId)
		ON UPDATE CASCADE ON DELETE NO ACTION,
	FOREIGN KEY (UserId)
		REFERENCES User(UserId)
		ON UPDATE CASCADE ON DELETE NO ACTION,
	PRIMARY KEY (GameId, UserId, Submission)
);
