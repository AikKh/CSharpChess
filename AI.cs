using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Chess
{
    public class AI
    {
        public static int Speed = 10;

        private readonly Random random = new();
        private readonly Board _board;
        private readonly Evaluation _pm;
        private BV _selfColor;

        public AI(Board board, BV selfColor)
        {
            _board = board;
            _pm = new Evaluation();
            _selfColor = selfColor;
        }

        public void Move()
        {
            var moves = _board.GenerateLegalMoves().ToArray();

            if (moves.Length == 0)
                return;

            for (int i = 0; i < moves.Length; i++)
            {
                int[,] saved = _board.Save();
                var castleValues = new Dictionary<char, bool>(_board._getMoves._queenKingCastleSide);

                _board.MakeMove(moves[i]);

                int eval = -AphaBetaSearch(3);
                //int eval = -Search(3);
                moves[i].Eval = eval;

                _board.LoadPosition(saved, castleValues);
                _board.SwitchTurn();
            }

            Dictionary<int, List<Move>> moveValues = new();

            foreach (Move move in moves)
            {
                if (moveValues.Keys.Any(k => k == move.Eval))
                    moveValues[move.Eval].Add(move);
                else
                    moveValues.Add(move.Eval, new List<Move>() { move });
            
            }
            int maxEval = moveValues.Keys.Max();
            Move[] strongestMoves = moveValues[maxEval].ToArray();

            _board.MakeMove(strongestMoves[random.Next(0, strongestMoves.Length)]);
        }

        public void RandomMove()
        {
            Move[] moves = _board.GenerateLegalMoves().ToArray();
            if (moves.Length == 0)
                return;

            _board.MakeMove(moves[random.Next(0, moves.Length - 1)]);
        }

        private int AphaBetaSearch(int depth, int alpha = -int.MaxValue, int beta = int.MaxValue, bool maximizing = true)
        {
            if (depth == 0)
                return _pm.Evaluate(_board._boardMatrix, _selfColor);

            Move[] moves = _board.GenerateLegalMoves().ToArray();

            if (moves.Length == 0)
            {
                if (_board.InCheck())
                    return int.MinValue;
                return 0;
            }

            int maxEval = int.MinValue;

            foreach (Move move in moves)
            {
                int[,] saved = _board.Save();
                var castleValues = new Dictionary<char, bool>(_board._getMoves._queenKingCastleSide);

                _board.MakeMove(move);
                int evaluation = -AphaBetaSearch(depth - 1, -alpha, -beta, !maximizing);
                maxEval = Math.Max(maxEval, evaluation);

                _board.LoadPosition(saved, castleValues);
                _board.SwitchTurn();

                if (maximizing)
                    alpha = Math.Max(maxEval, alpha);
                else
                    beta = Math.Max(maxEval, beta);

                if (beta < alpha)
                    break;

            }

            return maxEval;
        }

        private int Search(int depth)
        {
            if (depth == 0)
                return _pm.Evaluate(_board._boardMatrix, _selfColor);

            Move[] moves = _board.GenerateLegalMoves().ToArray();

            if (moves.Length == 0)
            {
                if (_board.InCheck())
                    return int.MinValue;
                return 0;
            }

            int maxEval = int.MinValue;

            foreach (Move move in moves)
            {
                int[,] saved = _board.Save();
                var castleValues = new Dictionary<char, bool>(_board._getMoves._queenKingCastleSide);

                _board.MakeMove(move);
                int evaluation = -Search(depth - 1);
                maxEval = Math.Max(maxEval, evaluation);

                _board.LoadPosition(saved, castleValues);
                _board.SwitchTurn();
            }
            return maxEval;
        }
    }
}
