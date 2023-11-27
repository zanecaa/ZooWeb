SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[zoo_user](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](32) NOT NULL,
	[PasswordHash] [nvarchar](256) NULL,
	[IsActive] [bit] NOT NULL,
	[UserRole] [varchar](32) NULL,
	[CreationDate] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[zoo_user] ADD PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[zoo_user] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[zoo_user] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[zoo_user] ADD  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[zoo_user]  WITH NOCHECK ADD FOREIGN KEY([UserRole])
REFERENCES [dbo].[zoo_user_role] ([RoleName])
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[trgPreventDisableAllUsers]
	ON [dbo].[zoo_user]
	FOR UPDATE
	AS
	BEGIN
		DECLARE @EnabledUsersCount INT;

		-- Count the number of enabled users after an update
		SELECT @EnabledUsersCount = COUNT(*)
		FROM zoo_user
		WHERE IsActive = 1 AND UserRole = 'admin';

		-- Rollback the transaction if there are no enabled users left
		IF @EnabledUsersCount = 0
		BEGIN
			; THROW 51000, 'At least one admin user must remain enabled.', 1;
			ROLLBACK TRANSACTION;
		END
	END;
GO
ALTER TABLE [dbo].[zoo_user] ENABLE TRIGGER [trgPreventDisableAllUsers]
GO
