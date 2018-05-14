ALTER TABLE Post DROP CONSTRAINT FKPost810927;
ALTER TABLE Thread DROP CONSTRAINT FKThread187810;
ALTER TABLE Post DROP CONSTRAINT FKPost744728;
ALTER TABLE Thread DROP CONSTRAINT FKThread592770;
ALTER TABLE Moderator DROP CONSTRAINT FKModerator249078;
ALTER TABLE Topic DROP CONSTRAINT FKTopic633761;
ALTER TABLE AspNetUsersGame DROP CONSTRAINT FKAspNetUser749272;
ALTER TABLE AspNetUsersGame DROP CONSTRAINT FKAspNetUser61499;
ALTER TABLE GameGameType DROP CONSTRAINT FKGameGameTy153833;
ALTER TABLE GameGameType DROP CONSTRAINT FKGameGameTy613766;
ALTER TABLE Topic DROP CONSTRAINT FKTopic726017;
ALTER TABLE Post DROP CONSTRAINT FKPost667311;
ALTER TABLE GamePlatform DROP CONSTRAINT FKGamePlatfo295053;
ALTER TABLE GamePlatform DROP CONSTRAINT FKGamePlatfo315600;


DROP TABLE User_group;
DROP TABLE Topic;
DROP TABLE Thread;
DROP TABLE Post;
DROP TABLE Moderator;
DROP TABLE Game;
DROP TABLE Genre;
DROP TABLE GameGameType;
DROP TABLE AspNetUsersGame;
DROP TABLE GamePlatform;
DROP TABLE Platform;


CREATE TABLE Topic (
  Id           int IDENTITY NOT NULL, 
  Name         varchar(100) NOT NULL UNIQUE, 
  Description  varchar(255) NOT NULL, 
  Created      datetimeoffset(0) DEFAULT SYSDATETIMEOFFSET() NOT NULL, 
  UsersId      int NOT NULL, 
  LastActivity datetimeoffset(0) NOT NULL, 
  PlatformId   int NOT NULL, 
  PRIMARY KEY (Id));
CREATE TABLE Thread (
  Id           int IDENTITY NOT NULL, 
  Name         varchar(100) NOT NULL UNIQUE, 
  Description  varchar(255) NOT NULL, 
  Created      datetimeoffset(0) NOT NULL, 
  TopicId      int NOT NULL, 
  UsersId      int NOT NULL, 
  LastActivity datetimeoffset(0) NOT NULL, 
  PRIMARY KEY (Id));
CREATE TABLE Post (
  Id           int IDENTITY NOT NULL, 
  Subject      varchar(150) NOT NULL, 
  Content      varchar(1000) NOT NULL, 
  Created      datetimeoffset(0) DEFAULT SYSDATETIMEOFFSET() NOT NULL, 
  ThreadId     int NOT NULL, 
  UserId       int NOT NULL, 
  UpCount      int DEFAULT 0 NOT NULL, 
  DownCount    int DEFAULT 0 NOT NULL, 
  SourcePostId int NULL, 
  LastActivity datetimeoffset(0) NOT NULL, 
  PRIMARY KEY (Id));
CREATE TABLE Moderator (
  UsersId int NOT NULL, 
  PRIMARY KEY (UsersId));
CREATE TABLE Game (
  GameId      int IDENTITY NOT NULL, 
  Name        varchar(30) NOT NULL, 
  ReleaseDate int NOT NULL, 
  Cover       varbinary(max) NOT NULL, 
  PRIMARY KEY (GameId));
CREATE TABLE Genre (
  GenreeId int IDENTITY NOT NULL, 
  Name     int NOT NULL, 
  PRIMARY KEY (GenreeId));
CREATE TABLE GameGameType (
  GameId  int NOT NULL, 
  GenreId int NOT NULL, 
  PRIMARY KEY (GameId, 
  GenreId));
CREATE TABLE AspNetUsersGame (
  UserId int NOT NULL, 
  GameId int NOT NULL, 
  PRIMARY KEY (UserId, 
  GameId));
CREATE TABLE GamePlatform (
  PlatformId int NOT NULL, 
  GameId     int NOT NULL, 
  PRIMARY KEY (PlatformId, 
  GameId));
CREATE TABLE Platform (
  PlatformId int IDENTITY NOT NULL, 
  Name       int NOT NULL, 
  PRIMARY KEY (PlatformId));
ALTER TABLE Post ADD CONSTRAINT FKPost810927 FOREIGN KEY (ThreadId) REFERENCES Thread (Id);
ALTER TABLE Thread ADD CONSTRAINT FKThread187810 FOREIGN KEY (TopicId) REFERENCES Topic (Id);
ALTER TABLE Post ADD CONSTRAINT FKPost744728 FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id);
ALTER TABLE Thread ADD CONSTRAINT FKThread592770 FOREIGN KEY (UsersId) REFERENCES AspNetUsers (Id);
ALTER TABLE Moderator ADD CONSTRAINT FKModerator249078 FOREIGN KEY (UsersId) REFERENCES AspNetUsers (Id);
ALTER TABLE Topic ADD CONSTRAINT FKTopic633761 FOREIGN KEY (UsersId) REFERENCES Moderator (UsersId);
ALTER TABLE AspNetUsersGame ADD CONSTRAINT FKAspNetUser749272 FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id);
ALTER TABLE AspNetUsersGame ADD CONSTRAINT FKAspNetUser61499 FOREIGN KEY (GameId) REFERENCES Game (GameId);
ALTER TABLE GameGameType ADD CONSTRAINT FKGameGameTy153833 FOREIGN KEY (GameId) REFERENCES Game (GameId);
ALTER TABLE GameGameType ADD CONSTRAINT FKGameGameTy613766 FOREIGN KEY (GenreId) REFERENCES Genre (GenreeId);
ALTER TABLE Post ADD CONSTRAINT FKPost667311 FOREIGN KEY (SourcePostId) REFERENCES Post (Id);
ALTER TABLE GamePlatform ADD CONSTRAINT FKGamePlatfo295053 FOREIGN KEY (GameId) REFERENCES Game (GameId);
ALTER TABLE GamePlatform ADD CONSTRAINT FKGamePlatfo315600 FOREIGN KEY (PlatformId) REFERENCES Platform (PlatformId);
ALTER TABLE Topic ADD CONSTRAINT FKTopic413407 FOREIGN KEY (PlatformId) REFERENCES Platform (PlatformId);
