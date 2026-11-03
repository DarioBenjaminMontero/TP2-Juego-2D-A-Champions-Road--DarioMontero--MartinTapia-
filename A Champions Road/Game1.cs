using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using Gum.Forms;
using Gum.Forms.Controls;
using MonoGameGum;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using System;
using A_Champion_s_Road.Scenes;


namespace A_Champion_s_Road
{
    public class Game1 : Core
    {
        private Song backgroundMusic;


        public Game1() : base("A Champions Road", 1280, 720, false)
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.HardwareModeSwitch = false;
            Graphics.IsFullScreen = true;
            Graphics.ApplyChanges();
        }
        private void InitializeGum()
        {

            GumService.Default.Initialize(this, DefaultVisualsVersion.V3);
            GumService.Default.ContentLoader.XnaContentManager = Core.Content;
            FrameworkElement.KeyboardsForUiControl.Add(GumService.Default.Keyboard);
            FrameworkElement.GamePadsForUiControl.AddRange(GumService.Default.Gamepads); // TODO ESTO ES LO ESENCIAL PARA QUE GUM ARRANQUE
            FrameworkElement.TabReverseKeyCombos.Add(
            new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Up });
            FrameworkElement.TabKeyCombos.Add(
            new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Down });
            GumService.Default.CanvasWidth = GraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;
            GumService.Default.CanvasHeight = GraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;
            GumService.Default.Renderer.Camera.Zoom = 4.0f;
        }

        protected override void Initialize()
        {
            base.Initialize();

            InitializeGum();
            ChangeScene(new MenuScene()); // nada mas arranca, pal menu
        }


        protected override void LoadContent()
        {
            backgroundMusic = Content.Load<Song>("Ten_Counts_to_Glory");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
        }


    }
}