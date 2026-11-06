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
        private AnimatedSprite _blockAnim;
        private AnimatedSprite _punchRightAnim;
        private AnimatedSprite _currentSprite;

public const int vidaMaxima = 1000;
        public int vidaActual {get; set;} = vidaMaxima;
        private float _tiempoVulnerableMax = 2.0f;
        private float _cronometroEstado = 0f;
        public int roundsGanados { get; set; } = 0;
        private float _tiempoHurtingMax = 0.2f;

        private float _cronometroHurting = 0f;
        private float _cronometroVulnerable = 0f;
        public bool Vulnerable = false;

        public enum EnemyState { Idle, Punching, PunchingRight, Hurting, Blocking }
        public EnemyState CurrentEnemyState { get; set; } = EnemyState.Idle;
        public bool IsCounterable { get; set; } = false;



        public int CurrentPunchFrame => _punchAnim.CurrentFrame;
        public int CurrentIdleFrame => _idleAnim.CurrentFrame;

public int CurrentAttackFrame =>
    CurrentEnemyState switch
    {
        EnemyState.Punching => _punchAnim.CurrentFrame,
        EnemyState.PunchingRight => _punchRightAnim.CurrentFrame,
        _ => -1
    };

        // CAMBIO: nuevo parámetro "escala" (factor según la pantalla)
        public Enemy(Animation idleAnim, Animation punchAnim, Animation hurtAnim,Animation blockAnim,Animation rightPunchAnim, Vector2 initialPosition, float escala = 1f)
        {
            Position = initialPosition;

            // Creamos la animación en bucle (true)
            _idleAnim = new AnimatedSprite(idleAnim, true); 
            _punchAnim = new AnimatedSprite(punchAnim, false); // No se repite
            _hurtAnim = new AnimatedSprite(hurtAnim, false); 
            _blockAnim = new AnimatedSprite(blockAnim, false); 
            _punchRightAnim = new AnimatedSprite(rightPunchAnim, false); 
            _currentSprite = _idleAnim;

            // CAMBIO: la escala base (3.5f) se multiplica por el factor de pantalla
            Vector2 escalaFinal = new Vector2(1.2f * escala, 1.2f * escala);
            _idleAnim.Scale = escalaFinal; 
            _punchAnim.Scale = escalaFinal; 
            _hurtAnim.Scale = escalaFinal; // antes no tenía escala asignada
            _blockAnim.Scale = escalaFinal; 
            _punchRightAnim.Scale = escalaFinal; 
        }
        public void ReiniciarRound()
        {
            vidaActual = 1000;
            CurrentEnemyState = EnemyState.Idle;
            Vulnerable = false;
            IsCounterable = false;
            _cronometroHurting = 0f;
            _cronometroVulnerable = 0f;
            _currentSprite = _idleAnim;
        }
        public void StartPunch()
        {
            CurrentEnemyState = EnemyState.Punching;
            _punchAnim.Reset();
            _cronometroEstado = 0f;
            IsCounterable = true;
        }
        public void StartPunchRight()
        {  
            CurrentEnemyState = EnemyState.PunchingRight;
            _punchRightAnim.Reset();
            _cronometroEstado = 0f;
            IsCounterable = true;
        }
        public void Bloquear()
        {
            CurrentEnemyState = EnemyState.Blocking;
_blockAnim.Reset();
        }
        public void HacerseVulnerable()
{
    Vulnerable = true;
    _cronometroVulnerable = 0f;

    CurrentEnemyState = EnemyState.Idle;
    IsCounterable = false;
}
        public void RecibirGolpe()
{
    if (!Vulnerable || vidaActual <= 0)
        return;

    CurrentEnemyState = EnemyState.Hurting;

    vidaActual = Math.Max(0, vidaActual - 10);

    _cronometroHurting = 0f;
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

    if (_hurtAnim.IsFinished)
    {
        CurrentEnemyState = EnemyState.Idle;
        _currentSprite = _idleAnim;
    }
            }


            switch (CurrentEnemyState)
            {

                case EnemyState.Idle:
                     if (!Vulnerable)
    {
        _currentSprite = _idleAnim;
    }
                    break;
                case EnemyState.Punching:
    _currentSprite = _punchAnim;

    IsCounterable = _punchAnim.CurrentFrame == 0;

    if (_punchAnim.IsFinished)
    {
        CurrentEnemyState = EnemyState.Idle;
        IsCounterable = false;
    }
    break;

case EnemyState.PunchingRight:
    _currentSprite = _punchRightAnim;

    IsCounterable = _punchRightAnim.CurrentFrame == 0;

    if (_punchRightAnim.IsFinished)
    {
        CurrentEnemyState = EnemyState.Idle;
        IsCounterable = false;
    }
    break;
    case EnemyState.Blocking:
_currentSprite = _blockAnim;
if (_blockAnim.IsFinished)
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