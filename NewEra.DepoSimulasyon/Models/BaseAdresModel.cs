using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra.DepoSimulasyon.Models
{
    public class BaseAdresModel
    {
        public BaseAdresModel(Guid id, Guid ulId, string name)
        {
            Id = id;
            UlId = ulId;
            Name = name;
        }
        public BaseAdresModel(Guid id, string name)
        {
            Id = id;

            Name = name;
        }
        public BaseAdresModel()
        {

        }
        public string Name { get; set; }
        public Guid Id { get; set; }
        public Guid UlId { get; set; }
    }
}
