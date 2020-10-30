using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODOT.ITS.CSS.BusinessLogic.BusinessEntities
{
    public class PublishResult
    {
        public int ThreadNumber { get; set; }
        public string DestinationName { get; set; }
        public EntityCamera Camera { get; set; }
    }
}
