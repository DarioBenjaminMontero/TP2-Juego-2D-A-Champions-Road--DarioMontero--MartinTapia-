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
        private Enemy _enemy; // <--- 1. Declarar el enemigo
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
            float enemyY = Window.ClientBounds.Height * 0.95f; // POR AHORA EL ENEMIGO ES UNA ANIMACION EN BUCLE NOMAS 
            _enemy = new Enemy(enemyIdleAnim, enemyPunchAnim, new Vector2(enemyX, enemyY));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _player.Update(gameTime);
            _enemy.Update(gameTime);
            CheckCombatInteractions();
            base.Update(gameTime);
        }

        public void CheckCombatInteractions()
{
    int frameActualEnemigo = _enemy.CurrentPunchFrame;

    switch ((_player.CurrentState, _enemy.CurrentEnemyState, _enemy.IsCounterable))
    {
  
case (Player.PlayerState.Idle, Enemy.EnemyState.Punching, true):
            // Se deja vacío a propósito para que el juego continúe normalmente
            
            break;
        case (Player.PlayerState.Punching, Enemy.EnemyState.Punching, true):
            _enemy.CurrentEnemyState = Enemy.EnemyState.Hurting;
            _enemy.IsCounterable = false;
            break;

   
        case (Player.PlayerState.Dodging, Enemy.EnemyState.Punching, true):
            _player.CurrentState = Player.PlayerState.Hurting;
            break;


        case (Player.PlayerState.Dodging, Enemy.EnemyState.Punching, false):
            if (frameActualEnemigo == 2 || frameActualEnemigo == 1)
            {
                _enemy.CurrentEnemyState = Enemy.EnemyState.Vulnerable;
            }
            break;

        // CASTIGO GOLPE TARDÍO: Intentás pegarle en el Frame 2 (ya no es counterable) -> Te frena el golpe y te daña
        case (Player.PlayerState.Punching, Enemy.EnemyState.Punching, false):
            _player.CurrentState = Player.PlayerState.Hurting;
            break;

        // ==========================================
        // COMBATE PASIVO (Te quedaste quieto esperando)
        // ==========================================
        default:
            // DAÑO PASIVO: Si estás en Idle y el enemigo llega al frame del impacto (Frame 2), te golpea físicamente
            if (_enemy.CurrentEnemyState == Enemy.EnemyState.Punching && frameActualEnemigo == 2 && _player.CurrentState == Player.PlayerState.Idle)
            {
                _player.CurrentState = Player.PlayerState.Hurting;
            }

            if (_player.CurrentState == Player.PlayerState.Vulnerable)
            {
                _enemy.CurrentEnemyState = Enemy.EnemyState.Punching;
            }
            break;
    }
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