using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ODOT.ITS.CSS.BusinessLogic.BusinessEntities;

namespace ODOT.ITS.CSS.BusinessLogic.Processors
{
    /// <summary>
    /// PUBLISH PROCESSOR
    /// </summary>
    public class PublishProcessor
    {
        /// <summary>
        /// PUBLISH
        /// </summary>
        /// <param name="dest"></param>
        /// <returns></returns>
        public PublishResult Publish(EntityDest dest)
        {
            var task = Task.Run(() =>
            {
                Thread.Sleep(dest.DemoWaitMs);
            });
            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(dest.PublishTimeoutMs));
            if (isCompletedSuccessfully)
            {
                //return task.Result;
                //DEBUG
                return new PublishResult
                {
                    DestinationName = "published " + dest.Name,
                    Camera = dest.Camera,
                    ThreadNumber = Thread.CurrentThread.ManagedThreadId
                };
            }
            else
            {
                throw new TimeoutException("Publish Timeout: " + dest.Name + " (thread " + Thread.CurrentThread.ManagedThreadId.ToString() + ")");
            }
        }

    }
}
