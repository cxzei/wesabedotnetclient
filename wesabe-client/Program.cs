using System;
using System.Collections.Generic;
using System.Text;
using websabelib;

namespace wesabe_client
{
    class Program
    {
        /// <summary>
        /// This is just a start...
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Accounts response = websabelib.wesabe_rest.getAccounts(args[0], args[1]);

            Console.WriteLine("Account Name                                Balance");
            Console.WriteLine("-----------------------------------  --------------");
            foreach (Account a in response.Items)
            {
                Console.WriteLine(String.Format("{0}{1}",a.name.PadRight(35),a.currentBalance.ToString("C").PadLeft(16)));
            }

        }
    }
}
