using System.Collections.Generic;
using System.Linq;
using SingleDetectLibrary.Code.Data;

namespace SingleDetectLibrary.Code.Grid
{
    public class GridContainer
    {
        private Grid Grid { get; set; }
        private double DX { get; set; } // delta x
        private double DY { get; set; } // delta y
        private readonly Rectangle _rect;

        public GridContainer(Rectangle rect, IEnumerable<P> points)
        {
            _rect = rect;
            DX = rect.Square;
            DY = rect.Square;
            Grid = new Grid(rect.XGrid, rect.YGrid);
            foreach (var p in points) Update(p);
        }

        protected GridIndex Delta(XY a)
        {
            return new GridIndex { X = (int)(a.X / DX), Y = (int)(a.Y / DY) };
        }

        protected MySet<P> GetSet(P p)
        {
            return this.Grid.Get(p.GridIndex);
        }

        // Single detect
        public List<P> GetGridNeighborContent(P p)
        {
            var i = p.GridIndex;

            // nearest neighbor
            var n = new GridIndex { X = i.X, Y = i.Y - 1 };
            var ne = new GridIndex { X = i.X + 1, Y = i.Y - 1 };
            var e = new GridIndex { X = i.X + 1, Y = i.Y };
            var se = new GridIndex { X = i.X + 1, Y = i.Y + 1 };
            var s = new GridIndex { X = i.X, Y = i.Y + 1 };
            var sw = new GridIndex { X = i.X - 1, Y = i.Y + 1 };
            var w = new GridIndex { X = i.X - 1, Y = i.Y };
            var nw = new GridIndex { X = i.X - 1, Y = i.Y - 1 };

            // outer north
            var nnw = new GridIndex { X = i.X - 1, Y = i.Y - 2 };
            var nn = new GridIndex { X = i.X, Y = i.Y - 2 };
            var nne = new GridIndex { X = i.X + 1, Y = i.Y - 2 };

            // outer east
            var nee = new GridIndex { X = i.X + 2, Y = i.Y - 1 };
            var ee = new GridIndex { X = i.X + 2, Y = i.Y };
            var see = new GridIndex { X = i.X + 2, Y = i.Y + 1 };

            // outer south
            var sse = new GridIndex { X = i.X + 1, Y = i.Y + 2 };
            var ss = new GridIndex { X = i.X, Y = i.Y + 2 };
            var ssw = new GridIndex { X = i.X - 1, Y = i.Y + 2 };

            // outer west
            var sww = new GridIndex { X = i.X - 2, Y = i.Y - 1 };
            var ww = new GridIndex { X = i.X - 2, Y = i.Y };
            var nww = new GridIndex { X = i.X - 2, Y = i.Y + 1 };

            var list = new List<P>();
            GridIndex gi;
            if (IsValidGridIndex(gi = n)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = ne)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = e)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = se)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = s)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = sw)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = w)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = nw)) list.AddRange(Grid.Get(gi));

            if (IsValidGridIndex(gi = nnw)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = nn)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = nne)) list.AddRange(Grid.Get(gi));

            if (IsValidGridIndex(gi = nee)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = ee)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = see)) list.AddRange(Grid.Get(gi));

            if (IsValidGridIndex(gi = sse)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = ss)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = ssw)) list.AddRange(Grid.Get(gi));

            if (IsValidGridIndex(gi = sww)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = ww)) list.AddRange(Grid.Get(gi));
            if (IsValidGridIndex(gi = nww)) list.AddRange(Grid.Get(gi));

            return list;
        }

        bool IsValidGridIndex(GridIndex i)
        {
            if (i.X < 0 || i.X >= Grid.X) return false;
            if (i.Y < 0 || i.Y >= Grid.Y) return false;
            return true;
        }

        public void Update(P p)
        {
            var set = GetSet(p);
            var b = set.Remove(p); // remove
            var d = Delta(p);
            p.GridIndex = d; // update ref
            set = GetSet(p);
            set.Add(p);  // add
        }

        public List<P> GetGridSingles()
        {
            return this.Grid.GetSingles(this);
        }

        // K nearest neighbor
        public void UpdateKnnGridStrategy(NearestNeighbor nn, double square, int max)
        {
            var currRing = new List<PDist>();
            var nextRing = new List<PDist>();

            for (var i = 1; i <= max; i++)
            {
                var temp = new List<PDist>();
                foreach (var p in nextRing)
                {
                    if (p.Distance < i * square) currRing.Add(p);
                    else temp.Add(p);
                }
                if (currRing.Count >= nn.K) break;

                nextRing.Clear();
                nextRing.AddRange(temp);

                var list = GetRing(nn.Origin, i);

                // First 9 squares, dont include origin
                if (i == 1) list.AddRange(GetSet(nn.Origin).Where(a => !a.Equals(nn.Origin)).ToList());

                foreach (var p in list)
                {
                    var dist = nn.Origin.Distance(p);
                    if (dist < i * square) currRing.Add(new PDist { Point = p, Distance = dist });
                    else nextRing.Add(new PDist { Point = p, Distance = dist });
                }
            }


            if (currRing.Count < nn.K)
            {
                currRing.AddRange(nextRing);
            }

            currRing.Sort();
            nn.NNs = currRing.Count > nn.K ? currRing.Take(nn.K).ToList() : currRing.ToList();
        }

        protected List<P> GetRing(P p, int ring)
        {
            if (ring == 0) return GetSet(p).ToList();

            var center = p.GridIndex;

            var indexes = new HashSet<GridIndex>();
            for (var i = -ring; i <= ring; i++)
            {
                // add horizontal
                indexes.Add(new GridIndex { X = center.X + i, Y = center.Y - ring });
                indexes.Add(new GridIndex { X = center.X + i, Y = center.Y + ring });

                // add vertical
                indexes.Add(new GridIndex { X = center.X - ring, Y = center.Y + i });
                indexes.Add(new GridIndex { X = center.X + ring, Y = center.Y + i });
            }

            var list = new List<P>();
            foreach (var i in indexes.Where(IsValidGridIndex)) list.AddRange(Grid.Get(i));

            return list;
        }
    }
}
