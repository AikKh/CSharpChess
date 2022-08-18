//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Chess.Figures
//{
//    public class King : Figure
//    {
//        private bool CanCastle = true;

//        private int[,] _board;

//        public King(int x, int y, Texture2D texture, BV color) : base(x, y, texture, color) 
//        { 
//        }

//        public override Move[] GetMoves(int[,] board)
//        {
//            var myPos = (X: (int)Pos.X, Y: (int)Pos.Y);

//            _board = board;
//            var res = new List<Move>();

//            for (int x = -1; x < 2; x++)
//            {
//                foreach (int y in new int[] { -1, 1 })
//                {
//                    var m = new Move(myPos, (myPos.X + x, myPos.Y + y), board);
//                    AddIf(res, m); 
//                }
//            }

//            var leftMove = new Move(myPos, (myPos.X - 1, myPos.Y), board);
//            var rightMove = new Move(myPos, (myPos.X + 1, myPos.Y), board);

//            AddIf(res, leftMove);
//            AddIf(res, rightMove);

//            if (CanCastle)
//            {
//                if (leftRook.CanCastleWithKing && leftMove.place == Place.Free)
//                {
//                    var leftCastle1 = new Move(myPos, (myPos.X - 2, myPos.Y), board);
//                    var leftCastle2 = new Move(myPos, (myPos.X - 3, myPos.Y), board);

//                    if (leftCastle2.place != Place.OutOfRange && leftCastle2.place != Place.Busy)
//                    {
//                        AddIf(res, leftCastle1);
//                    }
//                }
//                if (rightRook.CanCastleWithKing && rightMove.place == Place.Free)
//                {
//                    var rightCasle = new Move(myPos, (myPos.X + 2, myPos.Y), board);
//                    AddIf(res, rightCasle);
//                }
//            }

//            return res.ToArray();
//        }

//        public override void MoveTo(Move move)
//        {
//            int x = move.EndPos.X;
//            if (CanCastle)
//            {
//                if ((Pos.X - x) > 1)
//                {
//                    MoveRook(new Move((int)Pos.X - 1, (int)Pos.Y, _board));
//                }
//                else if ((Pos.X - x) < -1)
//                {
//                    MoveRook(new Move((int)Pos.X + 1, (int)Pos.Y, _board));
//                }
//            }

//            CanCastle = false;
//            base.MoveTo(move);
//        }

//        //public

//        private void AddIf(List<Move> list, Move move)
//        {
//            if (move.place != Place.OutOfRange && move.place != Place.Busy)
//            {
//                list.Add(move);
//            }
//        }

//        private void MoveRook(Move move)
//        {
//            var rook = move.SelfFigure;

//            _board[(int)rook.Pos.Y, (int)rook.Pos.X] = 0;
//            _board[(int)rook.Pos.Y, (int)rook.Pos.X] = (int)BV.Rook + (int)SelfColor;
//        }

//    }
//}
