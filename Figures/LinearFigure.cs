using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Figures
{
    public abstract class LinearFigure : Figure
    {
        public abstract (int, int)[] _directions { get; }

        protected LinearFigure(int x, int y, Texture2D texture, Color color) : base(x, y, texture, color) { }

        public override Move[] GetMoves(Figure[,] board)
        {
            var res = new List<Move>();

            foreach (var (x, y) in _directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    var m = new Move((int)Pos.X + (x * i), (int)Pos.Y + (y * i), this, board);

                    if (m.place == Place.Busy || m.place == Place.OutOfRange)
                        break;
                    else if (m.place == Place.CanEat)
                    {
                        res.Add(m);
                        break;
                    }

                    res.Add(m);
                }
            }

            return res.ToArray();
        }
    }
}
