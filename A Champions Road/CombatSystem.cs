using A_Champion_s_Road;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using System;
using System.Runtime.CompilerServices;


public class CombatSystem
{


    public Enemy _enemy { get; set; }
    public Player _player { get; set; }
    public bool impacto = false;


    public CombatSystem(Enemy _enemy, Player _player)
    {


        this._enemy = _enemy;
        this._player = _player;


    }


    public void Update(GameTime gameTime)
    {
        int frameActualEnemigo = _enemy.CurrentPunchFrame;
        int frameActualJugador = _player.CurrentPunchFrame;

if (_enemy.CurrentEnemyState != Enemy.EnemyState.Punching || frameActualEnemigo == 0)
        {
            impacto = false;
        }

        switch ((_player.CurrentState, _enemy.CurrentEnemyState, _enemy.IsCounterable))
        {
            case (Player.PlayerState.Idle, Enemy.EnemyState.Punching, true):
                break;


            case (Player.PlayerState.Punching, Enemy.EnemyState.Punching, true):
                if (frameActualJugador == 2)
                {
                    _enemy.CurrentEnemyState = Enemy.EnemyState.Hurting;
                    _enemy.IsCounterable = false;
                    // if(_player.staminaActual >= 10)
                    // {
                    // _player.staminaActual -= 10;
                    // }
                }
                break;
            case (Player.PlayerState.Punching, Enemy.EnemyState.Idle, false):
                if (frameActualJugador == 2)
                {
                    _enemy.RecibirGolpe();
                    // if(_player.staminaActual >= 10)
                    // {
                    // _player.staminaActual -= 10;
                    // }
                }
                break;


            case (Player.PlayerState.Dodging, Enemy.EnemyState.Punching, true):


                if (_player.IsDodgeFinished)
                {
                    _player.HacerVulnerable();
                }
                break;


            case (Player.PlayerState.Dodging, Enemy.EnemyState.Punching, false):

                if (_player.Vulnerable)
                {
                    if (frameActualEnemigo == 2)
                    {
                        if (impacto == false)
                        {
                            _player.RecibirGolpe(); 
                        impacto = true;
                        }
                    
                    }
                        
                }
                else if (frameActualEnemigo == 1 || frameActualEnemigo == 2)
                {
                    _enemy.Vulnerable = true;
                    _enemy.CurrentEnemyState = Enemy.EnemyState.Idle;
                    if(_player.staminaActual <= 95)
                    {
                        _player.staminaActual += 5;
                    }
                }
                break;


            case (Player.PlayerState.Punching, Enemy.EnemyState.Punching, false):
                // El jugador golpeó fuera de tiempo (cuando el enemigo ya no es counterable) -> El jugador recibe daño
                if (impacto == false)
                        {
                            _player.RecibirGolpe(); 
                        impacto = true;
                        }
                break;
            case (Player.PlayerState.Dodging, Enemy.EnemyState.Idle, false):
                if (!_enemy.Vulnerable)
                    _player.HacerVulnerable();

                break;
            default:
                if (_enemy.CurrentEnemyState == Enemy.EnemyState.Punching && frameActualEnemigo == 2 && _player.CurrentState == Player.PlayerState.Idle)
                {
                    if (impacto == false)
                        {
                            _player.RecibirGolpe(); 
impacto = true;
                        }
                }
                if (_player.Vulnerable &&
                _enemy.CurrentEnemyState == Enemy.EnemyState.Punching &&
                frameActualEnemigo >= 2 &&
                _player.CurrentState != Player.PlayerState.Hurting)
                {
                    if (impacto == false)
                        {
                            _player.RecibirGolpe(); 
impacto = true;
                        }
                }



                break;
        }
    }




}
