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
    public class KatShape
    {
        public int Width { get; set; }
        public int Height { get; set; }
        private int _katCount;
        private int _sideColWidth;
        private int _MiddleColWidth;
        private int _RowHeight;
        Line _myLine;
        private ColumnDefinition _colDef;
        private RowDefinition _rowDef;
        private Grid _grid;

        public KatShape(int width, int height)
        {
            Width = width;
            Height = height;


        }

        public Grid CanvasKat(int katCount)
        {
            _katCount = katCount;
            _sideColWidth = Width / 10;
            _MiddleColWidth = Width;
            _RowHeight = Height / katCount;

            _grid = new Grid();
            //CATI KATI İÇİN Gridinin ilk satırı
            _rowDef = new RowDefinition()
            {
                Height = new GridLength(_sideColWidth),
                MinHeight = 30
            };
            _grid.RowDefinitions.Add(_rowDef);
            CreateColumns();
            CreateRows();
            CanvasCati();
            CanvasWall();
            Polygon p;

            for (int i = 0; i <= _katCount * 2; i++)
            {
                if (i > 0 && i % 2 == 0)
                {
                    p = CanvasLayer();

                    Grid.SetColumn(p, 1);
                    Grid.SetRow(p, i);
                    _grid.Children.Add(p);
                }
            }

            return _grid;
        }
        /// <summary>
        /// Gridin sutunları oluşturulur
        /// </summary>
        private void CreateColumns()
        {
            //gridin sol tarafı
            _colDef = new ColumnDefinition()
            {
                Width = new GridLength(_sideColWidth),
                MinWidth = 30
            };
            _grid.ColumnDefinitions.Add(_colDef);

            //gridin orta bölümü
            _colDef = new ColumnDefinition()
            {
                Width = new GridLength(_MiddleColWidth),
                MinWidth = 30
            };
            _grid.ColumnDefinitions.Add(_colDef);

            //gridin sag tarafı
            _colDef = new ColumnDefinition()
            {
                Width = new GridLength(_sideColWidth),
                MinWidth = 30
            };
            _grid.ColumnDefinitions.Add(_colDef);
        }

        private void CreateRows()
        {
            for (int i = 0; i < _katCount; i++)
            {
                //Katın bulunacagı satır
                _rowDef = new RowDefinition()
                {
                    Height = new GridLength(_RowHeight),
                    MinHeight = 30
                };
                _grid.RowDefinitions.Add(_rowDef);

                //katın sınır çizgisinşn bulanacagı satır
                _rowDef = new RowDefinition()
                {
                    Height = new GridLength(_RowHeight / 5),
                    MinHeight = 30
                };
                _grid.RowDefinitions.Add(_rowDef);
            }
        }

        private void CanvasCati()
        {
            // Create a blue and a black Brush

            SolidColorBrush yellowBrush = new SolidColorBrush
            {
                Color = Colors.Yellow
            };

            SolidColorBrush blackBrush = new SolidColorBrush
            {
                Color = Colors.Black
            };

            Polygon yellowPolygon = new Polygon
            {
                Stroke = blackBrush,

                Fill = yellowBrush,

                StrokeThickness = 4
            };



            // Create a collection of points for a polygon
            List<Point> PList = new List<Point>();

            Point Point1 = new Point(0, _sideColWidth);

            Point Point2 = new Point(_sideColWidth, 0);

            Point Point3 = new Point(_sideColWidth + _MiddleColWidth, 0);

            Point Point4 = new Point(_sideColWidth * 2 + _MiddleColWidth, _sideColWidth);


            PointCollection polygonPoints = new PointCollection
            {
                Point1,

                Point2,

                Point3,

                Point4
            };



            // Set Polygon.Points properties

            yellowPolygon.Points = polygonPoints;



            // Add Polygon to the page

            Grid.SetRow(yellowPolygon, 0);
            Grid.SetColumn(yellowPolygon, 0);
            Grid.SetColumnSpan(yellowPolygon, 3);
            _grid.Children.Add(yellowPolygon);

        }

        private Polygon CanvasLayer()
        {
            // Create a blue and a black Brush

            SolidColorBrush fillBrush = new SolidColorBrush
            {
                Color = Colors.DarkBlue
            };

            SolidColorBrush blackBrush = new SolidColorBrush
            {
                Color = Colors.Black
            };

            Polygon yellowPolygon = new Polygon
            {
                Stroke = blackBrush,

                Fill = fillBrush,

                StrokeThickness = 4
            };
            int space = 0;

            if (_RowHeight / 5 < 30)
                space = 30;
            else
                space = _RowHeight / 5;

            // Create a collection of points for a polygon

            Point Point1 = new Point(0, 0);

            Point Point2 = new Point(_MiddleColWidth, 0);

            Point Point3 = new Point(_MiddleColWidth, space);

            Point Point4 = new Point(0, space);

            PointCollection polygonPoints = new PointCollection
            {
                Point1,

                Point2,

                Point3,

                Point4
            };



            // Set Polygon.Points properties

            yellowPolygon.Points = polygonPoints;



            return yellowPolygon;

        }

        private void CanvasWall()
        {
            //sol duvar
            _myLine = new Line
            {
                Stroke = Brushes.Black,
                X1 = 0,
                X2 = 0,
                Y1 = 0,
                Y2 = _RowHeight * _katCount * 2 - _sideColWidth,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = 5
            };

            Grid.SetRow(_myLine, 1);
            Grid.SetRowSpan(_myLine, _katCount * 2);
            Grid.SetColumn(_myLine, 0);
            _grid.Children.Add(_myLine);
            //sag duvar
            _myLine = new Line
            {
                Stroke = Brushes.Black,
                X1 = 1,
                X2 = 1,
                Y1 = 0,
                Y2 = _RowHeight * _katCount * 2 - _sideColWidth,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = 5
            };

            Grid.SetRow(_myLine, 1);
            Grid.SetRowSpan(_myLine, _katCount * 2);
            Grid.SetColumn(_myLine, 2);
            _grid.Children.Add(_myLine);
        }
    }
}
