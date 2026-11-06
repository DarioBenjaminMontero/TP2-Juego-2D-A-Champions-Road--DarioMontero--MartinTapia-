using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input; // Asegúrate de tener este using si tu KeyboardInfo está en ese namespace
using Microsoft.Xna.Framework.Input;
using System;
using System.Formats.Asn1;
using System.Security.Cryptography.X509Certificates;
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
        AnimatedSprite _vulnerableAnim;
        public const int vidaMaxima = 100;
        public int vidaActual { get; set; } = vidaMaxima;
        public const int staminaMaxima = 100;
        public int staminaActual { get; set; } = staminaMaxima;
        public int costoGolpe = 10;
        public int boostEsquive = 5;
        public int roundsGanados { get; set; } = 0;
        private float _tiempoVulnerableMax = 3.0f;
        private float _tiempoHurtingMax = 0.2f;

        private float _cronometroHurting = 0f;
        private float _cronometroVulnerable = 0f;


        public float limiteIzquierda, limiteDerecha;


        public enum PlayerState { Idle, Punching, Dodging, Hurting }
        public bool Vulnerable = false;
        public enum Direction { None, Left, Right }
        public PlayerState CurrentState { get; set; } = PlayerState.Idle;
        public Direction CurrentDirectionPunch { get; set; } = Direction.None;
        public Direction CurrentDirectionDodge { get; set; } = Direction.None;
        public bool IsDodgeFinished => _dodgeSprite.IsFinished;
        public int CurrentPunchFrame => _punchSprite.CurrentFrame;

        // CAMBIO: nuevo parámetro "escala" (factor según la pantalla)
        public Player(Animation idleAnim, Animation punchAnim, Animation dodgeAnim, Animation hurtAnimation, Animation vulnerableAnim, Vector2 initialPosition, float limiteD, float limiteI, float escala = 1f)
        {
            Position = initialPosition;
            originalPosition = initialPosition;
            limiteDerecha = limiteD;
            limiteIzquierda = limiteI;
            _vulnerableAnim = new AnimatedSprite(vulnerableAnim, true);
            _idleAnim = new AnimatedSprite(idleAnim, true);
            _punchSprite = new AnimatedSprite(punchAnim, false);
            _dodgeSprite = new AnimatedSprite(dodgeAnim, false);
            _hurtAnim = new AnimatedSprite(hurtAnimation, false);


            _currentSprite = _idleAnim;

            // CAMBIO: la escala base (2.3f) se multiplica por el factor de pantalla
            Vector2 escalaFinal = new Vector2(1.8f * escala, 1.8f * escala);
            _currentSprite.Scale = escalaFinal;
            _punchSprite.Scale = escalaFinal;
            _dodgeSprite.Scale = escalaFinal;
            _hurtAnim.Scale = escalaFinal;
            _vulnerableAnim.Scale = escalaFinal;
        }
        public void ReiniciarRound()
        {
            vidaActual = 100;
            CurrentState = PlayerState.Idle;
            Vulnerable = false;
            staminaActual = staminaMaxima;
            _cronometroHurting = 0f;
            _cronometroVulnerable = 0f;
            _currentSprite = _idleAnim;
        }
        public void RecibirGolpe()
        {
            CurrentState = PlayerState.Hurting;
            if (vidaActual <= 0) 
                return;

            vidaActual = Math.Max(0, vidaActual - 10);

            _cronometroHurting = 0f;
            _hurtAnim.Reset();
            _currentSprite = _hurtAnim;
        }

        public void HacerVulnerable()
        {
            Vulnerable = true;
            _cronometroVulnerable = 0f;
            _vulnerableAnim.Reset();
        }
        // Recibimos KeyboardInfo en lugar de consultar Keyboard.GetState() cada vez
        public void Update(GameTime gameTime, KeyboardInfo keyboard)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // 1. Gestionamos el temporizador de vulnerabilidad en segundo plano
            if (Vulnerable)
            {
                _cronometroVulnerable += dt;
                if (_cronometroVulnerable >= _tiempoVulnerableMax)
                {
                    Vulnerable = false;
                    _cronometroVulnerable = 0f;
                }
            }

            if (CurrentState == PlayerState.Hurting)
            {

                _currentSprite = _hurtAnim;
                _hurtAnim.Update(gameTime);

                _cronometroHurting += dt;

                if (_cronometroHurting >= _tiempoHurtingMax || _hurtAnim.IsFinished)
                {
                    _hurtAnim.Reset();
                    CurrentState = PlayerState.Idle;

                }

            }

            // 2. Máquina de estados principal
            switch (CurrentState)
            {
                case PlayerState.Idle:
                    Position = originalPosition;

                    // El sprite vulnerable solo aplica si estamos en Idle real y somos vulnerables
                    if (Vulnerable)
                    {
                        _currentSprite = _vulnerableAnim;
                        _vulnerableAnim.Update(gameTime);
                    }
                    else
                    {
                        _currentSprite = _idleAnim;

                        float oscilacionPaso = (float)gameTime.TotalGameTime.TotalSeconds * 8f;
                        Position.X = originalPosition.X + (float)Math.Sin(oscilacionPaso) * 20f;
                        float reboteY = Math.Abs((float)Math.Sin(oscilacionPaso)) * 6f;
                        Position.Y = originalPosition.Y + reboteY;
                    }

                    // Entradas de teclado para Punch o Dodge...
                    if (keyboard.WasKeyJustPressed(Keys.O) || keyboard.WasKeyJustPressed(Keys.P))
                    {
                        if (!Vulnerable)
                        {
                            if(staminaActual < costoGolpe)
                            {
                                HacerVulnerable();
                            }
                            if(staminaActual >= costoGolpe)
                            {
                                staminaActual -= costoGolpe;
                                CurrentState = PlayerState.Punching;
                                CurrentDirectionDodge = Direction.None;
                                _punchSprite.Reset();
                                _currentSprite = _punchSprite;
                            }
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
                    }
                    else if (keyboard.WasKeyJustPressed(Keys.A) || keyboard.WasKeyJustPressed(Keys.D))
                    {

                        if (!Vulnerable)
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