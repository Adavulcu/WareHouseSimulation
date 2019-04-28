using NewEra.DepoSimulasyon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NewEra.DepoSimulasyon
{
   public class RefreshData
    {
        private Data _data;
        private KatModel _KatModel;
        private KatDetayModel _KatDetayModel;
        public RefreshData(KatModel km)
        {
            _data = new Data();
            _KatModel = km;
        }

        public RefreshData(KatDetayModel kdm)
        {
            _data = new Data();
            _KatDetayModel = kdm;
        }
       

        public List<KatDetayModel> RefreshMainDataList()
        {
            try
            {
                List<KatDetayModel> list = new List<KatDetayModel>();
                DataTable dt = _data.KatDetayGetir(_KatModel.Id, _KatModel.UlId);
                foreach (DataRow row in dt.Rows)
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
                    list.Add(new KatDetayModel(id, UlId, new Point(x, y), name, oran, sira, koy, _KatModel.Id, _KatModel.UlId));
                }
                return list;
            }
            catch (Exception ex)
            {

                throw new Exception("\nRefreshDataList\n" + ex.Message);
            }
        }

        public DataTable RefreshRafDetayData()
        {
            try
            {
                
                DataTable dt = _data.RafGenelBilgiGetir(_KatDetayModel);          
                return dt;
            }
            catch (Exception ex)
            {

                throw new Exception("\nRefreshDataList\n"+ex.Message);
            }
        }
    }
}
