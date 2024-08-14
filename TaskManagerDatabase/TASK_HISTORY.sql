CREATE TABLE [dbo].[TASK_HISTORY]
(
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [TaskId] [int] NOT NULL,
  [User] [nvarchar](320) NOT NULL,
  [ChangeDate] [datetime2](7) NOT NULL,
  [BeforeChange] [nvarchar](max) NOT NULL,
  [AfterChange] [nvarchar](max) NOT NULL,
  CONSTRAINT [PK_TASK_HISTORY] PRIMARY KEY ([Id]),
  CONSTRAINT [FK_TASK_HISTORY_USER] FOREIGN KEY([User]) REFERENCES [dbo].[USER] ([Id]),
  CONSTRAINT [FK_TASK_HISTORY_TASK] FOREIGN KEY([TaskId]) REFERENCES [dbo].[TASK] ([Id])
)