using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BankingProject
{
    public class ATMFunctions
    {
        [Key]
        [Required]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Account number must be exactly 12 digits.")]
        public string AccountNumber { get; set; }

        [ForeignKey("AccountNumber")]
        public virtual AccountMaster AccountMaster { get; set; } // Navigation property

        //[Required]
        //[StringLength(12, MinimumLength = 12, ErrorMessage = "DebitCardNumber must be exactly 12 digits.")]
        public string DebitCardNumber { get; set; }

        //[Required]
        //[StringLength(12, MinimumLength = 12, ErrorMessage = "CreditCardNumber must be exactly 12 digits.")]
        public string CreditCardNumber { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "PIN CODE must be exactly 4 digits.")]
        public string PinCode { get; set; }

        public static string GeneratePinCode()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString("D4");
        }

        public static string GenerateCardNumber()
        {
            Random random = new Random();
            string part1 = random.Next(100000, 999999).ToString("D6");
            string part2 = random.Next(100000, 999999).ToString("D6");
            return part1 + part2;
        }
    }

    public class ATMContext : DbContext
    {
        public ATMContext() : base("name = MyContextDB") { }

        public DbSet<AccountMaster> Accounts { get; set; }
        public DbSet<ATMFunctions> ATMDetails { get; set; }
    }

    internal class ATMProgramMaster
    {
        // Function to retrieve account details
        public static AccountMaster GetAccountDetails(string accountNumber, ATMContext db)
        {
            return db.Accounts.SingleOrDefault(acc => acc.AccountNumber == accountNumber);
        }

        public static void Execute()
        {
            using (var db = new ATMContext())
            {
                while (true)
                {
                    Console.WriteLine("Please Choose an Option - \n1. Generate Debit/Credit Card\n2. Card Operations\n3. Exit");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Console.WriteLine("Enter your Account Number: ");
                        string accountNumber = Console.ReadLine();
                        var accountDetails = GetAccountDetails(accountNumber, db);

                        if (accountDetails != null)
                        {
                            var atmDetails = db.ATMDetails.SingleOrDefault(a => a.AccountNumber == accountNumber);

                            if (atmDetails == null)
                            {
                                atmDetails = new ATMFunctions
                                {
                                    AccountNumber = accountNumber,
                                    DebitCardNumber = accountDetails.AccountType == "Savings" ? ATMFunctions.GenerateCardNumber() : null,
                                    CreditCardNumber = accountDetails.AccountType == "Current" ? ATMFunctions.GenerateCardNumber() : null,
                                    PinCode = ATMFunctions.GeneratePinCode()
                                };

                                db.ATMDetails.Add(atmDetails);
                                db.SaveChanges();

                                Console.WriteLine($"Card Generated:\nDebit Card: {atmDetails.DebitCardNumber ?? "N/A"}\nCredit Card: {atmDetails.CreditCardNumber ?? "N/A"}");
                                Console.WriteLine($"PIN Code: {atmDetails.PinCode}");
                            }
                            else
                            {
                                Console.WriteLine("You already have cards associated with this account.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                    }
                    else if (choice == "2")
                    {
                        Console.WriteLine("Enter your Account Number: ");
                        string accountNumber = Console.ReadLine();
                        var accountDetails = GetAccountDetails(accountNumber, db);

                        if (accountDetails != null)
                        {
                            Console.WriteLine($"Your balance is: {accountDetails.Balance}");
                            Console.WriteLine("1. Deposit Money\n2. Withdraw Money\n3. Change PIN Code\n4. Exit");
                            string operation = Console.ReadLine();

                            if (operation == "1")
                            {
                                Console.WriteLine("Enter deposit amount: ");
                                double amount = double.Parse(Console.ReadLine());
                                accountDetails.Balance += amount;
                                Console.WriteLine($"New Balance: {accountDetails.Balance}");
                                db.SaveChanges();
                            }
                            else if (operation == "2")
                            {
                                Console.WriteLine("Enter withdrawal amount: ");
                                double amount = double.Parse(Console.ReadLine());

                                if (accountDetails.Balance >= amount)
                                {
                                    accountDetails.Balance -= amount;
                                    Console.WriteLine($"New Balance: {accountDetails.Balance}");
                                    db.SaveChanges();
                                }
                                else
                                {
                                    Console.WriteLine("Insufficient balance.");
                                }
                            }
                            else if (operation == "3")
                            {
                                var atmDetails = db.ATMDetails.SingleOrDefault(a => a.AccountNumber == accountNumber);
                                if (atmDetails != null)
                                {
                                    Console.WriteLine("Enter your current PIN Code: ");
                                    string currentPin = Console.ReadLine();

                                    if (atmDetails.PinCode == currentPin)
                                    {
                                        Console.WriteLine("Enter your new PIN Code: ");
                                        string newPin = Console.ReadLine();
                                        atmDetails.PinCode = newPin;
                                        Console.WriteLine("PIN Code updated successfully.");
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Incorrect PIN.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No ATM details found for this account.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Account not found.");
                        }
                    }
                    else if (choice == "3")
                    {
                        break;
                    }
                }
            }
        }
    }
}
