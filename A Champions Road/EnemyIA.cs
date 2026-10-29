using A_Champion_s_Road;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using System;


public class EnemyAI
{
    private float _tiempoParaDecidir = 1.0f;
    private float _cronometroIA = 0.0f;
    private Random _aleatorio = new Random();

    public void Update(GameTime gameTime, Enemy enemy, Player player)
    {
        if (enemy.CurrentEnemyState != Enemy.EnemyState.Idle)
            return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cronometroIA += dt;

        float tiempoActualDecision = (player.Vulnerable) ? 0.2f : _tiempoParaDecidir;

        if (_cronometroIA >= tiempoActualDecision)
        {
            _cronometroIA = 0f;
            
            if (player.Vulnerable || _aleatorio.Next(1, 3) == 2)
            {
                enemy.StartPunch(); 
            }
        }
    }
}