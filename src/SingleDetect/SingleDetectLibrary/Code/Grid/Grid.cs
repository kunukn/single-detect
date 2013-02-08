using System;
using System.Collections.Generic;
using System.Linq;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectLibrary.Code.Grid
{
    public class Grid
    {
        public MySet<IP>[,] Set { get; private set; }
        public readonly int X;
        public readonly int Y;

        public Grid(int x, int y)
        {
            X = x;
            Y = y;

            Set = new MySet<IP>[X, Y];
            for (var i = 0; i < X; i++)
            {
                for (var j = 0; j < Y; j++)
                {
                    Set[i, j] = new MySet<IP>();
                }
            }
        }

        public void Clear()
        {
            for (var i = 0; i < X; i++)
            {
                for (var j = 0; j < Y; j++)
                {
                    Set[i, j].Clear();
                }
            }
        }

        public List<IP> GetSingles(GridContainer gc)
        {
            var singles = new List<IP>();
            for (var i = 0; i < X; i++)
            {
                for (var j = 0; j < Y; j++)
                {
                    var set = Set[i, j];
                    if (set.Count != 1) continue;

                    var p = set.First();
                    singles.Add(p);
                }
            }
            return singles;
        }

        public MySet<IP> Get(GridIndex i)
        {
            return Get(i.X, i.Y);
        }

        public MySet<IP> Get(int x, int y)
        {
            try
            {
                return Set[x, y];
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
