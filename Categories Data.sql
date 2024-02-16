USE [TreeViewDemo-db]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (1, N'Electronic Items', 1, NULL)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (6, N'Laptops', 1, 1)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (8, N'Mobile Phones', 1, 1)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (9, N'Accessories', 1, 1)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (10, N'HP Laptops', 1, 6)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (11, N'Dell Laptops', 1, 6)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (12, N'Mac Book', 1, 6)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (16, N'Misc Items', 1, NULL)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (17, N'Stationary', 1, 16)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (18, N'Plastic Items', 1, 16)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (19, N'A4 Paper', 1, 17)
GO
INSERT [dbo].[Categories] ([Id], [Name], [Status], [ParentId]) VALUES (20, N'A3 Paper', 1, 17)
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
