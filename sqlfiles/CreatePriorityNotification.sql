CREATE TABLE priority(
	Level SMALLINT PRIMARY KEY,
	LevelString VARCHAR(16),
);

CREATE TABLE notification(
	MessageId BIGINT PRIMARY KEY IDENTITY(1,1),
	Title NVARCHAR(255),
	MessageBody NVARCHAR(4000),
	ReadStatus BIT,
	Priority SMALLINT,
	Recepient INT,
	CreationDate DATETIME DEFAULT GETDATE(),

	CONSTRAINT FK_notification_recepient FOREIGN KEY (Recepient)
		REFERENCES zoo_user(UserId),
	CONSTRAINT FK_notification_priority FOREIGN KEY (Priority)
		REFERENCES priority(Level),
);