using System;
using System.Linq;

namespace HospitalSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            int calenderSize = 365;
            DateTime today = DateTime.Now.Date;

            var sim = new Simulator();
            sim.Init(today, calenderSize);

            if (args.Length == 1)
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
