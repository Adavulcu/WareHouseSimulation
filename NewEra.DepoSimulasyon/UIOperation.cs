using NewEra.DepoSimulasyon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Converters;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;

namespace NewEra.DepoSimulasyon
{
    public static class UIOperation
    {
        #region ScaleColorClassRegion
        class ScaleColorsModel
        {
            public ScaleColorsModel(string name, SolidColorBrush color, double oran)
            {
                Name = name;
                Color = color;
                Oran = oran;
            }
            public Double Oran { get; set; }
            public string Name { get; set; }
            public SolidColorBrush Color { get; set; }
        }
        #endregion

        #region VariableRegşon
        private static readonly SolidColorBrush _brushEmpty = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffe6");
        private static readonly SolidColorBrush _brushOverLoaded = (SolidColorBrush)new BrushConverter().ConvertFrom("#660066");
        private static readonly SolidColorBrush _brush_0_25 = (SolidColorBrush)new BrushConverter().ConvertFrom("#99ff00");
        private static readonly SolidColorBrush _brush_25_50 = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffff6e");
        private static readonly SolidColorBrush _brush_50_75 = (SolidColorBrush)new BrushConverter().ConvertFrom("#ff9900");
        private static readonly SolidColorBrush _brush_75_100 = (SolidColorBrush)new BrushConverter().ConvertFrom("#f70000");

        #endregion
        /// <summary>
        /// oran degerine göre Renk döndürür.
        /// </summary>
        /// <param name="oran"></param>
        /// <returns></returns>
        public static SolidColorBrush GetScaleColor(double oran)
        {
            SolidColorBrush sb = new SolidColorBrush();
          

            if (oran > 0 && oran <= 25)
                sb = _brush_0_25;
            else if (oran > 25 && oran <= 50)
                sb = _brush_25_50;
            else if (oran > 50 && oran <= 75)
                sb = _brush_50_75;
            else if (oran > 75 && oran <= 100)
                sb = _brush_75_100;
            else if (oran > 100)
                sb = _brushOverLoaded;
            else
                sb = _brushEmpty;

            sb.Freeze();

            return sb;
        }

        /// <summary>
        /// Paramatre olrak gönderlen gridlerin satır ve sutunlarını oluşturur.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="size"></param>
        public static Grid CreateGridRowsColoums(this Grid grid, int rowCount, int colCount, int width, int height)
        {

            for (int i = 0; i < colCount; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition
                {
                    Width = new GridLength(width),

                };

                grid.ColumnDefinitions.Add(gridCol);


            }
            for (int i = 0; i < rowCount; i++)
            {
                RowDefinition gridRow = new RowDefinition
                {
                    Height = new GridLength(height)

                };
                grid.RowDefinitions.Add(gridRow);
            }

            return grid;
        }

        /// <summary>
        /// Gridin içerisine yerleştirilecek labe i üretir
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="x">Row Index</param>
        /// <param name="y">Column Index</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="koy"></param>
        public static void SetLabelIntoGrid(this Label lbl, int x, int y, int width, int height, KoyModel koy)
        {
            Grid.SetRow(lbl, x);
            Grid.SetColumn(lbl, y);
            lbl.Content = koy.KoyY;
            string str = "lbl" + (koy.KoyX + "_" + koy.KoyY).ToString();
            lbl.Name = str.ToString();
            lbl.Tag = koy;
            lbl.MinHeight = 40;
            lbl.MinWidth = 40;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.Height = height;
            lbl.Width = width;
            lbl.Margin = new Thickness(2, 2, 2, 2);
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            lbl.VerticalContentAlignment = VerticalAlignment.Center;
        }
        /// <summary>
        /// Gridin içerisine yerleştirilecek labe i üretir
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="content"></param>
        /// <param name="x">Row Index</param>
        /// <param name="y">Column Index</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetLabelIntoGrid(this Label lbl, string content, int x, int y, int width, int height)
        {
            Grid.SetRow(lbl, x);
            Grid.SetColumn(lbl, y);
            lbl.Content = content;
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.Height = height;
            lbl.Width = width;
            lbl.Margin = new Thickness(0.5, 0.5, 0.5, 0.5);
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            lbl.VerticalContentAlignment = VerticalAlignment.Center;
        }
        /// <summary>
        /// Gridin içerisine yerleştirilecek labe i üretir
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="x">Row Index</param>
        /// <param name="y">Column Index</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetLabelIntoGrid(this Label lbl, int x, int y, int width, int height)
        {
            Grid.SetRow(lbl, x);
            Grid.SetColumn(lbl, y);
            lbl.HorizontalAlignment = HorizontalAlignment.Center;
            lbl.VerticalAlignment = VerticalAlignment.Center;
            lbl.Height = height;
            lbl.Width = width;
            lbl.Margin = new Thickness(1, 1, 1, 1);
            lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            lbl.VerticalContentAlignment = VerticalAlignment.Center;
        }

