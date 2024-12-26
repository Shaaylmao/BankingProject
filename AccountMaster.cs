using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BankingProject
{
    public class AccountMaster
    {
        [Key]
        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Account number must be exactly 12 digits.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Account number must be exactly 12 digits.")]
        public string AccountNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "AadharCard must be exactly 12 digits.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "AadharCard must be exactly 12 digits.")]
        public string AadharCardID { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]$", ErrorMessage = "PAN Card must be 5 letters, followed by 4 digits, and end with 1 letter.")]
        public string PANCardID { get; set; }
        [Required]
        [RegularExpression(@"Savings|Current", ErrorMessage = "Account type must be either Savings or Current.")]
        public string AccountType { get; set; }
        public DateTime DateOfOpening { get; set; } = DateTime.Now;
        public string BankBranch { get; set; }
        public DateTime? DateOfModified { get; set; }
        public DateTime? DateOfClosing { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "PhoneNumber must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "PhoneNumber must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; } = true;
        [Required]
        [Range(5000, double.MaxValue, ErrorMessage = "Balance must be at least 5000 rupees.")]
        public double Balance { get; set; }
        public string NomineeName { get; set; }
        [Required]
        [RegularExpression(@"Parent|Sibling|Child|Spouse", ErrorMessage = "Nominee relation must be either Parent, Sibling, Child or Spouse.")]
        public string NomineeRelation { get; set; }

        public static string GenerateAccountNumber()
        {

            Random random = new Random();
            string accountNumber = (random.Next(100000000, 999999999) * 10000 + random.Next(0, 10000)).ToString("D12");
            return accountNumber;
        }

    }
    public class AccountContext : DbContext
    {
        public AccountContext() : base("name = MyContextDB") { }
        public DbSet<AccountMaster> Accounts { get; set; }

        public DbSet<ATMFunctions> ATMDetails { get; set; }
    }
    internal class ProgramAccountMaster
    {
        public static void Execute()
        {
            using (var db = new AccountContext())
            {
                
                for (; ; )
                {
                    Console.WriteLine("Please choose an option - 1. Create New Account 2. Update Account Details  3. Deactivate Account 4. Exit");
                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        var acc = new AccountMaster();

                        Console.Write("Enter your first name: ");
                        acc.FirstName = Console.ReadLine();

                        Console.Write("Enter your last name: ");
                        acc.LastName = Console.ReadLine();

                        acc.AccountNumber = AccountMaster.GenerateAccountNumber();

                        Console.Write("Enter your Aadhar Card ID (12 digits): ");
                        acc.AadharCardID = Console.ReadLine();

                        Console.Write("Enter your PAN Card ID: ");
                        acc.PANCardID = Console.ReadLine();

                        Console.Write("Enter your Date of birth: ");
                        acc.DateOfBirth = Convert.ToDateTime(Console.ReadLine());

                        Console.Write("Enter your Gender: ");
                        acc.Gender = Console.ReadLine();

                        Console.Write("Enter your account type (Savings or Current): ");
                        acc.AccountType = Console.ReadLine();

                        Console.Write("Enter your Bank Branch: ");
                        acc.BankBranch = Console.ReadLine();

                        Console.Write("Enter your phone number (10 digits): ");
                        acc.PhoneNumber = Console.ReadLine();

                        Console.Write("Enter your balance: ");
                        acc.Balance = double.Parse(Console.ReadLine());

                        Console.Write("Enter your nominee name: ");
                        acc.NomineeName = Console.ReadLine();

                        Console.Write("Enter your nominee relation (Parent, Sibling, Child, Spouse): ");
                        acc.NomineeRelation = Console.ReadLine();

                        acc.DateOfOpening = DateTime.Now;

                        db.Accounts.Add(acc);
                        db.SaveChanges();
                        Console.WriteLine("Account successfully created.");
                    }
                    if (choice == "2")
                    {
                        Console.WriteLine("Enter AccountNumber to update:");
                        string inputtedAcNo = Console.ReadLine();
                        var AcToUpdate = db.Accounts.Find(inputtedAcNo);

                        if (AcToUpdate != null)
                        {
                            Console.WriteLine("Please choose an option - 1. Update Name 2. Update Account Type");
                            string UpdateChoice = Console.ReadLine();
                            if (UpdateChoice == "1")
                            {
                                Console.WriteLine("Enter new first name.");
                                string newfirstname = Console.ReadLine();
                                AcToUpdate.FirstName = newfirstname;
                                Console.WriteLine("Enter new last name.");
                                string newlastname = Console.ReadLine();
                                AcToUpdate.LastName = newlastname;
                            }
                            if (UpdateChoice == "2")
                            {
                                Console.WriteLine("Your current account type is: " + AcToUpdate.AccountType);

                                if (AcToUpdate.AccountType == "Savings")
                                {
                                    Console.WriteLine("Would you like to change the account type to 'Current'? (Y/N)");
                                    choice = Console.ReadLine();

                                    if (choice == "Y")
                                    {
                                        AcToUpdate.AccountType = "Current";
                                        Console.WriteLine("Account type updated to 'Current'.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No changes made to the account type.");
                                    }
                                }
                                else if (AcToUpdate.AccountType == "Current")
                                {
                                    Console.WriteLine("Would you like to change the account type to 'Savings'? (Y/N)");
                                    choice = Console.ReadLine();

                                    if (choice == "Y")
                                    {
                                        AcToUpdate.AccountType = "Savings";
                                        Console.WriteLine("Account type updated to 'Savings'.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("No changes made to the account type.");
                                    }
                                }
                            }
                            db.SaveChanges();
                            Console.WriteLine("Account details updated successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Account Number not found.");
                        }
                    }
                    if (choice == "3")
                    {
                        Console.WriteLine("Enter Account Number to deactivate:");
                        string inputtedAcNo = Console.ReadLine();
                        var AccountToDeactivate = db.Accounts.Find(inputtedAcNo);

                        if (AccountToDeactivate != null)
                        {
                            if (AccountToDeactivate.IsActive)
                            {
                                AccountToDeactivate.IsActive = false;
                                Console.WriteLine("Account deactivated successfully!");
                            }
                            else
                            {
                                Console.WriteLine("Account is already deactivated. Would you like to reactivate it? (Y/N)");
                                string reactivateChoice = Console.ReadLine();
                                if (reactivateChoice == "Y")
                                {
                                    AccountToDeactivate.IsActive = true;
                                    Console.WriteLine("Account reactivated successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("No changes made to the account.");
                                }
                            }

                            db.SaveChanges();
                            Console.WriteLine("Account details updated successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Account Number not found.");
                        }
                    }
                    if (choice == "4")
                    {
                        break; ;
                    }
                }
            }
        }
    }
}
