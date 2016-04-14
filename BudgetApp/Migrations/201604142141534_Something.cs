namespace BudgetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Something : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Something", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Something");
        }
    }
}
