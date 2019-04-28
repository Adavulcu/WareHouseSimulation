using NewEra.DepoSimulasyon.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for WindowRafGenelDurum.xaml
    /// </summary>
    public partial class WindowRafGenelDurum : Window
    {
        Data _data;
        DataTable _rafGenelBilgi;
        string _rafIsim;
        public WindowRafGenelDurum()
        {
            InitializeComponent();

        }
        KatDetayModel _kdm;
        double _oran;
        int _toplam = 0;
        public void Init(KatDetayModel kdm, double oran)
        {
            try
            {
                _oran = oran;
                _kdm = kdm;
                _rafIsim = kdm.Name;
                _rafGenelBilgi = new DataTable();
                _data = new Data();
                this.Title = "RAF -" + kdm.Name;
                LblRafGenelDurum.Content = "RAF - " + kdm.Name + " = % " + Math.Round(oran, 1) + "";
                LblRafGenelDurum.Background = UIOperation.GetScaleColor(oran);

                _rafGenelBilgi = _data.RafGenelBilgiGetir(kdm);
                SetOranLabelsValues(_rafGenelBilgi);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Init\n" + ex.Message);
            }
        }

        private void BtnRafDetay_Click(object sender, RoutedEventArgs e)
        {
            //WindowDepo wd = new WindowDepo();
            //wd.Init("RAF A", 50, 7);
            //wd.ShowDialog();

            WindowRafDetay wrd = new WindowRafDetay();
            wrd.Init(_rafGenelBilgi, _kdm, _oran);
            wrd.ShowDialog();
          //  wrd.CreateColorScale();
          //  wrd.CreateTipGrid();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void SetOranLabelsValues(DataTable dt)
        {
            try
            {
                int bos = 0;
                int asiriYuklu = 0;
                int tunel = 0;
                int kullanilmaz = 0;
                int c0_25 = 0;
                int c25_50 = 0;
                int c50_75 = 0;
                int c75_100 = 0;
              

                foreach (DataRow row in dt.Rows)
                {
                    _toplam++;
                    int tip = row["Tip"].ToInt();
                    double oran = row["oran"].To<double>();
                    if (tip == 100)
                    {
                        tunel++;
                    }
                    else if (tip == 255)
                        kullanilmaz++;
                    else
                    {
                        if (oran == 0)
                            bos++;
                        else if (oran > 100)
                            asiriYuklu++;
                        else if (oran > 0 && oran <= 25)
                            c0_25++;
                        else if (oran > 25 && oran <= 50)
                            c25_50++;
                        else if (oran > 50 && oran <= 75)
                            c50_75++;
                        else if (oran > 75 && oran <= 100)
                            c75_100++;
                      
                        else
                            throw new Exception("Geçersiz Oran Değeri");

                    }

                }

                lblBos.Content = bos.ToString() + " ADET";
                lblOverloaded.Content = asiriYuklu.ToString() + " ADET";
                lblTunnel.Content = tunel.ToString() + " ADET";
                lblUnUsing.Content = kullanilmaz.ToString() + " ADET";
                lbl_0_25.Content = c0_25.ToString() + " ADET";
                lbl_25_50.Content = c25_50.ToString() + " ADET";
                lbl_50_75.Content = c50_75.ToString() + " ADET";
                lbl_75_100.Content = c75_100.ToString() + " ADET";
              
                lblToplam.Content = _toplam.ToString() + " ADET";
            }
            catch (Exception ex)
            {

                MessageBox.Show("SetOranLabelsValues\n" + ex.Message);
            }
        }
    }
}
