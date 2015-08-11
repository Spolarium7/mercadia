namespace Mercadia.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        CategoryId = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderId = c.Guid(nullable: false),
                        StoreId = c.Guid(nullable: false),
                        StoreName = c.String(),
                        UserId = c.Guid(nullable: false),
                        UserName = c.String(),
                        StoreOwnerId = c.Guid(nullable: false),
                        StoreOwnerName = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        ProductName = c.String(),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        ItemPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Timestamp = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StoreId = c.Guid(nullable: false),
                        StoreName = c.String(),
                        UserId = c.Guid(nullable: false),
                        UserName = c.String(),
                        StoreOwnerId = c.Guid(nullable: false),
                        StoreOwnerName = c.String(),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TaxDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentStatus = c.Int(nullable: false),
                        PaymentDetails = c.String(),
                        PaymentReference = c.String(),
                        PayDate = c.DateTime(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Sku = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        CategoryId = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Timestamp = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Address = c.String(),
                        ZipCode = c.String(),
                        LocationLat = c.String(),
                        LocationLong = c.String(),
                        Template = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreSettings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StoreId = c.Guid(nullable: false),
                        Key = c.String(),
                        Value = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Phone = c.String(),
                        DeliveryAddress = c.String(),
                        DeliveryState = c.String(),
                        DeliveryCountry = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.StoreSettings");
            DropTable("dbo.Stores");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
            DropTable("dbo.Categories");
        }
    }
}
