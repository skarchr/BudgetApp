namespace BudgetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Goals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MonthlySavingGoal", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "MonthlyExpensesGoal", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MonthlyExpensesGoal");
            DropColumn("dbo.AspNetUsers", "MonthlySavingGoal");
        }
    }
}
