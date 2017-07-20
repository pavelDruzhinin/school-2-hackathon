namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoordinates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Houses", "Lat", c => c.String());
            AddColumn("dbo.Houses", "Lon", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Houses", "Lon");
            DropColumn("dbo.Houses", "Lat");
        }
    }
}
