namespace BankingProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNewAccountInit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccountMasters", "DateOfModified", c => c.DateTime());
            AlterColumn("dbo.AccountMasters", "DateOfClosing", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccountMasters", "DateOfClosing", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AccountMasters", "DateOfModified", c => c.DateTime(nullable: false));
        }
    }
}
