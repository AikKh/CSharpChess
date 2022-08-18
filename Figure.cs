//using System;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Chess.Figures;

//namespace Chess;

//public abstract class Figure
//{
//    public Vector2 Pos;
//    public Texture2D Sprite;

//    public readonly BV SelfColor;

//    private Move[] moves;

//    protected Figure(int x, int y, Texture2D texture, BV color)
//    {
//        Pos = new Vector2(x, y);

//        Sprite = texture;
//        SelfColor = color;

//    }

//    public Move[] Moves
//    {
//        get => moves;
//        set => moves = value;
//    }

//    public virtual void MoveTo(Move move)
//    {
//        Pos.X = move.EndPos.X;
//        Pos.Y = move.EndPos.Y;
//    }

//    public abstract Move[] GetMoves(int[,] board);

//    public static BV GetColor(int value) =>
//        value / 8 > 1 ? BV.Black : BV.White;


//    public static Figure GetFigure(int x, int y, int value)
//    {
//        BV color = GetColor(value);
//        value %= 8;

//        switch (value)
//        {
//            case 1:
//                return new Pawn(x, y, ChessGame._textures[BV.Pawn | color], color);
//            case 2:
//                return new Horse(x, y, ChessGame._textures[BV.Horse | color], color);
//            case 3:
//                return new Bishop(x, y, ChessGame._textures[BV.Bishop | color], color);
//            case 4:
//                return new Rook(x, y, ChessGame._textures[BV.Rook | color], color);
//            case 5:
//                return new Queen(x, y, ChessGame._textures[BV.Queen | color], color);
//            case 6:
//                return new King(x, y, ChessGame._textures[BV.Queen | color], color);
//        };
//    }
//}
