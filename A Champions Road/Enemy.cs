using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using System;
namespace A_Champion_s_Road
{
    public class Enemy
    {
        public Vector2 Position;
        private AnimatedSprite _idleAnim;
        private AnimatedSprite _punchAnim;
        private AnimatedSprite _currentSprite;
        float _tiempoParaDecidir = 1.0f;
        float _cronometroIA = 0.0f;
        float _cronometroVulnerable = 0.0f;

        public enum EnemyState { Idle, Punching, Vulnerable, Hurting }
        public EnemyState CurrentEnemyState { get; set; } = EnemyState.Idle;
        public bool IsCounterable { get; set; } = false;
public int CurrentPunchFrame => _punchAnim.CurrentFrame;


        public Enemy(Animation idleAnim, Animation punchAnim, Vector2 initialPosition)
        {
            Position = initialPosition;

            // Creamos la animación en bucle (true)
            _idleAnim = new AnimatedSprite(idleAnim, true);
            _punchAnim = new AnimatedSprite(punchAnim, false); // No se repite
            _currentSprite = _idleAnim;
            _idleAnim.Scale = new Vector2(2.5f, 2.5f);
            _punchAnim.Scale = new Vector2(2.5f, 2.5f); // altura del tyke mason
        }
        static Random aleatorio = new Random();
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _currentSprite.Update(gameTime);

            if (_currentSprite.Region != null)
            {
                _currentSprite.Origin = new Vector2(_currentSprite.Region.Width / 2f, _currentSprite.Region.Height);
            }

            if (CurrentEnemyState == EnemyState.Idle)
            {
                _cronometroIA += dt;
                if (_cronometroIA >= _tiempoParaDecidir)
                {
                    _cronometroIA = 0f;
                    int numeroAleatorio = aleatorio.Next(1, 5);
                    if (numeroAleatorio == 3)
                    {
                        CurrentEnemyState = EnemyState.Punching;
                        _punchAnim.Reset();
                    }
                }
            }
            switch (CurrentEnemyState)
            {

                case EnemyState.Idle:

                    _currentSprite = _idleAnim;
                    break;
                case EnemyState.Punching:
                    _currentSprite = _punchAnim;
                   if (_punchAnim.CurrentFrame == 0)
                    {
                        IsCounterable = true;
                    }
                    else
                    {
                        IsCounterable = false;
                    }

                    if (_punchAnim.IsFinished)
                    {
                        CurrentEnemyState = EnemyState.Idle;
                        _cronometroIA = 0f;
                    }

                    break;
                case EnemyState.Vulnerable:

                    _cronometroVulnerable += dt;
                    if (_cronometroVulnerable >= 1.0f)
                    {
                        _cronometroVulnerable = 0f;
                        CurrentEnemyState = EnemyState.Idle;
                    }

                    break;
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentSprite.Draw(spriteBatch, Position);
        }
    }
}