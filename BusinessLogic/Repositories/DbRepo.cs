using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ODOT.ITS.CSS.BusinessLogic.BusinessEntities;

namespace ODOT.ITS.CSS.BusinessLogic.Repositories
{
    public class DbRepo
    {
        /// <summary>
        /// LIST SERVERS BY MINUTE
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        public static List<EntityServer> ListServersByMinute(int minute)
        {
            var result = new List<EntityServer>();

            //demo http server
            var items = new List<EntityCamera>();
            for (int i = 10; i <= 20; i += 10)
            {
                items.Add(new EntityCamera
                {
                    Name = "HttpCamera" + i.ToString(),
                    DemoWaitMs = i * 1000,
                    GatherTimeoutMs = 13000
                });
            }
            result.Add(new EntityServer
            {
                Name = "Http Server",
                RefreshFrequency = 1,
                ServerType = "HTTP",
                Cameras = items
            });

            //demo ftp server with 10 sec wait
            var items2 = new List<EntityCamera>();
            items2.Add(new EntityCamera
            {
                Name = "FtpACamera",
                DemoWaitMs = 10000,
                GatherTimeoutMs = 13000
            });
            result.Add(new EntityServer
            {
                Name = "Ftp-ServerA",
                RefreshFrequency = 1,
                ServerType = "FTP",
                FtpDemoWaitMs = 1000,
                FtpGatherTimeoutMs = 13000,
                Cameras = items2
            });

            //demo ftp server with 20 sec wait but only 13 sec timeout
            var items3 = new List<EntityCamera>();
            for (int i = 10; i <= 20; i += 10)
            {
                items3.Add(new EntityCamera
                {
                    Name = "Ftp20-Camera" + i.ToString(),
                    DemoWaitMs = i * 1000,
                    GatherTimeoutMs = 13000
                });
            }
            result.Add(new EntityServer
            {
                Name = "Ftp-ServerB",
                RefreshFrequency = 1,
                ServerType = "FTP",
                FtpDemoWaitMs = 20000,
                FtpGatherTimeoutMs = 13000,
                Cameras = items3
            });

            return result;
        }

        /// <summary>
        /// LIST PUB IMAGES
        /// </summary>
        /// <param name="gatherResult"></param>
        /// <returns></returns>
        public static List<EntityPubImage> ListPubImages(GatherResult gatherResult)
        {
            var items = new List<EntityPubImage>();
            for (int i = 10; i <= 20; i += 10)
            {
                items.Add(new EntityPubImage
                {
                    Name = gatherResult.Camera.Name + "-PubImage" + i.ToString(),
                    DemoWaitMs = i * 1000,
                    GenerateImageTimeoutMs = 13000,
                    Camera = gatherResult.Camera
                });
            }
            return items;
        }

        /// <summary>
        /// LIST DESTINATIONS
        /// </summary>
        /// <param name="imageResult"></param>
        /// <returns></returns>
        public static List<EntityDest> ListDestinations(GenerateImageResult imageResult)
        {
            var items = new List<EntityDest>();
            for (int i = 10; i <= 20; i += 10)
            {
                items.Add(new EntityDest
                {
                    Name = imageResult.ImageName + "-dest" + i.ToString(),
                    DemoWaitMs = i * 1000,
                    PublishTimeoutMs = 13000,
                    Camera = imageResult.Camera
                });
            }
            return items;
        }


    }
}
