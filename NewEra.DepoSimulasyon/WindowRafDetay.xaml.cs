using NewEra.DepoSimulasyon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for WindowRafDetay.xaml
    /// </summary>
    public partial class WindowRafDetay : Window
    {
        public WindowRafDetay()
        {
            InitializeComponent();
        }
       // Guid _depoId, _katId;
        DataTable _dt;
        KatDetayModel _Kdm;
        private int _width = 50;
        private int _height = 50;
        List<IEnumerable<DataRow>> _rows;
        List<int> _ListTip;
        private int _hucreRowCount;
        private int _hucreColCount;

        private int _tipLabelWidth;
        private int _tipLabelHeight;

        double _genelToplam = 0;
        double _genelOrtalama;
        int _counter = 0;
        int _bos = 0;
        int _asiriYuklu = 0;
        int _c0_25 = 0;
        int _c25_50 = 0;
        int _c50_75 = 0;
        int _c75_100 = 0;

       // string _RafIsim;
        Timer _Timer;


        public void Init(DataTable dt, KatDetayModel kdm, double genOrt)
        {
            try
            {
                _Kdm = kdm;
              //  _RafIsim = kdm.Name;
              //  _depoId = kdm.DepoId;
               // _katId = kdm.KatId;
                _dt = dt;
                _genelOrtalama = genOrt;
                this.Title = "RAF - " + _Kdm.Name + " TÜM PALETLER";
                int maxYukseklik = _dt.AsEnumerable()
                        .Max(row => row["Yukseklik"]).ToString().ToInt();
                int count = dt.Rows.Count;

                _ListTip = SelectTips();
                _rows = ClassifyingTips(_ListTip);
                var datas = _dt.Select("Yukseklik=" + 1);

                int c = ((DataRow[])datas).Length;

                _hucreRowCount = maxYukseklik;
                _hucreColCount = c;


                GridOran.CreateGridRowsColoums(1, _hucreColCount, _width, _height);
                GridHucre.CreateGridRowsColoums(_hucreRowCount, _hucreColCount, _width, _height);

                CreateHucre();
                _Timer = new Timer(10000);
                _Timer.Elapsed += new ElapsedEventHandler(StartRefreshData);
                _Timer.Start();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Init\n" + ex.Message);
            }
        }

        private async void StartRefreshData(object o, ElapsedEventArgs a)
        {
            await Task.Run(() => RefrashDataAsync());
        }

        private Task<Boolean> RefrashDataAsync()
        {
            return Task.Run<Boolean>(() =>
            {
                this.Dispatcher.Invoke((Action)(() => {


                    this.Cursor = Cursors.Wait;
                    RefreshData rd=new RefreshData(_Kdm);
                    _dt = rd.RefreshRafDetayData();
                  
                    _rows = ClassifyingTips(_ListTip);
                    RefreshHucre();
                    RefreshTipGrid();
                 
                    this.Cursor = Cursors.Arrow;

                }));

                return true;
            }
          );
        }

      

        private void RefreshHucre()
        {
            try
            {
                int counter = 0;
                for (int i = 0; i < _hucreColCount; i++)
                {
                    int oranCounter = 0;
                    double toplam = 0;
                    double Or = 0;

                    int yukseklikControl = _hucreRowCount;

                    for (int j = 0; j < _hucreRowCount; j++)
                    {


                        Label lblHucre = UIOperation.Getlabel(GridHucre, j, i);

                        DataRow row = _dt.Rows[counter];
                        int yukseklik = row["Yukseklik"].ToInt();

                        if (yukseklikControl != yukseklik)
                        {
                            yukseklikControl--;
                            continue;
                        }
                        yukseklikControl--;
                        string adres = row["Adres"].ToString();
                        int tip = row["Tip"].ToInt();
                        string tipIsim = row["TipIsim"].ToString();
                        Guid id = new Guid(row["AdresId"].ToString());
                        Guid ulId = new Guid(row["RafId"].ToString());

                        string koy = row["Koy"].ToString();
                        int sira = row["Sira"].ToInt();
                        string raf = row["Raf"].ToString();
                        double oran = Convert.ToDouble(row["Oran"].ToString());

                        HucreModel hm = new HucreModel(_Kdm.KatId, _Kdm.DepoId, id, ulId, adres, koy, sira, yukseklik, raf, oran, tip, tipIsim);


                        lblHucre.Tag = hm;

                     
                    
                        if (tip == 100)
                        {
                            //lblHucre.Background = Brushes.Transparent;
                            //lblHucre.Content = "";
                        }
                        else if (tip == 255)
                        {
                            //lblHucre.Background = Brushes.Black;
                            //lblHucre.Content = "K";
                            //lblHucre.Foreground = Brushes.White;
                        }
                        else
                        {
                            lblHucre.Background = UIOperation.GetScaleColor(oran);
                            lblHucre.Content = "%" + Math.Round(oran, 0).ToString();
                            if (oran >= 75)
                                lblHucre.Foreground = Brushes.White;
                            else
                                lblHucre.Foreground = Brushes.Black;

                            oranCounter++;
                            toplam += oran;
                        }

                   

                        counter++;
                    }
                    if (oranCounter > 0)
                        Or = (double)toplam / oranCounter;
                    else
                        Or = -1;
                    Label lblOran = UIOperation.Getlabel(GridOran, 0, i);
                    lblOran.SetLabelIntoGrid(0, i, _width, _height);
                    lblOran.FontSize = 13;
                    lblOran.FontWeight = FontWeights.Bold;

                    if (Or == -1)
                    {
                        lblOran.Content = " ";
                        lblOran.Background = Brushes.DarkGray;
                    }

                    else
                    {
                        lblOran.Content = "%" + Math.Round(Or, 0).ToString();
                        lblOran.Background = UIOperation.GetScaleColor(Or);

                    }
                    if (Or >= 75)
                        lblOran.Foreground = Brushes.White;
                    else
                        lblOran.Foreground = Brushes.Black;


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("RefreshHucre\n" + ex.Message);
            }
        }

        private void RefreshTipGrid()
        {
            try
            {
                _bos =0;
                _asiriYuklu =0;
                _c0_25 =0;
                _c25_50 =0;
                _c50_75 =0;
                _c75_100 =0;
                _counter =0;
                _genelToplam = 0;
                for (int i = 0; i < _rows.Count; i++)
                {
                    DataRow[] rowArray = (DataRow[])_rows[i];
                    string tipIsim = rowArray[0]["TipIsim"].ToString();
                    RefreshTipColorsLabels(rowArray, i);
                }

                RefreshGenelToplamLabels(_rows.Count);
            }
            catch (Exception ex)
            {

                MessageBox.Show("RefreshTipGrid\n"+ex.Message);
            }
        }

        private void RefreshTipColorsLabels(DataRow[] rows,int gridRowIndex)
        {
            try
            {
                Label lblOrtalamaSonuc = UIOperation.Getlabel(GridTip, gridRowIndex + 1, 8);
                Label lbl_bos = UIOperation.Getlabel(GridTip, gridRowIndex + 1, 1);
                Label lbl_asiriYuklu = UIOperation.Getlabel(GridTip, gridRowIndex+1, 6);
                Label lbl_0_25 = UIOperation.Getlabel(GridTip, gridRowIndex + 1,2);
                Label lbl_25_50 = UIOperation.Getlabel(GridTip, gridRowIndex + 1, 3);
                Label lbl_50_75 = UIOperation.Getlabel(GridTip, gridRowIndex + 1, 4);
                Label lbl_75_100 = UIOperation.Getlabel(GridTip, gridRowIndex + 1, 5);
                Label lbl_toplam = UIOperation.Getlabel(GridTip, gridRowIndex + 1,7);

                int bos = 0;
                int asiriYuklu = 0;
                int c0_25 = 0;
                int c25_50 = 0;
                int c50_75 = 0;
                int c75_100 = 0;

                int counter = 0;
                double toplam = 0;
                double ortalama = 0;
                for (int i = 0; i < rows.Length; i++)
                {
                    double oran = rows[i]["oran"].To<double>();
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

                    counter++;
                    toplam += oran;
                }

                _bos += bos;
                _asiriYuklu += asiriYuklu;
                _c0_25 += c0_25;
                _c25_50 += c25_50;
                _c50_75 += c50_75;
                _c75_100 += c75_100;
                _genelToplam += toplam;
                ortalama = toplam / counter;
                _counter += counter;

                lblOrtalamaSonuc.Background = UIOperation.GetScaleColor(ortalama);
                lblOrtalamaSonuc.Content = "% " + Math.Round(ortalama, 0).ToString();
                lbl_toplam.Content = counter.ToString();

                lbl_bos.Content = bos.ToString();
                lbl_asiriYuklu.Content = asiriYuklu.ToString();
                lbl_0_25.Content = c0_25.ToString();
                lbl_25_50.Content = c25_50.ToString();
                lbl_50_75.Content = c50_75.ToString();
                lbl_75_100.Content = c75_100.ToString();

            }
            catch (Exception ex)
            {

                MessageBox.Show("RefreshTipColorsLabels\n"+ex.Message);
            }
        }

        private void RefreshGenelToplamLabels(int rowCount)
        {
            try
            {

                Label lblToplam = UIOperation.Getlabel(GridTip, rowCount + 1, 7);
                lblToplam.Content = _counter.ToString();

                _genelOrtalama = Math.Round(_genelToplam / _counter, 0);
                Label lblGenelOrtalama = UIOperation.Getlabel(GridTip, rowCount + 1, 8);
                lblGenelOrtalama.Content = "% " +_genelOrtalama ;
                lblGenelOrtalama.Background = UIOperation.GetScaleColor(_genelOrtalama);


                Label lbl_bos = UIOperation.Getlabel(GridTip, rowCount + 1, 1);
                lbl_bos.Content = _bos.ToString();
                       
                Label lbl_asiriYuklu = UIOperation.Getlabel(GridTip, rowCount + 1, 6);
                lbl_asiriYuklu.Content = _asiriYuklu.ToString();

                Label lbl_0_25 = UIOperation.Getlabel(GridTip, rowCount + 1, 2);
                lbl_0_25.Content = _c0_25.ToString();

                Label lbl_25_50 = UIOperation.Getlabel(GridTip, rowCount + 1, 3);
                lbl_25_50.Content = _c25_50.ToString();

                Label lbl_50_75 = UIOperation.Getlabel(GridTip, rowCount + 1, 4);
                lbl_50_75.Content = _c50_75.ToString();

                Label lbl_75_100 = UIOperation.Getlabel(GridTip, rowCount + 1, 5);
                lbl_75_100.Content = _c75_100.ToString();
         
            }
            catch (Exception ex)
            {

                MessageBox.Show("RefreshGelenToplamLabels\n"+ex.Message);
            }
        }

        private void CreateHucre()
        {
            try
            {
                int counter = 0;
                for (int i = 0; i < _hucreColCount; i++)
                {
                    int oranCounter = 0;
                    double toplam = 0;
                    double Or = 0;

                    int yukseklikControl = _hucreRowCount;

                    for (int j = 0; j < _hucreRowCount; j++)
                    {


                        Label lblHucre = new Label();

                        DataRow row = _dt.Rows[counter];
                        int yukseklik = row["Yukseklik"].ToInt();

                        if (yukseklikControl != yukseklik)
                        {
                            yukseklikControl--;
                            continue;
                        }
                        yukseklikControl--;
                        string adres = row["Adres"].ToString();
                        int tip = row["Tip"].ToInt();
                        string tipIsim = row["TipIsim"].ToString();
                        Guid id = new Guid(row["AdresId"].ToString());
                        Guid ulId = new Guid(row["RafId"].ToString());

                        string koy = row["Koy"].ToString();
                        int sira = row["Sira"].ToInt();
                        string raf = row["Raf"].ToString();
                        double oran = Convert.ToDouble(row["Oran"].ToString());

                        HucreModel hm = new HucreModel(_Kdm.KatId, _Kdm.DepoId, id, ulId, adres, koy, sira, yukseklik, raf, oran, tip, tipIsim);


                        lblHucre.Tag = hm;

                        lblHucre.SetLabelIntoGrid(j, i, _width, _height);
                        lblHucre.FontSize = 12;
                        if (tip == 100)
                        {
                            lblHucre.Background = Brushes.Transparent;
                            lblHucre.Content = "";
                        }
                        else if (tip == 255)
                        {
                            lblHucre.Background = Brushes.Black;
                            lblHucre.Content = "K";
                            lblHucre.Foreground = Brushes.White;
                        }
                        else
                        {
                            lblHucre.MouseDoubleClick += delegate (object sender, MouseButtonEventArgs e) { UIOperation.HucreClick(sender, e, ((HucreModel)lblHucre.Tag)); };
                            lblHucre.Background = UIOperation.GetScaleColor(oran);
                            lblHucre.Content = "%" + Math.Round(oran, 0).ToString();
                            if (oran >= 75)
                                lblHucre.Foreground = Brushes.White;
                            else
                                lblHucre.Foreground = Brushes.Black;

                            oranCounter++;
                            toplam += oran;
                        }

                        GridHucre.Children.Add(lblHucre);

                        counter++;
                    }
                    if (oranCounter > 0)
                        Or = (double)toplam / oranCounter;
                    else
                        Or = -1;
                    Label lblOran = new Label();
                    lblOran.SetLabelIntoGrid(0, i, _width, _height);
                    lblOran.FontSize = 13;
                    lblOran.FontWeight = FontWeights.Bold;

                    if (Or == -1)
                    {
                        lblOran.Content = " ";
                        lblOran.Background = Brushes.DarkGray;
                    }

                    else
                    {
                        lblOran.Content = "%" + Math.Round(Or, 0).ToString();
                        lblOran.Background = UIOperation.GetScaleColor(Or);

                    }
                    if (Or >=75)
                        lblOran.Foreground = Brushes.White;
                    else
                        lblOran.Foreground = Brushes.Black;

                    Grid.SetRow(lblOran, 0);
                    Grid.SetColumn(lblOran, i);
                    GridOran.Children.Add(lblOran);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateHucre\n" + ex.Message);
            }
        }

       

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                //      this.Close();
            }
        }

        private void CreateColorScale()
        {
            try
            {

                GridColor = UIOperation.CreateColorsGrid(MainGrid, 2, 0, 1, 1);
                Grid.SetColumn(GridColor, 0);
                Grid.SetRow(GridColor, 2);
                MainGrid.Children.Add(GridColor);

                if (_hucreColCount * _width < this.Width)
                {
                    GrBox.Width = this.Width;
                    GridHucre.HorizontalAlignment = HorizontalAlignment.Center;
                    GridOran.HorizontalAlignment = HorizontalAlignment.Center;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("CreateColorScale\n" + ex.Message);
            }

        }

        private List<int> SelectTips()
        {
            try
            {
                List<int> list = new List<int>();

                int tipVal = -1;
                List<int> listControl = new List<int>
                {
                    tipVal
                };
                foreach (DataRow row in _dt.Rows)
                {
                    int tip = row["Tip"].ToString().ToInt();
                    if (!listControl.Contains(tip) && tip != 100 && tip != 255)
                    {
                        list.Add(tip);
                        listControl.Add(tip);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {

                MessageBox.Show("SelectTips\n" + ex.Message);
                return null;
            }
        }

        private List<IEnumerable<DataRow>> ClassifyingTips(List<int> tipList)
        {
            try
            {

                List<IEnumerable<DataRow>> rows = new List<IEnumerable<DataRow>>();
                for (int i = 0; i < tipList.Count; i++)
                {
                    var datas = _dt.Select("Tip=" + tipList[i]);

                    if (datas != null)
                    {
                        rows.Add(datas);
                    }

                }

                return rows;
            }
            catch (Exception ex)
            {

                MessageBox.Show("ClassifyingTips\n" + ex.Message);
                return null;
            }
        }

        private void CreateTipGrid()
        {
            try
            {
                CreateTipGridRowsAndColumns(_rows.Count);
                Label lblOrtalama = new Label
                {
                    Background = Brushes.LightGray,
                     FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lblOrtalama.SetLabelIntoGrid("Doluluk Oranı", 0, 8, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lblOrtalama);

                Label lblToplam = new Label
                {
                    Background = Brushes.LightGray,
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                
                lblToplam.SetLabelIntoGrid("Kapasite", 0, 7, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lblToplam);

                Label lblRafBilgi = new Label() {
                    Background = Brushes.LightCyan,
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lblRafBilgi.SetLabelIntoGrid("RAF - "+_Kdm.Name+"   KAPASİTE KULLANIM RAPORU", 0, 1, _tipLabelWidth * 6, _tipLabelHeight);
                Grid.SetColumnSpan(lblRafBilgi, 6);
                GridTip.Children.Add(lblRafBilgi);
                for (int i = 0; i < _rows.Count; i++)
                {
                    DataRow[] rowArray = (DataRow[])_rows[i];
                    string tipIsim = rowArray[0]["TipIsim"].ToString();
                    CreateTipColorsLabels(rowArray, tipIsim, i);
                }

                CreateGenelToplamLabels(_rows.Count);
            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateTipGrid\n" + ex.Message);
            }


        }

        private void CreateTipGridRowsAndColumns(int rowCount)
        {
            try
            {
                MainGrid.GetColAndRowSize(out int colWidth, out int rowHeight, 0, 0);
                _tipLabelHeight = rowHeight / (rowCount + 2);
                _tipLabelWidth = colWidth / 9;
                GridTip.CreateGridRowsColoums(rowCount + 2, 9, _tipLabelWidth, _tipLabelHeight);
            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateTipGridRowsAndColumns\n" + ex.Message);
            }

        }

        private void CreateTipColorsLabels(DataRow[] rows, string tipIsim, int gridRowIndex)
        {
            try
            {
                Label lblTipIsim = new Label
                {
                    Background = Brushes.LightGray,
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lblTipIsim.SetLabelIntoGrid(tipIsim, gridRowIndex + 1, 0, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lblTipIsim);

                Label lblOrtalamaSonuc = new Label();
                lblOrtalamaSonuc.SetLabelIntoGrid(gridRowIndex + 1, 8, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lblOrtalamaSonuc);

                Label lbl_bos = new Label { Background = UIOperation.GetScaleColor(0) };
                Label lbl_asiriYuklu = new Label() { Background = UIOperation.GetScaleColor(110), Foreground = Brushes.White };
                Label lbl_0_25 = new Label() { Background = UIOperation.GetScaleColor(5) };
                Label lbl_25_50 = new Label() { Background = UIOperation.GetScaleColor(35) };
                Label lbl_50_75 = new Label() { Background = UIOperation.GetScaleColor(65) };
                Label lbl_75_100 = new Label() { Background = UIOperation.GetScaleColor(85) };

                Label lbl_toplam = new Label() {
                    Background = Brushes.DarkGray,
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                int bos = 0;
                int asiriYuklu = 0;
                int c0_25 = 0;
                int c25_50 = 0;
                int c50_75 = 0;
                int c75_100 = 0;
              

                int counter = 0;
                double toplam = 0;
                double ortalama = 0;
                for (int i = 0; i < rows.Length; i++)
                {
                    double oran = rows[i]["oran"].To<double>();
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

                    counter++;
                    toplam += oran;
                }

                _bos += bos;
                _asiriYuklu += asiriYuklu;
                _c0_25 += c0_25;
                _c25_50 += c25_50;
                _c50_75 += c50_75;
                _c75_100 += c75_100;
             

                ortalama = toplam / counter;
                _counter += counter;



                lblOrtalamaSonuc.Background = UIOperation.GetScaleColor(ortalama);
                lblOrtalamaSonuc.Content = "% " + Math.Round(ortalama, 0).ToString();
                lblOrtalamaSonuc.FontWeight = FontWeights.Bold;
                lblOrtalamaSonuc.FontSize = 20;

                lbl_toplam.SetLabelIntoGrid(counter.ToString(), gridRowIndex + 1, 7, _tipLabelWidth, _tipLabelHeight);
                lbl_toplam.FontWeight = FontWeights.Bold;
                lbl_toplam.FontSize = 20;
                GridTip.Children.Add(lbl_toplam);

                lbl_bos.SetLabelIntoGrid(bos.ToString(), gridRowIndex + 1, 1, _tipLabelWidth, _tipLabelHeight);
                lbl_bos.FontWeight = FontWeights.Bold;
                lbl_bos.FontSize = 20;
                GridTip.Children.Add(lbl_bos);

                lbl_asiriYuklu.SetLabelIntoGrid(asiriYuklu.ToString(), gridRowIndex + 1, 6, _tipLabelWidth, _tipLabelHeight);
                lbl_asiriYuklu.FontWeight = FontWeights.Bold;
                lbl_asiriYuklu.FontSize = 20;
                GridTip.Children.Add(lbl_asiriYuklu);

                lbl_0_25.SetLabelIntoGrid(c0_25.ToString(), gridRowIndex + 1, 2, _tipLabelWidth, _tipLabelHeight);
                lbl_0_25.FontWeight = FontWeights.Bold;
                lbl_0_25.FontSize = 20;
                GridTip.Children.Add(lbl_0_25);

                lbl_25_50.SetLabelIntoGrid(c25_50.ToString(), gridRowIndex + 1, 3, _tipLabelWidth, _tipLabelHeight);
                lbl_25_50.FontWeight = FontWeights.Bold;
                lbl_25_50.FontSize = 20;
                GridTip.Children.Add(lbl_25_50);

                lbl_50_75.SetLabelIntoGrid(c50_75.ToString(), gridRowIndex + 1, 4, _tipLabelWidth, _tipLabelHeight);
                lbl_50_75.FontWeight = FontWeights.Bold;
                lbl_50_75.FontSize = 20;
                GridTip.Children.Add(lbl_50_75);

                lbl_75_100.SetLabelIntoGrid(c75_100.ToString(), gridRowIndex + 1, 5, _tipLabelWidth, _tipLabelHeight);
                lbl_75_100.FontWeight = FontWeights.Bold;
                lbl_75_100.FontSize = 20;
                lbl_75_100.Foreground = Brushes.White;
                GridTip.Children.Add(lbl_75_100);

               

            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateTipColorsLabels\n" + ex.Message);
            }


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_Timer.Enabled)
            {
                _Timer.Dispose(); 
            }
        }

        private void CreateGenelToplamLabels(int rowCount)
        {
            try
            {
                Label lblGenelToplam = new Label
                {
                    Background = Brushes.DarkGray,
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lblGenelToplam.SetLabelIntoGrid("Genel Toplam", rowCount + 1, 0, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lblGenelToplam);

                Label lblToplam = new Label
                {
                    Background = Brushes.DarkGray,
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lblToplam.SetLabelIntoGrid(_counter.ToString(), rowCount + 1, 7, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lblToplam);

                Label lblGenelOrtalama = new Label {
                    Background = UIOperation.GetScaleColor(_genelOrtalama),
                     FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lblGenelOrtalama.SetLabelIntoGrid("% " + _genelOrtalama.ToString(), rowCount + 1, 8, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lblGenelOrtalama);

                Label lbl_bos = new Label { Background = UIOperation.GetScaleColor(0),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lbl_bos.SetLabelIntoGrid(_bos.ToString(), rowCount + 1, 1, _tipLabelWidth, _tipLabelWidth);
                GridTip.Children.Add(lbl_bos);

                Label lbl_asiriYuklu = new Label() { Background = UIOperation.GetScaleColor(110), Foreground = Brushes.White ,
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lbl_asiriYuklu.SetLabelIntoGrid(_asiriYuklu.ToString(), rowCount + 1, 6, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lbl_asiriYuklu);

                Label lbl_0_25 = new Label() { Background = UIOperation.GetScaleColor(5),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lbl_0_25.SetLabelIntoGrid(_c0_25.ToString(), rowCount + 1, 2, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lbl_0_25);

                Label lbl_25_50 = new Label() { Background = UIOperation.GetScaleColor(30),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lbl_25_50.SetLabelIntoGrid(_c25_50.ToString(), rowCount + 1, 3, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lbl_25_50);

                Label lbl_50_75 = new Label() { Background = UIOperation.GetScaleColor(65),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lbl_50_75.SetLabelIntoGrid(_c50_75.ToString(), rowCount + 1, 4, _tipLabelWidth, _tipLabelHeight);
                GridTip.Children.Add(lbl_50_75);

                Label lbl_75_100 = new Label() { Background = UIOperation.GetScaleColor(85),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20
                };
                lbl_75_100.SetLabelIntoGrid(_c75_100.ToString(), rowCount + 1, 5, _tipLabelWidth, _tipLabelHeight);
                lbl_75_100.Foreground = Brushes.White;
                GridTip.Children.Add(lbl_75_100);

            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateGenelToplamLabels\n" + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateColorScale();
            CreateTipGrid();
        }
    }
}
