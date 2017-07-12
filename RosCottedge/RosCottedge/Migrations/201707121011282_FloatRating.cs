namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FloatRating : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Houses", "Rating", c => c.Single(nullable: false));
            AlterColumn("dbo.Reviews", "Rating", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviews", "Rating", c => c.Int(nullable: false));
            AlterColumn("dbo.Houses", "Rating", c => c.Int(nullable: false));
        }
    }
}
