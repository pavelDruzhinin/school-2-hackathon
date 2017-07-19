namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateLK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReservationNotices", "HouseId", "dbo.Houses");
            DropForeignKey("dbo.ReservationNotices", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReviewsNotices", "HouseId", "dbo.Houses");
            DropForeignKey("dbo.ReviewsNotices", "UserId", "dbo.Users");
            DropIndex("dbo.ReservationNotices", new[] { "UserId" });
            DropIndex("dbo.ReservationNotices", new[] { "HouseId" });
            DropIndex("dbo.ReviewsNotices", new[] { "UserId" });
            DropIndex("dbo.ReviewsNotices", new[] { "HouseId" });
            AddColumn("dbo.Reservations", "Tenant", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reservations", "Landlord", c => c.Boolean(nullable: false));
            AddColumn("dbo.Reviews", "Landlord", c => c.Boolean(nullable: false));
            DropTable("dbo.ReservationNotices");
            DropTable("dbo.ReviewsNotices");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Reviews", "Landlord");
            DropColumn("dbo.Reservations", "Landlord");
            DropColumn("dbo.Reservations", "Tenant");
            CreateIndex("dbo.ReviewsNotices", "HouseId");
            CreateIndex("dbo.ReviewsNotices", "UserId");
            CreateIndex("dbo.ReservationNotices", "HouseId");
            CreateIndex("dbo.ReservationNotices", "UserId");
            AddForeignKey("dbo.ReviewsNotices", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ReviewsNotices", "HouseId", "dbo.Houses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ReservationNotices", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ReservationNotices", "HouseId", "dbo.Houses", "Id", cascadeDelete: true);
        }
    }
}
