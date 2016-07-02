using System;

namespace Simulator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            int calenderSize = 365;
            DateTime today = DateTime.Now.Date;

            var sim = new global::Simulator.Console.Simulator();
            sim.Init(today, calenderSize);

            if (args.Length == 2)
            {
                int newCalSize;
                if (int.TryParse(args[1],out newCalSize))
                {
                    calenderSize = newCalSize;
                }
            }

            System.Console.WriteLine("Calender size is {0} days.",calenderSize);

            if (args.Length >= 1)
            {
                sim.LoadAndProcessRequestFile(args[0], today);
            }
            else
            {
                sim.StartMenu();                
            }

        }
    }
}