        /// <summary>
        /// İndis bilgisi verilen Labeli bulup geriye döndürür
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static Label Getlabel(this Grid grid, int row, int col)
        {
            //parametre olarak gelen grid in içerisindeki labeli metotda gelen row ve col degerlerine göre bularak geriye döndürü
            foreach (Label label in grid.Children)
            {
                if (Grid.GetRow(label) == row && Grid.GetColumn(label) == col)
                    return label;

            }
            return null;//eger parametre olarak verilen indislerde label yoksa null döndürülür
        }

        /// <summary>
        /// Parametre olarak gelen sutun indexine göre satırdaki degerleri oranını hesaplar
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public static int GetLabelOran(this Grid grid, int colIndex)
        {
            int total = 0;
            Label lbl;
            try
            {
                for (int i = 0; i < grid.RowDefinitions.Count; i++)
                {
                    lbl = grid.Getlabel(i, colIndex);
                    KoyModel k = (KoyModel)lbl.Tag;
                    if (k.State)
                        total++;
                }
                return (total * 100) / grid.RowDefinitions.Count;
            }
            catch (Exception ex)
            {

                throw new Exception("\nGetLabelOran\n" + ex.Message);
            }
        }

        /// <summary>
        /// Parametre olarak aldıgı Size degerine göre gelen Image i yeni boyutunu ayarlar
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static System.Windows.Controls.Image ResizeImage(this System.Windows.Controls.Image imgToResize, System.Windows.Size size)
        {
            imgToResize.Width = size.Width;
            imgToResize.Height = size.Height;
            return imgToResize;
        }

        /// <summary>
        /// Parametre olarak aldıgı byte[] dizisini ImageSoruce e dönüstürerek geriye döndürür.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static BitmapImage ByteToImage(this byte[] array)
        {
            BitmapImage bitmapImage = new BitmapImage();
            MemoryStream ms = new MemoryStream(array);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            return bitmapImage;
        }

        /// <summary>
        /// Parametre olarak gelen Image i byte[] dizisine dönüştürürek geriye döndürür
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImgToByte(this System.Windows.Controls.Image img)
        {
            byte[] arr;
            ImageConverter converter = new ImageConverter();
            arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return arr;
        }

        /// <summary>
        /// Parametre olarak aldıgı string yolunu ImageSoruce e dönüştürerek geriye döndürür
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage StrToImage(this string path)
        {
            Uri imageUri = new Uri(@path);
            BitmapImage imageBitmap = new BitmapImage(imageUri);
            return imageBitmap;
        }

        /// <summary>
        /// Parametre olarak aldıgı string yolunu Byte[] dizisine dönüştürerek geriye döndürür
        /// </summary>
        /// <param name="imgPath"></param>
        /// <returns></returns>
        public static byte[] StrToByte(this string imgPath)
        {
            byte[] b = File.ReadAllBytes(imgPath);
            return b;
        }

        private static Random random = new Random();

        /// <summary>
        /// Random SoldiColorBrush nesnesini oluşturarak geriye döndürür
        /// </summary>
        /// <returns></returns>
        public static SolidColorBrush RandomBrushes()
        {

            SolidColorBrush brush =
                new SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(
                    (byte)random.Next(255),
                    (byte)random.Next(255),
                    (byte)random.Next(255)
                    ));
            return brush;
        }

