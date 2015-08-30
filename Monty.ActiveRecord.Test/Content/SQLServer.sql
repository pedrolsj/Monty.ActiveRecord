CREATE TABLE [dbo].[montyTJob] (
	JobId INT NOT NULL IDENTITY,
	Name varchar(200) NOT NULL,
	Description varchar(MAX) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[JobId] ASC
	)
)

CREATE TABLE [dbo].[montyTPerson] (
	PersonId INT NOT NULL IDENTITY,
	Name varchar(200) NOT NULL,
	Birthday DATETIME NULL,
	CurrentJob INT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[PersonId] ASC
	)
)

ALTER TABLE [dbo].[montyTPerson]  WITH CHECK 
	ADD CONSTRAINT [montyTPerson_Job] 
	FOREIGN KEY([CurrentJob])
	REFERENCES [dbo].[montyTJob] ([JobId])

CREATE TABLE [dbo].[montyTDocument] (
	DocumentId INT NOT NULL IDENTITY,
	Name varchar(200) NOT NULL,
	Owner INT NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[DocumentId] ASC
	)
)

ALTER TABLE [dbo].[montyTDocument]  WITH CHECK 
	ADD CONSTRAINT [montyTDocument_Person] 
	FOREIGN KEY([Owner])
	REFERENCES [dbo].[montyTPerson] ([PersonId])
	
CREATE VIEW montyVPeopleNames
AS
select Name, count(*) as Occurrences from montyTPerson group by name