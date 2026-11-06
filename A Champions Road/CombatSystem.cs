
using A_Champion_s_Road;
using Microsoft.Xna.Framework;

public class CombatSystem
{
    public Enemy _enemy { get; set; }
    public Player _player { get; set; }

    public bool impacto = false;


    private bool _impactoJugador = false;
    private bool _impactoEnemigo = false;

    public CombatSystem(Enemy enemy, Player player)
    {
        _enemy = enemy;
        _player = player;
    }

    public void Update(GameTime gameTime)
    {
        int frameActualEnemigo = _enemy.CurrentAttackFrame;
        int frameActualJugador = _player.CurrentPunchFrame;

     
        bool enemigoAtacando =
            _enemy.CurrentEnemyState == Enemy.EnemyState.Punching ||
            _enemy.CurrentEnemyState == Enemy.EnemyState.PunchingRight;

        bool enemigoCounterable =
            enemigoAtacando && _enemy.IsCounterable;


        if (!enemigoAtacando || frameActualEnemigo != 2)
        {
            _impactoJugador = false;
            impacto = false;
        }

        if (_player.CurrentState != Player.PlayerState.Punching ||
            frameActualJugador != 2)
        {
            _impactoEnemigo = false;
        }



        if (_player.CurrentState == Player.PlayerState.Punching)
        {
            if (frameActualJugador == 2 && !_impactoEnemigo)
            {
                if (enemigoAtacando && enemigoCounterable)
                {
               
                    _enemy.CurrentEnemyState = Enemy.EnemyState.Hurting;
                    _enemy.RecibirGolpe();
                    _enemy.RecibirGolpe();
                    _enemy.IsCounterable = false;

                    _impactoEnemigo = true;
                }
                else if (_enemy.CurrentEnemyState == Enemy.EnemyState.Idle && _enemy.Vulnerable == false)
                {
                
                    _enemy.Bloquear();

                    _impactoEnemigo = true;
                }
                else if (_enemy.Vulnerable)
                {
                    
                    _enemy.RecibirGolpe();
                    _impactoEnemigo = true;
                }
                else if (enemigoAtacando && !enemigoCounterable)
                {
   
                    if (!_impactoJugador)
                    {
                        _player.RecibirGolpe();
                        _impactoJugador = true;
                        impacto = true;
                    }
                }
            }

            return;
        }



        if (_player.CurrentState == Player.PlayerState.Dodging)
        {
            if (enemigoAtacando)
            {
                if (enemigoCounterable)
                {
                    if (_player.IsDodgeFinished)
                    {
                        _player.HacerVulnerable();
                    }
                }
                else
                {
                    if (_player.Vulnerable)
                    {
                        if (frameActualEnemigo == 2 &&
                            !_impactoJugador)
                        {
                            _player.RecibirGolpe();
                            _impactoJugador = true;
                            impacto = true;
                        }
                    }
                    else if (frameActualEnemigo == 1 ||
                             frameActualEnemigo == 2)
                    {

                        _enemy.HacerseVulnerable();

                        if (_player.staminaActual <= 95)
                        {
                            _player.staminaActual += 5;
                        }
                    }
                }
            }
            else if (_enemy.CurrentEnemyState == Enemy.EnemyState.Idle)
            {
                if (!_enemy.Vulnerable)
                {
                    _player.HacerVulnerable();
                }
            }

            return;
        }



        if (_player.CurrentState == Player.PlayerState.Idle)
        {
            if (enemigoAtacando &&
                !enemigoCounterable &&
                frameActualEnemigo == 2 &&
                !_impactoJugador)
            {
                _player.RecibirGolpe();

                _impactoJugador = true;
                impacto = true;
            }
        }


        if (_player.Vulnerable &&
            enemigoAtacando &&
            frameActualEnemigo >= 2 &&
            _player.CurrentState != Player.PlayerState.Hurting &&
            !_impactoJugador)
        {
            _player.RecibirGolpe();

            _impactoJugador = true;
            impacto = true;
        }
    }
}
