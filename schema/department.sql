SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[department](
	[Dnumber] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](30) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[department] ADD PRIMARY KEY CLUSTERED 
(
	[Dnumber] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[department] ADD UNIQUE NONCLUSTERED 
(
	[Dnumber] ASC,
	[Name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
