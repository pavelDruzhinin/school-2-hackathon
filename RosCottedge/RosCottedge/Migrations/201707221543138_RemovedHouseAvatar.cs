namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedHouseAvatar : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Houses", "Avatar");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Houses", "Avatar", c => c.String());
        }
    }
}
