using NewEra.DepoSimulasyon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NewEra.DepoSimulasyon
{
    /// <summary>
    /// Interaction logic for WindowHucre.xaml
    /// </summary>
    public partial class WindowHucre : Window
    {
        public WindowHucre()
        {
            InitializeComponent();
        }
        HucreModel _hm;
        int _width;
        int _height;
        string _imgPath = "C:\\Oznet\\boya.jpg";
       

        public void Init(HucreModel hm, int paletMiktar, double oran, int toplamMiktar)
        {
            _hm = hm;
            this.KeyDown += Windows_KeyDown;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            LblAdres.Content = hm.Name;
           // oran =(Double)toplamMiktar / paletMiktar*100;
            LblOran.Content = "% " + Math.Round(oran, 1).ToString();
            if (oran >=75)
            {
                LblOran.Foreground = Brushes.White;
            }
            else
                LblOran.Foreground = Brushes.Black;
            LblOran.Background = UIOperation.GetScaleColor(oran);
            LblTipIsim.Content = hm.TipIsim;
            LblMiktar.Content = toplamMiktar.ToString() + " / " + paletMiktar.ToString();


        }



        private void Windows_KeyDown(object sender, KeyEventArgs e)
        {
            Window w = (Window)sender;
            if (e.Key == Key.Escape)
            {
                w.Close();
            }
        }

        public void CreateUrun()
        {
            try
            {
                MainGrid.GetColAndRowSize(out int colWidth, out int rowHeight, 0, 2);
                _width = colWidth * 3;
                _height = 240;

                Grid g = new Grid();
                ColumnDefinition col = new ColumnDefinition()
                { Width = new GridLength(_width) };
                g.ColumnDefinitions.Add(col);

                for (int i = 0; i < _hm.Urun.Count(); i++)
                {

                    RowDefinition row = new RowDefinition()
                    {
                        Height = new GridLength(_height)
                    };
                    g.RowDefinitions.Add(row);
                    g.Children.Add(CreateUrunControls(i));


                }
                Grid.SetRow(g, 3);
                Grid.SetColumn(g, 0);
                Grid.SetColumnSpan(g, 3);
                GridHucreBilgi.Children.Add(g);
            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateUrun\n" + ex.Message);
            }
        }

        private GroupBox CreateUrunControls(int index)
        {

            try
            {
                Grid g = new Grid
                {
                    Background = Brushes.MediumPurple

                };
                GroupBox grb = new GroupBox()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = _width,
                    Height = _height,
                    Background = Brushes.MediumVioletRed
                };
                Grid.SetColumn(grb, 0);
                Grid.SetRow(grb, index);
                g.Margin = new Thickness(10);
                for (int i = 0; i < 4; i++)
                {
                    RowDefinition row = new RowDefinition()
                    {
                        Height = new GridLength(_height / 5)
                    };
                    g.RowDefinitions.Add(row);
                }
                int length = 1;
                for (int i = 0; i < 3; i++)
                {
                    if (i == 2)
                        length = 2;

                    ColumnDefinition col = new ColumnDefinition()
                    {
                        Width = new GridLength(_width / 4 * length - 10)
                    };
                    g.ColumnDefinitions.Add(col);
                }

                Image img = new Image
                {
                    Source = UIOperation.StrToImage(_imgPath)

                };
                img.ResizeImage(new Size(_width / 4, _height));

                Grid.SetRow(img, 0);
                Grid.SetRowSpan(img, 4);
                Grid.SetColumn(img, 0);

                g.Children.Add(img);

                Label lblKod = new Label
                {
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.Wheat
                };
                lblKod.SetLabelIntoGrid(_hm.Urun[index].Kod, 0, 2, _width / 2 - 10, _height / 5);
                g.Children.Add(lblKod);
                Label lblIsim = new Label
                {
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.Wheat
                };
                lblIsim.SetLabelIntoGrid(_hm.Urun[index].Name, 1, 2, _width / 2 - 10, _height / 5);
                g.Children.Add(lblIsim);
                Label lblMiktar = new Label
                {
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.Wheat
                };
                lblMiktar.SetLabelIntoGrid(_hm.Urun[index].Miktar.ToString(), 2, 2, _width / 2 - 10, _height / 5);
                g.Children.Add(lblMiktar);
                Label lblKodText = new Label
                {
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.LightGreen
                };
                lblKodText.SetLabelIntoGrid("ÜRÜN KODU :", 0, 1, _width / 4 - 10, _height / 5);
                g.Children.Add(lblKodText);
                Label lblIsimText = new Label
                {
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.LightGreen
                };
                lblIsimText.SetLabelIntoGrid("ÜRÜN İSMİ", 1, 1, _width / 4 - 10, _height / 5);
                g.Children.Add(lblIsimText);
                Label lblMiktarText = new Label
                {
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.LightGreen
                };
                lblMiktarText.SetLabelIntoGrid("MİKTAR", 2, 1, _width / 4 - 10, _height / 5);
                g.Children.Add(lblMiktarText);

                Button btn = new Button()
                {

                    Content = "DETAY",
                    Width = _width / 4,
                    Height = _height / 4,
                    FontSize = 15,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Background = Brushes.WhiteSmoke


                };
                btn.Click += delegate (object sender, RoutedEventArgs e) { GetPic(); };
                Grid.SetRow(btn, 3);
                Grid.SetColumn(btn, 2);
                Grid.SetColumnSpan(btn, 2);
                g.Children.Add(btn);
                grb.Content = g;

                return grb;
            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateUrunControls\n" + ex.Message);
                return null;
            }
        }
        private void GetPic()
        {

        }
    }
}
