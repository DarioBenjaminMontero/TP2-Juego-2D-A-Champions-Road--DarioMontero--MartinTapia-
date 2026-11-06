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

public class Options : Scene
{
    private Texture2D _opciones; 
    private Panel _panelMenu;
    private SpriteFont _titulo;
    private SpriteFont _texto;
    private Rectangle _pantallaCompleta; 
    private Vector2 _posTitulo;
    private Vector2 _origenTitulo;
    private Vector2 _posVolumen;
    private Vector2 _origenVolumen;
    private const string ETIQUETA = "Volumen";
    private int _nivelVolumen;
    public override void Initialize()
    {
        base.Initialize();

        _pantallaCompleta = Core.GraphicsDevice.PresentationParameters.Bounds;

        _posTitulo = new Vector2(_pantallaCompleta.Width * 0.5f, _pantallaCompleta.Height * 0.5f - _pantallaCompleta.Height * 0.2f);
        _origenTitulo = new Vector2(_opciones.Width * 0.5f, _opciones.Height);

        _posVolumen = new Vector2(_pantallaCompleta.Width * 0.15f, _pantallaCompleta.Height * 0.7f);
        _origenVolumen = _texto.MeasureString(ETIQUETA) * 0.5f;
        _nivelVolumen = (int)Math.Round(MediaPlayer.Volume * 10);

        IniciarUI();
    }

    public override void LoadContent()
    {
        _opciones = Content.Load<Texture2D>("opciones");
        _texto = Content.Load<SpriteFont>("texto");
    }

    private void IniciarUI()
    {
        GumService.Default.Root.Children.Clear(); 
        Configuraciones(); // UI Actual, la función de mas adelante
    }

    private void Configuraciones()
    {
        _panelMenu = new Panel();
        _panelMenu.Dock(Gum.Wireframe.Dock.Fill);
        _panelMenu.AddToRoot();

        var botonSubir = new Button();
        botonSubir.Anchor(Gum.Wireframe.Anchor.Left); 
        botonSubir.Y = 60;
        botonSubir.X = 30;                   
        botonSubir.Width = 30;
        botonSubir.Height = 10;
        botonSubir.Text = "+";
        botonSubir.Click += SubirVolumen;
        _panelMenu.AddChild(botonSubir);

        var botonBajar = new Button();
        botonBajar.Anchor(Gum.Wireframe.Anchor.Left); 
        botonBajar.Y = 60;
        botonBajar.X = 90;                   
        botonBajar.Width = 30;
        botonBajar.Height = 10;
        botonBajar.Text = "-";
        botonBajar.Click += BajarVolumen;
        _panelMenu.AddChild(botonBajar);

        var botonVolver = new Button();
        botonVolver.Anchor(Gum.Wireframe.Anchor.Right);
        botonVolver.Y = 60;
        botonVolver.X = -30;
        botonVolver.Width = 60;
        botonVolver.Height = 17;
        botonVolver.Text = "Volver";
        botonVolver.IsFocused = true;
        botonVolver.Click += VolverFuncion;
        _panelMenu.AddChild(botonVolver);     
    }

    private void BajarVolumen(object sender, EventArgs e)
    {
        _nivelVolumen = Math.Max(0, _nivelVolumen - 1);
        MediaPlayer.Volume = _nivelVolumen / 10f;
    }

    private void SubirVolumen(object sender, EventArgs e)
    {
        _nivelVolumen = Math.Min(10, _nivelVolumen + 1);
        MediaPlayer.Volume = _nivelVolumen / 10f;
    }
    private void VolverFuncion(object sender, EventArgs e)
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

        Core.SpriteBatch.Draw(_opciones, _posTitulo, null, Color.White, 0f, _origenTitulo, 3f, SpriteEffects.None, 0f);

        Core.SpriteBatch.DrawString
        (_texto, ETIQUETA, _posVolumen, Color.White, 0f, _origenVolumen, 1f, SpriteEffects.None, 0f);

        string porcentaje = (_nivelVolumen * 10) + "%";
        Vector2 tamPorcentaje = _texto.MeasureString(porcentaje);

        Vector2 posPorcentaje = new Vector2(
        _posVolumen.X, _posVolumen.Y + _origenVolumen.Y * 2f + 10f);

        Core.SpriteBatch.DrawString
        (_texto, porcentaje, posPorcentaje, Color.White, 0f, tamPorcentaje * 0.5f, 1f, SpriteEffects.None, 0f);

        Core.SpriteBatch.End();

        GumService.Default.Draw();
    }
}