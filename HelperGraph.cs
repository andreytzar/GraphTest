using System;
using System.Drawing;
using System.Linq;

namespace GraphTest
{
    public class GraphData
    {
        public string XText { get; set; } = string.Empty;
        public int Value { get; set; } = 0;
        public Color Color { get; set; } = Color.Black;

        public GraphData(string xLabel, int value, Color color)
        {
            XText = xLabel;
            Value = value;
            Color = color;
        }
    }

    public static class HelperGraph
    {
        public static Bitmap GraphColumns(int width, int height, GraphData[] arrvalues, int top = 0, int footerheigt = 0, int maxXordHeight = 80)
        {
            int ordFontSize = 9;
            using Font ordFont = new Font("Calibri", ordFontSize);
            Bitmap res = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(res);
            var yOrdMarg = MeasureYOrdMargin(g, arrvalues, ordFont);
            var xOrdMarg = MeasureXOrdMargin(g, arrvalues, maxXordHeight, ordFont);
            int GraphHeigt = height - top - footerheigt - xOrdMarg;
            int GraphWidth = width - yOrdMarg;
            using var yordbmp = DrawYOrdGraphColumn(arrvalues, GraphHeigt, yOrdMarg, ordFont);
            g.DrawImage(yordbmp, new Point(0, top));
            using var хordbmp = DrawХOrdGraphColumn(g, arrvalues, GraphWidth, xOrdMarg, ordFont);
            g.DrawImage(хordbmp, new Point(yOrdMarg, GraphHeigt + top));
            using var graphbmp = DrawGraphColumn(GraphWidth, GraphHeigt, arrvalues);
            g.DrawImage(graphbmp, new Point(yOrdMarg, top));
            return res;
        }

        public static Bitmap LineGraphic(int width, int height, GraphData[] arrvalues, int top = 0, int footerheigt = 0, int maxXordHeight = 80)
        {
            int ordFontSize = 9;
            using Font ordFont = new Font("Calibri", ordFontSize);
            Bitmap res = new Bitmap(width, height);
            using Graphics g = Graphics.FromImage(res);
            var yOrdMarg = MeasureYOrdMargin(g, arrvalues, ordFont);
            var xOrdMarg = MeasureXOrdMargin(g, arrvalues, maxXordHeight, ordFont);
            int GraphHeigt = height - top - footerheigt - xOrdMarg;
            int GraphWidth = width - yOrdMarg;
            using var yordbmp = DrawYOrdGraphColumn(arrvalues, GraphHeigt, yOrdMarg, ordFont);
            g.DrawImage(yordbmp, new Point(0, top));
            using var хordbmp = DrawХOrdGraphColumn(g, arrvalues, GraphWidth, xOrdMarg, ordFont);
            g.DrawImage(хordbmp, new Point(yOrdMarg, GraphHeigt + top));
            using var graphbmp = DrawGraphLine(GraphWidth, GraphHeigt, arrvalues);
            g.DrawImage(graphbmp, new Point(yOrdMarg, top));
            return res;
        }

        public static Bitmap DrawGraphLine(int width, int height, GraphData[] arrvalues)
        {
            Bitmap bmp = new Bitmap(width, height);
            using Font font = new Font("Calibri", 9);
            using Graphics g = Graphics.FromImage(bmp);
            using Pen penBlack = new Pen(Color.Red, 1);
            double stepx = (double)width / (double)arrvalues.Length;
            double maxValue = arrvalues.Max(x => x.Value);
            double currentX = 0;
            Point? beforepoint = null;
            foreach (var data in arrvalues)
            {
                currentX += stepx;
                Point curpoint = new Point((int)currentX, (int)(height - data.Value / maxValue * height));
                if (beforepoint != null) g.DrawLine(penBlack, beforepoint.Value, curpoint);
                g.DrawString(data.Value.ToString(), font, Brushes.Black, new PointF(curpoint.X - 3, curpoint.Y - 16));
                beforepoint = curpoint;
            }
            return bmp;
        }

