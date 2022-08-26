using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using Microsoft.Xna.Framework.Input;

namespace Chess;

public class Board
{
    #region DrawingFields
    public const int XLen = 8;
    public const int YLen = 8;

    public int xScale;
    public int yScale;
    #endregion

    public int[,] _boardMatrix = new int[8, 8];
    public BV Turn = BV.White;

    public GetMoves _getMoves = new GetMoves();
    private Texture2D[,] _textureBoard = new Texture2D[8, 8];


    public Board(int width, int height, string fen)
    {
        xScale = width / XLen;
        yScale = height / YLen;

        LoadPosition(fen);
    }


    public void MakeMove(Move move, bool switchTurn = true)
    {
        int figureValue = _boardMatrix[move.StartPos.Y, move.StartPos.X];

        _boardMatrix[move.StartPos.Y, move.StartPos.X] = 0;
        _boardMatrix[move.EndPos.Y, move.EndPos.X] = figureValue;

        MoveTracker(figureValue, move);

        if (switchTurn)
            SwitchTurn();
    }

    public List<Move> GenerateMoves()
    {
        var allMoves = new List<Move>();

        AcrossBoard((x, y) =>
        {
            int value = _boardMatrix[y, x];
            if (value == 0)
                return;

            var color = GetBV(value).Color;

            if (color == Turn)
                allMoves.AddRange(_getMoves.For(x, y, _boardMatrix));
        });

        return allMoves;
    }

    public List<Move> GenerateLegalMoves()
    {
        var res = new List<Move>();
        var moves = GenerateMoves();

        foreach (Move move in moves)
        {
            var saved = Save();
            Dictionary<char, bool> castleValues = new(_getMoves._queenKingCastleSide);

            MakeMove(move);

            if (GenerateMoves().Any(m => m.EatenFigure == BV.King))
            {
                // There is a check
            }
            else
                res.Add(move);

            SwitchTurn();
            LoadPosition(saved, castleValues);
        }

        return res;
    }


    public static (BV Figure, BV Color) GetBV(int value)
    {
        BV figure = (BV)(value % 8);
        BV color = (BV)(value / 8 * 8);

        return (figure, color);
    }

    public int[,] Save() =>
        (int[,])_boardMatrix.Clone();

    public void SwitchTurn() =>
        Turn = Turn == BV.Black ? BV.White : BV.Black;


    public static void AcrossBoard(Action<int, int> func)
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
                func(x, y);
        }
    }

    private void LoadPosition(string fen)
    {
        Dictionary<char, BV> charToFigure = new()
        {
            ['p'] = BV.Pawn,
            ['n'] = BV.Horse,
            ['b'] = BV.Bishop,
            ['r'] = BV.Rook,
            ['q'] = BV.Queen,
            ['k'] = BV.King
        };

        int x = 0, y = 0;
        string[] ferParts = fen.Split(' ');

        string ferCode = ferParts[0];
        char turn = ferParts[1][0];

        if (ferParts.Length > 2)
            foreach (char c in ferParts[2])
                _getMoves._queenKingCastleSide[c] = true;

        foreach (char c in ferCode)
        {
            if (c == '/')
            {
                x = 0;
                y++; 
            }
            else
            {
                if (char.IsDigit(c))
                    x += int.Parse(c.ToString());
                else
                {
                    x++;
                    BV color = char.IsUpper(c) ? BV.White : BV.Black;
                    _boardMatrix[y, x - 1] = (int)charToFigure[char.ToLower(c)] + (int)color;
                }
            }
        }

        Turn = turn == 'w' ? BV.White : BV.Black;

        UpdateTextures();
    }

    public void LoadPosition(int[,] matrix, Dictionary<char, bool> castleValues)
    {
        _boardMatrix = matrix;
        _getMoves._queenKingCastleSide = castleValues;
    }

    public bool InCheck()
    {
        SwitchTurn();
        var moves = GenerateLegalMoves();
        SwitchTurn(); 
        return (moves.Any(m => m.EatenFigure == BV.King));
    }

    private void MoveTracker(int figureValue, Move move)
    {
        var (figure, color) = GetBV(figureValue);
        

        if (figure == BV.King)
        {
            int rookY = move.StartPos.Y;

            if ((move.StartPos.X - move.EndPos.X) > 1)
                MakeMove(new Move((0, rookY), (move.StartPos.X - 1, move.StartPos.Y), _boardMatrix), switchTurn: false);
            else if ((move.StartPos.X - move.EndPos.X) < -1)
                MakeMove(new Move((7, rookY), (move.StartPos.X + 1, move.StartPos.Y), _boardMatrix), switchTurn: false);
        }
        else if (figure == BV.Rook)
        {
            char side = move.StartPos.X == 0 ? 'q' : 'k';
            side = color == BV.White ? char.ToUpper(side) : side;

            _getMoves._queenKingCastleSide[side] = false;
        }
        else if (figure == BV.Pawn)
        {
            int finalPos = color == BV.White ? 0 : 7;
            if (move.EndPos.Y == finalPos)
            {
                _boardMatrix[move.EndPos.Y, move.EndPos.X] = (int)BV.Queen + (int)color;
            }
        }
    }

    #region DrawingPart

    public void DrawAll(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        AcrossBoard((x, y) =>
        {
            if (_textureBoard[y, x] is Texture2D texture)
            {
                var posOnScreen = new Vector2(x * xScale + 20, y * yScale + 15);
                spriteBatch.Draw(texture, posOnScreen, Color.White);
            }
        });

        spriteBatch.End();
    }

    public void UpdateTextures()
    {
        _textureBoard = new Texture2D[8, 8];

        AcrossBoard((x, y) =>
        {
            int value = _boardMatrix[y, x];
            if (value > 0)
            {
                var (figure, color) = GetBV(value);
                _textureBoard[y, x] = ChessGame._textures[figure | color];
            }
        });
    }


    #endregion
}
