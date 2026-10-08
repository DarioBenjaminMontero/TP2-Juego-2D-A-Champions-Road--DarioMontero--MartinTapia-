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
        public TextureRegion currentRegion; // variable que va a cambiar dependiendo q pase
        public TextureRegion idleRegion;
        public TextureRegion chargeRegion;
        public TextureRegion punchingRegion;

        public float limiteIzquierda, limiteDerecha;
        public float Speed = 10f;
        public double tiempoTranscurrido = 0;
        public double limite = 0.10f;
        public double limiteGolpe = 0.999999999f;
        public int frameActual = 0;
        public int limiteFrames = 2;
        public Player(TextureRegion idle, TextureRegion charge, TextureRegion punching, Vector2 initialPosition, float limiteD, float limiteI)
        {
            idleRegion = idle;
            chargeRegion = charge;
            punchingRegion = punching;

            currentRegion = idleRegion; // por default va a quedar en idle

            Position = initialPosition;
            limiteDerecha = limiteD;
            limiteIzquierda = limiteI;
        }
        public enum PlayerState
        {
            Idle,
            PunchingLeft,
            PunchingRight,
            Blocking,
            Hurt
        }
        private PlayerState currentState = PlayerState.Idle;
        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            if (currentState == PlayerState.Idle)
            {
                if (kState.IsKeyDown(Keys.A))
                {
                    currentState = PlayerState.Idle;
                    currentRegion = idleRegion;
    
                    if (Position.X - Speed > limiteIzquierda)
                    {
                        Position.X -= Speed;
                    }
                    else
                    {
                        Position.X = limiteIzquierda;
                    }
                }
                else if (kState.IsKeyDown(Keys.D))
                {
                    currentState = PlayerState.Idle;
                    // textura = texturaNormal;
                    if (Position.X + Speed < limiteDerecha - currentRegion.Width) // textura.Width
                    {
                        Position.X += Speed;
                    }
                    else
                    {
                        Position.X = limiteDerecha - currentRegion.Width; // textura.Width
                    }
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
            else if (currentState == PlayerState.PunchingRight)
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
                        // textura = texturaNormal;
                    }
                    else
                    {  
             frameActual = frameActual + 1;
                    }
                    tiempoTranscurrido = tiempoTranscurrido -limite;
                }
            }
            else if (currentState == PlayerState.PunchingLeft)
    {
        tiempoTranscurrido += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frameActual == 0)
                {
                    // textura = texturaNormal;
                    currentRegion = idleRegion;
                }
                else if (frameActual == 1)
                {
                    // textura = _cargandoGolpeIzquierda;
                    currentRegion = chargeRegion;
                }
                else if (frameActual == 2)
                {
                    // textura = texturaAlternativa;
                    currentRegion = punchingRegion;
                }
                if (tiempoTranscurrido >= limite)
                {
                    if (frameActual == 2)
                {
                        frameActual = 0;
                        currentState = PlayerState.Idle;
                        // textura = texturaNormal;
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

            SpriteEffects effect = (currentState == PlayerState.PunchingLeft) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            currentRegion.Draw(
                spriteBatch, 
                Position, 
                Color.White, 
                0.0f, 
                Vector2.Zero, 
                1.0f, 
                effect, 
                0.0f
            );
        }
    }
}
