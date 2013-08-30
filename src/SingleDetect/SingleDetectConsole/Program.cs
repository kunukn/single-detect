using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Kunukn.SingleDetectLibrary.Code;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;
using Kunukn.SingleDetectLibrary.Code.StrategyPattern;
using System.Linq;

namespace Kunukn.SingleDetectConsole
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
      
        /// <summary>
        /// Run fast and slow version with same  test data and display the running time
        /// </summary>
        static void Run()
        {
            // Config, test data showing ability to use negative positions
            var rect = new Rectangle
                           {
                               XMin = -300, XMax = 300, 
                               YMin = -200, YMax = 200,
                               MaxDistance = 10,
                           };
            rect.Validate();
     
            // Random points
            IPoints points = new Points();
            var rand = new Random();
            for (var i = 0; i < 5000; i++)
            {
                var x = rect.XMin + rand.NextDouble() * rect.Width;
                var y = rect.YMin + rand.NextDouble() * rect.Height;
                points.Data.Add(new P
                {
                    X = x,
                    Y = y,
                });
            }
            points.Round(3);

            // Init algo
            IAlgorithm algo =
                new Algorithm(points, rect, StrategyType.Grid);

            // Use algo
            var duration = algo.UpdateSingles();

            // Print result
            WL(string.Format("{0} msec. {1}:", algo.Strategy.Name, duration));
            WL("Singles:\n");
            algo.Singles.OrderBy(i => i.Uid).ToList().ForEach(WL);

            // Update strategy
            algo.SetAlgorithmStrategy(new NaiveStrategy());

            // Use algo
            duration = algo.UpdateSingles();

            // Print result
            WL(string.Format("\n{0} msec. {1}:", algo.Strategy.Name, duration));
            WL("Singles:\n");
            algo.Singles.OrderBy(i => i.Uid).ToList().ForEach(WL);
        }
    }
}
