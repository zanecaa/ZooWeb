SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[feeding_pattern](
	[Animal_ID] [int] NULL,
	[Meal] [varchar](128) NULL,
	[Portion] [decimal](18, 0) NULL,
	[Schedule_days] [varchar](56) NULL,
	[Schedule_time] [time](7) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[feeding_pattern] ADD  CONSTRAINT [uc_feeding_pattern_aid_meal_schedule_daystime] UNIQUE NONCLUSTERED 
(
	[Animal_ID] ASC,
	[Meal] ASC,
	[Schedule_days] ASC,
	[Schedule_time] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[feeding_pattern]  WITH CHECK ADD FOREIGN KEY([Animal_ID])
REFERENCES [dbo].[animal] ([Animal_ID])
GO
