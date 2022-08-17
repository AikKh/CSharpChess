using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess;

public abstract class Figure
{
    public Vector2 Pos;
    public Texture2D Sprite;

    public readonly Color SelfColor;

    private Move[] moves;

    protected Figure(int x, int y, Texture2D texture, Color color)
    {
        Pos = new Vector2(x, y);

        Sprite = texture;
        SelfColor = color;

    }

    public Move[] Moves
    {
        get => moves;
        set => moves = value;
    }

    public virtual void MoveTo(Move move)
    {
        Pos.X = move.EndPos.X;
        Pos.Y = move.EndPos.Y;
    }

    public abstract Move[] GetMoves(Figure[,] board);

}
