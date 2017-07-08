namespace RosCottedge.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReservationDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "Description");
        }
    }
}
