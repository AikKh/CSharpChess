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

    public int EatenFigure = 0;

    public Move((int, int) Start, (int, int) End, int[,] board)
    {
        StartPos = Start;
        EndPos = End;

        //Set place
        place = GetPlace(board); 
    }

    private Place GetPlace(int[,] board)
    {
        if (0 <= EndPos.X && EndPos.X < Board.XLen && 0 <= EndPos.Y && EndPos.Y < Board.YLen)
        {
            int selfFigure = board[StartPos.Y, StartPos.X];
            int sqFigure = board[EndPos.Y, EndPos.X];

            if (sqFigure != 0)
            {
                if (Board.GetBV(selfFigure).Color == Board.GetBV(sqFigure).Color)
                    return Place.Busy;
                else
                {
                    EatenFigure = sqFigure;
                    return Place.CanEat;
                }
            }
            return Place.Free;

        }
        return Place.OutOfRange;
    }
}


