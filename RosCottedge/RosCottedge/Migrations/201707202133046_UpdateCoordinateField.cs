namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCoordinateField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Houses", "lng", c => c.String());
            DropColumn("dbo.Houses", "Lon");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Houses", "Lon", c => c.String());
            DropColumn("dbo.Houses", "lng");
        }
    }
}