        public static Bitmap DrawGraphColumn(int width, int height, GraphData[] arrvalues)
        {
            Bitmap bmp = new Bitmap(width, height);
            using Font font = new Font("Calibri", 9);
            using Graphics g = Graphics.FromImage(bmp);
            using Pen penBlack = new Pen(Color.Black, 1);
            double stepx = (double)width / (double)arrvalues.Length;
            double maxValue = arrvalues.Max(x => x.Value);
            double currentX = 0;
            foreach (var data in arrvalues)
            {
                Rectangle rect = new Rectangle((int)currentX, (int)(height - data.Value / maxValue * height), (int)stepx,
                   (int)(data.Value / maxValue * height));
                currentX += stepx;
                g.FillRectangle(new SolidBrush(data.Color), rect);
                g.DrawRectangle(penBlack, rect);
                g.DrawString(data.Value.ToString(), font, Brushes.Black, new PointF(rect.X, rect.Y));
            }
            return bmp;
        }

        private static int MeasureYOrdMargin(Graphics g, GraphData[] arrvalues, Font font)
        {
            int OrdLine = 5;
            int maxvalue = arrvalues.Max(x => x.Value);
            Size MaxTextSize = g.MeasureString(maxvalue.ToString(), font).ToSize();
            int leftmarg = MaxTextSize.Width;
            return leftmarg + OrdLine;
        }

        private static int MeasureXOrdMargin(Graphics g, GraphData[] arrvalues, int maxXOrdHeight, Font font)
        {
            int OrdLine = 5;
            int maxTextBlockHeight = 0;
            maxTextBlockHeight = arrvalues.Max(x => g.MeasureString(
                x.XText, font).ToSize().Width);
            if (maxTextBlockHeight >= maxXOrdHeight) maxTextBlockHeight = maxXOrdHeight;
            return maxTextBlockHeight + OrdLine;
        }

        public static Bitmap DrawYOrdGraphColumn(GraphData[] arrvalues, int height, int width, Font font)
        {
            Bitmap bmp = new Bitmap(width, height);
            using Graphics gg = Graphics.FromImage(bmp);
            int OrdLine = 5;
            using Pen pen = new Pen(Color.Black, 1);
            width = width - 1;
            gg.DrawLine(pen, new Point(width, 0), new Point(width, height));
            int maxvalue = (int)Math.Ceiling((double)arrvalues.Max(x => x.Value));
            Size MaxTextSize = gg.MeasureString(maxvalue.ToString(), font).ToSize();
            int beforey = -50;
            foreach (var data in arrvalues.OrderByDescending(x => x.Value))
            {
                if (data.Value <= 0) continue;
                Size yOrdTextSize = gg.MeasureString($"{data.Value}", font).ToSize();
                int y = height - (int)((double)data.Value / (double)maxvalue * height);
                if (y - beforey > yOrdTextSize.Height * 0.9)
                {
                    beforey = y;
                    gg.DrawLine(pen, new Point(width - 4, y), new Point(width, y));
                    gg.DrawString(data.Value.ToString(), font, Brushes.Black, new PointF(width - yOrdTextSize.Width, y));
                }
                else continue;
            }
            return bmp;
        }

        public static Bitmap DrawХOrdGraphColumn(Graphics g, GraphData[] arrvalues, int width, int maxXOrdHeight, Font font)
        {
            int OrdLineWidth = 5;
            using Pen pen = new Pen(Color.Black, 1);
            Bitmap bmp = new Bitmap(width, maxXOrdHeight);
            using Graphics gg = Graphics.FromImage(bmp);
            gg.DrawLine(pen, new Point(0, 0), new Point(width, 0));
            double beforeX = -50;
            double step = (double)width / (double)arrvalues.Length;
            double currentX = 0;
            foreach (var data in arrvalues)
            {
                Size OrdTextSize = gg.MeasureString($"{data.XText}", font).ToSize();
                currentX += step;
                gg.DrawLine(pen, new Point((int)currentX, 0), new Point((int)currentX, 5));
                if (currentX - beforeX > OrdTextSize.Height * 0.9)
                {
                    beforeX = currentX;
                    using Bitmap textbmp = new(OrdTextSize.Width, OrdTextSize.Height);
                    using var gtext = Graphics.FromImage(textbmp);
                    gtext.DrawString(data.XText, font, Brushes.Black, new PointF(0, 0));
                    textbmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    gg.DrawImage(textbmp, new Point((int)(currentX - textbmp.Width), 2));
                }
            }
            return bmp;
        }
    }
}