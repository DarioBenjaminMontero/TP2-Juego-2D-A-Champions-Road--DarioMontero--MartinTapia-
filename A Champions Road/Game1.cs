using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media; 
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace A_Champion_s_Road
{

    public class Game1 : Core
    {
        private Player _player; 
        private Texture2D _playerTexture;
        private Sprite _idle;
        private Sprite _charge;
        private Sprite _punching;

        private Song backgroundMusic; 
        private Texture2D _ring;

        public Game1() : base("A Champions Road", 1280, 720, false)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.HardwareModeSwitch = false;
            Graphics.IsFullScreen = true; // true
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

            // png utilizado para el Texture Atlas
            _playerTexture = Content.Load<Texture2D>("punch-danteVega");

            // declaracion formal del Texture Atlas
            TextureAtlas atlasPunch = new TextureAtlas(_playerTexture);

            // declarar regiones 
            atlasPunch.AddRegion("quieto", 49, 89, 201, 256); // primeros dos parametros: X, Y donde comienza el sprite en la esquina SUP. IZQ
            atlasPunch.AddRegion("intermedio", 321, 91, 269, 254); // ultimos dos parametros, la diferencia entre las X y las Y (X/Y esquina INF. DER - X/Y esquina SUP. IZQ)
            atlasPunch.AddRegion("golpe", 52, 354, 228, 338);

            _idle = atlasPunch.CreateSprite("quieto");
            _charge = atlasPunch.CreateSprite("intermedio");
            _punching = atlasPunch.CreateSprite("golpe");
            
            // Posición sprite Dante-Vega
            float playerX = Window.ClientBounds.Width * 0.5f;
            float playerY = Window.ClientBounds.Height - _idle.Height; // _playerTexture.Height

            // _player = new Player(_playerTexture, new Vector2(playerX, playerY), Window.ClientBounds.Width * 0.75f,Window.ClientBounds.Width * 0.25f);
            _player = new Player(
                _idle, 
                _charge, 
                _punching, 
                new Vector2(playerX, playerY), 
                Window.ClientBounds.Width * 0.75f, 
                Window.ClientBounds.Width * 0.25f
            );

            
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
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

                Rectangle fullscreenBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            SpriteBatch.Draw(_ring, fullscreenBounds, Color.White); //ring
            _player.Draw(SpriteBatch); // jugador

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}