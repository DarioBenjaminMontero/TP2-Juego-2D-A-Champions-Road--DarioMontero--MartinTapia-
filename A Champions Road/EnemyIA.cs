using A_Champion_s_Road;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using System;


public class EnemyAI
{
    private float _tiempoParaDecidir = 0.8f;
    private float _cronometroIA = 0.0f;
    private Random _aleatorio = new Random();

    public void Update(GameTime gameTime, Enemy enemy, Player player)
{
    if (enemy.CurrentEnemyState != Enemy.EnemyState.Idle || enemy.Vulnerable)
        return;

    float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
    _cronometroIA += dt;

    float tiempoActualDecision = player.Vulnerable ? 0.2f : _tiempoParaDecidir;

    if (_cronometroIA >= tiempoActualDecision && enemy.CurrentIdleFrame == 0)
    {
        _cronometroIA = 0f;

        if (player.Vulnerable || _aleatorio.Next(1, 3) == 2)
        {
            // Elegir aleatoriamente entre ambos golpes
            int golpe = _aleatorio.Next(0, 2);

            if (golpe == 0)
            {
                enemy.StartPunch();
            }
            else
            {
                enemy.StartPunchRight();
            }
        }
    }
}
}