USE [master]
GO
/****** Object:  Database [TwoDrive]    Script Date: 10/10/2019 6:15:30 PM ******/
CREATE DATABASE [TwoDrive]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TwoDrive', FILENAME = N'D:\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TwoDrive.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TwoDrive_log', FILENAME = N'D:\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\TwoDrive_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TwoDrive] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TwoDrive].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TwoDrive] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TwoDrive] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TwoDrive] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TwoDrive] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TwoDrive] SET ARITHABORT OFF 
GO
ALTER DATABASE [TwoDrive] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TwoDrive] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TwoDrive] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TwoDrive] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TwoDrive] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TwoDrive] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TwoDrive] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TwoDrive] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TwoDrive] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TwoDrive] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TwoDrive] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TwoDrive] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TwoDrive] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TwoDrive] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TwoDrive] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TwoDrive] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [TwoDrive] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TwoDrive] SET RECOVERY FULL 
GO
ALTER DATABASE [TwoDrive] SET  MULTI_USER 
GO
ALTER DATABASE [TwoDrive] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TwoDrive] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TwoDrive] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TwoDrive] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TwoDrive] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TwoDrive', N'ON'
GO
ALTER DATABASE [TwoDrive] SET QUERY_STORE = OFF
GO
USE [TwoDrive]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/10/2019 6:15:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomClaim]    Script Date: 10/10/2019 6:15:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomClaim](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ElementId] [int] NULL,
	[Type] [int] NOT NULL,
	[WriterId] [int] NULL,
 CONSTRAINT [PK_CustomClaim] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Elements]    Script Date: 10/10/2019 6:15:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Elements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[ParentFolderId] [int] NULL,
	[OwnerId] [int] NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[DateModified] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDate] [datetime2](7) NOT NULL,
	[Discriminator] [nvarchar](max) NOT NULL,
	[Content] [nvarchar](max) NULL,
 CONSTRAINT [PK_Elements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modifications]    Script Date: 10/10/2019 6:15:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ElementId] [int] NOT NULL,
	[type] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Modifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sessions]    Script Date: 10/10/2019 6:15:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [uniqueidentifier] NOT NULL,
	[WriterId] [int] NULL,
 CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Writers]    Script Date: 10/10/2019 6:15:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Writers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Role] [int] NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[FriendId] [int] NULL,
 CONSTRAINT [PK_Writers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_CustomClaim_ElementId]    Script Date: 10/10/2019 6:15:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_CustomClaim_ElementId] ON [dbo].[CustomClaim]
(
	[ElementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_CustomClaim_WriterId]    Script Date: 10/10/2019 6:15:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_CustomClaim_WriterId] ON [dbo].[CustomClaim]
(
	[WriterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Elements_OwnerId]    Script Date: 10/10/2019 6:15:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_Elements_OwnerId] ON [dbo].[Elements]
(
	[OwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Elements_ParentFolderId]    Script Date: 10/10/2019 6:15:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_Elements_ParentFolderId] ON [dbo].[Elements]
(
	[ParentFolderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Modifications_ElementId]    Script Date: 10/10/2019 6:15:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_Modifications_ElementId] ON [dbo].[Modifications]
(
	[ElementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Sessions_WriterId]    Script Date: 10/10/2019 6:15:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_Sessions_WriterId] ON [dbo].[Sessions]
(
	[WriterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Writers_FriendId]    Script Date: 10/10/2019 6:15:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_Writers_FriendId] ON [dbo].[Writers]
(
	[FriendId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomClaim]  WITH CHECK ADD  CONSTRAINT [FK_CustomClaim_Elements_ElementId] FOREIGN KEY([ElementId])
REFERENCES [dbo].[Elements] ([Id])
GO
ALTER TABLE [dbo].[CustomClaim] CHECK CONSTRAINT [FK_CustomClaim_Elements_ElementId]
GO
ALTER TABLE [dbo].[CustomClaim]  WITH CHECK ADD  CONSTRAINT [FK_CustomClaim_Writers_WriterId] FOREIGN KEY([WriterId])
REFERENCES [dbo].[Writers] ([Id])
GO
ALTER TABLE [dbo].[CustomClaim] CHECK CONSTRAINT [FK_CustomClaim_Writers_WriterId]
GO
ALTER TABLE [dbo].[Elements]  WITH CHECK ADD  CONSTRAINT [FK_Elements_Elements_ParentFolderId] FOREIGN KEY([ParentFolderId])
REFERENCES [dbo].[Elements] ([Id])
GO
ALTER TABLE [dbo].[Elements] CHECK CONSTRAINT [FK_Elements_Elements_ParentFolderId]
GO
ALTER TABLE [dbo].[Elements]  WITH CHECK ADD  CONSTRAINT [FK_Elements_Writers_OwnerId] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Writers] ([Id])
GO
ALTER TABLE [dbo].[Elements] CHECK CONSTRAINT [FK_Elements_Writers_OwnerId]
GO
ALTER TABLE [dbo].[Modifications]  WITH CHECK ADD  CONSTRAINT [FK_Modifications_Elements_ElementId] FOREIGN KEY([ElementId])
REFERENCES [dbo].[Elements] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Modifications] CHECK CONSTRAINT [FK_Modifications_Elements_ElementId]
GO
ALTER TABLE [dbo].[Sessions]  WITH CHECK ADD  CONSTRAINT [FK_Sessions_Writers_WriterId] FOREIGN KEY([WriterId])
REFERENCES [dbo].[Writers] ([Id])
GO
ALTER TABLE [dbo].[Sessions] CHECK CONSTRAINT [FK_Sessions_Writers_WriterId]
GO
ALTER TABLE [dbo].[Writers]  WITH CHECK ADD  CONSTRAINT [FK_Writers_Writers_FriendId] FOREIGN KEY([FriendId])
REFERENCES [dbo].[Writers] ([Id])
GO
ALTER TABLE [dbo].[Writers] CHECK CONSTRAINT [FK_Writers_Writers_FriendId]
GO
USE [master]
GO
ALTER DATABASE [TwoDrive] SET  READ_WRITE 
GO