        /// <summary>
        /// Köşe koordinatları parametre olarak gelen bir nesneinin Width ve Height degerlerini (10 ile çarparak) hesaplar
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p4"></param>
        public static void CalculateWidthAndHeight(out int width, out int height, System.Windows.Point p1, System.Windows.Point p2, System.Windows.Point p4)
        {
            try
            {
                width = ((p2.X - p1.X) * 10).ToInt();
                height = ((p4.Y - p1.Y) * 10).ToInt();
            }
            catch (Exception ex)
            {

                throw new Exception("\nCalculateWidth\n" + ex.Message);
            }
        }


        //public static PointModel NewPoints(PointModel chilPoint, PointModel parentPoint)
        //{
        //    PointModel newPoint = new PointModel
        //    {
        //        P1 = new System.Windows.Point(chilPoint.P1.X - parentPoint.P1.X, chilPoint.P1.Y - parentPoint.P1.Y),
        //        P2 = new System.Windows.Point(chilPoint.P2.X - parentPoint.P1.X, chilPoint.P2.Y - parentPoint.P1.Y),
        //        P3 = new System.Windows.Point(chilPoint.P3.X - parentPoint.P1.X, chilPoint.P3.Y - parentPoint.P1.Y),
        //        P4 = new System.Windows.Point(chilPoint.P4.X - parentPoint.P1.X, chilPoint.P4.Y - parentPoint.P1.Y)
        //    };

        //    return newPoint;
        //}

        /// <summary>
        /// Indexleri parametre olarak gelen Gridi hücresinin Width ve Height degerini Hesaplar
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="colWidth"></param>
        /// <param name="rowHeight"></param>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        public static void GetColAndRowSize(this Grid grid, out int colWidth, out int rowHeight, int colIndex, int rowIndex)
        {
            RowDefinition r = grid.RowDefinitions[rowIndex];
            ColumnDefinition c = grid.ColumnDefinitions[colIndex];
            GridLength gl;
            gl = r.Height;
            rowHeight = (int)r.ActualHeight;
            colWidth = (int)c.ActualWidth;


        }
        /// <summary>
        /// Yatay renk skalası oluşturur
        /// </summary>
        /// <param name="Parentgrid"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colSpan"></param>
        /// <returns></returns>
        public static Grid CreateColorsGrid(Grid Parentgrid, int row, int col, int rowSpan = 1, int colSpan = 1)
        {
            try
            {
                Grid grid = new Grid();
                grid.BeginInit();
                Parentgrid.GetColAndRowSize(out int colWidth, out int rowHeight, col, row);
                grid = grid.CreateGridRowsColoums(1, 6, colWidth * colSpan / 6, rowHeight * rowSpan);
                grid.EndInit();

                List<ScaleColorsModel> list = new List<ScaleColorsModel>()
            {
                new ScaleColorsModel("BOŞ",GetScaleColor(0),0),
             //  new ScaleColorsModel("% 0-10",GetScaleColor(5),5),
                new ScaleColorsModel("% 0-25",GetScaleColor(15),15),
              //  new ScaleColorsModel("% 20-30",GetScaleColor(25),25),
                new ScaleColorsModel("% 25-50",GetScaleColor(35),35),
              //  new ScaleColorsModel("% 40-50",GetScaleColor(45),45),
                new ScaleColorsModel("% 50-75",GetScaleColor(55),55),
               // new ScaleColorsModel("% 60-70",GetScaleColor(65),65),
                new ScaleColorsModel("% 75-100",GetScaleColor(85),85),
            //    new ScaleColorsModel("% 80-90",GetScaleColor(85),85),
             //   new ScaleColorsModel("% 90-100",GetScaleColor(95),95),
                 new ScaleColorsModel("AŞIRI YÜKLÜ",GetScaleColor(110),110)

            };


                for (int i = 0; i < list.Count; i++)
                {
                    Label lbl = new Label()
                    {


                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        FontSize = rowHeight / 4,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(5, 5, 5, 5),
                        Background = list[i].Color,
                        Content = list[i].Name

                    };
                    if (list[i].Oran > 80)
                    {
                        lbl.Foreground = Brushes.White;
                    }
                    else
                        lbl.Foreground = Brushes.Black;
                    Grid.SetRow(lbl, 0);
                    Grid.SetColumn(lbl, i);
                    grid.Children.Add(lbl);

                }
                return grid;
            }
            catch (Exception ex)
            {

                throw new Exception("\nCreateGridColors\n" + ex.Message);
            }
        }

