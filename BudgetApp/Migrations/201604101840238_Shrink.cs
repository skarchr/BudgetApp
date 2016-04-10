namespace BudgetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Shrink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Shrink", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Shrink");
        }
    }
}
