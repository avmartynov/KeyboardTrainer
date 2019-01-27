using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Twidlle.Infrastructure.Drawing
{
    public class GraphicsContext : IDisposable
    {
        public GraphicsContext(Graphics graphics)
        {
            _graphics = graphics;
            _graphicsState = graphics.Save();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _graphics.Restore(_graphicsState);
            _disposed = true;
        }

        private bool _disposed;

        private readonly GraphicsState _graphicsState;
        private readonly Graphics _graphics;
    }
}
