using CsvHelper;
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
        private AnimatedSprite _hurtAnim;
        private AnimatedSprite _currentSprite;

public const int vidaMaxima = 1000;
public int vidaActual {get; set;} = vidaMaxima;
        private float _tiempoVulnerableMax = 2.0f;
        private float _cronometroEstado = 0f;


        private float _tiempoHurtingMax = 0.2f;

        private float _cronometroHurting = 0f;
        private float _cronometroVulnerable = 0f;
        public bool Vulnerable = false;

        public enum EnemyState { Idle, Punching, Hurting }
        public EnemyState CurrentEnemyState { get; set; } = EnemyState.Idle;
        public bool IsCounterable { get; set; } = false;



        public int CurrentPunchFrame => _punchAnim.CurrentFrame;


        // CAMBIO: nuevo parámetro "escala" (factor según la pantalla)
        public Enemy(Animation idleAnim, Animation punchAnim, Animation hurtAnim, Vector2 initialPosition, float escala = 1f)
        {
            Position = initialPosition;

            // Creamos la animación en bucle (true)
            _idleAnim = new AnimatedSprite(idleAnim, true);
            _punchAnim = new AnimatedSprite(punchAnim, false); // No se repite
            _hurtAnim = new AnimatedSprite(hurtAnim, false);
            _currentSprite = _idleAnim;

            // CAMBIO: la escala base (3.5f) se multiplica por el factor de pantalla
            Vector2 escalaFinal = new Vector2(1.2f * escala, 1.2f * escala);
            _idleAnim.Scale = escalaFinal;
            _punchAnim.Scale = escalaFinal;
            _hurtAnim.Scale = escalaFinal; // antes no tenía escala asignada
        }
        public void StartPunch()
        {
            CurrentEnemyState = EnemyState.Punching;
            _punchAnim.Reset();
            _cronometroEstado = 0f;
            IsCounterable = true;
        }
        public void RecibirGolpe()
        {
            if (Vulnerable)
            {
                
            }
            CurrentEnemyState = EnemyState.Hurting;
if (vidaActual <= 0)
    return;

vidaActual = Math.Max(0, vidaActual - 10);
            Vulnerable = false;
            _cronometroHurting = 0f;
            _cronometroVulnerable = 0f;
            _hurtAnim.Reset();
            _currentSprite = _hurtAnim;
        }
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Vulnerable)
            {
                _cronometroVulnerable += dt;
                if (_cronometroVulnerable >= _tiempoVulnerableMax)
                {
                    Vulnerable = false;
                    _cronometroVulnerable = 0f;
                }
            }


            if (CurrentEnemyState == EnemyState.Hurting)
            {

                _currentSprite = _hurtAnim;
                _hurtAnim.Update(gameTime);

                _cronometroHurting += dt;

                if (_cronometroHurting >= _tiempoHurtingMax || _hurtAnim.IsFinished)
                {
                    _hurtAnim.Reset();
                    CurrentEnemyState = EnemyState.Idle;
                    _cronometroVulnerable = 0f;
                }
            }


            switch (CurrentEnemyState)
            {

                case EnemyState.Idle:
                    if (Vulnerable)
                    {
                    }
                    else
                    {
                        _currentSprite = _idleAnim;
                    }
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
                    }

                    break;
            }
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