        /// <summary>
        /// Parametre olarak aldıgı Color rengini tersine cevirir
        /// </summary>
        /// <param name="originalColor"></param>
        /// <returns></returns>
        private static System.Windows.Media.Color InvertColors(System.Windows.Media.Color originalColor)
        {
            System.Windows.Media.Color invertedColor = new System.Windows.Media.Color
            {
                ScR = 1.0F - originalColor.ScR,
                ScG = 1.0F - originalColor.ScG,
                ScB = 1.0F - originalColor.ScB,
                ScA = originalColor.ScA
            };
            return invertedColor;
        }

        private static System.Windows.Media.Color DarkerColors(System.Windows.Media.Color originalColor, int KatSayi)
        {
            float f = 0.1F * KatSayi;

            float fr = originalColor.ScR + f;
            float fg = originalColor.ScG + f;
            float fb = originalColor.ScB + f;
            System.Windows.Media.Color invertedColor = new System.Windows.Media.Color
            {
                ScR = 1.0F - fr,
                ScG = 1.0F - fg,
                ScB = 1.0F - fb,
                ScA = originalColor.ScA
            };
            return invertedColor;
        }

        /// <summary>
        /// Parametre olarak aldıgı SolidColorBrush rengini tersine cevirir
        /// </summary>
        /// <param name="scb"></param>
        /// <returns></returns>
        public static SolidColorBrush InvertSoldibrushes(this SolidColorBrush scb)
        {
            System.Windows.Media.Color c = ((SolidColorBrush)scb).Color;
            c = InvertColors(c);
            scb = new SolidColorBrush(c);
            return scb;
        }

        public static SolidColorBrush DarkerSolidBrushes(this SolidColorBrush scb, int lvl = 5)
        {
            System.Windows.Media.Color c = ((SolidColorBrush)scb).Color;
            c = DarkerColors(c, lvl);
            scb = new SolidColorBrush(c);
            return scb;
        }

