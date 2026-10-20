using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace A_Champion_s_Road
{
    public class Enemy
    {
        public Vector2 Position;
        private AnimatedSprite _idleAnim;
        private AnimatedSprite _currentSprite;

        public Enemy(Animation idleAnim, Vector2 initialPosition)
        {
            Position = initialPosition;

            // Creamos la animación en bucle (true)
            _idleAnim = new AnimatedSprite(idleAnim, true);
            _currentSprite = _idleAnim;
            _currentSprite.Scale = new Vector2(1.0f, 1.0f); // altura del tyke mason
        }

        public void Update(GameTime gameTime)
        {
            _currentSprite.Update(gameTime);

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