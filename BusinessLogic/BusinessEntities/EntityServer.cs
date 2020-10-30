using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODOT.ITS.CSS.BusinessLogic.BusinessEntities
{
    public class EntityServer
    {
        public string Name { get; set; }
        public int RefreshFrequency { get; set; }
        public string ServerType { get; set; }
        public List<EntityCamera> Cameras { get; set; }
        public int FtpDemoWaitMs { get; set; }
        public int FtpGatherTimeoutMs { get; set; }


        public EntityServer()
        {
            Cameras = new List<EntityCamera>();
        }
    }
}
