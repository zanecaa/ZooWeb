CREATE TABLE zoo_user(
	UserId INT IDENTITY (1,1) PRIMARY KEY,
	Username NVARCHAR(255) NOT NULL,
	PasswordHash NVARCHAR(64) NOT NULL,
	IsActive BIT NOT NULL DEFAULT 1,
	CreationDate DATETIME DEFAULT GETDATE(),

	CONSTRAINT AK_zu_Username UNIQUE (Username)
);

CREATE TABLE zoo_user_employee(
	UserId INT REFERENCES zoo_user(UserId),
	EmployeeId INT REFERENCES employee(EmployeeId),

	CONSTRAINT AK_zu_e_UserId UNIQUE (UserId),
	CONSTRAINT AK_zu_e_EmployeeId UNIQUE (EmployeeId)
);

CREATE TABLE zoo_user_visitor(
	UserId INT REFERENCES zoo_user(UserId),
	VisitorId BIGINT,

	FOREIGN KEY (VisitorId)
		REFERENCES visitor(PhoneNumber)
		ON UPDATE CASCADE,

	CONSTRAINT AK_zu_v_UserId UNIQUE (UserId),
	CONSTRAINT AK_zu_v_VisitorId UNIQUE (VisitorId)
);