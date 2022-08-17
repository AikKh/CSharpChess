using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess;

public class Board
{
    public const int XLen = 8;
    public const int YLen = 8;

    public int xScale;
    public int yScale;

    private readonly List<Figure> figures = new List<Figure>();

    public readonly Figure[,] BoardMatrix = new Figure[8, 8];

    public Color Turn = Color.White;

    public Board(int width, int height)
    {
        xScale = width / XLen;
        yScale = height / YLen;
    }

    public Board(int width, int height, string fen) : this(width, height) { }

    public void Add(Figure figure)
    {
        figures.Add(figure);
        BoardMatrix[(int)figure.Pos.Y, (int)figure.Pos.X] = figure;
    }


    public void MakeMove(Move move)
    {
        var figure = move.SelfFigure;

        if (move.place == Place.CanEat)
            figures.Remove(BoardMatrix[move.EndPos.Y, move.EndPos.X]);

        BoardMatrix[(int)figure.Pos.Y, (int)figure.Pos.X] = null;
        figure.MoveTo(move);
        BoardMatrix[(int)figure.Pos.Y, (int)figure.Pos.X] = figure;

        UpdateMoves();
        Turn = Turn == Color.Black ? Color.White : Color.Black;
    }


    public void UpdateMoves()
    {
        foreach (Figure figure in figures)
            figure.Moves = figure.GetMoves(BoardMatrix);
    }

    public void DrawAll(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        foreach (Figure figure in figures)
        {
            var posOnScreen = new Vector2(figure.Pos.X * xScale + 20, figure.Pos.Y * yScale + 15);
            spriteBatch.Draw(figure.Sprite, posOnScreen, Color.White);
        }

        spriteBatch.End();
    }

    private void LoadPosition(string fen)
    {
        
    }

}
