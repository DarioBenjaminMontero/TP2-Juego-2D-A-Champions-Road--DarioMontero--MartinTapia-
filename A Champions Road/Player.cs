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
public Texture2D texturaNormal;
public float Speed = 10f;


public Player(Texture2D texture, Texture2D altTexture, Texture2D normalTexture, Vector2 initialPosition)
    {
        
        this.textura = texture;
        this.texturaAlternativa = altTexture;
        this.texturaNormal = normalTexture;
        Position = initialPosition;
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
                Position.X -= Speed;
            }
            else if(kState.IsKeyDown(Keys.D))
            {
                currentState = PlayerState.Idle;
                textura = texturaNormal;
Position.X += Speed;
            }
            }
            

            if (!kState.IsKeyDown(Keys.P))
            {
                 
                currentState = PlayerState.Idle;
                textura = texturaNormal;

            }
            else if(kState.IsKeyDown(Keys.P))
            {
                currentState = PlayerState.PunchingLeft;
                textura = texturaAlternativa;

            }
    }

    // 4. El Draw (su forma de pintarse en pantalla)
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(textura, Position, Color.White);
    }
}
    

}

