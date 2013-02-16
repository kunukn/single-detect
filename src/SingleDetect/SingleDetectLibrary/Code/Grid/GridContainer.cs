using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kunukn.SingleDetectLibrary.Code.Contract;
using Kunukn.SingleDetectLibrary.Code.Data;

namespace Kunukn.SingleDetectLibrary.Code.Grid
{
    public class GridContainer
    {
        private Grid Grid { get; set; }
        private double DX { get; set; } // delta x
        private double DY { get; set; } // delta y
        private readonly IRectangle _rect;

        public GridContainer(IRectangle rect, IEnumerable<IP> points)
        {
            _rect = rect;
            DX = rect.Square;
            DY = rect.Square;
            Grid = new Grid(rect.XGrid, rect.YGrid);
            foreach (var p in points) UpdatePosition(p);
        }

        protected GridIndex Delta(IP a)
        {
            var x = (int)((_rect.XO + a.X) / DX);
            var y = (int)((_rect.YO + a.Y) / DY);

            if (x < 0 || y < 0) throw new ApplicationException(
                string.Format("Algo error: {0}", MethodBase.GetCurrentMethod())
                );

            return new GridIndex { X = x, Y = y };
        }

        public MySet<IP> GetSet(IP p)
        {
            return this.Grid.Get(p.GridIndex);
        }

        // Single detect
        // 20 nearest boxes
        public List<IP> GetGridNeighborContent(IP p)
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

            var list = new List<IP>();
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

        // Primarily used for updating position, insert p into grid
        public void UpdatePosition(IP p)
        {            
            var b = Remove(p);  // remove prev position          
            UpdateIndex(p);  // update ref
            var set = GetSet(p);
            set.Add(p);  // add new position
        }

        // Don't insert p into grid, only update
        public void UpdateIndex(IP p)
        {
            var d = Delta(p);
            p.GridIndex = d; // update ref
        }

        public bool Remove(IP p)
        {            
            var set = GetSet(p);
            return set.Remove(p); // remove                        
        }


        public List<IP> GetGridSingles()
        {
            return this.Grid.GetSingles(this);
        }

        public List<IP> GetRing(IP p, int ring)
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

            var list = new List<IP>();
            foreach (var i in indexes.Where(IsValidGridIndex)) list.AddRange(Grid.Get(i));

            return list;
        }
    }
}
