SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[enclosure](
	[LocationID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](40) NULL,
	[Capacity] [int] NOT NULL,
	[Occupant_Num] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[enclosure] ADD PRIMARY KEY CLUSTERED 
(
	[LocationID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[PreventOvercrowding]
ON [dbo].[enclosure]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewOccupants INT, @Capacity INT;

    SELECT @NewOccupants = I.Occupant_Num, @Capacity = E.Capacity
    FROM INSERTED I
    INNER JOIN [dbo].[enclosure] E ON I.LocationID = E.LocationID;

    IF @NewOccupants > @Capacity
    BEGIN
        RAISERROR('The enclosure is already at full capacity. Cannot add more occupants.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO
ALTER TABLE [dbo].[enclosure] ENABLE TRIGGER [PreventOvercrowding]
GO
