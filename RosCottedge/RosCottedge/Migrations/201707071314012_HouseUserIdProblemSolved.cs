namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HouseUserIdProblemSolved : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Houses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                        NumberOfPersons = c.Int(nullable: false),
                        Region = c.String(),
                        Locality = c.String(),
                        Area = c.String(),
                        HouseNumber = c.String(),
                        Description = c.String(),
                        Food = c.String(),
                        Transfer = c.String(),
                        ServicesIncluded = c.String(),
                        AdditionalServices = c.String(),
                        Accomodations = c.String(),
                        BookingConditions = c.String(),
                        Rating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReservationDate = c.DateTime(nullable: false),
                        ArrivalDate = c.DateTime(nullable: false),
                        DepartureDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        HouseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Houses", t => t.HouseId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.HouseId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        Rating = c.String(),
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
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reviews", "HouseId", "dbo.Houses");
            DropForeignKey("dbo.Reservations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reservations", "HouseId", "dbo.Houses");
            DropForeignKey("dbo.Houses", "UserId", "dbo.Users");
            DropIndex("dbo.Reviews", new[] { "HouseId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.Reservations", new[] { "HouseId" });
            DropIndex("dbo.Reservations", new[] { "UserId" });
            DropIndex("dbo.Houses", new[] { "UserId" });
            DropTable("dbo.Reviews");
            DropTable("dbo.Reservations");
            DropTable("dbo.Users");
            DropTable("dbo.Houses");
        }
    }
}