        /// <summary>
        /// Bir sıraya ait bilgileri içeren bir Window oluşturur
        /// </summary>
        /// <param name="list"></param>
        /// <param name="oran"></param>
        /// <returns></returns>
        public static Window CreateRafSıra(List<HucreModel> list, double oran)
        {
            try
            {
                string windowName = "RAF " + list[0].Raf + " - KOY " + list[0].Koy + " - SİRA " + list[0].Sira.ToString() + "";
                int height = 500;
                int width = 374;

                Window window = new Window
                {
                    Height = height,
                    Width = width,
                    Title = windowName,
                    MaxHeight = height,
                    MaxWidth = width,
                    MinHeight = height,
                    MinWidth = width,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold

                };

                Grid mainGrid = new Grid();
                mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(70) });
                mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10) });
                mainGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(420) });
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width / 2) });
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(width / 2) });

                mainGrid.VerticalAlignment = VerticalAlignment.Stretch;
                mainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                mainGrid.Background = Brushes.DarkBlue;

                Label lbl1 = new Label();
                lbl1.SetLabelIntoGrid("GENEL ORAN", 0, 0, width / 2, 70);
                lbl1.Background = Brushes.LightBlue;
                mainGrid.Children.Add(lbl1);
                Label lbl2 = new Label();
                lbl2.SetLabelIntoGrid("% " + Math.Round(oran, 1) + "", 0, 1, width / 2, 70);
                lbl2.Background = GetScaleColor(oran);
                if (oran >=75)
                {
                    lbl2.Foreground = Brushes.White;
                }
                else
                    lbl2.Foreground = Brushes.Black;

                mainGrid.Children.Add(lbl2);

                Grid grid = new Grid();
                grid.CreateGridRowsColoums(list.Count, 2, width / 2, 420 / (list.Count + 1));

                for (int i = 0; i < list.Count(); i++)
                {
                    Label lblAdres = new Label();
                    lblAdres.SetLabelIntoGrid(list[i].Name, i, 0, width / 2, 420 / (list.Count + 1));
                    lblAdres.Background = Brushes.Wheat;
                    grid.Children.Add(lblAdres);
                    HucreModel hm = list[i];
                    double HucreOran = list[i].Oran;
                    Label lblOran = new Label();

                    if (list[i].Tip == 100)
                    {
                        lblOran.SetLabelIntoGrid("TÜNEL", i, 1, width / 2, 420 / (list.Count + 1));
                        lblOran.Background = Brushes.Aqua;
                    }
                    else if (list[i].Tip == 255)
                    {
                        lblOran.SetLabelIntoGrid("KULLANILMAZ", i, 1, width / 2, 420 / (list.Count + 1));
                        lblOran.Background = Brushes.DarkGray;
                        lblOran.Foreground = Brushes.White;
                    }
                    else
                    {


                        lblOran.SetLabelIntoGrid("% " + list[i].Oran.ToString() + "", i, 1, width / 2, 420 / (list.Count + 1));
                        lblOran.Background = GetScaleColor(list[i].Oran);
                        if (list[i].Oran >=75)
                        {
                            lblOran.Foreground = Brushes.White;
                        }
                        else
                            lblOran.Foreground = Brushes.Black;

                    }
                    if (true)
                    {
                        lblAdres.MouseDoubleClick += delegate (object sender, MouseButtonEventArgs e) { HucreClick(sender, e, hm); };
                        lblOran.MouseDoubleClick += delegate (object sender, MouseButtonEventArgs e) { HucreClick(sender, e, hm); };
                    }
                    grid.Children.Add(lblOran);
                }


                Grid.SetRow(grid, 2);
                Grid.SetColumn(grid, 0);
                Grid.SetColumnSpan(grid, 2);

                mainGrid.Children.Add(grid);

                window.Content = mainGrid;

                return window;
            }
            catch (Exception ex)
            {

                throw new Exception("\nCreateRafSıra\n" + ex.Message);
            }
        }

        public static void HucreClick(object sender, MouseButtonEventArgs e, HucreModel hm)
        {
            Label lbl = (Label)sender;
            Data data = new Data();
            DataTable dt = new DataTable();

            dt = data.HucreBilgiGetir(hm);

            if (dt.Rows.Count > 0)
            {
                List<UrunModel> u = new List<UrunModel>();

                int paletMiktar = dt.AsEnumerable()
                    .Sum(r => r["PaletMiktar"].ToString().ToInt());
                int toplamMiktar = 0;
                foreach (DataRow row in dt.Rows)
                {
                    Guid id = new Guid(row["StokId"].ToString());
                    int miktar = row["Miktar"].ToInt();
                    string kod = row["Kod"].ToString();
                    string isim = row["Isim"].ToString();
                    toplamMiktar += miktar;
                    u.Add(new UrunModel(id, isim, kod, miktar));
                }

                HucreModel h = new HucreModel(hm.Id, hm.Name, hm.Tip, hm.TipIsim, u);
                WindowHucre wh = new WindowHucre
                {
                    Topmost = true
                };
                wh.Init(h, paletMiktar, hm.Oran, toplamMiktar);
                wh.Show();
                wh.CreateUrun();
            }



        }

        public static void SetGridIndex(dynamic control, int rowIndex, int colIndex, int rowSpan = 1, int colSpan = 1)
        {
            Grid.SetColumn(control, colIndex);
            Grid.SetRow(control, rowIndex);
            Grid.SetRowSpan(control, rowSpan);
            Grid.SetColumnSpan(control, colSpan);
        }

        /// <summary>
        /// Projenim Debug dosyasında bulunan dosyanın adını paramete olarak alıp , o dosyanın yolunu oluşturu
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static string CreateFilePath(string FileName)
        {
            try
            {
                string executableLocation = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string path = System.IO.Path.Combine(executableLocation, FileName);

                return path;
            }
            catch (Exception ex)
            {

                throw new Exception("\nFindImagePath\n" + ex.Message);
            }
        }
        public static void SetImagesVisibiyt(Visibility v, params System.Windows.Controls.Image[] controls)
        {
            try
            {
                foreach (System.Windows.Controls.Image c in controls)
                {
                    c.Visibility = v;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("\nSetControlsVisibilyt\n" + ex.Message);
            }
        }
    }
}
