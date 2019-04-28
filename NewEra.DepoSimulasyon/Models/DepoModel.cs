using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra.DepoSimulasyon.Models
{
    public class DepoModel:BaseAdresModel
    {
        public Byte[] Img { get; set; }


        public DepoModel(Byte[] img, string depoName, Guid depoId) : base(depoId, depoName)
        {
            Img = img;
            Name = depoName;
            Id = depoId;
        }

    }
}
