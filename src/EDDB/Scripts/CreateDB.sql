﻿USE [master]
GO

/****** Object:  Database [EDSystems]    Script Date: 26-01-2019 16:49:38 ******/
CREATE DATABASE [EDSystems]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EDSystems', FILENAME = N'd:\Data\Galaxy\EDSystems.mdf' , SIZE = 6000MB , MAXSIZE = 1000MB , FILEGROWTH = 50%)
 LOG ON 
( NAME = N'EDSystems_Log', FILENAME = N'd:\Data\Galaxy\EDSystems.ldf' , SIZE = 1300MB , MAXSIZE = 2000MB , FILEGROWTH = 20%)


ALTER DATABASE [EDSystems] SET COMPATIBILITY_LEVEL = 130
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EDSystems].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [EDSystems] SET ANSI_NULL_DEFAULT ON 
GO

ALTER DATABASE [EDSystems] SET ANSI_NULLS ON 
GO

ALTER DATABASE [EDSystems] SET ANSI_PADDING ON 
GO

ALTER DATABASE [EDSystems] SET ANSI_WARNINGS ON 
GO

ALTER DATABASE [EDSystems] SET ARITHABORT ON 
GO

ALTER DATABASE [EDSystems] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [EDSystems] SET AUTO_SHRINK ON
GO

ALTER DATABASE [EDSystems] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [EDSystems] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [EDSystems] SET CURSOR_DEFAULT  LOCAL 
GO

ALTER DATABASE [EDSystems] SET CONCAT_NULL_YIELDS_NULL ON 
GO

ALTER DATABASE [EDSystems] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [EDSystems] SET QUOTED_IDENTIFIER ON 
GO

ALTER DATABASE [EDSystems] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [EDSystems] SET  DISABLE_BROKER 
GO

ALTER DATABASE [EDSystems] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [EDSystems] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [EDSystems] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [EDSystems] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [EDSystems] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [EDSystems] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [EDSystems] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [EDSystems] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [EDSystems] SET  MULTI_USER 
GO

ALTER DATABASE [EDSystems] SET PAGE_VERIFY NONE  
GO

ALTER DATABASE [EDSystems] SET DB_CHAINING OFF 
GO

ALTER DATABASE [EDSystems] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [EDSystems] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [EDSystems] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [EDSystems] SET QUERY_STORE = OFF
GO

USE [EDSystems]
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE [EDSystems] SET  READ_WRITE 
GO


