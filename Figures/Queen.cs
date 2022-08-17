using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Figures;

public class Queen : LinearFigure
{
    public Queen(int x, int y, Texture2D texture, Color color) : base(x, y, texture, color) { }

    public override (int, int)[] _directions => new (int, int)[] { (1, 0), (0, -1), (-1, 0), (0, 1),
                                                                   (1, 1), (1, -1), (-1, 1), (-1, -1)};
}
