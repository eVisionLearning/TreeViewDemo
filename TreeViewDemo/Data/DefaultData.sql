-- Enable IDENTITY_INSERT for AppUsers table
SET IDENTITY_INSERT [dbo].[AppUsers] ON;

-- Insert data into AppUsers table
INSERT INTO [dbo].[AppUsers] ([Id], [LoginId], [Password], [TreeName]) 
VALUES 
(2, N'U2', N'V2tKjVyHS//VCwcCSf4ywQ==', NULL); -- password is same U2

-- Disable IDENTITY_INSERT for AppUsers table
SET IDENTITY_INSERT [dbo].[AppUsers] OFF;


SET IDENTITY_INSERT [dbo].[Persons] ON 
-- Insert data into Persons table
INSERT INTO [dbo].[Persons] ([Id], [Name], [Status], [ParentId], [TextColor], [BgColor], [UserId], [PhotoUrl], [MaritalStatus], [Gender]) 
VALUES 
(6, N'Software Development', 1, NULL, N'#000000', N'#ffffff', 1, NULL,0,0),
(7, N'App Development', 1, 6, N'#000000', N'#ffffff', 1, NULL,0,0),
(8, N'Android Apps', 1, 7, N'#000000', N'#ffffff', 1, NULL,0,0),
(9, N'Web Development', 1, 6, N'#000000', N'#ffffff', 1, NULL,0,0),
(10, N'Desktop Development', 1, 6, N'#000000', N'#ffffff', 1, NULL,0,0),
(11, N'Cross Platform', 1, 6, N'#000000', N'#ffffff', 1, NULL,0,0),
(12, N'PHP', 1, 9, N'#000000', N'#ffffff', 1, NULL,0,0),
(13, N'Node.js', 1, 9, N'#000000', N'#ffffff', 1, NULL,0,0),
(14, N'.Net', 1, 9, N'#000000', N'#ffffff', 1, NULL,0,0),
(15, N'Windows Apps', 1, 10, N'#000000', N'#ffffff', 1, NULL,0,0),
(16, N'Store Apps', 1, 10, N'#000000', N'#ffffff', 1, NULL,0,0),
(17, N'.Net Standard', 1, 14, N'#000000', N'#ffffff', 1, NULL,0,0),
(18, N'.Net Core', 1, 14, N'#000000', N'#ffffff', 1, NULL,0,0),
(19, N'Desktop Apps', 1, 26, N'#000000', N'#ffffff', 1, NULL,0,0),
(20, N'Web Apps', 1, 26, N'#000000', N'#ffffff', 1, NULL,0,0),
(21, N'Android Apps', 1, 26, N'#000000', N'#ffffff', 1, NULL,0,0),
(25, N'iOS Apps', 1, 7, N'#000000', N'#ffffff', 1, NULL,0,0),
(26, N'Java', 1, 9, N'#000000', N'#ffffff', 1, NULL,0,0),
(27, N'Xamarin', 1, 18, N'#000000', N'#ffffff', 1, NULL,0,0),
(28, N'Blazor', 1, 18, N'#000000', N'#ffffff', 1, NULL,0,0),
(29, N'MAUI', 1, 18, N'#000000', N'#ffffff', 1, NULL,0,0),
(30, N'Desktop Apps', 1, 18, N'#000000', N'#ffffff', 1, NULL,0,0),
(31, N'Web Apps', 1, 18, N'#000000', N'#ffffff', 1, NULL,0,0),
(32, N'Desktop Apps', 1, 17, N'#000000', N'#ffffff', 1, NULL,0,0),
(33, N'Web Apps', 1, 17, N'#000000', N'#ffffff', 1, NULL,0,0);

SET IDENTITY_INSERT [dbo].[Persons] OFF
