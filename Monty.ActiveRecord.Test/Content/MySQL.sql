CREATE TABLE `montytjob` (
  `JobId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Description` longtext,
  PRIMARY KEY (`JobId`),
  UNIQUE KEY `JobId_UNIQUE` (`JobId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `montytperson` (
  `PersonId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Birthday` datetime DEFAULT NULL,
  `CurrentJob` int(11) DEFAULT NULL,
  PRIMARY KEY (`PersonId`),
  KEY `CurrentJob_idx` (`CurrentJob`),
  CONSTRAINT `CurrentJob` FOREIGN KEY (`CurrentJob`) REFERENCES `montytjob` (`JobId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `montytdocument` (
  `DocumentId` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Owner` int(11) NOT NULL,
  PRIMARY KEY (`DocumentId`),
  UNIQUE KEY `DocumentId_UNIQUE` (`DocumentId`),
  KEY `Owner_idx` (`Owner`),
  CONSTRAINT `Owner` FOREIGN KEY (`Owner`) REFERENCES `montytperson` (`PersonId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE VIEW montyVPeopleNames
AS
select Name, count(*) as Occurrences from montytperson group by name