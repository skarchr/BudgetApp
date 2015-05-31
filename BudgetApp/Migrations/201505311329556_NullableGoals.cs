namespace BudgetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableGoals : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "MonthlySavingGoal", c => c.Double());
            AlterColumn("dbo.AspNetUsers", "MonthlyExpensesGoal", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "MonthlyExpensesGoal", c => c.Double(nullable: false));
            AlterColumn("dbo.AspNetUsers", "MonthlySavingGoal", c => c.Double(nullable: false));
        }
    }
}
