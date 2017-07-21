namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePersonalArea : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReservDelNotices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReservationDate = c.DateTime(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                        DepartureDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        UserId = c.Int(nullable: false),
                        HouseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Houses", t => t.HouseId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.HouseId);
            
            AddColumn("dbo.Users", "OldPassword", c => c.String());
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "MiddleName", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Phone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReservDelNotices", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReservDelNotices", "HouseId", "dbo.Houses");
            DropIndex("dbo.ReservDelNotices", new[] { "HouseId" });
            DropIndex("dbo.ReservDelNotices", new[] { "UserId" });
            AlterColumn("dbo.Users", "Phone", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "LastName", c => c.String());
            AlterColumn("dbo.Users", "MiddleName", c => c.String());
            AlterColumn("dbo.Users", "FirstName", c => c.String());
            DropColumn("dbo.Users", "OldPassword");
            DropTable("dbo.ReservDelNotices");
        }
    }
}
