namespace BudgetApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mappings",
                c => new
                    {
                        MappingId = c.Int(nullable: false, identity: true),
                        UserName = c.Int(nullable: false),
                        Category = c.Int(),
                        TextDescription = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MappingId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Amount = c.Double(nullable: false),
                        Category = c.Int(),
                        Description = c.String(),
                        TransactionDate = c.DateTime(nullable: false),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transactions");
            DropTable("dbo.Mappings");
        }
    }
}
