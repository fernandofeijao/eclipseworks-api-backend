CREATE TABLE [dbo].[PROJECT]
(
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Name] [nvarchar](50) NOT NULL,
  [Owner] [nvarchar](320) NOT NULL,
  [CreateDate] [datetime2](7) NOT NULL,
  [StartDate] [datetime2](7) NULL,
  [FinishDate] [datetime2](7) NULL,
  CONSTRAINT [PK_PROJECT] PRIMARY KEY([Id]),
  CONSTRAINT [FK_PROJECT_USUARIO] FOREIGN KEY([Owner]) REFERENCES [dbo].[USER] ([Id])
)