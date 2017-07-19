namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRegistrationDate_HouseAvatar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Houses", "Avatar", c => c.String());
            AddColumn("dbo.Users", "RegistrationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RegistrationDate");
            DropColumn("dbo.Houses", "Avatar");
        }
    }
}
