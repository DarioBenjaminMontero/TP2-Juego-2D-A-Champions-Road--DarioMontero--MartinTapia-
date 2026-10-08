using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media; 
using MonoGameLibrary;

namespace A_Champion_s_Road
{

    public class Game1 : Core
    {

        private Player _player;
        private Texture2D _playerTexture;
        private Song backgroundMusic;
        private Texture2D _ring;


        
        public Game1() : base("A Champions Road", 1280, 720, false)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.HardwareModeSwitch = false;
            Graphics.IsFullScreen = true;
            Graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            base.Initialize();
        }


        protected override void LoadContent()
        {

            // Musica de fondo
            backgroundMusic = Content.Load<Song>("Ten_Counts_To_Glory"); 
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true; 

            // Fondo del ring
            _ring = Content.Load<Texture2D>("ring3");

            // Texture atlas Dante Vega
            _playerTexture = Content.Load<Texture2D>("punch-danteVega");
            
            // Posición sprite Dante-Vega
            float playerX = Window.ClientBounds.Width * 0.5f;
            float playerY = Window.ClientBounds.Height - _playerTexture.Height;

            _player = new Player(_playerTexture, new Vector2(playerX, playerY), Window.ClientBounds.Width * 0.75f,Window.ClientBounds.Width * 0.25f);
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
            Rectangle texturaActual = new Rectangle(0, 0, 458, 58);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin();

            Rectangle fullscreenBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            SpriteBatch.Draw(_ring, fullscreenBounds, Color.White);
            _player.Draw(SpriteBatch);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}