using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Twidlle.Infrastructure.Drawing
{
    public class BufferedRender : IDisposable
    {
        /// <summary>
        /// _render = new BufferedRender(this.pannel.CreateGraphics, this.pannel.DisplayRectangle, Action_Graphics_ draw)
        /// </summary>
        /// 
        public BufferedRender(Graphics graphics, Rectangle displayRectangle, Action<Graphics> draw = null)
        {
            _draw = draw;

            var context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(displayRectangle.Width + 1, displayRectangle.Height + 1);

            _bufferedGraphics = context.Allocate(graphics, displayRectangle);

            _bufferedGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }


        public Graphics Graphics { get { return _bufferedGraphics.Graphics; } }
        public Action<Graphics> DrawProcedure { get { return _draw;  } }

        public void Draw(Action<Graphics> draw = null)
        {
            _bufferedGraphics.Graphics.Clear(Color.White);

            if (draw == null)
                _draw(_bufferedGraphics.Graphics);
            else
                draw(_bufferedGraphics.Graphics);

            _bufferedGraphics.Render();
        }


        public void Dispose()
        {
            if (_disposed)
                return;

            _bufferedGraphics.Dispose();
            _disposed = true;
        }

        private bool _disposed;

        private readonly BufferedGraphics _bufferedGraphics;

        private readonly Action<Graphics> _draw;
    }
}
