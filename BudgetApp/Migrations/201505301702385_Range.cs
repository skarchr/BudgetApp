namespace BudgetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Range : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Range", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Range");
        }
    }
}
