using System;
using System.Collections.Generic;
using System.Diagnostics;
using SingleDetectLibrary.Code;
using SingleDetectLibrary.Code.Contract;
using SingleDetectLibrary.Code.Data;

namespace SingleDetectConsole
{
    class Program
    {
        private static readonly Action<object> WL = Console.WriteLine;

        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            Run();

            sw.Stop();

            WL(string.Format("\nElapsed: {0}", sw.Elapsed.ToString()));
            WL(string.Format("\nPress a key to exit ... "));
            Console.ReadKey();
        }


        static void Run()
        {
            var rect = new Rectangle {XMin = 0, XMax = 400, YMin = 0, YMax = 500};
            const double distance = 33.3;
            var points = new List<P>();
            var rand = new Random();

            for (var i = 0; i < 100; i++)
            {
                points.Add(new P
                {
                    X = rand.Next((int)rect.Width),
                    Y = rand.Next((int)rect.Height),
                });
            }

            ISingleDetectAlgorithm algo = 
                new SingleDetectAlgorithm(points, rect, distance);

            algo.UpdateSingles();

            WL(string.Format("Singles:\n"));
            algo.Singles.ForEach(WL);
        }
    }
}
