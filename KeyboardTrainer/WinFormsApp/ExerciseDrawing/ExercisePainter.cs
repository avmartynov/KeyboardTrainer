using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using Twidlle.Infrastructure;
using Twidlle.KeyboardTrainer.Core;

namespace Twidlle.KeyboardTrainer.WinFormsApp.ExerciseDrawing
{
    public class ExercisePainter
    {
        public ExercisePainter(ExerciseAppearance appearance)
        {
            _brushText      = FromKnownColorName(appearance.TextColor);
            _brushTextLocal = FromKnownColorName(appearance.LocalTextColor);

            _brushHead      = FromKnownColorName(appearance.HeadColor);
            _brushTail      = FromKnownColorName(appearance.TailColor);

            _brushCurrent   = FromKnownColorName(appearance.CurrentCharColor);
            _brushIncorrect = FromKnownColorName(appearance.IncorrectCharColor);

            _brushBackgrMargin = FromKnownColorName(appearance.BackgrColor);

            _font = new Font(appearance.FontName, appearance.FontSize, FontStyle.Regular);
        }


        public void Draw(Graphics g, Rectangle displayRectangle, Exercise exercise)
        {
            g.FillRectangle(_brushBackgrMargin, displayRectangle);
            g.DrawRectangle(new Pen(Color.DimGray), 0, 0, displayRectangle.Width - 1, displayRectangle.Height - 1);

            var center = new PointF(((float)displayRectangle.Width) / 2, ((float)displayRectangle.Height) / 2);

            var boundBox = GetTestStringSize(g, exercise.ExeciseString);

            var charLocation = new PointF(center.X - boundBox.Width / 2, center.Y - boundBox.Height / 2);

            for (var idxOfChar = 0; idxOfChar < exercise.ExeciseString.Count(); idxOfChar++)
            {
                var keyItem = exercise.ExeciseString[idxOfChar];

                var sizeChar = GetKeyItemSize(g, keyItem.DisplayText);

                PaintKeyItem(g, keyItem, new RectangleF(charLocation, sizeChar),
                    idxOfChar < exercise.CurrentPosition ? _brushHead :
                    idxOfChar > exercise.CurrentPosition ? _brushTail :
                                exercise.WrongTyping     ? _brushIncorrect :
                                                            _brushCurrent);
                charLocation.X += sizeChar.Width;
            }
        }


        private void PaintKeyItem(Graphics g, ExerciseItem keyItem, RectangleF rectangle, Brush brushBackground)
        {
            var localCharacter = (keyItem is CharacterItem) && ((CharacterItem)keyItem).IsLocalCharacter;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(brushBackground, rectangle);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            g.DrawString(keyItem.DisplayText, _font, localCharacter ? _brushTextLocal : _brushText, rectangle, _format);
        }


        private SizeF GetTestStringSize(IDeviceContext g, IEnumerable<ExerciseItem> testString)
        {
            return testString
                .Select(ki => GetKeyItemSize(g, ki.DisplayText))
                .Aggregate((current, charSize) =>
                    new SizeF
                    {
                        Width  = current.Width + charSize.Width,
                        Height = Math.Max(current.Height, charSize.Height)
                    });
        }


        private SizeF GetKeyItemSize(IDeviceContext g, string keyName)
        {
            return TextRenderer.MeasureText(g, keyName, _font, new Size(), 
                keyName.Length > 1 
                    ? TextFormatFlags.Default 
                    : TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
        }


        private static Brush FromKnownColorName(string s)
        {
            return new SolidBrush(Color.FromKnownColor(s.ParseEnumName<KnownColor>()));
        }


        private readonly Font  _font;

        private readonly Brush _brushText;
        private readonly Brush _brushTextLocal;

        private readonly Brush _brushHead;
        private readonly Brush _brushCurrent;
        private readonly Brush _brushIncorrect;
        private readonly Brush _brushTail;
        private readonly Brush _brushBackgrMargin;

        private static readonly StringFormat _format = new StringFormat
        {
            Alignment     = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
        };
    }
}
