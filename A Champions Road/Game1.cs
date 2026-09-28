using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace A_Champion_s_Road
{
    // La clase principal del juego que hereda de Game (el núcleo de MonoGame)
    public class Game1 : Game
    {
        // Administrador de los gráficos y la resolución de la ventana
        private GraphicsDeviceManager _graphics;
        
        // Utilidad fundamental para dibujar imágenes (sprites) 2D en la pantalla
        private SpriteBatch _spriteBatch;

        // Declaración del objeto jugador y de su textura (imagen)
        private Player _player;
        private Texture2D _playerTexture;
        private Texture2D _playerAltTexture;
        private Texture2D _playerAltTexture2;
        private Texture2D _playerNormalTexture;

        public Game1()
        {
            // Inicializa el administrador de gráficos para la ventana
            _graphics = new GraphicsDeviceManager(this);
            
            // Define la carpeta raíz dentro de Content donde se buscarán los recursos multimedia
            Content.RootDirectory = "Content";
            
            // Permite que el cursor del mouse sea visible dentro de la ventana del juego
            IsMouseVisible = true;
        }

        // Método que se ejecuta una sola vez al iniciar la aplicación antes de cargar contenido
        protected override void Initialize()
        {
            // Lógica de inicialización propia de MonoGame
            base.Initialize();
        }

        // Método encargado de cargar todos los recursos del juego (texturas, sonidos, fuentes)
        protected override void LoadContent()
        {
            // Inicializa el SpriteBatch pasándole el dispositivo gráfico actual
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Carga la textura del protagonista desde la pipeline de contenido de MonoGame
            _playerTexture = Content.Load<Texture2D>("backSprite-danteVega2");
            _playerAltTexture = Content.Load<Texture2D>("backSprite-punch1-danteVega");
            _playerAltTexture2 = Content.Load<Texture2D>("backSprite-punch2-danteVega");
            _playerNormalTexture = Content.Load<Texture2D>("backSprite-danteVega2");
            // Instancia el objeto Player pasándole su textura y su posición inicial (X: 400, Y: 300)
            _player = new Player(_playerTexture, _playerAltTexture, _playerAltTexture2, _playerNormalTexture, new Vector2(200, 180));
        }

        // Método que se ejecuta continuamente en cada fotograma (frame) para actualizar la lógica
        protected override void Update(GameTime gameTime)
        {
            // Condición para cerrar el juego si se presiona el botón Back del mando o la tecla Escape del teclado
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Llama al método Update del jugador para procesar sus movimientos y acciones
            _player.Update(gameTime);

            base.Update(gameTime);
        }

        // Método que se ejecuta en cada fotograma para renderizar y pintar los gráficos en la pantalla
        protected override void Draw(GameTime gameTime)
        {
            // Limpia la pantalla y la pinta con un color de fondo (Azul Cornflower clásico de MonoGame)
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Comienza el lote de renderizado de sprites
            _spriteBatch.Begin();
            
            // Dibuja al jugador utilizando el SpriteBatch
            _player.Draw(_spriteBatch);
            
            // Finaliza el lote de renderizado (necesario para que se muestren los gráficos en pantalla)
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}