using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra.DepoSimulasyon.Models
{
    public class KatModel:BaseAdresModel
    {
        public DataTable KatData { get; set; }
        public double KatOran { get; set; }
        public KatModel()
        {

        }

        public KatModel(Guid ulId, string katName, Guid katId, double katOran, DataTable dt) : base(katId, ulId, katName)
        {
            KatData = dt;
            KatOran = katOran;
            UlId = ulId;
            Name = katName;
            Id = katId;
        }
    }
}
