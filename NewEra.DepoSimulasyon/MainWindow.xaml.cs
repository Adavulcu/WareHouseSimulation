using NewEra.DepoSimulasyon.Models;
using NewEra.DepoSimulasyon.ShapeControls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace NewEra.DepoSimulasyon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int _PK = 10;//point kat sayısı
        Data _data;
        Grid _gridKat;
        Canvas _canvas;
        Grid _gridColors;
        Grid _rafOranGrid;
        List<RafOran> _ro;
        DataTable _DtDepo;
        DataTable _DtKat;
        DataTable _DtDepoToplamPalet;
        List<string> _raflar;
        double _DepoOrtalama;
        KatModel _CurrentKatModel;
        List<KatDetayModel> _CurrentDataList;
        Timer _Timer;
       
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            Init();

        }

     

        private void Init()
        {
            try
            {
                _gridKat = new Grid();
                _data = new Data();
                _gridColors = new Grid();
                _rafOranGrid = new Grid();
                MyGrid.Background = Brushes.LightGray;
                
                _canvas = new Canvas();




            }
            catch (Exception ex)
            {

                MessageBox.Show("Init\n" + ex.Message);
            }
        }

   

        private async void StartRefreshData(object o, ElapsedEventArgs a)
        {
                
                await Task.Run(() => RefreshDataAsync());
                
           
        }
        /// <summary>
        /// Canvas içerisindeki polygonları güncelleyen metot
        /// </summary>
        /// <param name="canvas"></param>
        private void RefrashCanvas(Canvas canvas)
        {
            try
            {
                int i = 0;
                foreach (object poly in canvas.Children)
                {
                    if (poly is Polygon)
                    {
                        KatDetayModel raf = _CurrentDataList[i];
                        string ttString = "";
                        ttString = "Raf = " + raf.Name + "\nKoy = " + raf.Koy + "\nSıra = " + raf.Sira + "\nDoluluk Oranı = % " + Math.Round(raf.Oran, 1) + "";
                        ((Polygon)poly).Tag = raf;
                        ((Polygon)poly).Fill = raf.Color;
                        ((Polygon)poly).ToolTip = ttString;
                        i++;
                    }
                  
                }
            }
            catch (Exception ex) 
            {
                 
                MessageBox.Show("RefrashCanvas\n"+ex.Message);
            }
        }

        private Task<Boolean> RefreshDataAsync()
        {
            return Task.Run<Boolean>(() =>
            {
            this.Dispatcher.Invoke((Action)(() => {

             
                    this.Cursor = Cursors.Wait;
                    RefreshData rfData = new RefreshData(_CurrentKatModel);
                    List<KatDetayModel> list = rfData.RefreshMainDataList();
                    _canvas.Tag = list;
                    _CurrentDataList = list;
                    RefrashCanvas(_canvas);
                    SetDepoLblValues(_CurrentKatModel.UlId, _CurrentKatModel.Id);
                    SetKatLblValues(_CurrentKatModel);
                    RefreshRafOranGrid();
                    this.Cursor = Cursors.Arrow; 
                
            }));
             
                return true;
            }
           );
        }

        private void RefreshRafOranGrid()
        {
            try
            {
                List<RafOran> newRafOranlist = GetRafOran(_CurrentKatModel.KatData,_CurrentDataList);
                int i = 0;
                foreach (Object item in _rafOranGrid.Children)
                {
                    if (item is Label)
                    {
                        string lblName = newRafOranlist[i].RafName + " = % " + newRafOranlist[i].Oran;
                        ((Label)item).Content = lblName;
                        if (newRafOranlist[i].Oran >= 75)
                        {
                           ((Label)item).Foreground = Brushes.White;
                        }
                        else
                            ((Label)item).Foreground = Brushes.Black;
                        ((Label)item).Background = UIOperation.GetScaleColor(newRafOranlist[i].Oran);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("RefreshRafOranGrid\n"+ex.Message);
            }
        }
        
        private void CbDepo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox cbİtem = (ComboBox)sender;
                DepoModel dm = (DepoModel)cbİtem.SelectedItem;
                KatDoldur(dm.Id);
                CreateKatDetay(_CurrentKatModel, _DepoOrtalama);
                _Timer = new Timer(10000);
                _Timer.Elapsed += new ElapsedEventHandler(StartRefreshData);
                _Timer.Start();
            }
            catch (Exception ex)
            {

                MessageBox.Show("CbDepo_SelectionChanged\n" + ex.Message);
            }
        }

        private void FillComboBoxex(ComboBox cb)
        {
            try
            {
                switch (cb.Name)
                {
                    case "CbDepo":
                        _DtDepo = _data.DepoGetir();
                        List<DepoModel> listDepo = new List<DepoModel>();
                        foreach (DataRow row in _DtDepo.Rows)
                        {
                            Guid depoId = new Guid(row["DepoId"].ToString());
                            byte[] arr = (byte[])row["Img"];
                            string name = row["DepoIsim"].ToString();
                            listDepo.Add(new DepoModel(arr, name, depoId));
                        }
                        CbDepo.DisplayMemberPath = "Name";
                        CbDepo.SelectedValuePath = "Id";
                        CbDepo.ItemsSource = listDepo;
                        CbDepo.SelectedIndex = 0;
                        break;
                    case "CbKat":
                        DepoModel dm = (DepoModel)CbDepo.SelectedItem;
                        KatDoldur(dm.Id);
                        break;
                    default:
                        break;
                }

                
            }
            catch (Exception ex)
            {

                MessageBox.Show("FillComboBoxex\n" + ex.Message);
            }
        }

        private void CbKat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox cb = (ComboBox)sender;
                KatModel km = (KatModel)cb.SelectedItem;
                CreateKatDetay(km, _DepoOrtalama);
            }
            catch (Exception ex)
            {

                MessageBox.Show("CbKat_SelectionChanged\n"+ex.Message);
            }
        }

        private void RemoveControls()
        {
            CbKat.SelectionChanged -= CbKat_SelectionChanged;
            CbKat.ItemsSource = null;
            MyGrid.Children.Remove(_canvas);
            MyGrid.Children.Remove(_rafOranGrid);
            _rafOranGrid.Visibility=Visibility.Hidden;

            string ttString = "0";
            string ttStringKatoran = "0";

            ToolTip tt = new ToolTip()
            {

                Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse,

            };

            lblDepoDolulukOran.Content = "% 0" ;
            tt.Content = ttString;
            lblDepoDolulukOran.ToolTip = tt;
            tt.Content = ttStringKatoran;
            lblKatDolulukOran.ToolTip = tt;
            lblKatDolulukOran.Content = "% 0";

            lblKatBosDolu.Content = "0 / 0";
            lblDepoBosDolu.Content = "0 / 0";

            lblDepoDolulukOran.Background = UIOperation.GetScaleColor(0);
            lblKatDolulukOran.Background = UIOperation.GetScaleColor(0);


        }

        private void SetDepoLblValues(Guid depoID,Guid katID)
        {
            try
            {
                double toplam = 0;
                _DepoOrtalama = 0;
                _DtDepoToplamPalet = _data.DepoDoluBosBilgiGetir(depoID);
                int depoDoluPalet = 0;
                if (_DtDepoToplamPalet.Rows.Count > 0)
                {
                    foreach (DataRow row in _DtDepoToplamPalet.Rows)
                    {
                        double oran = row["oran"].To<double>();
                        if (oran > 0)
                        {
                            depoDoluPalet++;
                        }
                    }
                    lblDepoBosDolu.Content = ("" + depoDoluPalet + " / " + _DtDepoToplamPalet.Rows.Count).ToString();
                }

                DataTable katDt = _data.KatDetayGetir(katID,depoID);
               
                double katOran = katDt.AsEnumerable()
                             .Sum(r => r["oran"].ToString().To<double>());

                if (katOran > 0)
                {
                    toplam += (katOran) / katDt.Rows.Count;

                }
                
                ToolTip tt = new ToolTip()
                {

                    Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse,

                };
                _DepoOrtalama = (toplam) / _DtKat.Rows.Count;
                string ttString = Math.Round(_DepoOrtalama, 3).ToString();
                _DepoOrtalama = Math.Round(_DepoOrtalama, 0);
                 lblDepoDolulukOran.Content = "% " + _DepoOrtalama.ToString();
                
                tt.Content = ttString;
                lblDepoDolulukOran.ToolTip = tt;
                lblDepoDolulukOran.Background = UIOperation.GetScaleColor(_DepoOrtalama);
                KatModel newKm = new KatModel(depoID, _CurrentKatModel.Name, katID, katOran/katDt.Rows.Count, katDt);
                _CurrentKatModel = newKm;
            }
            catch (Exception ex)
            {

                MessageBox.Show("SetDepoLblValues\n"+ex.Message);
            }
        }

        private void SetKatLblValues(KatModel km)
        {
            try
            {
                int KatToplamPalet = 0;
                int katToplamDoluPalet = 0;
                double katOrtalama = Math.Round(km.KatOran, 0);
                foreach (DataRow row in _DtDepoToplamPalet.Rows)
                {
                    Guid KatId = new Guid(row["DepoKatID"].ToString());
                    if (KatId.Equals(km.Id))
                    {
                        double oran = row["oran"].To<double>();
                        if (oran > 0)
                        {
                            katToplamDoluPalet++;
                        }
                        KatToplamPalet++;
                    }
                }
                lblKatBosDolu.Content = ("" + katToplamDoluPalet + " / " + KatToplamPalet).ToString();
                ToolTip tt = new ToolTip()
                {

                    Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse,

                };
                string ttStringKatoran = Math.Round(km.KatOran, 3).ToString();
                tt.Content = ttStringKatoran;
                lblKatDolulukOran.ToolTip = tt;
                lblKatDolulukOran.Content = "% " + katOrtalama.ToString();

               
                lblKatDolulukOran.Background = UIOperation.GetScaleColor(katOrtalama);
            }
            catch (Exception ex)
            {

                MessageBox.Show("SetKatLblValues\n"+ex.Message);
            }
        }

        private void KatDoldur(Guid depoId)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() => 
                {
                    _DtKat = _data.KatGetir(depoId);

                    double toplam = 0;
                    _DepoOrtalama = 0;

                    if (_DtKat.Rows.Count > 0)
                    {
                        //_DtDepoToplamPalet = _data.DepoDoluBosBilgiGetir(depoId);
                        //int depoDoluPalet = 0;
                        //if (_DtDepoToplamPalet.Rows.Count > 0)
                        //{
                        //    foreach (DataRow row in _DtDepoToplamPalet.Rows)
                        //    {
                        //        double oran = row["oran"].To<double>();
                        //        if (oran > 0)
                        //        {
                        //            depoDoluPalet++;
                        //        }
                        //    }
                        //    lblDepoBosDolu.Content = ("" + depoDoluPalet + " / " + _DtDepoToplamPalet.Rows.Count).ToString();
                        //}
                       
                        List<KatModel> listKat = new List<KatModel>();
                        foreach (DataRow row in _DtKat.Rows)
                        {
                            Guid ulId = new Guid(row["UlId"].ToString());
                            Guid id = new Guid(row["Id"].ToString());
                            string isim = row["Isim"].ToString();
                            DataTable katDt = _data.KatDetayGetir(id, ulId);

                            if (katDt.Rows.Count == 0)
                            {
                                RemoveControls();
                                MessageBox.Show("BU DEPOYA AİT FİZİKSEL ADRES TANIMLAMASI YOKTUR");
                                return;
                            }

                            double katOran = katDt.AsEnumerable()
                                .Sum(r => r["oran"].ToString().To<double>());

                            if (katOran > 0)
                            {
                                toplam += (katOran) / katDt.Rows.Count;

                            }
                            listKat.Add(new KatModel(ulId, isim, id, (katOran) / katDt.Rows.Count, katDt));
                        }
                        _CurrentKatModel = listKat[0];
                        SetDepoLblValues(depoId, _CurrentKatModel.Id);
                        //_DepoOrtalama = (toplam) / _DtKat.Rows.Count;
                        //_DepoOrtalama = Math.Round(_DepoOrtalama, 0);
                        CbKat.DisplayMemberPath = "Name";
                        CbKat.SelectedValuePath = "Id";
                        CbKat.SelectedIndex = 0;
                        CbKat.ItemsSource = listKat;
                        CbKat.SelectionChanged += CbKat_SelectionChanged;
                        
                       
                    }
                    else
                    {
                        RemoveControls();
                        MessageBox.Show("BU DEPO İÇİN TANIMLI BİR KAT YOKTUR");
                    }
                }));
               
                    


            }
            catch (Exception ex)
            {

                MessageBox.Show("KatDoldur\n" + ex.Message);
            }

        }


        private void CreateKatDetay(KatModel km, double DepoOrtalama)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(()=>{
                    //int KatToplamPalet = 0;
                    //int katToplamDoluPalet = 0;
                    //foreach (DataRow row in _DtDepoToplamPalet.Rows)
                    //{
                    //    Guid KatId = new Guid(row["DepoKatID"].ToString());
                    //    if (KatId.Equals(km.Id))
                    //    {
                    //        double oran = row["oran"].To<double>();
                    //        if (oran > 0)
                    //        {
                    //            katToplamDoluPalet++;
                    //        }
                    //        KatToplamPalet++;
                    //    }
                    //}
                    //lblKatBosDolu.Content = ("" + katToplamDoluPalet + " / " + KatToplamPalet).ToString();
                    _gridColors = UIOperation.CreateColorsGrid(GridColor, 0, 1);
                    Grid.SetRow(_gridColors, 0);
                    Grid.SetColumn(_gridColors, 1);
                    GridColor.Children.Remove(_gridColors);
                    GridColor.Children.Add(_gridColors);

                    // DataTable dt = _data.KatDetayGetir(new Guid());

                    _CurrentDataList = new List<KatDetayModel>();
                    MyGrid.GetColAndRowSize(out int b, out int a, 0, 1);

                    DataTable dt = _data.KatBoyutGetir(km.Id);
                    int katx = 0, katy = 0;
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        katx = row["X"].ToString().ToInt();
                        katy = row["Y"].ToString().ToInt();
                    }


                    //  MessageBox.Show("height="+ a.ToString()+"\n width="+b.ToString());
                    KatDetayShape kds = new KatDetayShape(b, a, katx, katy);

                    foreach (DataRow row in km.KatData.Rows)
                    {
                        Guid id = new Guid(row["AdresId"].ToString());
                        Guid UlId = new Guid(row["RafID"].ToString());
                        string name = row["Raf"].ToString();
                        int x = row["KX"].ToInt();
                        int y = row["KY"].ToInt();
                        string koy = row["Koy"].ToString();
                        double oran = row["oran"].To<double>();
                        //if (oran > 0)
                        //{
                        //    toplam += (1 / oran) * 100;
                        //}

                        int sira = row["Sira"].ToInt();
                        _CurrentDataList.Add(new KatDetayModel(id, UlId, new Point(x, y), name, oran, sira, koy, km.Id, km.UlId));
                    }
                    SetKatLblValues(km);
                    CreateRafOranGrid(GetRafOran(km.KatData, _CurrentDataList));
                    //   ortalama = (100 * toplam) / dt.Rows.Count;
                  //  string ttString = Math.Round(DepoOrtalama, 3).ToString();
                 //   string ttStringKatoran = Math.Round(km.KatOran, 3).ToString();

                    //ToolTip tt = new ToolTip()
                    //{

                    //    Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse,

                    //};

                    //DepoOrtalama = Math.Round(DepoOrtalama, 0);
                    //double katOrtalama = Math.Round(km.KatOran, 0);
                    //lblDepoDolulukOran.Content = "% " + DepoOrtalama.ToString();
                    //tt.Content = ttString;
                    //lblDepoDolulukOran.ToolTip = tt;
                    //tt.Content = ttStringKatoran;
                    //lblKatDolulukOran.ToolTip = tt;
                    //lblKatDolulukOran.Content = "% " + katOrtalama.ToString();

                    //lblDepoDolulukOran.Background = UIOperation.GetScaleColor(DepoOrtalama);
                    //lblKatDolulukOran.Background = UIOperation.GetScaleColor(katOrtalama);


                    _canvas.Tag = _CurrentDataList;
                    _canvas = kds.DrawCanvas((List<KatDetayModel>)_canvas.Tag);


                    _canvas.MouseLeftButtonDown += CanvasClick;
                    _canvas.MouseMove += CanvasOver;
                    _canvas.MouseRightButtonDown += CanvasRightClick;


                    Grid.SetRow(_canvas, 1);
                    Grid.SetColumn(_canvas, 0);
                    _canvas.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#ccccff");
                    MyGrid.Children.Add(_canvas);
                }));

              

            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateKatDetay\n" + ex.Message);
            }
        }

        private void CanvasRightClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (e.Source is FrameworkElement o && o is Polygon)
                {
                    Polygon p = (Polygon)o;
                    KatDetayModel km = (KatDetayModel)p.Tag;
                    WindowRafGenelDurum wrgd = new WindowRafGenelDurum();

                    RafOran oran = new RafOran();
                    for (int i = 0; i < _ro.Count; i++)
                    {
                        if (_ro[i].RafName == km.Name)
                        {
                            oran = _ro[i];
                            break;
                        }
                    }
                    this.Cursor = Cursors.Wait;
                    wrgd.Init(km, oran.Oran);
                    Cursor = Cursors.AppStarting;
                    _Timer.Stop();
                    wrgd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    wrgd.ShowDialog();
                    _Timer.Start();
                   
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("CanvasRightClick\n" + ex.Message);
            }
        }

        private void CanvasOver(object sender, MouseEventArgs e)
        {

            if (e.Source is FrameworkElement o && o is Polygon)
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void CreateRafOranGrid(List<RafOran> list)
        {
            try
            {
                int colCount = 0;
                _rafOranGrid = new Grid();
                if (list.Count > 0)
                {
                    if (list.Count % 2 == 0)
                    {
                        colCount = list.Count / 2;
                    }
                    else
                        colCount = (list.Count + 1) / 2;
                    UIOperation.GetColAndRowSize(GridRafOran, out int width, out int height, 3, 0);
                    _rafOranGrid.CreateGridRowsColoums(2, colCount, width / colCount, (height * 4) / 2);
                    int rowIndex = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list.Count % 2 == 0)
                        {
                            if (i > 0 && i % (list.Count / 2) == 0)
                                rowIndex++;
                        }
                        else
                        {
                            if (i > 0 && i % ((list.Count + 1) / 2) == 0)
                                rowIndex++;
                        }

                        Label lbl = new Label();
                        string lblName = list[i].RafName + " = % " + list[i].Oran;
                        int colIndex = i % (list.Count / 2);
                        lbl.SetLabelIntoGrid(lblName, rowIndex, colIndex, width / colCount, (height * 4) / 2);
                        if (list[i].Oran >=75)
                        {
                            lbl.Foreground = Brushes.White;
                        }
                        else
                            lbl.Foreground = Brushes.Black;
                        lbl.Background = UIOperation.GetScaleColor(list[i].Oran);
                        lbl.FontWeight = FontWeights.Bold;
                        lbl.FontSize = 16;
                        _rafOranGrid.Children.Add(lbl);

                    }

                    _rafOranGrid.Background = Brushes.Black;
                    Grid.SetRow(_rafOranGrid, 0);
                    Grid.SetRowSpan(_rafOranGrid, 3);
                    Grid.SetColumn(_rafOranGrid, 3);

                    try
                    {
                        GridRafOran.Children.Remove(_rafOranGrid);
                    }
                    catch (Exception)
                    {


                    }
                    GridRafOran.Children.Add(_rafOranGrid);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("CreateRafOranGrid\n" + ex.Message);
            }
        }

        private void CanvasClick(object sender, MouseButtonEventArgs e)
        {

            if (e.Source is FrameworkElement o && o is Polygon)
            {
                Polygon p = (Polygon)o;
                //  MessageBox.Show(((KatDetayModel)p.Tag).Name.ToString());
                KatDetayModel kdm = (KatDetayModel)p.Tag;

                List<HucreModel> list = new List<HucreModel>();
                DataTable dt = _data.RafSıraDetayGetir(kdm);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {

                        string adres = row["Adres"].ToString();
                        int tip = row["Tip"].ToInt();
                        string tipIsim = row["TipIsim"].ToString();
                        Guid id = new Guid(row["AdresId"].ToString());
                        Guid ulId = new Guid(row["RafId"].ToString());
                        int yukseklik = row["Yukseklik"].ToInt();
                        string koy = row["Koy"].ToString();
                        int sira = row["Sira"].ToInt();
                        string raf = row["Raf"].ToString();
                        double oran = Convert.ToDouble(row["Oran"].ToString());
                        list.Add(new HucreModel(kdm.KatId, kdm.DepoId, id, ulId, adres, koy, sira, yukseklik, raf, oran, tip, tipIsim));
                    }
                    
                    Window w = UIOperation.CreateRafSıra(list, kdm.Oran);
                    w.WindowStyle = WindowStyle.ToolWindow;
                    Point mousePoint = Mouse.GetPosition(this);
                    w.Topmost = true;
                    w.KeyDown += Windows_KeyDown;
                    double wWidth = w.Width;
                    double wHeigt = w.Height;
                    double x = mousePoint.X;
                    double y = mousePoint.Y;
                    if (x > this.Width - wWidth)
                    {
                        x = this.Width - wWidth;
                    }
                    if (y > this.Height - wHeigt)
                    {
                        y = this.Height - wHeigt;
                    }
                    w.Left = x;
                    w.Top = y;
                    w.Show();
                    
                }


            }
            Canvas c = (Canvas)sender;

        }

        private void Windows_KeyDown(object sender, KeyEventArgs e)
        {
            Window w = (Window)sender;
            if (e.Key == Key.Escape)
            {
                w.Close();
            }
        }



        private List<RafOran> GetRafOran(DataTable dt, List<KatDetayModel> kdm)
        {
            string rafName = "xxxxxxx";
            _ro = new List<RafOran>();
            _raflar = new List<string>();
        
            foreach (DataRow row in dt.Rows)
            {
                string raf = row["Raf"].ToString();
                if (!rafName.Equals(raf))
                {
                    _raflar.Add(raf);
                    rafName = raf;
                }
            }

            for (int i = 0; i < _raflar.Count; i++)
            {
                int counter = 0;
                double toplam = 0;
                double ortalama = 0;

                for (int j = 0; j < kdm.Count; j++)
                {
                    if (_raflar[i].Equals(kdm[j].Name))
                    {
                        if (kdm[j].Oran > 0)
                        {
                            // toplam += (1 / kdm[j].Oran) * 100;
                            
                            toplam += kdm[j].Oran;
                           
                        }
                     
                        counter++;
                    }
                }
                ortalama = Math.Round((toplam) / counter, 0);
              
                _ro.Add(new RafOran() { RafName = _raflar[i], Oran = ortalama });
            }
            return _ro;
        }
        class RafOran
        {
            public string RafName { get; set; }
            public double Oran { get; set; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBoxex(CbDepo);
        }
    }
}
