using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEra.DepoSimulasyon.Models
{
    public class KoyModel
    {
        public string KoyY { get; set; }
        public string KoyX { get; set; }
        public bool State { get; set; }

        public KoyModel(string koyX, string koyY, bool state)
        {
            KoyX = koyX;
            KoyY = koyY;
            State = state;
        }
    }
}
