namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReservationNotices",
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
            
            CreateTable(
                "dbo.ReviewsNotices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        Rating = c.Single(nullable: false),
                        CommentDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        HouseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Houses", t => t.HouseId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.HouseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReviewsNotices", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReviewsNotices", "HouseId", "dbo.Houses");
            DropForeignKey("dbo.ReservationNotices", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReservationNotices", "HouseId", "dbo.Houses");
            DropIndex("dbo.ReviewsNotices", new[] { "HouseId" });
            DropIndex("dbo.ReviewsNotices", new[] { "UserId" });
            DropIndex("dbo.ReservationNotices", new[] { "HouseId" });
            DropIndex("dbo.ReservationNotices", new[] { "UserId" });
            DropTable("dbo.ReviewsNotices");
            DropTable("dbo.ReservationNotices");
        }
    }
}
