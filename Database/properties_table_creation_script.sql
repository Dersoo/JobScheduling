USE [Database name]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QRTZ_JOB_PROPERTIES](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](25) NOT NULL,
	[CRON_EXPRESSION] [nvarchar](120) NOT NULL,
	[IS_ACTIVE] [bit] NOT NULL,
 CONSTRAINT [PK_QRTZ_JOB_PROPERTIES] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[QRTZ_JOB_PROPERTIES] ON 

INSERT [dbo].[QRTZ_JOB_PROPERTIES] ([ID], [NAME], [CRON_EXPRESSION], [IS_ACTIVE]) VALUES (1, N'emailing_job', N'0 0 0/1 1/1 * ? *', 1)
SET IDENTITY_INSERT [dbo].[QRTZ_JOB_PROPERTIES] OFF
GO
