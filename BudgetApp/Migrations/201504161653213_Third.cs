namespace BudgetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Third : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Transactions", "Import", c => c.Boolean(nullable: false));
            DropColumn("dbo.Transactions", "TransactionDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "TransactionDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Transactions", "Import");
            DropColumn("dbo.Transactions", "Date");
        }
    }
}
