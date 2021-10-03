namespace ChangeUserProfileSample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProfileData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Dob", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Height", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Weight");
            DropColumn("dbo.AspNetUsers", "Height");
            DropColumn("dbo.AspNetUsers", "Dob");
        }
    }
}
