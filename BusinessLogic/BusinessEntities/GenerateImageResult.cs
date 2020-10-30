using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODOT.ITS.CSS.BusinessLogic.BusinessEntities
{
    public class GenerateImageResult
    {
        public int ThreadNumber { get; set; }
        public string ImageName { get; set; }
        public Image PubImage { get; set; }
        public EntityCamera Camera { get; set; }
    }
}
