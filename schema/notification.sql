SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[notification](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](64) NOT NULL,
	[Message] [nvarchar](4000) NOT NULL,
	[Recipient] [int] NULL,
	[Timestamp] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[notification] ADD PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[notification] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[notification]  WITH CHECK ADD FOREIGN KEY([Recipient])
REFERENCES [dbo].[zoo_user] ([UserId])
GO
