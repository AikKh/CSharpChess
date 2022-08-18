using System;
using System.Collections.Generic;
//using Chess.Figures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess;

public class ChessGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Board _board;

    private Texture2D _boardTexture;
    private Texture2D _selectedTexture;

    private MouseState mState;

    private (int X, int Y)? SelectedFigureCors;
    private Move[] SFMoves;
    private bool CanSelect = true;

    private readonly ContentManager _content;

    public static Dictionary<BV, Texture2D> _textures = new();
    


    public ChessGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _content = Content;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        int width = 800;
        int height = 800;

        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;

        _graphics.ApplyChanges();

        #region InitTextures
        _textures.Add(BV.Pawn | BV.White, _content.Load<Texture2D>("Pawn"));
        _textures.Add(BV.Horse | BV.White, _content.Load<Texture2D>("Horse"));
        _textures.Add(BV.Bishop | BV.White, _content.Load<Texture2D>("Bishop"));
        _textures.Add(BV.Rook | BV.White, _content.Load<Texture2D>("Rook"));
        _textures.Add(BV.Queen | BV.White, _content.Load<Texture2D>("Queen"));
        _textures.Add(BV.King | BV.White, _content.Load<Texture2D>("King"));

        _textures.Add(BV.Pawn | BV.Black, _content.Load<Texture2D>("B_Pawn"));
        _textures.Add(BV.Horse | BV.Black, _content.Load<Texture2D>("B_Horse"));
        _textures.Add(BV.Bishop | BV.Black, _content.Load<Texture2D>("B_Bishop"));
        _textures.Add(BV.Rook | BV.Black, _content.Load<Texture2D>("B_Rook"));
        _textures.Add(BV.Queen | BV.Black, _content.Load<Texture2D>("B_Queen"));
        _textures.Add(BV.King | BV.Black, _content.Load<Texture2D>("B_King"));

        #endregion
        string startPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQK2R w KQkq";
        string testPosition = "8/8/8/8/4B w";
        string testPosition2 = "8/4P2p w";
        _board = new Board(width, height, testPosition2);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _boardTexture = Content.Load<Texture2D>("BoardImg");
        _selectedTexture = Content.Load<Texture2D>("Selected");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        mState = Mouse.GetState();

        if (mState.LeftButton == ButtonState.Pressed)
        {
            //Mouse position
            var (mX, mY) = mState.Position;
            mX /= _board.xScale;
            mY /= _board.yScale;

            // If in a range
            if (0 <= mX && mX < 8 && 0 <= mY && mY < 8)
            {
                
                if (SelectedFigureCors.HasValue)
                {
                    foreach (Move move in SFMoves)
                    {
                        if (move.EndPos.X == mX && move.EndPos.Y == mY)
                        {
                            _board.MakeMove(move);
                            _board.UpdateTextures();
                            SelectedFigureCors = null; SFMoves = null;
                            break;
                        }
                    }
                }

                int value = _board._boardMatrix[mY, mX];
                if (CanSelect && value > 0)
                {
                    if (Board.GetBV(value).Color == _board.Turn)
                    {
                        SelectedFigureCors = (mX, mY);
                        SFMoves = _board._getMoves.For(mX, mY, _board._boardMatrix);
                    }
                }
                //else
                //    _board.SelectedFigure = null;
            }

            CanSelect = false;
        }
        else if (mState.LeftButton == ButtonState.Released)
        {
            CanSelect = true;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_boardTexture, new Vector2(0, 0), Color.Gray);

        if (SelectedFigureCors is not null)
        {
            foreach (Move m in SFMoves)
            {
                var posOnScreen = new Vector2(m.EndPos.X * _board.xScale, m.EndPos.Y * _board.xScale);
                _spriteBatch.Draw(_selectedTexture, posOnScreen, Color.White);
            }
        }
        _spriteBatch.End();

        _board.DrawAll(_spriteBatch);

        base.Draw(gameTime);
    }


}