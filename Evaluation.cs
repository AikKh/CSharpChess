using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Evaluation
    {
        public static Dictionary<BV, int> _values = new()
        {
            [BV.Pawn] = 1,
            [BV.Horse] = 3,
            [BV.Bishop] = 3,
            [BV.Rook] = 5,
            [BV.Queen] = 9,
        };

        private int CountMaterial(int[,] matrix, BV color)
        {
            int mark = 0;

            Board.AcrossBoard((x, y) =>
            {
                var (figure, figureColor) = Board.GetBV(matrix[y, x]);

                if (color == figureColor && figure != BV.King)
                    mark += _values[figure];
            });

            return mark;
        }

        public int Evaluate(int[,] matrix, BV turnColor)
        {
            int whiteMat = CountMaterial(matrix, BV.White);
            int blackMat = CountMaterial(matrix, BV.Black);

            int evaluation = whiteMat - blackMat;

            int perspective = turnColor == BV.White? 1 : -1;
            return evaluation * perspective;
        }
    }
}
