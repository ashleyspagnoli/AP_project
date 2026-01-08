namespace AP_project
{
    public partial class GameForm : Form
    {
        private Engine.GameEngine engine;
        private Game.GameScene gameScene;
        private BufferedGraphicsContext context;
        private BufferedGraphics buffer;

        public GameForm()
        {
            InitializeComponent();

            // Setup form
            this.Text = "Asteroids";
            this.Width = 800;
            this.Height = 600;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Setup double buffering
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.Opaque, true);

            // Setup buffered graphics
            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
            buffer = context.Allocate(this.CreateGraphics(), this.ClientRectangle);

            // Initialize engine
            engine = new Engine.GameEngine();
            gameScene = new Game.GameScene(this, this.ClientRectangle);
            engine.CurrentScene = gameScene;

            // Hook up input
            this.KeyDown += (s, e) => Engine.InputManager.Instance.SetKeyDown(e.KeyCode);
            this.KeyUp += (s, e) => Engine.InputManager.Instance.SetKeyUp(e.KeyCode);

            // Prevent arrow keys from triggering form navigation
            this.KeyPreview = true;

            engine.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw to buffer
            Graphics g = buffer.Graphics;
            g.Clear(Color.Black);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            gameScene.Draw(g);

            // Render buffer to screen
            buffer.Render(e.Graphics);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            engine.Stop();
            buffer?.Dispose();
            context?.Dispose();
            base.OnFormClosing(e);
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }
    }
}
