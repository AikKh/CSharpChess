//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Chess.Figures
//{
//    public class Horse : Figure
//    {
//        private (int x, int y)[] _movePoses =
//        {
//            (1, 2), (2, 1),
//            (-1, 2), (-2, 1),
//            (1, -2), (2, -1),
//            (-1, -2), (-2, -1)
//        };

//        public Horse(int x, int y, Texture2D texture, BV color) : base(x, y, texture, color) { }

//        public override Move[] GetMoves(int[,] board)
//        {
//            var myPos = (X: (int)Pos.X, Y: (int)Pos.Y);
//            var res = new List<Move>();

//            foreach (var (x, y) in _movePoses)
//                res.Add(new Move(myPos, (myPos.X + 2, myPos.Y), board));

//            return res.Where(move => move.place != Place.OutOfRange && move.place != Place.Busy).ToArray();
//        }
//    }
//}
