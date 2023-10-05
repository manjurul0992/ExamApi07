namespace ExamApi07.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class A : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        DeviceName = c.String(nullable: false, maxLength: 50),
                        ReleaseDate = c.DateTime(nullable: false, storeType: "date"),
                        Price = c.Decimal(nullable: false, storeType: "money"),
                        OnSale = c.Boolean(nullable: false),
                        Picture = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.DeviceId);
            
            CreateTable(
                "dbo.Specs",
                c => new
                    {
                        SpecId = c.Int(nullable: false, identity: true),
                        SpecName = c.String(),
                        Value = c.String(),
                        DeviceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SpecId)
                .ForeignKey("dbo.Devices", t => t.DeviceId, cascadeDelete: true)
                .Index(t => t.DeviceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Specs", "DeviceId", "dbo.Devices");
            DropIndex("dbo.Specs", new[] { "DeviceId" });
            DropTable("dbo.Specs");
            DropTable("dbo.Devices");
        }
    }
}
