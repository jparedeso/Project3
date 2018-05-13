CREATE TABLE AspNetUsers (
  id              int IDENTITY NOT NULL, 
  display_name    varchar(30) NOT NULL UNIQUE, 
  hashed_password varchar(50) NOT NULL, 
  first_name      varchar(30) NOT NULL, 
  last_name       varchar(30) NOT NULL, 
  email           varchar(254) NOT NULL UNIQUE, 
  created         datetimeoffset(0) NOT NULL, 
  picture         varbinary(max) NULL, 
  last_Activity   datetimeoffset(0) NOT NULL, 
  status          varchar(100) NULL, 
  gender          char(1) NULL, 
  user_statusid   int DEFAULT 1 NOT NULL, 
  PRIMARY KEY (id));
CREATE TABLE groups (
  id   int IDENTITY NOT NULL, 
  name varchar(100) NOT NULL, 
  PRIMARY KEY (id));
CREATE TABLE User_group (
  [Column] int NULL);
CREATE TABLE user_group (
  UserId  int NOT NULL, 
  GroupId int NOT NULL, 
  PRIMARY KEY (UserId, 
  GroupId));
CREATE TABLE topic (
  id          int IDENTITY NOT NULL, 
  name        varchar(100) NOT NULL UNIQUE, 
  description varchar(255) NOT NULL, 
  created     datetimeoffset(0) DEFAULT SYSDATETIMEOFFSET() NOT NULL, 
  UserId      int NOT NULL, 
  GameTypeId  int NOT NULL, 
  PRIMARY KEY (id));
CREATE TABLE thread (
  id       int IDENTITY NOT NULL, 
  subject  varchar(100) NOT NULL, 
  created  datetime NOT NULL, 
  topic_id int NOT NULL, 
  user_id  int NOT NULL, 
  PRIMARY KEY (id));
CREATE TABLE post (
  id           int IDENTITY NOT NULL, 
  subject      varchar(255) NOT NULL, 
  content      varchar(1000) NOT NULL, 
  created      datetimeoffset(0) DEFAULT SYSDATETIMEOFFSET() NOT NULL, 
  thread_id    int NOT NULL, 
  user_id      int NOT NULL, 
  UpCount      int DEFAULT 0 NOT NULL, 
  DownCount    int DEFAULT 0 NOT NULL, 
  SourcePostId int NULL, 
  PRIMARY KEY (id));
CREATE TABLE user_status (
  id   int IDENTITY NOT NULL, 
  name varchar(50) NULL, 
  PRIMARY KEY (id));
CREATE TABLE status (
  id   int NOT NULL UNIQUE, 
  name varchar(50) NULL);
CREATE TABLE Moderator (
  userid int NOT NULL, 
  PRIMARY KEY (userid));
CREATE TABLE Game (
  GameId int IDENTITY NOT NULL, 
  PRIMARY KEY (GameId));
CREATE TABLE GameType (
  GameTypeId int IDENTITY NOT NULL, 
  PRIMARY KEY (GameTypeId));
CREATE TABLE GameGameType (
  GameId     int NOT NULL, 
  GameTypeId int NOT NULL, 
  PRIMARY KEY (GameId, 
  GameTypeId));
CREATE TABLE AspNetUsersGame (
  UserId int NOT NULL, 
  GameId int NOT NULL, 
  PRIMARY KEY (UserId, 
  GameId));
ALTER TABLE AspNetUsers ADD CONSTRAINT FKAspNetUser161163 FOREIGN KEY (user_statusid) REFERENCES user_status (id);
ALTER TABLE post ADD CONSTRAINT FKpost741178 FOREIGN KEY (thread_id) REFERENCES thread (id);
ALTER TABLE thread ADD CONSTRAINT FKthread187386 FOREIGN KEY (topic_id) REFERENCES topic (id);
ALTER TABLE post ADD CONSTRAINT FKpost240127 FOREIGN KEY (user_id) REFERENCES AspNetUsers (id);
ALTER TABLE thread ADD CONSTRAINT FKthread840387 FOREIGN KEY (user_id) REFERENCES AspNetUsers (id);
ALTER TABLE user_group ADD CONSTRAINT FKuser_group39106 FOREIGN KEY (UserId) REFERENCES AspNetUsers (id);
ALTER TABLE user_group ADD CONSTRAINT FKuser_group666142 FOREIGN KEY (GroupId) REFERENCES groups (id);
ALTER TABLE Moderator ADD CONSTRAINT FKModerator320792 FOREIGN KEY (userid) REFERENCES AspNetUsers (id);
ALTER TABLE topic ADD CONSTRAINT FKtopic585656 FOREIGN KEY (UserId) REFERENCES Moderator (userid);
ALTER TABLE AspNetUsersGame ADD CONSTRAINT FKAspNetUser750264 FOREIGN KEY (UserId) REFERENCES AspNetUsers (id);
ALTER TABLE AspNetUsersGame ADD CONSTRAINT FKAspNetUser61499 FOREIGN KEY (GameId) REFERENCES Game (GameId);
ALTER TABLE GameGameType ADD CONSTRAINT FKGameGameTy153833 FOREIGN KEY (GameId) REFERENCES Game (GameId);
ALTER TABLE GameGameType ADD CONSTRAINT FKGameGameTy312612 FOREIGN KEY (GameTypeId) REFERENCES GameType (GameTypeId);
ALTER TABLE topic ADD CONSTRAINT FKtopic172123 FOREIGN KEY (GameTypeId) REFERENCES GameType (GameTypeId);
ALTER TABLE post ADD CONSTRAINT FKpost574929 FOREIGN KEY (SourcePostId) REFERENCES post (id);
