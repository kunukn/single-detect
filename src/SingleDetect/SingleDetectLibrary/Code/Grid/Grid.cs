using System.Collections.Generic;
using System.Linq;
using SingleDetectLibrary.Code.Data;

namespace SingleDetectLibrary.Code.Grid
{
    public class Grid
    {
        public MySet<P>[,] Set { get; private set; }
        public readonly int X;
        public readonly int Y;        

        public Grid(int x, int y)
        {
            X = x;
            Y = y;            

            Set = new MySet<P>[X, Y];
            for (var i = 0; i < X; i++)
            {
                for (var j = 0; j < Y; j++)
                {
                    Set[i, j] = new MySet<P>();
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

        public List<P> GetSingles(GridContainer gc)
        {
            var singles = new List<P>();
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

        public MySet<P> Get(GridIndex i)
        {
            return Get(i.X, i.Y);
        }

        public MySet<P> Get(int x, int y)
        {
            return Set[x, y];
        }
    }
}
