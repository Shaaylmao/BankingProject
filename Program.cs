using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace BankingProject
{
    class Run
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please Choose your operation 1. Account Operations \t 2. Card Operations");
            string opchoice = Console.ReadLine();
            if (opchoice == "1") { ProgramAccountMaster.Execute(); }
            if (opchoice == "2") {ATMProgramMaster.Execute(); }
            
        }
    }
}

