namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hide : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Houses", "Hide", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Houses", "Hide");
        }
    }
}
