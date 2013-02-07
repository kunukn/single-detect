﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using SingleDetectLibrary.Code;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;
using SingleDetectLibrary.Code.StrategyPattern;
using System.Linq;

namespace KNearestNeighborConsole
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
                XMin = -180,
                XMax = 180,
                YMin = -90,
                YMax = 90,
                MaxDistance = 20,
            };
            rect.Validate();

            const int k = 3;

            // Random points
            IPoints points = new Points();
            var rand = new Random();
            for (var i = 0; i < 100; i++)
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
            ISingleDetectAlgorithm algo =
                new SingleDetectAlgorithm(new Points { Data = points.Data }, rect, StrategyType.Grid);

            // Use algo
            var origin = points.Data.First();
            var duration = algo.UpdateKnn(origin, k);

            // Print result
            WL(string.Format("{0} msec. {1}:", algo.Strategy.Name, duration));
            WL("K Nearest Neighbors:");
            WL(string.Format("Origin: {0}", origin));
            WL(string.Format("Distance sum: {0}", algo.Knn.GetDistanceSum()));
            algo.Knn.NNs.Data.OrderBy(i => i.Distance).ToList().ForEach(WL);


            // Update strategy
            algo.SetAlgorithmStrategy(new NaiveStrategy());

            // Use algo
            duration = algo.UpdateKnn(origin, k);

            // Print result
            WL(string.Format("\n{0} msec. {1}:", algo.Strategy.Name, duration));
            WL("K Nearest Neighbors:");
            WL(string.Format("Distance sum: {0}", algo.Knn.GetDistanceSum()));
            algo.Knn.NNs.Data.OrderBy(i => i.Distance).ToList().ForEach(WL);
        }
    }
}
