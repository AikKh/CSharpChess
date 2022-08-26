using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class GetMoves
    {
        public Dictionary<char, bool> _queenKingCastleSide = new()
        {
            ['K'] = false,
            ['Q'] = false,
            ['k'] = false,
            ['q'] = false
        };

        public static void AddIf(List<Move> list, Move move)
        {
            if (move.place != Place.OutOfRange && move.place != Place.Busy)
            {
                list.Add(move);
            }
        }

        public Move[] For(int x, int y, int[,] board)
        {
            var figure = Board.GetBV(board[y, x]).Figure;
            switch (figure)
            {
                case BV.Pawn:
                    return ForPawn(x, y, board);
                case BV.Horse:
                    return ForHorse(x, y, board);
                case BV.Bishop:
                    return ForBishop(x, y, board);
                case BV.Rook:
                    return ForRook(x, y, board);
                case BV.Queen:
                    return ForQueen(x, y, board);
                case BV.King:
                    return ForKing(x, y, board);
                default:
                    throw new Exception("smth not right");
            }
        }


        private Move[] ForPawn(int x, int y, int[,] board)
        {
            var myPos = (X: x, Y: y);
            var res = new List<Move>();

            var (_, color) = Board.GetBV(board[y, x]);

            int dir = color == BV.Black ? 1 : -1;
            var firstPos = color == BV.White ? 6 : 1;
            var finalPos = color == BV.White ? 0 : 7;

            var forwardMove = new Move(myPos, (myPos.X, myPos.Y + dir), board);
            var leftMove = new Move(myPos, (myPos.X - 1, myPos.Y + dir), board);
            var rightMove = new Move(myPos, (myPos.X + 1, myPos.Y + dir), board);

            if (forwardMove.place == Place.Free)
                res.Add(forwardMove);
            if (rightMove.place == Place.CanEat)
                res.Add(rightMove);
            if (leftMove.place == Place.CanEat)
                res.Add(leftMove);


            if (y == firstPos)
            {
                var fMove = new Move(myPos, (myPos.X, myPos.Y + dir * 2), board);
                if (forwardMove.place == Place.Free && fMove.place == Place.Free)
                    res.Add(fMove);
            }
            else if (y == finalPos)
            {
                board[y, x] = (int)BV.Queen + (int)color;
            }
            return res.ToArray();
        }

        private Move[] ForLinear(int x, int y, int[,] board, (int, int)[] _directions)
        {
            var res = new List<Move>();

            foreach (var (dX, dY) in _directions)
            {
                for (int i = 1; i < 8; i++)
                {
                    var m = new Move((x, y), (x + dX * i, y + dY * i), board);

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

        private Move[] ForBishop(int x, int y, int[,] board)
        {
            var directions = new (int, int)[] { (1, 1), (1, -1), (-1, 1), (-1, -1) };
            return ForLinear(x, y, board, directions);
        }

        private Move[] ForRook(int x, int y, int[,] board)
        {
            var directions = new (int, int)[] { (1, 0), (0, -1), (-1, 0), (0, 1) };
            return ForLinear(x, y, board, directions);
        }

        private Move[] ForQueen(int x, int y, int[,] board)
        {
            var directions = new (int, int)[]{ (1, 0), (0, -1), (-1, 0), (0, 1),
                                               (1, 1), (1, -1), (-1, 1), (-1, -1)};
            return ForLinear(x, y, board, directions);
        }

        private Move[] ForHorse(int x, int y, int[,] board)
        {
            (int x, int y)[] places =
            {
                (1, 2), (2, 1),
                (-1, 2), (-2, 1),
                (1, -2), (2, -1),
                (-1, -2), (-2, -1)
            };

            var res = new List<Move>();

            foreach (var (dX, dY) in places)
                AddIf(res, new Move((x, y), (x + dX, y + dY), board));

            return res.ToArray();
        }

        private Move[] ForKing(int x, int y, int[,] board)
        {
            var res = new List<Move>();

            for (int dX = -1; dX < 2; dX++)
            {
                foreach (int dY in new int[] { -1, 1 })
                {
                    var move = new Move((x, y), (x + dX, y + dY), board);
                    AddIf(res, move);
                }
            }

            var leftMove = new Move((x, y), (x - 1, y), board);
            var rightMove = new Move((x, y), (x + 1, y), board);

            AddIf(res, leftMove);
            AddIf(res, rightMove);

            BV color = Board.GetBV(board[y, x]).Color;

            char queenSide = color == BV.White ? 'Q' : 'q';
            char kingSige = color == BV.White ? 'K' : 'k';

            if (Board.GetBV(board[y, 0]).Figure == BV.Rook && _queenKingCastleSide[queenSide] && leftMove.place == Place.Free)
            {
                var leftCastle1 = new Move((x, y), (x - 2, y), board);
                var leftCastle2 = new Move((x, y), (x - 3, y), board);

                if (leftCastle2.place != Place.OutOfRange && leftCastle2.place != Place.Busy)
                {
                    AddIf(res, leftCastle1);
                }
            }
            if (Board.GetBV(board[y, 7]).Figure == BV.Rook && _queenKingCastleSide[kingSige] && rightMove.place == Place.Free)
            {
                var rightCasle = new Move((x, y), (x + 2, y), board);
                AddIf(res, rightCasle);
            }

            return res.ToArray();
        }
    }
}
