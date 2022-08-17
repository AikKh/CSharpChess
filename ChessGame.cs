using System;
using Chess.Figures;
using Microsoft.Xna.Framework;
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

    private Figure SelectedFigure;
    private bool CanSelect = true;


    public ChessGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        int width = 800;
        int height = 800;

        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;

        _graphics.ApplyChanges();

        _board = new Board(width, height);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _boardTexture = Content.Load<Texture2D>("BoardImg");
        _selectedTexture = Content.Load<Texture2D>("Selected");

        #region Figures init
        var blr = new Rook(0, 0, Content.Load<Texture2D>("B_Rook"), Color.Black);
        var brr = new Rook(7, 0, Content.Load<Texture2D>("B_Rook"), Color.Black);
        _board.Add(blr);
        _board.Add(brr);
        _board.Add(new Horse(1, 0, Content.Load<Texture2D>("B_Horse"), Color.Black));
        _board.Add(new Horse(6, 0, Content.Load<Texture2D>("B_Horse"), Color.Black));
        _board.Add(new Bishop(2, 0, Content.Load<Texture2D>("B_Bishop"), Color.Black));
        _board.Add(new Bishop(5, 0, Content.Load<Texture2D>("B_Bishop"), Color.Black));
        _board.Add(new Queen(3, 0, Content.Load<Texture2D>("B_Queen"), Color.Black));
        _board.Add(new King(4, 0, Content.Load<Texture2D>("B_King"), Color.Black, blr, brr));
        for (int x = 0; x < 8; x++)
        {
            _board.Add(new Pawn(x, 1, Content.Load<Texture2D>("B_Pawn"), Color.Black));
        }

        var wlr = new Rook(0, 7, Content.Load<Texture2D>("Rook"), Color.White);
        var wrr = new Rook(7, 7, Content.Load<Texture2D>("Rook"), Color.White);
        _board.Add(wlr);
        _board.Add(wrr);
        _board.Add(new Horse(1, 7, Content.Load<Texture2D>("Horse"), Color.White));
        _board.Add(new Horse(6, 7, Content.Load<Texture2D>("Horse"), Color.White));
        _board.Add(new Bishop(2, 7, Content.Load<Texture2D>("Bishop"), Color.White));
        _board.Add(new Bishop(5, 7, Content.Load<Texture2D>("Bishop"), Color.White));
        _board.Add(new Queen(3, 7, Content.Load<Texture2D>("Queen"), Color.White));
        _board.Add(new King(4, 7, Content.Load<Texture2D>("King"), Color.White, wlr, wrr));
        for (int x = 0; x < 8; x++)
        {
            _board.Add(new Pawn(x, 6, Content.Load<Texture2D>("Pawn"), Color.White));
        }
        //_board.Add(new Bishop(0, 5, Content.Load<Texture2D>("B_Horse"), Color.Black));
        //_board.Add(new Bishop(4, 4, Content.Load<Texture2D>("B_Bishop"), Color.Black));
        //_board.Add(new Queen(7, 1, Content.Load<Texture2D>("B_Queen"), Color.Black));
        //_board.Add(new King(6, 5, Content.Load<Texture2D>("King"), Color.White));

        _board.UpdateMoves();
        #endregion
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        mState = Mouse.GetState();

        if (mState.LeftButton == ButtonState.Pressed)
        {

            var (x, y) = mState.Position;
            x /= _board.xScale;
            y /= _board.yScale;

            if (0 <= x && x < 8 && 0 <= y && y < 8)
            {
                if (SelectedFigure is not null)
                {
                    //Console.Beep();
                    foreach (Move m in SelectedFigure.Moves)
                    {
                        if (m.EndPos.X == x && m.EndPos.Y == y)
                        {
                            _board.MakeMove(m);
                            SelectedFigure = null;
                            break;
                        }
                    }
                }
                if (CanSelect && _board.BoardMatrix[y, x] is Figure figure)
                {
                    if (figure.SelfColor == _board.Turn)
                        SelectedFigure = figure;
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

        if (SelectedFigure is not null)
        {
            foreach (Move m in SelectedFigure.Moves)
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