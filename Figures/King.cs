using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Figures
{
    public class King : Figure
    {
        private bool CanCastle = true;
        private Rook leftRook;
        private Rook rightRook;

        private Figure[,] _board;

        public King(int x, int y, Texture2D texture, Color color, Rook leftRook, Rook rightRook) : base(x, y, texture, color) 
        { 
            this.leftRook = leftRook;
            this.rightRook = rightRook;
        }

        public override Move[] GetMoves(Figure[,] board)
        {
            _board = board;
            var res = new List<Move>();

            for (int x = -1; x < 2; x++)
            {
                foreach (int y in new int[] { -1, 1 })
                {
                    var m = new Move((int)Pos.X + x, (int)Pos.Y + y, this, board);
                    AddIf(res, m); 
                }
            }

            var leftMove = new Move((int)Pos.X - 1, (int)Pos.Y, this, board);
            var rightMove = new Move((int)Pos.X + 1, (int)Pos.Y, this, board);

            AddIf(res, leftMove);
            AddIf(res, rightMove);

            if (CanCastle)
            {
                if (leftRook.CanCastleWithKing && leftMove.place == Place.Free)
                {
                    var leftCastle1 = new Move((int)Pos.X - 2, (int)Pos.Y, this, board);
                    var leftCastle2 = new Move((int)Pos.X - 3, (int)Pos.Y, this, board);

                    if (leftCastle2.place != Place.OutOfRange && leftCastle2.place != Place.Busy)
                    {
                        AddIf(res, leftCastle1);
                    }
                }
                if (rightRook.CanCastleWithKing && rightMove.place == Place.Free)
                {
                    var rightCasle = new Move((int)Pos.X + 2, (int)Pos.Y, this, board);
                    AddIf(res, rightCasle);
                }
            }

            return res.ToArray();
        }

        public override void MoveTo(Move move)
        {
            int x = move.EndPos.X;
            if (CanCastle)
            {
                if ((Pos.X - x) > 1)
                {
                    MoveRook(new Move((int)Pos.X - 1, (int)Pos.Y, leftRook, _board));
                }
                else if ((Pos.X - x) < -1)
                {
                    MoveRook(new Move((int)Pos.X + 1, (int)Pos.Y, rightRook, _board));
                }
            }

            CanCastle = false;
            base.MoveTo(move);
        }

        //public

        private void AddIf(List<Move> list, Move move)
        {
            if (move.place != Place.OutOfRange && move.place != Place.Busy)
            {
                list.Add(move);
            }
        }

        private void MoveRook(Move move)
        {
            var rook = move.SelfFigure;

            _board[(int)rook.Pos.Y, (int)rook.Pos.X] = null;
            rook.MoveTo(move);
            _board[(int)rook.Pos.Y, (int)rook.Pos.X] = rook;
        }

    }
}
