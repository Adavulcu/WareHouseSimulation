using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra.DepoSimulasyon.Models
{
    public class HucreModel:BaseAdresModel
    {
        public int Tip { get; set; }
        public string TipIsim { get; set; }
        public int Yukseklik { get; set; }
        public string Raf { get; set; }
        public string Koy { get; set; }
        public double Oran { get; set; }
        public int Sira { get; set; }
        public int PaletMiktar { get; set; }
        public List<UrunModel> Urun { get; set; }
        public Guid DepoId { get; set; }
        public Guid KatId { get; set; }

        public HucreModel()
        {

        }

        public HucreModel(Guid katId, Guid depoId, Guid id, Guid rafId, string adres, string koy, int sira, int yukseklik, string raf, double oran, int tip, string tipIsim) : base(id, rafId, adres)
        {
            KatId = katId;
            DepoId = depoId;
            Id = id;
            Name = adres;
            Koy = koy;
            Raf = raf;
            Oran = oran;
            Sira = sira;
            Tip = tip;
            TipIsim = tipIsim;
            Yukseklik = yukseklik;
            UlId = rafId;
        }

        public HucreModel(Guid id, string adres, int tip, string tipIsim, List<UrunModel> urun) : base(id, adres)
        {
            Id = id;
            Name = adres;
            Tip = tip;
            TipIsim = tipIsim;
            Urun = urun;
        }
    }
}
