using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODOT.ITS.CSS.BusinessLogic.BusinessEntities
{
    public class GatherResult
    {
        public string ServerName { get; set; }
        public int ThreadNumber { get; set; }
        public string RawImageName { get; set; }
        public Image RawImage { get; set; }
        public EntityCamera Camera { get; set; }
    }
}
