USE [master]
GO
/****** Object:  Database [EasyDocs]    Script Date: 9/25/2015 2:06:27 PM ******/
CREATE DATABASE [EasyDocs]
GO
ALTER DATABASE [EasyDocs] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EasyDocs].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EasyDocs] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EasyDocs] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EasyDocs] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EasyDocs] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EasyDocs] SET ARITHABORT OFF 
GO
ALTER DATABASE [EasyDocs] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EasyDocs] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EasyDocs] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EasyDocs] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EasyDocs] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EasyDocs] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EasyDocs] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EasyDocs] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EasyDocs] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EasyDocs] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EasyDocs] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EasyDocs] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EasyDocs] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EasyDocs] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EasyDocs] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EasyDocs] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [EasyDocs] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EasyDocs] SET RECOVERY FULL 
GO
ALTER DATABASE [EasyDocs] SET  MULTI_USER 
GO
ALTER DATABASE [EasyDocs] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EasyDocs] SET DB_CHAINING OFF 
GO
USE [EasyDocs]
GO
/****** Object:  Table [dbo].[Articles]    Script Date: 9/25/2015 2:06:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Section_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Articles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 9/25/2015 2:06:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Section_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Downloads] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ExternalLinks]    Script Date: 9/25/2015 2:06:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExternalLinks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Title] [nvarchar](max) NULL,
	[Section_Id] [int] NULL,
 CONSTRAINT [PK_dbo.ExternalLinks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[HeaderLinks]    Script Date: 9/25/2015 2:06:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HeaderLinks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Display] [nvarchar](max) NULL,
	[Url] [nvarchar](max) NULL,
	[Settings_Id] [int] NULL,
 CONSTRAINT [PK_dbo.HeaderLinks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Pages]    Script Date: 9/25/2015 2:06:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NULL,
	[UrlKey] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[Content] [text] NULL,
	[Draft] [text] NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NOT NULL,
	[Order] [int] NOT NULL,
	[SideBarContent] [text] NULL,
	[CreatedBy_Id] [int] NULL,
	[ModifiedBy_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Pages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Sections]    Script Date: 9/25/2015 2:06:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sections](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[URLKey] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Contents] [text] NOT NULL,
	[ParentID] [int] NULL,
	[SearchTerms] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[VersionId] [int] NOT NULL,
	[Order] [int] NULL,
	[Deleted] [bit] NULL,
	[Draft] [text] NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF__Sections__Create__47DBAE45]  DEFAULT ('1900-01-01T00:00:00.000'),
	[Modified] [datetime] NOT NULL CONSTRAINT [DF__Sections__Modifi__48CFD27E]  DEFAULT ('1900-01-01T00:00:00.000'),
	[CreatedBy_Id] [int] NULL,
	[ModifiedBy_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Sections] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 9/25/2015 2:06:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogoUrl] [nvarchar](max) NULL,
	[HeaderText] [nvarchar](max) NULL,
	[HomePage] [nvarchar](256) NULL,
 CONSTRAINT [PK_dbo.Settings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Users]    Script Date: 9/25/2015 2:06:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Versions]    Script Date: 9/25/2015 2:06:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Versions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Desc] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Versions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Index [IX_Section_Id]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_Section_Id] ON [dbo].[Articles]
(
	[Section_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Section_Id]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_Section_Id] ON [dbo].[Downloads]
(
	[Section_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Section_Id]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_Section_Id] ON [dbo].[ExternalLinks]
(
	[Section_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_Settings_Id]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_Settings_Id] ON [dbo].[HeaderLinks]
(
	[Settings_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_CreatedBy_Id]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedBy_Id] ON [dbo].[Pages]
(
	[CreatedBy_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_ModifiedBy_Id]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedBy_Id] ON [dbo].[Pages]
(
	[ModifiedBy_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_CreatedBy_Id]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_CreatedBy_Id] ON [dbo].[Sections]
(
	[CreatedBy_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_ModifiedBy_Id]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_ModifiedBy_Id] ON [dbo].[Sections]
(
	[ModifiedBy_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_ParentID]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_ParentID] ON [dbo].[Sections]
(
	[ParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
/****** Object:  Index [IX_VersionId]    Script Date: 9/25/2015 2:06:28 PM ******/
CREATE NONCLUSTERED INDEX [IX_VersionId] ON [dbo].[Sections]
(
	[VersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[Articles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Articles_dbo.Sections_Section_Id] FOREIGN KEY([Section_Id])
REFERENCES [dbo].[Sections] ([Id])
GO
ALTER TABLE [dbo].[Articles] CHECK CONSTRAINT [FK_dbo.Articles_dbo.Sections_Section_Id]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Downloads_dbo.Sections_Section_Id] FOREIGN KEY([Section_Id])
REFERENCES [dbo].[Sections] ([Id])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_dbo.Downloads_dbo.Sections_Section_Id]
GO
ALTER TABLE [dbo].[ExternalLinks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ExternalLinks_dbo.Sections_Section_Id] FOREIGN KEY([Section_Id])
REFERENCES [dbo].[Sections] ([Id])
GO
ALTER TABLE [dbo].[ExternalLinks] CHECK CONSTRAINT [FK_dbo.ExternalLinks_dbo.Sections_Section_Id]
GO
ALTER TABLE [dbo].[HeaderLinks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.HeaderLinks_dbo.Settings_Settings_Id] FOREIGN KEY([Settings_Id])
REFERENCES [dbo].[Settings] ([Id])
GO
ALTER TABLE [dbo].[HeaderLinks] CHECK CONSTRAINT [FK_dbo.HeaderLinks_dbo.Settings_Settings_Id]
GO
ALTER TABLE [dbo].[Pages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Pages_dbo.Users_CreatedBy_Id] FOREIGN KEY([CreatedBy_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Pages] CHECK CONSTRAINT [FK_dbo.Pages_dbo.Users_CreatedBy_Id]
GO
ALTER TABLE [dbo].[Pages]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Pages_dbo.Users_ModifiedBy_Id] FOREIGN KEY([ModifiedBy_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Pages] CHECK CONSTRAINT [FK_dbo.Pages_dbo.Users_ModifiedBy_Id]
GO
ALTER TABLE [dbo].[Sections]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Sections_dbo.Sections_ParentID] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Sections] ([Id])
GO
ALTER TABLE [dbo].[Sections] CHECK CONSTRAINT [FK_dbo.Sections_dbo.Sections_ParentID]
GO
ALTER TABLE [dbo].[Sections]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Sections_dbo.Users_CreatedBy_Id] FOREIGN KEY([CreatedBy_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Sections] CHECK CONSTRAINT [FK_dbo.Sections_dbo.Users_CreatedBy_Id]
GO
ALTER TABLE [dbo].[Sections]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Sections_dbo.Users_ModifiedBy_Id] FOREIGN KEY([ModifiedBy_Id])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Sections] CHECK CONSTRAINT [FK_dbo.Sections_dbo.Users_ModifiedBy_Id]
GO
ALTER TABLE [dbo].[Sections]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Sections_dbo.Versions_VersionId] FOREIGN KEY([VersionId])
REFERENCES [dbo].[Versions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sections] CHECK CONSTRAINT [FK_dbo.Sections_dbo.Versions_VersionId]
GO
USE [master]
GO
ALTER DATABASE [EasyDocs] SET  READ_WRITE 
GO
USE [EasyDocs]
GO

INSERT INTO [dbo].[Users]([Email],[Password],[Active],[CreatedDate])
     VALUES('test@test.com','$2a$10$UdVgWSqPE9Sps1ZUbKetN.BgO6RlOEzI.zVMO2uO9zvWxT6lFsjf6',1,getdate())
GO
USE [EasyDocs]
GO

INSERT INTO [dbo].[Versions]
           ([Desc])
     VALUES
           ('Version 1')
GO
USE [EasyDocs]
GO

INSERT INTO [dbo].[Settings]
           ([LogoUrl]
           ,[HeaderText]
           ,[HomePage])
     VALUES
           (''
           ,''
           ,'')
GO
