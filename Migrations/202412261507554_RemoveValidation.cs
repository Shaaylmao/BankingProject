namespace BankingProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ATMFunctions", "DebitCardNumber", c => c.String());
            AlterColumn("dbo.ATMFunctions", "CreditCardNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ATMFunctions", "CreditCardNumber", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("dbo.ATMFunctions", "DebitCardNumber", c => c.String(nullable: false, maxLength: 12));
        }
    }
}
