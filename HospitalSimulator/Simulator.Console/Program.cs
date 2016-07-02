using System;

namespace Simulator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            uint calenderSize = 365;
            DateTime today = DateTime.Now.Date;

            if (args.Length >= 1)
            {
                uint newCalSize;
                if (uint.TryParse(args[0],out newCalSize))
                {
                    calenderSize = newCalSize;
                }
            }
            var sim = new Simulator();
            sim.Init(today, calenderSize);

            System.Console.WriteLine("Calender size is {0} days.",calenderSize);

            if (args.Length >= 2)
            {
                sim.LoadAndProcessRequestFile(args[1], today);
            }
            else
            {
                sim.StartMenu();                
            }

        }
    }
}
