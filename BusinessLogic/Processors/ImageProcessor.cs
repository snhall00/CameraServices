using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ODOT.ITS.CSS.BusinessLogic.BusinessEntities;

namespace ODOT.ITS.CSS.BusinessLogic.Processors
{
    public class ImageProcessor
    {
        /// <summary>
        /// GENERATE PUB IMAGE
        /// </summary>
        /// <param name="pubImage"></param>
        /// <returns></returns>
        public GenerateImageResult GeneratePubImage(EntityPubImage pubImage)
        {
            var task = Task.Run(() =>
            {
                Thread.Sleep(pubImage.DemoWaitMs);
            });
            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(pubImage.GenerateImageTimeoutMs));
            if (isCompletedSuccessfully)
            {
                //return task.Result;
                //DEBUG
                return new GenerateImageResult
                {
                    ImageName = pubImage.Name,
                    Camera = pubImage.Camera,
                    ThreadNumber = Thread.CurrentThread.ManagedThreadId
                };
            }
            else
            {
                throw new TimeoutException("GeneratePubImage Timeout: " + pubImage.Name + " (thread " + Thread.CurrentThread.ManagedThreadId.ToString() + ")");
            }
        }

    }
}
