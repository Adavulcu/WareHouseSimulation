using NewEra.DepoSimulasyon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NewEra.DepoSimulasyon.ShapeControls
{
   public class KatDetayShape
    {
        private Canvas MyCanvas { get; set; }

        private readonly SolidColorBrush _tunnelBrush;
        private readonly SolidColorBrush _rafBrush;
        private readonly int _x;
        private readonly int _y;

        public KatDetayShape(int width, int height, int katX, int katY)
        {
            Width = width;
            Height = height;
            KatX = katX;
            KatY = katY;

            _x = Width / KatX;
            _y = Height / KatY;
            _tunnelBrush = new SolidColorBrush
            {
                Color = Colors.Purple
            };
            _rafBrush = new SolidColorBrush
            {
                Color = Colors.Blue
            };
        }
        public int KatX { get; set; }
        public int KatY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Canvas DrawCanvas(List<KatDetayModel> KatModel)
        {
            MyCanvas = new Canvas()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,


            };
            MyCanvas.BeginInit();
            Polygon poly = new Polygon();
            for (int i = 0; i < KatModel.Count; i++)
            {
                poly = DrawPolygon(AlterPoints(KatModel[i].Points), KatModel[i]);
                MyCanvas.Children.Add(poly);
                if (KatModel[i].Koy == "01" && KatModel[i].Sira == 1)
                {
                    TextBlock text = new TextBlock();
                    text.Text = KatModel[i].Name;
                    Canvas.SetLeft(text, KatModel[i].Points.P1.X * _x + 3);
                    Canvas.SetTop(text, KatModel[i].Points.P1.Y * _y - 3);
                    Canvas.SetZIndex(text, 1);
                    MyCanvas.Children.Add(text);
                }
            }

            List<Line> lines = WallLine();
            for (int i = 0; i < lines.Count; i++)
            {
                MyCanvas.Children.Add(lines[i]);
            }
            MyCanvas.Height = KatY * _y;
            MyCanvas.Width = KatX * _x;
            MyCanvas.EndInit();
            return MyCanvas;
        }

        private Polygon DrawPolygon(PointModel points, KatDetayModel raf)
        {

           // SolidColorBrush s = new SolidColorBrush();
            string ttString = "";


          //  s = UIOperation.GetScaleColor(raf.Oran);
            ttString = "Raf = " + raf.Name + "\nKoy = " + raf.Koy + "\nSıra = " + raf.Sira + "\nDoluluk Oranı = % " + Math.Round(raf.Oran,1) + "";


            ToolTip tt = new ToolTip()
            {

                Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse,
                Content = ttString

            };

            
            Polygon polygon = new Polygon()
            {
                Tag = raf,

                ToolTip = tt,
                Stroke = Brushes.Black,
                StrokeThickness = 0.2,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            polygon.Fill = ((KatDetayModel)polygon.Tag).Color;
            PointCollection polygonPoints = new PointCollection
            {
                points.P1,
                points.P2,
                points.P3,
                points.P4
            };

            polygon.Points = polygonPoints;
            
            return polygon;
        }

        private PointModel AlterPoints(PointModel points)
        {

            PointModel p = new PointModel()
            {
                P1 = new Point(points.P1.X * _x, points.P1.Y * _y),
                P2 = new Point(points.P2.X * _x, points.P2.Y * _y),
                P3 = new Point(points.P3.X * _x, points.P3.Y * _y),
                P4 = new Point(points.P4.X * _x, points.P4.Y * _y)
            };

            return p;
        }

        private List<Line> WallLine()
        {
            List<Line> list = new List<Line>
            {
                new Line(){ Stroke = Brushes.Black,
                X1 = 0,
                X2 = KatX*_x,
                Y1 = 0,
                Y2 = 0,

                StrokeThickness = _y},

                new Line(){ Stroke = Brushes.Black,
                X1 = KatX*_x,
                X2 = KatX*_x,
                Y1 = 0,
                Y2 = KatY*_y,

                StrokeThickness = _y},

                new Line(){ Stroke = Brushes.Black,
                X1 = KatX*_x,
                X2 = 0,
                Y1 = KatY*_y,
                Y2 = KatY*_y,

                StrokeThickness = _y},

                new Line(){ Stroke = Brushes.Black,
                X1 = 0,
                X2 = 0,
                Y1 = KatY*_y,
                Y2 = 0,

                StrokeThickness = _y}

            };

            return list;
        }
    }
}
