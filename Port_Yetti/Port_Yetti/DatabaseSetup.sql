CREATE TABLE [dbo].[Address](
	[AddressId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL CONSTRAINT [DF_Addresses_Name]  DEFAULT (''),
	[Company] [nvarchar](100) NOT NULL CONSTRAINT [DF_Addresses_Company]  DEFAULT (''),
	[Line1] [nvarchar](100) NOT NULL CONSTRAINT [DF_Addresses_Line1]  DEFAULT (''),
	[Line2] [nvarchar](100) NOT NULL CONSTRAINT [DF_Addresses_Line2]  DEFAULT (''),
	[Line3] [nvarchar](100) NOT NULL CONSTRAINT [DF_Addresses_Line3]  DEFAULT (''),
	[City] [nvarchar](100) NOT NULL CONSTRAINT [DF_Addresses_City]  DEFAULT (''),
	[PostCode] [nvarchar](20) NOT NULL CONSTRAINT [DF_Addresses_PostCode]  DEFAULT (''),
	[CountryISO2] [nchar](2) NOT NULL,
	[PhoneNumber] [nvarchar](50) NOT NULL CONSTRAINT [DF_Addresses_PhoneNumber]  DEFAULT (''),
	[EmailAddress] [nvarchar](254) NOT NULL CONSTRAINT [DF_Addresses_EmailAddress]  DEFAULT (''),
	[Deleted] [bit] NOT NULL CONSTRAINT [DF_Address_Deleted]  DEFAULT ((0)),
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[AddressId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Country]    Script Date: 21/09/2017 15:36:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Country](
	[CountryId] [int] NOT NULL,
	[Iso] [char](2) NOT NULL,
	[Iso3] [char](3) NULL CONSTRAINT [DF__Country__iso3__5FB337D6]  DEFAULT (NULL),
	[Name] [nvarchar](80) NOT NULL,
	[NiceName] [nvarchar](80) NOT NULL,
	[NumCode] [int] NULL CONSTRAINT [DF__Country__numcode__60A75C0F]  DEFAULT (NULL),
	[PhoneCode] [int] NOT NULL,
 CONSTRAINT [PK__Country__3213E83FB8E944A7] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Service]    Script Date: 21/09/2017 15:36:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Service](
	[ServiceID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 21/09/2017 15:36:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[SettingID] [int] IDENTITY(1,1) NOT NULL,
	[SettingNameID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL CONSTRAINT [DF_Settings_Value]  DEFAULT (''),
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[SettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SettingName]    Script Date: 21/09/2017 15:36:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SettingName](
	[SettingNameID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SettingName] PRIMARY KEY CLUSTERED 
(
	[SettingNameID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO