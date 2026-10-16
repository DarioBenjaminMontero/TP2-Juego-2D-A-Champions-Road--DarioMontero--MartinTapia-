using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Graphics;

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

        public float limiteIzquierda, limiteDerecha;

        public enum PlayerState { Idle, Punching, Dodging }
        private PlayerState currentState = PlayerState.Idle;

        public Player(Animation idleAnim, Animation punchAnim, Animation dodgeAnim, Vector2 initialPosition, float limiteD, float limiteI)
        {
            Position = initialPosition;
            originalPosition = initialPosition;
            limiteDerecha = limiteD;
            limiteIzquierda = limiteI;

            _idleAnim = new AnimatedSprite(idleAnim, true);
            _punchSprite = new AnimatedSprite(punchAnim, false);
            _dodgeSprite = new AnimatedSprite(dodgeAnim, false);

            _currentSprite = _idleAnim;
            _currentSprite.Scale = new Vector2(2.8f, 2.8f);
            _punchSprite.Scale = new Vector2(2.8f, 2.8f);
            _dodgeSprite.Scale = new Vector2(2.8f, 2.8f);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            switch (currentState)
            {
                case PlayerState.Idle:
                    Position = originalPosition;
                    _currentSprite = _idleAnim;

                    if (kState.IsKeyDown(Keys.O) || kState.IsKeyDown(Keys.P))
                    {
                        currentState = PlayerState.Punching;
                        _punchSprite.Reset();
                        _currentSprite = _punchSprite;
                    }
                    else if (kState.IsKeyDown(Keys.A) || kState.IsKeyDown(Keys.D))
                    {
                        currentState = PlayerState.Dodging;
                        _dodgeSprite.Reset();
                        _currentSprite = _dodgeSprite;
                        Position.X = kState.IsKeyDown(Keys.A) ? originalPosition.X - 45 : originalPosition.X + 45;
                    }
                    break;

                case PlayerState.Punching:
                    _currentSprite = _punchSprite;
                    _punchSprite.Update(gameTime);
                    if (_punchSprite.IsFinished)
                    {
                        _punchSprite.Reset();
                        currentState = PlayerState.Idle;
                    }
                    break;

                case PlayerState.Dodging:
                    _currentSprite = _dodgeSprite;
                    _dodgeSprite.Update(gameTime);
                    if (_dodgeSprite.IsFinished)
                    {
                        _dodgeSprite.Reset();
                        currentState = PlayerState.Idle;
                    }
                    break;
            }

            if (_currentSprite.Region != null)
            {
                _currentSprite.Origin = new Vector2(_currentSprite.Region.Width / 2f, _currentSprite.Region.Height);
            }

            if (kState.IsKeyDown(Keys.P) || kState.IsKeyDown(Keys.D))
            {
                _currentSprite.Effects = SpriteEffects.FlipHorizontally;
            }
            else if (kState.IsKeyDown(Keys.O) || kState.IsKeyDown(Keys.A))
            {
                _currentSprite.Effects = SpriteEffects.None;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentSprite.Draw(spriteBatch, Position);
        }
    }
}