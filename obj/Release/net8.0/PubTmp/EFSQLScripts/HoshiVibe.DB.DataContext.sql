IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [Products] (
        [Product_Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Category] nvarchar(max) NOT NULL,
        [Stock] int NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        [Status] nvarchar(max) NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Product_Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [Users] (
        [User_Id] uniqueidentifier NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Account] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Role] nvarchar(max) NOT NULL,
        [IsDisabled] bit NOT NULL,
        [resetToken] nvarchar(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([User_Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [Vouchers] (
        [Voucher_Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        [DiscountAmount] decimal(18,2) NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Vouchers] PRIMARY KEY ([Voucher_Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [ShoppingCarts] (
        [Cart_Id] uniqueidentifier NOT NULL,
        [User_Id] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_ShoppingCarts] PRIMARY KEY ([Cart_Id]),
        CONSTRAINT [FK_ShoppingCarts_Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [Users] ([User_Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [UserProfiles] (
        [UserProfile_Id] uniqueidentifier NOT NULL,
        [User_Id] uniqueidentifier NOT NULL,
        [AvatarUrl] nvarchar(max) NULL,
        [FullName] nvarchar(max) NOT NULL,
        [Point] int NOT NULL,
        [Age] int NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Yob] datetime2 NOT NULL,
        [YobDestination] nvarchar(max) NOT NULL,
        [Zodiac] nvarchar(max) NOT NULL,
        [ZodiacUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_UserProfiles] PRIMARY KEY ([UserProfile_Id]),
        CONSTRAINT [FK_UserProfiles_Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [Users] ([User_Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [Orders] (
        [Order_Id] nvarchar(450) NOT NULL,
        [User_Id] uniqueidentifier NOT NULL,
        [Product_Id] uniqueidentifier NOT NULL,
        [Voucher_Id] uniqueidentifier NULL,
        [TotalPrice] decimal(18,2) NOT NULL,
        [DiscountAmount] decimal(18,2) NOT NULL,
        [FinalPrice] decimal(18,2) NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        [Status] nvarchar(max) NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Order_Id]),
        CONSTRAINT [FK_Orders_Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [Users] ([User_Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Orders_Vouchers_Voucher_Id] FOREIGN KEY ([Voucher_Id]) REFERENCES [Vouchers] ([Voucher_Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [ShoppingCartItems] (
        [CartItem_Id] uniqueidentifier NOT NULL,
        [Cart_Id] uniqueidentifier NOT NULL,
        [Product_Id] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_ShoppingCartItems] PRIMARY KEY ([CartItem_Id]),
        CONSTRAINT [FK_ShoppingCartItems_Products_Product_Id] FOREIGN KEY ([Product_Id]) REFERENCES [Products] ([Product_Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ShoppingCartItems_ShoppingCarts_Cart_Id] FOREIGN KEY ([Cart_Id]) REFERENCES [ShoppingCarts] ([Cart_Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [OrderDetails] (
        [OrderDetail_Id] uniqueidentifier NOT NULL,
        [OrderId] nvarchar(450) NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [UnitPrice] decimal(18,2) NOT NULL,
        [Discount] decimal(18,2) NOT NULL,
        [Product_Id] uniqueidentifier NULL,
        CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([OrderDetail_Id]),
        CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Order_Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Product_Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_OrderDetails_Products_Product_Id] FOREIGN KEY ([Product_Id]) REFERENCES [Products] ([Product_Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [Payments] (
        [Payment_Id] uniqueidentifier NOT NULL,
        [Order_Id] nvarchar(450) NOT NULL,
        [PaymentMethod] nvarchar(max) NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [PaymentDate] datetime2 NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Payments] PRIMARY KEY ([Payment_Id]),
        CONSTRAINT [FK_Payments_Orders_Order_Id] FOREIGN KEY ([Order_Id]) REFERENCES [Orders] ([Order_Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE TABLE [PaymentTransactions] (
        [Transaction_Id] uniqueidentifier NOT NULL,
        [Payment_Id] uniqueidentifier NOT NULL,
        [GatewayTransactionId] int NOT NULL,
        [TransactionDate] datetime2 NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_PaymentTransactions] PRIMARY KEY ([Transaction_Id]),
        CONSTRAINT [FK_PaymentTransactions_Payments_Payment_Id] FOREIGN KEY ([Payment_Id]) REFERENCES [Payments] ([Payment_Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE INDEX [IX_OrderDetails_OrderId] ON [OrderDetails] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE INDEX [IX_OrderDetails_Product_Id] ON [OrderDetails] ([Product_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE INDEX [IX_OrderDetails_ProductId] ON [OrderDetails] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE INDEX [IX_Orders_User_Id] ON [Orders] ([User_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE INDEX [IX_Orders_Voucher_Id] ON [Orders] ([Voucher_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Payments_Order_Id] ON [Payments] ([Order_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE INDEX [IX_PaymentTransactions_Payment_Id] ON [PaymentTransactions] ([Payment_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE INDEX [IX_ShoppingCartItems_Cart_Id] ON [ShoppingCartItems] ([Cart_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE INDEX [IX_ShoppingCartItems_Product_Id] ON [ShoppingCartItems] ([Product_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ShoppingCarts_User_Id] ON [ShoppingCarts] ([User_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    CREATE UNIQUE INDEX [IX_UserProfiles_User_Id] ON [UserProfiles] ([User_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041338_AdjustProductTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251001041338_AdjustProductTable', N'9.0.9');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001041547_InitialCreateDB'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251001041547_InitialCreateDB', N'9.0.9');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    DECLARE @var sysname;
    SELECT @var = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'Product_Id');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT [' + @var + '];');
    ALTER TABLE [Orders] DROP COLUMN [Product_Id];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    ALTER TABLE [Orders] ADD [CProduct_Id] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    ALTER TABLE [Orders] ADD [Cart_Id] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    ALTER TABLE [Orders] ADD [Cart_Id1] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    ALTER TABLE [Orders] ADD [CustomProductCProduct_Id] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    ALTER TABLE [OrderDetails] ADD [CustomProductCProduct_Id] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    CREATE TABLE [CustomProduct] (
        [CProduct_Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [Price] decimal(18,2) NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_CustomProduct] PRIMARY KEY ([CProduct_Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    CREATE INDEX [IX_Orders_Cart_Id1] ON [Orders] ([Cart_Id1]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    CREATE INDEX [IX_Orders_CustomProductCProduct_Id] ON [Orders] ([CustomProductCProduct_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    CREATE INDEX [IX_OrderDetails_CustomProductCProduct_Id] ON [OrderDetails] ([CustomProductCProduct_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    ALTER TABLE [OrderDetails] ADD CONSTRAINT [FK_OrderDetails_CustomProduct_CustomProductCProduct_Id] FOREIGN KEY ([CustomProductCProduct_Id]) REFERENCES [CustomProduct] ([CProduct_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    ALTER TABLE [Orders] ADD CONSTRAINT [FK_Orders_CustomProduct_CustomProductCProduct_Id] FOREIGN KEY ([CustomProductCProduct_Id]) REFERENCES [CustomProduct] ([CProduct_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    ALTER TABLE [Orders] ADD CONSTRAINT [FK_Orders_ShoppingCarts_Cart_Id1] FOREIGN KEY ([Cart_Id1]) REFERENCES [ShoppingCarts] ([Cart_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251001071124_UpdatePK'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251001071124_UpdatePK', N'9.0.9');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [Orders] DROP CONSTRAINT [FK_Orders_ShoppingCarts_Cart_Id1];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [ShoppingCartItems] DROP CONSTRAINT [FK_ShoppingCartItems_Products_Product_Id];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [ShoppingCartItems] DROP CONSTRAINT [FK_ShoppingCartItems_ShoppingCarts_Cart_Id];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [ShoppingCarts] DROP CONSTRAINT [FK_ShoppingCarts_Users_User_Id];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [ShoppingCarts] DROP CONSTRAINT [PK_ShoppingCarts];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [ShoppingCartItems] DROP CONSTRAINT [PK_ShoppingCartItems];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    EXEC sp_rename N'[ShoppingCarts]', N'Carts', 'OBJECT';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    EXEC sp_rename N'[ShoppingCartItems]', N'CartItems', 'OBJECT';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    EXEC sp_rename N'[Carts].[IX_ShoppingCarts_User_Id]', N'IX_Carts_User_Id', 'INDEX';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    EXEC sp_rename N'[CartItems].[IX_ShoppingCartItems_Product_Id]', N'IX_CartItems_Product_Id', 'INDEX';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    EXEC sp_rename N'[CartItems].[IX_ShoppingCartItems_Cart_Id]', N'IX_CartItems_Cart_Id', 'INDEX';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [Carts] ADD CONSTRAINT [PK_Carts] PRIMARY KEY ([Cart_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [CartItems] ADD CONSTRAINT [PK_CartItems] PRIMARY KEY ([CartItem_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [CartItems] ADD CONSTRAINT [FK_CartItems_Carts_Cart_Id] FOREIGN KEY ([Cart_Id]) REFERENCES [Carts] ([Cart_Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [CartItems] ADD CONSTRAINT [FK_CartItems_Products_Product_Id] FOREIGN KEY ([Product_Id]) REFERENCES [Products] ([Product_Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [Carts] ADD CONSTRAINT [FK_Carts_Users_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [Users] ([User_Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    ALTER TABLE [Orders] ADD CONSTRAINT [FK_Orders_Carts_Cart_Id1] FOREIGN KEY ([Cart_Id1]) REFERENCES [Carts] ([Cart_Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007153301_editProperties'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251007153301_editProperties', N'9.0.9');
END;

COMMIT;
GO

