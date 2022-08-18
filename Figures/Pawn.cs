//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Chess.Figures
//{
//    public class Pawn : Figure
//    {
//        private bool _firstMove;
//        private int _dir;

//        public Pawn(int x, int y, Texture2D texture, BV color) : base(x, y, texture, color)
//        {
//            _dir = color == BV.Black ? 1 : -1;
//            _firstMove = true;
//        }

//        public override Move[] GetMoves(int[,] board)
//        {
//            var myPos = (X: (int)Pos.X, Y: (int)Pos.Y);
//            var res = new List<Move>();

//            var forwardMove = new Move(myPos, (myPos.X, myPos.Y + _dir), board);
//            var leftMove = new Move(myPos, (myPos.X - 1, myPos.Y + _dir), board);
//            var rightMove = new Move(myPos, (myPos.X + 1, myPos.Y + _dir), board);

//            if (forwardMove.place == Place.Free)
//                res.Add(forwardMove);
//            if (rightMove.place == Place.CanEat)
//                res.Add(rightMove);
//            if (leftMove.place == Place.CanEat)
//                res.Add(leftMove);


//            if (_firstMove)
//            {
//                var fMove = new Move(myPos, (myPos.X, myPos.Y + _dir * 2), board);
//                if (fMove.place == Place.Free)
//                    res.Add(fMove);
//            }
//            return res.ToArray();
//        }

//        public override void MoveTo(Move move)
//        {
//            _firstMove = false;
//            base.MoveTo(move);
//        }
//    }
//}
