namespace BudgetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Mappings", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Mappings", "UserName", c => c.Int(nullable: false));
        }
    }
}
