using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input; // Asegúrate de tener este using si tu KeyboardInfo está en ese namespace
using Microsoft.Xna.Framework.Input;
using System;
namespace A_Champion_s_Road
{
    public class Player
    {
        public Vector2 Position;
        private Vector2 originalPosition;

        private AnimatedSprite _punchSprite;
        private AnimatedSprite _dodgeSprite;
        private AnimatedSprite _idleAnim;
        private AnimatedSprite _currentSprite;
        private AnimatedSprite _hurtAnim;
        private float _tiempoVulnerableMax = 1.0f;
        private float _tiempoHurtingMax = 0.1f;
        private float _cronometroEstado = 0f;

        public float limiteIzquierda, limiteDerecha;

        public enum PlayerState { Idle, Punching, Dodging, Hurting }
        public bool Vulnerable = false;
        public enum Direction { None, Left, Right }
        public PlayerState CurrentState { get; set; } = PlayerState.Idle;
        public Direction CurrentDirectionPunch { get; set; } = Direction.None;
        public Direction CurrentDirectionDodge { get; set; } = Direction.None;
        public bool IsDodgeFinished => _dodgeSprite.IsFinished;
        public int CurrentPunchFrame => _punchSprite.CurrentFrame;
        public Player(Animation idleAnim, Animation punchAnim, Animation dodgeAnim, Animation hurtAnimation, Vector2 initialPosition, float limiteD, float limiteI)
        {
            Position = initialPosition;
            originalPosition = initialPosition;
            limiteDerecha = limiteD;
            limiteIzquierda = limiteI;

            _idleAnim = new AnimatedSprite(idleAnim, true);
            _punchSprite = new AnimatedSprite(punchAnim, false);
            _dodgeSprite = new AnimatedSprite(dodgeAnim, false);
            _hurtAnim = new AnimatedSprite(hurtAnimation, false);

            _currentSprite = _idleAnim;
            _currentSprite.Scale = new Vector2(1.5f, 1.5f);
            _punchSprite.Scale = new Vector2(1.5f, 1.5f);
            _dodgeSprite.Scale = new Vector2(1.5f, 1.5f);
            _hurtAnim.Scale = new Vector2(1.5f, 1.5f);
        }
        public void RecibirGolpe()
        {
            CurrentState = PlayerState.Hurting;
            _cronometroEstado = 0f;
            _hurtAnim.Reset();
            _currentSprite = _hurtAnim;
        }
        // Recibimos KeyboardInfo en lugar de consultar Keyboard.GetState() cada vez
       public void Update(GameTime gameTime, KeyboardInfo keyboard)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 1. Gestionamos el temporizador de vulnerabilidad de forma independiente en segundo plano
            if (Vulnerable)
            {
                _cronometroEstado += dt;
                if (_cronometroEstado >= _tiempoVulnerableMax)
                {
                    Vulnerable = false;     
                    _cronometroEstado = 0f;  
                }
            }

            switch (CurrentState)
            {
                case PlayerState.Idle:
                    Position = originalPosition;
                    _currentSprite = _idleAnim;

                   
                    float oscilacionPaso = (float)gameTime.TotalGameTime.TotalSeconds * 8f;
                    Position.X = originalPosition.X + (float)Math.Sin(oscilacionPaso) * 20f;
                    float reboteY = Math.Abs((float)Math.Sin(oscilacionPaso)) * 6f;
                    Position.Y = originalPosition.Y + reboteY;

                    // Usamos WasKeyJustPressed para mayor precisión en juegos de combate
                    if (keyboard.WasKeyJustPressed(Keys.O) || keyboard.WasKeyJustPressed(Keys.P))
                    {
                        CurrentState = PlayerState.Punching;
                        CurrentDirectionDodge = Direction.None;
                        _punchSprite.Reset();
                        _currentSprite = _punchSprite;

                        if (keyboard.WasKeyJustPressed(Keys.P))
                        {
                            CurrentDirectionPunch = Direction.Right;
                            _currentSprite.Effects = SpriteEffects.FlipHorizontally;
                        }
                        else if (keyboard.WasKeyJustPressed(Keys.O))
                        {
                            CurrentDirectionPunch = Direction.Left;
                            _currentSprite.Effects = SpriteEffects.None;
                        }
                    }
                    else if (keyboard.WasKeyJustPressed(Keys.A) || keyboard.WasKeyJustPressed(Keys.D))
                    {
                        CurrentState = PlayerState.Dodging;
                        CurrentDirectionPunch = Direction.None;
                        _dodgeSprite.Reset();
                        _currentSprite = _dodgeSprite;

                        if (keyboard.WasKeyJustPressed(Keys.D))
                        {
                            _currentSprite.Effects = SpriteEffects.FlipHorizontally;
                            Position.X = originalPosition.X + 45;
                            CurrentDirectionDodge = Direction.Right;
                        }
                        else if (keyboard.WasKeyJustPressed(Keys.A))
                        {
                            _currentSprite.Effects = SpriteEffects.None;
                            Position.X = originalPosition.X - 45;
                            CurrentDirectionDodge = Direction.Left;
                        }
                    }
                    else
                    {
                        CurrentDirectionPunch = Direction.None;
                        CurrentDirectionDodge = Direction.None;
                        _currentSprite.Effects = SpriteEffects.None;
                    }
                    break;

                case PlayerState.Punching:
                    _currentSprite = _punchSprite;
                    _punchSprite.Update(gameTime);
                    if (_punchSprite.IsFinished)
                    {
                        _punchSprite.Reset();
                        CurrentState = PlayerState.Idle;
                    }
                    break;

                case PlayerState.Dodging:
                    _currentSprite = _dodgeSprite;
                    _dodgeSprite.Update(gameTime);
                    if (_dodgeSprite.IsFinished)
                    {
                        _dodgeSprite.Reset();
                        CurrentState = PlayerState.Idle;
                    }
                    break;

                case PlayerState.Hurting:
                    _currentSprite = _hurtAnim;
                    _hurtAnim.Update(gameTime);

                    _cronometroEstado += dt;

                    if (_cronometroEstado >= _tiempoHurtingMax || _hurtAnim.IsFinished)
                    {
                        _hurtAnim.Reset();
                        CurrentState = PlayerState.Idle; 
                        Vulnerable = true;              
                        _cronometroEstado = 0f;        
                    }
                    break;
            }

            if (_currentSprite.Region != null)
            {
                _currentSprite.Origin = new Vector2(_currentSprite.Region.Width / 2f, _currentSprite.Region.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentSprite.Draw(spriteBatch, Position);
        }
    }
}