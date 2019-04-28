using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra.DepoSimulasyon.Models
{
   public class UrunModel:BaseAdresModel
    {
        public string Kod { get; set; }
        public int Miktar { get; set; }

        public UrunModel()
        { }

        public UrunModel(Guid id, string name, string kod, int miktar) : base(id, name)
        {
            Id = id;
            Name = name;
            Kod = kod;
            Miktar = miktar;
        }
    }
}
