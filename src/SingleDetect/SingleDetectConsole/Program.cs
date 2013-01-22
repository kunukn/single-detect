using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using SingleDetectLibrary.Code;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.StrategyPattern;

namespace SingleDetectConsole
{
    /// <summary>
    /// Author: Kunuk Nykjaer    
    /// MIT license        
    /// </summary>
    class Program
    {
        private static readonly Action<object> WL = Console.WriteLine;

        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            var sw = new Stopwatch();
            sw.Start();

            Run();
            
            sw.Stop();

            WL(string.Format("\nElapsed: {0}", sw.Elapsed.ToString()));
            WL(string.Format("Press a key to exit ... "));
            Console.ReadKey();
        }         

        static void Run()
        {
            // Config
            var rect = new Rectangle
                           {
                               XMin = 0, XMax = 400, 
                               YMin = 0, YMax = 500,
                               MaxDistance = 10,
                           };       
     
            // Random points
            var points = new List<P>();
            var rand = new Random();
            for (var i = 0; i < 5000; i++)
            {
                points.Add(new P
                {
                    X = rand.Next((int)rect.Width),
                    Y = rand.Next((int)rect.Height),
                });
            }

            // Init algo
            ISingleDetectAlgorithm algo =
                new SingleDetectAlgorithm(points, rect, StrategyType.Grid);

            // Use algo
            var duration = algo.UpdateSingles();

            // Print result
            WL(string.Format("{0} msec. {1}:", algo.Strategy.Name, duration));
            WL("Singles:\n");
            algo.Singles.ForEach(WL);

            // Update strategy
            algo.SetAlgorithmStrategy(new NaiveStrategy());

            // Use algo
            duration = algo.UpdateSingles();

            // Print result
            WL(string.Format("\n{0} msec. {1}:", algo.Strategy.Name, duration));
            WL("Singles:\n");
            algo.Singles.ForEach(WL);
        }
    }
}
