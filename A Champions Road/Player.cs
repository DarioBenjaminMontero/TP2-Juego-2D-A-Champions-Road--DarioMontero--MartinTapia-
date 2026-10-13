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

        // public Texture2D texture;
        public Sprite currentRegion; // variable que va a cambiar dependiendo q pase
        public Sprite idleRegion;
        public Sprite chargeRegion;
        public Sprite punchingRegion;
        public Sprite dodgingRegion;
        public float limiteIzquierda, limiteDerecha;
        public float Speed = 10f;
        public double tiempoTranscurrido = 0;
        public double limite = 0.10f;
        public double limiteGolpe = 0.999999999f;
        public int frameActual = 0;
        public int limiteFrames = 2;
        public Player(Sprite idle, Sprite charge, Sprite punching, Sprite dodging, Vector2 initialPosition, float limiteD, float limiteI)
        {
            idleRegion = idle;
            chargeRegion = charge;
            punchingRegion = punching;
            dodgingRegion = dodging;

            currentRegion = idleRegion; // por default va a quedar en idle

            Position = initialPosition;
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
                // Si apretas [A] el personaje esquiva hacia la izquierda
                if (kState.IsKeyDown(Keys.A))
                {
                    currentState = PlayerState.DodgingLeft;
                    frameActual = 0;
                    tiempoTranscurrido = 0;
                }

                // Si apretas [D] el peronaje esquiva hacia la derecha
                else if (kState.IsKeyDown(Keys.D))
                {
                    currentState = PlayerState.DodgingRight;
                    frameActual = 0;
                    tiempoTranscurrido = 0;
                }

                // Si apretas [O] el personaje va a hacer un golpe con el brazo izquierdo
                if (kState.IsKeyDown(Keys.O))
                {
                    currentState = PlayerState.PunchingLeft;
                    frameActual = 0;
                    tiempoTranscurrido = 0;
                }

                // Si apretas [P] el personaje va a hacer un golpe con el brazo derecho
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
                    // textura = texturaNormal;
                    currentRegion = idleRegion;
                }
                else if (frameActual == 1)
                {
                    // textura = _cargandoGolpeDerecha;
                    currentRegion = chargeRegion;
                }
                else if (frameActual == 2)
                {
                    // textura = texturaAlternativa2;
                    currentRegion = punchingRegion;
                }
                if (tiempoTranscurrido >= limite)
                {
                    if (frameActual == 2)
                    {
                        frameActual = 0;
                        currentState = PlayerState.Idle;
                        currentRegion = idleRegion;
                        // textura = texturaNormal;
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
                if(tiempoTranscurrido >= limite)
                {
                    if(frameActual == 1)
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

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.Draw(textura, Position, Color.White);

        if(currentState == PlayerState.PunchingRight || currentState == PlayerState.DodgingRight)
        {
            currentRegion.Effects = SpriteEffects.FlipHorizontally;        
        } else
            {
                currentRegion.Effects = SpriteEffects.None;
            }

            currentRegion.Draw(spriteBatch, Position);
        }
    }
}
