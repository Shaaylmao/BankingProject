namespace BankingProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountMasters",
                c => new
                    {
                        AccountNumber = c.String(nullable: false, maxLength: 12),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        AadharCardID = c.String(nullable: false, maxLength: 12),
                        PANCardID = c.String(nullable: false),
                        AccountType = c.String(nullable: false),
                        DateOfOpening = c.DateTime(nullable: false),
                        BankBranch = c.String(),
                        DateOfModified = c.DateTime(nullable: false),
                        DateOfClosing = c.DateTime(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 10),
                        Gender = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Balance = c.Double(nullable: false),
                        NomineeName = c.String(),
                        NomineeRelation = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AccountNumber);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccountMasters");
        }
    }
}
