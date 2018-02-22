namespace saasdemo.data.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Transaction_ID = c.Guid(nullable: false),
                        Transaction_Date = c.DateTime(nullable: false),
                        Product = c.String(),
                        Price = c.Single(nullable: false),
                        Payment_Type = c.String(),
                        Name = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        Account_Created = c.DateTime(nullable: false),
                        Last_Login = c.DateTime(nullable: false),
                        Latitude = c.Single(nullable: false),
                        Longitude = c.Single(nullable: false),
                        Created_Date = c.DateTime(nullable: false),
                        Updated_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Transaction_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sales");
        }
    }
}
