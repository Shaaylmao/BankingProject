namespace BankingProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ATMTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ATMFunctions",
                c => new
                    {
                        AccountNumber = c.String(nullable: false, maxLength: 12),
                        DebitCardNumber = c.String(nullable: false, maxLength: 12),
                        CreditCardNumber = c.String(nullable: false, maxLength: 12),
                        PinCode = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.AccountNumber)
                .ForeignKey("dbo.AccountMasters", t => t.AccountNumber)
                .Index(t => t.AccountNumber);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ATMFunctions", "AccountNumber", "dbo.AccountMasters");
            DropIndex("dbo.ATMFunctions", new[] { "AccountNumber" });
            DropTable("dbo.ATMFunctions");
        }
    }
}
