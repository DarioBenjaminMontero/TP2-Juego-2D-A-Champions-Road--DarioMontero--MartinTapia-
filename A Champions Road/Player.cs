


using System.Runtime.Serialization.Formatters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A_Champion_s_Road
{

    public class Player
    {

        public Vector2 Position;
        public Texture2D textura;
        public Texture2D texturaAlternativa;
        public Texture2D texturaAlternativa2;
        public Texture2D texturaNormal;
        public Texture2D _cargandoGolpeDerecha;
        public Texture2D _cargandoGolpeIzquierda;
        public float limiteIzquierda, limiteDerecha;
        public float Speed = 10f;
        public double tiempoTranscurrido = 0;
        public double limite = 0.10f;
        public double limiteGolpe = 0.999999999f;
        public int frameActual = 0;
        public int limiteFrames = 2;
        public Player(Texture2D texture, Texture2D altTexture, Texture2D altTexture2, Texture2D normalTexture, Texture2D _cargandoGolpeDerecha, Texture2D _cargandoGolpeIzquierda, Vector2 initialPosition, float limiteD, float limiteI)
        {
            this.textura = texture;
            this.texturaAlternativa = altTexture;
            this.texturaAlternativa2 = altTexture2;
            this.texturaNormal = normalTexture;
            this._cargandoGolpeDerecha = _cargandoGolpeDerecha;
            this._cargandoGolpeIzquierda = _cargandoGolpeIzquierda;
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
                    textura = texturaNormal;

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
                    textura = texturaNormal;
                    if (Position.X + Speed < limiteDerecha - textura.Width)
                    {
                        Position.X += Speed;
                    }
                    else
                    {
                        Position.X = limiteDerecha - textura.Width;
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
                    textura = texturaNormal;
                }
                else if (frameActual == 1)
                {
                    textura = _cargandoGolpeDerecha;
                }
                else if (frameActual == 2)
                {
                    textura = texturaAlternativa2;
                }
                if (tiempoTranscurrido >= limite)
                {
                    if (frameActual == 2)
                {
                        frameActual = 0;
                        currentState = PlayerState.Idle;
                        textura = texturaNormal;
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
        // Aquí puedes agregar lógica similar si el golpe izquierdo también lleva animación por frames,
        // o si es temporal, controlarlo con su propio tiempo. Por ahora lo dejamos simple:
        tiempoTranscurrido += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frameActual == 0)
                {
                    textura = texturaNormal;
                }
                else if (frameActual == 1)
                {
                    textura = _cargandoGolpeIzquierda;
                }
                else if (frameActual == 2)
                {
                    textura = texturaAlternativa;
                }
                if (tiempoTranscurrido >= limite)
                {
                    if (frameActual == 2)
                {
                        frameActual = 0;
                        currentState = PlayerState.Idle;
                        textura = texturaNormal;
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
            spriteBatch.Draw(textura, Position, Color.White);
        }
    }
}
