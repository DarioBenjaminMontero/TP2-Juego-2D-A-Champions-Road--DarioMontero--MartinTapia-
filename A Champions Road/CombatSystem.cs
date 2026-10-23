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

public Enemy _enemy {get; set;}
public Player _player {get; set;}

public CombatSystem(Enemy _enemy, Player _player)
       {
              
this._enemy = _enemy;
this._player = _player; 

       }

       public void Update(GameTime gameTime)
       {
              int frameActualEnemigo = _enemy.CurrentPunchFrame;

            switch ((_player.CurrentState, _enemy.CurrentEnemyState, _enemy.IsCounterable))
            {
                case (Player.PlayerState.Idle, Enemy.EnemyState.Punching, true):
                    // Se deja vacío a propósito para que el juego continúe normalmente
                    break;
                case (Player.PlayerState.Punching, Enemy.EnemyState.Punching, true):
                    _enemy.CurrentEnemyState = Enemy.EnemyState.Hurting;
                    _enemy.IsCounterable = false;
                    break;

                case (Player.PlayerState.Dodging, Enemy.EnemyState.Punching, true):
                    _player.CurrentState = Player.PlayerState.Hurting;
                    break;

                case (Player.PlayerState.Dodging, Enemy.EnemyState.Punching, false):
                    if (frameActualEnemigo == 2 || frameActualEnemigo == 1)
                    {
                        _enemy.CurrentEnemyState = Enemy.EnemyState.Vulnerable;
                    }
                    break;

                case (Player.PlayerState.Punching, Enemy.EnemyState.Punching, false):
                    _player.CurrentState = Player.PlayerState.Hurting;
                    break;

                default:
                    if (_enemy.CurrentEnemyState == Enemy.EnemyState.Punching && frameActualEnemigo == 2 && _player.CurrentState == Player.PlayerState.Idle)
                    {
                        _player.CurrentState = Player.PlayerState.Hurting;
                    }

                    if (_player.CurrentState == Player.PlayerState.Vulnerable)
                    {
                        _enemy.CurrentEnemyState = Enemy.EnemyState.Punching;
                    }
                    break;
            }

       }


}