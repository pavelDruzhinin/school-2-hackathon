namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntRating : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Houses", "Rating", c => c.Int(nullable: false));
            AlterColumn("dbo.Reviews", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviews", "Rating", c => c.String());
            AlterColumn("dbo.Houses", "Rating", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
