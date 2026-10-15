using System.Runtime.Serialization.Formatters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;

namespace A_Champion_s_Road
{
    public class Player
    {
        public Vector2 Position;
        private float originalY; // Guardamos la posición Y original (el suelo)
private float originalX;
        public Sprite currentRegion; 
        public Sprite idleRegion;
        public Sprite chargeRegion;
        public Sprite punchingRegion;
        public Sprite dodgingRegion;
        public Sprite dodgingRegion2;
        public Sprite dodgingRegion3;
        public float limiteIzquierda, limiteDerecha;
        public float Speed = 10f;
        public double tiempoTranscurrido = 0;
        public double limite = 0.10f;
        public double limiteGolpe = 0.999999999f;
        public int frameActual = 0;
        public int limiteFrames = 3;

        public Player(Sprite idle, Sprite charge, Sprite punching, Sprite dodging, Sprite dodging2, Sprite dodging3, Vector2 initialPosition, float limiteD, float limiteI)
        {
            idleRegion = idle;
            chargeRegion = charge;
            punchingRegion = punching;
            dodgingRegion = dodging;
            dodgingRegion2 = dodging2;
            dodgingRegion3 = dodging3;

            currentRegion = idleRegion; 

            Position = initialPosition;
            originalY = initialPosition.Y; // Registramos la posición del suelo al iniciar
            originalX = initialPosition.X;
            limiteDerecha = limiteD;
            limiteIzquierda = limiteI;
        }

        public enum PlayerState { Idle, PunchingLeft, PunchingRight, Blocking, Hurt, DodgingLeft, DodgingRight }
        private PlayerState currentState = PlayerState.Idle;

        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            if (currentState == PlayerState.Idle)
            {
                if (kState.IsKeyDown(Keys.A))
                {
                    currentState = PlayerState.DodgingLeft;
                    frameActual = 0;
                    tiempoTranscurrido = 0;
                }
                else if (kState.IsKeyDown(Keys.D))
                {
                    currentState = PlayerState.DodgingRight;
                    frameActual = 0;
                    tiempoTranscurrido = 0;
                }

                if (kState.IsKeyDown(Keys.O))
                {
                    currentState = PlayerState.PunchingLeft;
                    frameActual = 0;
                    tiempoTranscurrido = 0;
                }
                else if (kState.IsKeyDown(Keys.P))
                {
                    currentState = PlayerState.PunchingRight;
                    frameActual = 0;
                    tiempoTranscurrido = 0;
                }
            }

            // ANIMACION DEL GOLPE
            else if (currentState == PlayerState.PunchingRight || currentState == PlayerState.PunchingLeft)
            {
                tiempoTranscurrido += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frameActual == 0)
                {
                    currentRegion = idleRegion;
                }
                else if (frameActual == 1)
                {
                    currentRegion = chargeRegion;
                }
                else if (frameActual == 2)
                {
                    currentRegion = punchingRegion;
                }
                if (tiempoTranscurrido >= limite)
                {
                    if (frameActual == 3)
                    {
                        frameActual = 0;
                        currentState = PlayerState.Idle;
                        currentRegion = idleRegion;
                    }
                    else
                    {
                        frameActual = frameActual + 1;
                    }
                    tiempoTranscurrido = tiempoTranscurrido - limite;
                }
            }

            // ANIMACION DEL ESQUIVE
            if(currentState == PlayerState.DodgingRight || currentState == PlayerState.DodgingLeft)
            {
                tiempoTranscurrido += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if(frameActual == 0)
                {
                    currentRegion = idleRegion;
                }
                else if(frameActual == 1)
                {
                    currentRegion = dodgingRegion;
                }
                else if(frameActual == 2)
                {
                    currentRegion = dodgingRegion2;
                }
                else if(frameActual == 3)
                {
                    currentRegion = dodgingRegion3;
                }
                if(tiempoTranscurrido >= limite)
                {
                    if(frameActual == 3)
                    {
                        frameActual = 0;
                        currentState = PlayerState.Idle;
                        currentRegion = idleRegion;
                    }
                    else
                    {
                        frameActual = frameActual + 1;
                    }
                    tiempoTranscurrido = tiempoTranscurrido - limite;
                }
            }

if (currentState == PlayerState.DodgingRight || currentState == PlayerState.DodgingLeft)
{

    float diferenciaAltura = idleRegion.Height - currentRegion.Height;
    Position.Y = originalY + diferenciaAltura;
    if(currentState == PlayerState.DodgingLeft)
                {
                    Position.X = originalX -45;
                }
                else if(currentState == PlayerState.DodgingRight)
                {
                    Position.X = originalX + 45;

                }
                else
                {
                    Position.X = originalX;

                }
}
else if (currentRegion == punchingRegion)
{
    Position.Y = originalY - 70f; 
}
else
{
    Position.Y = originalY;
}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(currentState == PlayerState.PunchingRight || currentState == PlayerState.DodgingRight)
            {
                currentRegion.Effects = SpriteEffects.FlipHorizontally;        
            } 
            else
            {
                currentRegion.Effects = SpriteEffects.None;
            }

            currentRegion.Draw(spriteBatch, Position);
        }
    }
}