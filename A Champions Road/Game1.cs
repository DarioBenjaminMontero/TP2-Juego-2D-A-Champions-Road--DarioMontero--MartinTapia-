using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media; 


namespace A_Champion_s_Road
{

    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        
  
        private SpriteBatch _spriteBatch;


        private Player _player;
        private Texture2D _playerTexture;
        private Texture2D _playerAltTexture;
        private Texture2D _playerAltTexture2;
        private Texture2D _playerNormalTexture;
        private Texture2D _cargandoGolpeIzquierda;
        private Texture2D _cargandoGolpeDerecha;
private Song backgroundMusic;
private Texture2D _ring;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
       
     _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
    _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;


     _graphics.HardwareModeSwitch = false;
    _graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }


        protected override void LoadContent()
        {
            backgroundMusic = Content.Load<Song>("Ten_Counts_To_Glory"); 
    MediaPlayer.Play(backgroundMusic);
    MediaPlayer.IsRepeating = true; 

            _spriteBatch = new SpriteBatch(GraphicsDevice);
_ring = Content.Load<Texture2D>("ring3");


            _playerTexture = Content.Load<Texture2D>("backSprite-danteVega2");
            float playerX = Window.ClientBounds.Width * 0.5f;
            float playerY = Window.ClientBounds.Height - _playerTexture.Height;
            _playerAltTexture = Content.Load<Texture2D>("backSprite-punch1-danteVega");
            _playerAltTexture2 = Content.Load<Texture2D>("backSprite-punch2-danteVega");
            _playerNormalTexture = Content.Load<Texture2D>("backSprite-danteVega2");
            _cargandoGolpeDerecha = Content.Load<Texture2D>("backSprite-punch2-danteVega3");
            _cargandoGolpeIzquierda = Content.Load<Texture2D>("backSprite-punch1-danteVega3");

            _player = new Player(_playerTexture, _playerAltTexture, _playerAltTexture2, _playerNormalTexture,_cargandoGolpeDerecha, _cargandoGolpeIzquierda, new Vector2(playerX, playerY), Window.ClientBounds.Width * 0.75f,Window.ClientBounds.Width * 0.25f);
        }


        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _player.Update(gameTime);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
             Rectangle fullscreenBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
    _spriteBatch.Draw(_ring, fullscreenBounds, Color.White);
            _player.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}