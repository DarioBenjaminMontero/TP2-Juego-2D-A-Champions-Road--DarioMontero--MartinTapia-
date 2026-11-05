using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using Gum.Forms.Controls;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;          
using MonoGameLibrary.Scenes;   

namespace A_Champion_s_Road.Scenes;

public class GameOver : Scene
{
    private Texture2D _gameOver; 
    private Panel _panelMenu;
    private Rectangle _pantallaCompleta; 
    public override void Initialize()
    {
        base.Initialize();
        Core.ExitOnEscape = true; // apreto ESC y se cierra el juego

        _pantallaCompleta = Core.GraphicsDevice.PresentationParameters.Bounds;
        IniciarUI();
    }

    public override void LoadContent()
    {
        _gameOver = Content.Load<Texture2D>("gameover");
    }

    private void IniciarUI()
    {
        GumService.Default.Root.Children.Clear(); 
        EscenaPerder(); // UI Actual, la función de mas adelante
    }

    private void EscenaPerder()
    {
        _panelMenu = new Panel();
        _panelMenu.Dock(Gum.Wireframe.Dock.Fill);
        _panelMenu.AddToRoot();

        var botonRetry = new Button();
        botonRetry.Anchor(Gum.Wireframe.Anchor.Center); 
        botonRetry.Y = 60;
        botonRetry.X = 0;                   
        botonRetry.Width = 90;
        botonRetry.Text = "Reintentar";
        botonRetry.Click += RetryFuncion;
        _panelMenu.AddChild(botonRetry);

        var botonMenu = new Button();
        botonMenu.Anchor(Gum.Wireframe.Anchor.Center);
        botonMenu.Y = 30;
        botonMenu.X = 0;
        botonMenu.Width = 160;
        botonMenu.Text = "Menu Principal";
        botonMenu.Click += MenuFuncion;
        _panelMenu.AddChild(botonMenu);     
    }

    private void RetryFuncion(object sender, EventArgs e)
    {
        Core.ChangeScene(new FightScene());
    }

    private void MenuFuncion(object sender, EventArgs e)
    {
        Core.ChangeScene(new MenuScene());
    }

    public override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.Black);

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        float centroX = Core.GraphicsDevice.Viewport.Width * 0.5f;
        float centroY = Core.GraphicsDevice.Viewport.Height * 0.5f;

        Vector2 origen = new Vector2(_gameOver.Width * 0.5f, _gameOver.Height);

        Core.SpriteBatch.Draw(
            _gameOver,
            new Vector2(centroX, centroY - 50), // 20 pixeles por encima de los botones
            null,
            Color.White,
            0f,
            origen,
            3f,
            SpriteEffects.None,
            0f
        );

        Core.SpriteBatch.End();

        GumService.Default.Draw();
    }
}