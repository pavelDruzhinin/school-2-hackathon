namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddbriefDescriptioninthemodelHouse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Houses", "briefDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Houses", "briefDescription");
        }
    }
}
