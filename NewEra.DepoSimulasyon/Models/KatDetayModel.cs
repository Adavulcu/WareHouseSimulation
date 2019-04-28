using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NewEra.DepoSimulasyon.Models
{
    public class KatDetayModel:BaseAdresModel
    {
        public PointModel Points { get; set; }
        public double Oran { get; set; }
        public int Sira { get; set; }
        public string Koy { get; set; }
        public Guid KatId { get; set; }
        public Guid DepoId { get; set; }
        public SolidColorBrush Color { get; set; }
        public KatDetayModel(Guid id, Guid ulId, Point p, string name, double oran, int sira, string koy, Guid katId, Guid depoId) : base(id, ulId, name)
        {
            DepoId = depoId;
            KatId = katId;
            Koy = koy;
            Sira = sira;
            Oran = oran;
            UlId = ulId;
            Id = id;
            Name = name;
            Point p1 = new Point(p.X, p.Y);
            Point p2 = new Point(p.X + 1, p.Y);
            Point p3 = new Point(p.X + 1, p.Y + 1);
            Point p4 = new Point(p.X, p.Y + 1);
            Points = new PointModel(p1, p2, p3, p4);
            Color = UIOperation.GetScaleColor(Oran);
        }
    }
}
