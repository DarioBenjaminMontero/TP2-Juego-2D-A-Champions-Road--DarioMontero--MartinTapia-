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

    public void Update(GameTime gameTime, Enemy enemy)
    {
        
        if (enemy.CurrentEnemyState != Enemy.EnemyState.Idle)
            return;

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _cronometroIA += dt;

        if (_cronometroIA >= _tiempoParaDecidir)
        {
            _cronometroIA = 0f;
            int numeroAleatorio = _aleatorio.Next(1, 3);
            
            if (numeroAleatorio == 2)
            {
                enemy.StartPunch(); // Método dentro de Enemy para iniciar el ataque
            }
        }
    }
}