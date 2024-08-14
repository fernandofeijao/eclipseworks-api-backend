CREATE TABLE [dbo].[TASK]
(
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [User] [nvarchar](320) NOT NULL,
  [Title] [nvarchar](50) NOT NULL,
  [Description] [nvarchar](max) NOT NULL,
  [State] [tinyint] NOT NULL,
  [Priority] [tinyint] NOT NULL,
  [TargetDate] [datetime2](7) NOT NULL,
  [ProjectId] [int] NOT NULL,
  CONSTRAINT [PK_TASK] PRIMARY KEY([Id]),
  CONSTRAINT [FK_TASK_USER] FOREIGN KEY([User]) REFERENCES [dbo].[USER] ([Id]),
  CONSTRAINT [FK_TASK_PROJECT] FOREIGN KEY([ProjectId]) REFERENCES [dbo].[PROJECT] ([Id])
)
