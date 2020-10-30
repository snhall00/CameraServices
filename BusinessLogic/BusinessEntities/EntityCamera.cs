using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODOT.ITS.CSS.BusinessLogic.BusinessEntities
{
    public class EntityCamera
    {
        public string Name { get; set; }
        public int DemoWaitMs { get; set; }
        public int GatherTimeoutMs { get; set; }
    }
}
