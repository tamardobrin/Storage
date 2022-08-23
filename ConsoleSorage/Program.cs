using Logic;
using System;


namespace ConsoleSorage
{
    internal class Program
    {
        static Manager manager = new Manager();
        
        static string forBuyer = "\n welcome to te store. please tell me the size of the box you are looking for, and the amount you would like to buy.";
        static  string forSupplier = "\n welcome to te storage. please tell me the size of the box you are suppling , and the amount you would like to suplly.";
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("please choose command: \n supply \n buy \n exit");

                String command = Console.ReadLine();
                switch (command)
                {
                    case "exit":
                        return; 
                    case "buy":
                        WantsToBuy();
                        break;
                    case "supply":
                        WantsToSupply();
                            break;
                }
            }
        }
        static void WantsToSupply()
        {
            manager.Supply(InsertBase(forSupplier), InsertHeight(), InsertAmount(), Display, Console.WriteLine);
        }
        static void WantsToBuy()
        {
            manager.Buy(InsertBase(forBuyer), InsertHeight(), InsertAmount(), Display, Console.WriteLine);
        }
        static bool Display(string nnn)
        {
            string answer = "";
            do
            {
                Console.WriteLine(nnn);
                answer = Console.ReadLine();
                if (answer == "yes") return true;
                if (answer == "no") return false;
            } while (answer != "yes" || answer != "no");
            return false;
        }
        static double InsertBase(string forr)
        {
            double baseX = 0;
            Console.WriteLine(forr);
            try
            {
                double.TryParse(Console.ReadLine(), out baseX);
            }
            catch (Exception)
            {
                Console.WriteLine("wrong input. please type the size of the base of the box you want in numbers:)");
                InsertBase(forr);
            }
            return baseX;
        }
        static double InsertHeight()
        {
            double heightY = 0;
            try
            {
                double.TryParse(Console.ReadLine(), out heightY);
            }
            catch (Exception)
            {
                Console.WriteLine("wrong input. please type the size of the height of the box you want in numbers:)");
                InsertHeight();
            }
            return heightY;
        }
        static int InsertAmount()
        {
            int amount = 0;
            try
            {
                int.TryParse(Console.ReadLine(), out amount);
            }
            catch (Exception)
            {
                Console.WriteLine("wrong input. please type the size of the amount of boxes you want in numbers:)");
                InsertAmount();
            }
            return amount;
        }
    }
}
