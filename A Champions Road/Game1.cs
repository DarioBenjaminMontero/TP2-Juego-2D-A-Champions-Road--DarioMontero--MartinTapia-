using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using System;

namespace A_Champion_s_Road
{
    public class Game1 : Core
    {
        private Player _player;
        private Enemy _enemy; 
        private Song backgroundMusic;
        private Texture2D _ring;
        private CombatSystem _combat;
        
        // Declaramos el InputManager
        private InputManager _inputManager;

        public Game1() : base("A Champions Road", 1280, 720, false)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.HardwareModeSwitch = false;
            Graphics.IsFullScreen = true;
            Graphics.ApplyChanges();

            // Inicializamos el InputManager en el constructor
            _inputManager = new InputManager();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            backgroundMusic = Content.Load<Song>("Ten_Counts_to_Glory");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;

            _ring = Content.Load<Texture2D>("ring3");

            TextureAtlas atlas = TextureAtlas.FromFile(Content, "atlas-definition.xml");

            // Animaciones del Player
            Animation idleAnim = atlas.GetAnimation("idleAnimation");
            Animation punchAnim = atlas.GetAnimation("punchingAnimation");
            Animation dodgeAnim = atlas.GetAnimation("dodgingAnimation");

            float playerX = Window.ClientBounds.Width * 0.5f;
            float playerY = Window.ClientBounds.Height;

            _player = new Player(
                idleAnim,
                punchAnim,
                dodgeAnim,
                new Vector2(playerX, playerY),
                Window.ClientBounds.Width * 0.75f,
                Window.ClientBounds.Width * 0.25f
            );

            Animation enemyIdleAnim = atlas.GetAnimation("enemyIdle");
            Animation enemyPunchAnim = atlas.GetAnimation("enemyPunch");
            float enemyX = Window.ClientBounds.Width * 0.5f;
            float enemyY = Window.ClientBounds.Height * 0.95f; 
            _enemy = new Enemy(enemyIdleAnim, enemyPunchAnim, new Vector2(enemyX, enemyY));
            _combat = new CombatSystem(_enemy, _player);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // 1. Actualizamos el administrador de entradas primero
            _inputManager.Update(gameTime);

            // 2. Le pasamos el KeyboardInfo al Update del jugador
            _player.Update(gameTime, _inputManager.Keyboard);

            _enemy.Update(gameTime);
            _combat.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Rectangle fullscreenBounds = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            SpriteBatch.Draw(_ring, fullscreenBounds, Color.White);

            _enemy.Draw(SpriteBatch);
            _player.Draw(SpriteBatch);

            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}