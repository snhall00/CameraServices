using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ODOT.ITS.CSS.BusinessLogic.BusinessEntities;

namespace ODOT.ITS.CSS.BusinessLogic.Processors
{
    public class GatherProcessor
    {
        /// <summary>
        /// PERFORM GATHER
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public GatherResult PerformGather(EntityCamera camera)
        {
            var task = Task.Run(() =>
            {
                Thread.Sleep(camera.DemoWaitMs);
            });
            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(camera.GatherTimeoutMs));
            if (isCompletedSuccessfully)
            {
                //return task.Result;
                return new GatherResult { RawImageName = camera.Name, Camera = camera, ThreadNumber = Thread.CurrentThread.ManagedThreadId };
            }
            else
            {
                throw new TimeoutException("PerformGather Timeout: " + camera.Name + " (thread " + Thread.CurrentThread.ManagedThreadId.ToString() + ")");
            }
        }

        /// <summary>
        /// PERFORM FTP GATHER
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public List<GatherResult> PerformFtpGather(EntityServer server)
        {
            var task = Task.Run(() =>
            {
                Thread.Sleep(server.FtpDemoWaitMs);
            });
            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(server.FtpGatherTimeoutMs));
            if (isCompletedSuccessfully)
            {
                //return task.Result;
                //DEBUG
                var result = new List<GatherResult>();
                foreach (var camera in server.Cameras)
                {
                    result.Add(new GatherResult
                    {
                        ServerName = server.Name,
                        RawImageName = camera.Name,
                        Camera = camera,
                        ThreadNumber = Thread.CurrentThread.ManagedThreadId
                    });
                }
                return result;
            }
            else
            {
                throw new TimeoutException("PerformFtpGather Timeout: " + server.Name + " (thread " + Thread.CurrentThread.ManagedThreadId.ToString() + ")");
            }
        }

    }
}
