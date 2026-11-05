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

public class MenuScene : Scene
{
    // Acá van los campos de la escena: texturas, fuentes, posiciones, puntajes...
    // Ejemplo:

    private Texture2D _fondo;
    private SpriteFont _titulo;
    private SpriteFont _texto;
    private Song _musica;
    private Panel _panelMenu; // contenedor de los tres botones principales (o esenciales) que tendrá el menu
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
        _texto = Content.Load<SpriteFont>("texto");
        _titulo = Content.Load<SpriteFont>("textoTitulo");
        _fondo = Content.Load<Texture2D>("background");
        _musica = Content.Load<Song>("Ten_Counts_to_Glory");
    }

    private void IniciarUI()
    {
        GumService.Default.Root.Children.Clear();
        EscenaMenu(); 
    }

    private void EscenaMenu()
    {
        _panelMenu = new Panel();
        _panelMenu.Dock(Gum.Wireframe.Dock.Fill);
        _panelMenu.AddToRoot();

        // Boton Jugar
        var botonJugar = new Button();
        botonJugar.Anchor(Gum.Wireframe.Anchor.Center); 
        botonJugar.Y = 30;
        botonJugar.X = -90;                   
        botonJugar.Width = 70;
        botonJugar.Text = "Jugar";
        botonJugar.Click += JugarFuncion;
        _panelMenu.AddChild(botonJugar);

        // Botón de Opciones
        var botonOpciones = new Button();
        botonOpciones.Anchor(Gum.Wireframe.Anchor.Center);
        botonOpciones.Y = 30;
        botonOpciones.X = 0;
        botonOpciones.Width = 80;
        botonOpciones.Text = "Opciones";
        botonOpciones.Click += OpcionesFuncion;
        _panelMenu.AddChild(botonOpciones);     

        // --- Botón SALIR ---
        var botonSalir = new Button();
        botonSalir.Anchor(Gum.Wireframe.Anchor.Center);
        botonSalir.Y = 30;
        botonSalir.X = 90;
        botonSalir.Width = 70;
        botonSalir.Text = "Salir";
        botonSalir.Click += SalirFuncion;
        _panelMenu.AddChild(botonSalir);           
    }

    // funciones de cada boton a llamar

    private void JugarFuncion(object sender, EventArgs e)
    {
        Core.ChangeScene(new FightScene()); 
    }

    private void OpcionesFuncion(object sender, EventArgs e)
    {
        Core.ChangeScene(new Options());
    }

    private void SalirFuncion(object sender, EventArgs e)
    {
        Core.Instance.Exit(); 
    }

    public override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.Black);

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        Core.SpriteBatch.Draw(_fondo, _pantallaCompleta, Color.White);
        Core.SpriteBatch.End();

        GumService.Default.Draw();
    }
}