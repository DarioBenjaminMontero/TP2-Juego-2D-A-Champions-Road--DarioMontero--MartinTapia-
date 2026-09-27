using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A_Champion_s_Road;

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

public void Update(GameTime gameTime)
    {
        KeyboardState kState = Keyboard.GetState();

        
        if (kState.IsKeyDown(Keys.A))
        {
            Position.X -= Speed; // Se mueve a la izquierda
        }
        if (kState.IsKeyDown(Keys.D))
        {
            Position.X += Speed; // Se mueve a la derecha
        }
        if (kState.IsKeyDown(Keys.P)) 
        {
            textura = texturaAlternativa; // golpe izq
        }
        if(kState.IsKeyUp(Keys.P))
        {
            textura = texturaNormal;
        }
    }

    // 4. El Draw (su forma de pintarse en pantalla)
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(textura, Position, Color.White);
    }
}
    