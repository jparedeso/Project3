namespace Project3.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeToDateTimeOffset : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Joined", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Joined", c => c.DateTime(nullable: false));
        }
    }
}
