using System;

namespace VotingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                Voting voit = new Voting(args[0]);
                Console.WriteLine(voit.StsartVoiting());
            }
            else
            {
                VrongParams();
            }

            if (args.Length > 1 && args[1] == "-e")
            {
                Console.WriteLine("Ошибки в процессе выполнения: ");
                Console.WriteLine(Logger.Log);
            }
            Console.Read();
        }

        static void VrongParams()
        {
            Console.WriteLine("Неверные параметры");
        }
    }
}
