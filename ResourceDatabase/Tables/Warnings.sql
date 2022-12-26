﻿CREATE TABLE [dbo].[Warnings] (
    [ID_warning]    INT           IDENTITY (1, 1) NOT NULL,
    [latitude]      FLOAT (53)    NOT NULL,
    [longitude]     FLOAT (53)    NOT NULL,
    [province]      NVARCHAR (64) NULL,
    [is_active]     BINARY (1)    NOT NULL,
    [duration_days] INT           NULL,
    [ID_post]       INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ID_warning] ASC),
    FOREIGN KEY ([ID_post]) REFERENCES [dbo].[Posts] ([ID_post]),
    INDEX [Warnings_latitude_longitude_idx] NONCLUSTERED ([latitude], [longitude])
);

