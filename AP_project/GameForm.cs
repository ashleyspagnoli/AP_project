using AP_project.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AP_project
{
    public partial class GameForm : Form
    {
        private GameEngine engine;
        private Scene gameScene;
        private BufferedGraphicsContext context;
        private BufferedGraphics buffer;

        public GameForm()
        {
            InitializeComponent();

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
            engine = new GameEngine();
            gameScene = new Scene(this);
            engine.CurrentScene = gameScene;

            // Hook up input
            this.KeyDown += (s, e) => InputManager.Instance.SetKeyDown(e.KeyCode);
            this.KeyUp += (s, e) => InputManager.Instance.SetKeyUp(e.KeyCode);

            engine.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw to buffer
            Graphics g = buffer.Graphics;
            g.Clear(Color.Black);

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
    }
}
