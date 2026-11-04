using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using Gum.Forms;
using Gum.Forms.Controls;
using MonoGameGum;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using System;  
using MonoGameLibrary.Scenes; // media hora no me tomaba el FightScene : Scene y era porq no importe esto, ojo 

namespace A_Champion_s_Road.Scenes;

public class FightScene : Scene // COMO NO HEREDA DE CORE (porque es escena ahora), SPRITEBATCH Y DEMAS TIENEN CORE.[etiqueta]
{

        private Player _player;
        private Enemy _enemy;
        private Texture2D _ring;
        private CombatSystem _combat;
        private EnemyAI _enemyAI;
        private TextureRegion _vidaVerde;
        private TextureRegion _vidaRoja;
        private TextureRegion _barraAmarilla;
        private TextureRegion _barraRoja;
        private InputManager _inputManager;
        private TextureAtlas _atlas;

    private const int AnchoBarraVida = 220;
    private const int AltoBarraVida = 18;

    private const int AnchoStamina = 196;
    private const int AltoStamina = 15;

    public override void Initialize()
    {
        base.Initialize();
        _inputManager = new InputManager();
    }

    public override void LoadContent()
    {
        // CAMBIO: factor de escala según el alto de la pantalla.
        // Las escalas originales (2.3f y 3.5f) estaban pensadas para 720 px de alto.
        float escala = MathHelper.Clamp(Core.Graphics.PreferredBackBufferHeight / 720f, 0.5f, 3f);
        Console.WriteLine($"Backbuffer: {Core.Graphics.PreferredBackBufferWidth}x{Core.Graphics.PreferredBackBufferHeight} | escala: {escala}");


        _ring = Content.Load<Texture2D>("ring3");
        _atlas = TextureAtlas.FromFile(Content, "atlas-definition.xml");

        TextureAtlas atlas = TextureAtlas.FromFile(Content, "atlas-definition.xml");

            // Animaciones del Player
            Animation idleAnim = atlas.GetAnimation("idleAnimation");
            Animation punchAnim = atlas.GetAnimation("punchingAnimation");
            Animation dodgeAnim = atlas.GetAnimation("dodgingAnimation");
            Animation hurtAnim = atlas.GetAnimation("hurtAnimation");
            Animation vulnerableAnim = atlas.GetAnimation("vulnerableAnimation");
            
            // 67
            _enemyAI = new EnemyAI();
            // float playerX = _pantalla.Width * 0.5f; // _pantalla es Window.ClientBounds
            // float playerY = _pantalla.Height;
            float playerX = Core.Instance.Window.ClientBounds.Width * 0.5f;
            float playerY = Core.Instance.Window.ClientBounds.Height;


            // animaciones jugador
            _player = new Player(
                idleAnim,
                punchAnim,
                dodgeAnim,
                hurtAnim,
                vulnerableAnim,
                new Vector2(playerX, playerY),
                Core.Instance.Window.ClientBounds.Width * 0.75f,
                Core.Instance.Window.ClientBounds.Width * 0.25f,
                escala 
            );

            // interfaz de vida
            _vidaVerde = atlas.GetRegion("vidaVerde");
            _vidaRoja = atlas.GetRegion("vidaRoja");

            // interfaz de stamina
            _barraAmarilla = atlas.GetRegion("staminaAmarilla");
            _barraRoja = atlas.GetRegion("staminaRoja");

            // animaciones del enemigo
            Animation enemyIdleAnim = atlas.GetAnimation("enemyIdle");
            Animation enemyPunchAnim = atlas.GetAnimation("enemyPunch");
            Animation enemyHurtAnim = atlas.GetAnimation("enemyHurt");
            float enemyX = Core.Instance.Window.ClientBounds.Width * 0.5f;
            float enemyY = Core.Instance.Window.ClientBounds.Height * 0.9f;
            _enemy = new Enemy(enemyIdleAnim, enemyPunchAnim, enemyHurtAnim, new Vector2(enemyX, enemyY), escala); // CAMBIO: nuevo parámetro
            _combat = new CombatSystem(_enemy, _player);   
    }

    public override void Update(GameTime gameTime)
    {
            // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            // Exit(); 
            // Lo comento porque la idea es que ahora al apretar escape, en vez de que nos saque del juego, nos ponga un menu de pausa

            // 1. Actualizamos el administrador de entradas primero
            _inputManager.Update(gameTime);


            // 2. Le pasamos el KeyboardInfo al Update del jugador
            _player.Update(gameTime, _inputManager.Keyboard);
            _enemyAI.Update(gameTime, _enemy, _player);
            _enemy.Update(gameTime);
            _combat.Update(gameTime);
            base.Update(gameTime);
    }

    private void DibujarBarraVida(int vida, int vidaMaxima, Vector2 posicion)
    {
        int anchoVerde = (int)(AnchoBarraVida *
            MathHelper.Clamp((float)vida / vidaMaxima, 0f, 1f));

        Core.SpriteBatch.Draw(
            _vidaRoja.Texture,
            new Rectangle((int)posicion.X, (int)posicion.Y, AnchoBarraVida, AltoBarraVida),
            _vidaRoja.SourceRectangle,
            Color.White
        );

        if (anchoVerde > 0)
        {
            Core.SpriteBatch.Draw
            (
                _vidaVerde.Texture,
                new Rectangle((int)posicion.X, (int)posicion.Y, anchoVerde, AltoBarraVida),
                _vidaVerde.SourceRectangle,
                Color.White
            );
        }
    }

    private void BarraStamina()
    {
        Core.SpriteBatch.Draw(
            _barraRoja.Texture,
            new Rectangle(32, 60, AnchoStamina, AltoStamina),
            _barraRoja.SourceRectangle,
            Color.White
        );

        // Core.SpriteBatch.Draw(
        //     _barraAmarilla.Texture,
        //     new Rectangle(32, 60, AnchoStamina, AnchoStamina),
        //     _barraAmarilla.SourceRectangle,
        //     Color.White
        // );
    } 

            public override void Draw(GameTime gameTime) // lo pase de protected a public porque tiraba un error
            {
                Core.GraphicsDevice.Clear(Color.CornflowerBlue);
                Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);


                Rectangle fullscreenBounds = new Rectangle(0, 0, Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height);
                Core.SpriteBatch.Draw(_ring, fullscreenBounds, Color.White);


                _enemy.Draw(Core.SpriteBatch);
                _player.Draw(Core.SpriteBatch);
                
                DibujarBarraVida(_player.vidaActual, Player.vidaMaxima, new Vector2(30, 30));

                DibujarBarraVida
                (
                    _enemy.vidaActual,
                    Enemy.vidaMaxima,
                    new Vector2(Core.GraphicsDevice.Viewport.Width - AnchoBarraVida - 30, 30)
                );

                BarraStamina();
                        
                Core.SpriteBatch.End();
                base.Draw(gameTime);
            }
    }