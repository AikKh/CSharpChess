using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    private readonly ContentManager _content;

    private Board _board;
    private AI _bot;

    private Texture2D _boardTexture;
    private Texture2D _selectedTexture;

    private MouseState mState;

    #region SelectedFigureFields
    private bool CanSelect = false;
    private (int X, int Y) _selectedFigureCors;
    private Move[] _sFigureMoves;
    private Move[] _allMoves;
    #endregion


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

        string startPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq";
        string startPosition1 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/R3K2n w KQkq";
        string startPosition2 = "rnbqkb1r/pppp1ppp/5n2/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R w KQkq";
        string pos2 = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq";
        string pos5 = "rnbqk2r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ";
        string testPos = "8/5k2/4q3/8/2P5/1B6/K7 w";
        string testPos2 = "8/5k2/4q3/8/8/1B6/K1P4 w";
        string testPos3 = "1K6/7q/2k5/8/8/ w";

        //string testPosition = "8/8/8/8/4B w";
        //string testPosition2 = "8/4P2p w";

        _board = new Board(width, height, testPos);
        _allMoves = _board.GenerateLegalMoves().ToArray();
        _bot = new AI(_board, BV.Black);

        //var iii = Enumerable.Range(1, 4).Select(i => _bot.Test(i)).ToArray();
        //_ = 0;
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

        //BotAgainstBot();
        HumanAgainstBot();
        //Debug();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_boardTexture, new Vector2(0, 0), Color.Gray);

        if (CanSelect)
        {
            foreach (Move m in _sFigureMoves)
            {
                var posOnScreen = new Vector2(m.EndPos.X * _board.xScale, m.EndPos.Y * _board.xScale);
                _spriteBatch.Draw(_selectedTexture, posOnScreen, Color.White);
            }
        }
        _spriteBatch.End();

        _board.DrawAll(_spriteBatch);

        base.Draw(gameTime);
    }

    private void HumanAgainstBot()
    {
        mState = Mouse.GetState();

        if (mState.LeftButton == ButtonState.Pressed)
        {
            //Mouse position
            var (mX, mY) = mState.Position;
            mX /= _board.xScale;
            mY /= _board.yScale;


            // If in a range
            if (!(0 <= mX && mX < 8 && 0 <= mY && mY < 8))
                return;

            int value = _board._boardMatrix[mY, mX];

            if (CanSelect)
            {
                foreach (Move move in _sFigureMoves)
                {
                    if (move.EndPos.X == mX && move.EndPos.Y == mY)
                    {
                        _board.MakeMove(move);
                        _bot.Move();
                        _board.UpdateTextures();

                        _allMoves = _board.GenerateLegalMoves().ToArray();
                        CanSelect = false;

                        break;
                    }
                }
            }

            if (value > 0)
            {
                if (Board.GetBV(value).Color == _board.Turn)
                {
                    CanSelect = true;
                    _selectedFigureCors = (mX, mY);
                    _sFigureMoves = _allMoves.Where(move => move.StartPos == (mX, mY)).ToArray();
                }
            }
        }
    }
}