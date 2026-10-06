


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
public float limiteIzquierda, limiteDerecha;
public float Speed = 10f;


public Player(Texture2D texture, Texture2D altTexture, Texture2D altTexture2, Texture2D normalTexture, Vector2 initialPosition, float limiteD, float limiteI)
    {
        
        this.textura = texture;
        this.texturaAlternativa = altTexture;
        this.texturaAlternativa2 = altTexture2;
        this.texturaNormal = normalTexture;
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


if(currentState == PlayerState.Idle)
            {
                
                 if(kState.IsKeyDown(Keys.A))
            {
                currentState = PlayerState.Idle;
                textura = texturaNormal;
                
                if(Position.X - Speed > limiteIzquierda)
                    {
                        Position.X -= Speed;
                    }
                    else
    {
        Position.X = limiteIzquierda; 
    }
            }
            else if(kState.IsKeyDown(Keys.D))
            {
                currentState = PlayerState.Idle;
                textura = texturaNormal;
                if(Position.X + Speed < limiteDerecha - textura.Width)
                    {
                        Position.X += Speed;
                    }
                    else {
        Position.X = limiteDerecha- textura.Width; 
    }

            }
            }
            

            if (kState.IsKeyDown(Keys.O))
{
    currentState = PlayerState.PunchingLeft;
    textura = texturaAlternativa;
}
else if (kState.IsKeyDown(Keys.P))
{
    currentState = PlayerState.PunchingRight;
    textura = texturaAlternativa2;
}
else
{
    currentState = PlayerState.Idle;
    textura = texturaNormal;
}
            
    }

    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(textura, Position, Color.White);
    }
}
    

}
