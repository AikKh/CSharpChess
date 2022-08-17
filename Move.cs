using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess;

public struct Move
{
    public (int X, int Y) StartPos;
    public (int X, int Y) EndPos;

    public Place place = Place.OutOfRange;

    public Figure EatenFigure = null;
    public Figure SelfFigure;

    public Move(int x, int y, Figure figure, Figure[,] board)
    {
        SelfFigure = figure;

        StartPos = ((int)figure.Pos.X, (int)figure.Pos.Y);
        EndPos = (x, y);

        //Set place
        place = GetPlace(board, figure.SelfColor);
        
    }

    private Place GetPlace(Figure[,] board, Color figureColor)
    {
        if (0 <= EndPos.X && EndPos.X < Board.XLen && 0 <= EndPos.Y && EndPos.Y < Board.YLen)
        {
            var fPlace = board[EndPos.Y, EndPos.X];

            if (fPlace is not null)
            {
                if (fPlace.SelfColor == figureColor)
                    return Place.Busy;
                else
                {
                    EatenFigure = board[EndPos.Y, EndPos.X];
                    return Place.CanEat;
                }
            }
            return Place.Free;

        }
        return Place.OutOfRange;

    }

    public Vector2 Pos => new(EndPos.X, EndPos.Y);
}


