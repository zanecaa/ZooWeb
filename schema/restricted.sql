SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[restricted](
	[Location_ID] [bigint] NOT NULL,
	[Close_date] [datetime] NULL,
	[Reopen_date] [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[restricted] ADD PRIMARY KEY CLUSTERED 
(
	[Location_ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[restricted]  WITH CHECK ADD FOREIGN KEY([Location_ID])
REFERENCES [dbo].[enclosure] ([LocationID])
GO
