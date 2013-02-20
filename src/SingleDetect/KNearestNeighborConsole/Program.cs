using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Kunukn.SingleDetectLibrary.Code;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;
using Kunukn.SingleDetectLibrary.Code.StrategyPattern;
using System.Linq;
using P = Kunukn.SingleDetectLibrary.Code.Data.P;
using Points = Kunukn.SingleDetectLibrary.Code.Data.Points;
using Rectangle = Kunukn.SingleDetectLibrary.Code.Data.Rectangle;

namespace Kunukn.KNearestNeighborConsole
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
                XMin = -200,
                XMax = 200,
                YMin = -100,
                YMax = 100,
                MaxDistance = 20,
            };
            rect.Validate();

            var conf = new KnnConfiguration { K = 4 };

            // Random points
            IPoints points = new Points();
            var rand = new Random();
            for (var i = 0; i < 500000; i++)
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
            
            var origin = new P { X = 0, Y = 0 };                        
            var duration = algo.UpdateKnn(origin, conf);

            // Print result
            WL(string.Format("{0} msec. {1}:", algo.Strategy.Name, duration));
            WL("K Nearest Neighbors:");
            WL(string.Format("Origin: {0}", origin));
            WL(string.Format("Distance sum: {0}", algo.Knn.GetDistanceSum()));
            algo.Knn.NNs.OrderBy(i => i.Distance).ToList().ForEach(WL);


            // Update strategy
            algo.SetAlgorithmStrategy(new NaiveStrategy());

            // Use algo
            duration = algo.UpdateKnn(origin, conf);

            // Print result
            WL(string.Format("\n{0} msec. {1}:", algo.Strategy.Name, duration));
            WL("K Nearest Neighbors:");
            WL(string.Format("Distance sum: {0}", algo.Knn.GetDistanceSum()));
            algo.Knn.NNs.OrderBy(i => i.Distance).ToList().ForEach(WL);
        }
    }
}
