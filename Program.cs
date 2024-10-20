using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

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
        public DbSet<AccountMaster> Accounts { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AccountContext())
            {
                for (; ; )
                {
                    Console.WriteLine("Please choose an option - 1. Create New Account 2. Exit");
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

                        Console.Write("Enter your account type (Savings or Current): ");
                        acc.AccountType = Console.ReadLine();

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
                        break; ;
                    }
                }
            }
        }
    }
}
