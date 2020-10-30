using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ODOT.ITS.CSS.BusinessLogic.BusinessEntities;
using ODOT.ITS.CSS.BusinessLogic.Logging;
using ODOT.ITS.CSS.BusinessLogic.Repositories;

namespace ODOT.ITS.CSS.BusinessLogic.Processors
{
    /// <summary>
    /// TASK PROCESSOR
    /// Task methods:
    /// -GenerateTopLevelTasks
    /// -GenerateImageTasks
    /// -GeneratePublishTasks
    /// </summary>
    public class TaskProcessor
    {
        /// <summary>
        /// GENERATE TOP LEVEL TASKS
        /// Combination of direct copy camera tasks, http camera tasks, and ftp server tasks.
        /// </summary>
        /// <param name="servers"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<Task> GenerateTopLevelTasks(List<EntityServer> servers, CancellationToken token)
        {
            var processor = new GatherProcessor();
            var items = new List<Task>();
            foreach (var server in servers)
            {
                if (server.ServerType == "FTP")
                {
                    items.Add(Task<List<GatherResult>>.Factory.StartNew(()
                        => processor.PerformFtpGather(server), token, TaskCreationOptions.None, TaskScheduler.Default)
                        .ContinueWith(taskInfo =>
                        {
                            try
                            {
                                token.ThrowIfCancellationRequested();
                                var gatherResults = (List<GatherResult>)taskInfo.Result;
                                Logit.LogGatherResults(gatherResults);
                                List<Task> imageTasks = new List<Task>();
                                foreach (var gatherResult in gatherResults)
                                {
                                    imageTasks.AddRange(GenerateImageTasks(gatherResult, token));
                                }
                                Parallel.ForEach(imageTasks, imageResult =>
                                {
                                });
                                Task.WaitAll(imageTasks.ToArray());
                            }
                            catch (Exception exc)
                            {
                                Logit.LogError(exc);
                            }
                        })
                        );
                }
                else
                {
                    foreach (var camera in server.Cameras)
                    {
                        items.Add(Task<GatherResult>.Factory.StartNew(()
                            => processor.PerformGather(camera), token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                            .ContinueWith(taskInfo =>
                            {
                                try
                                {
                                    token.ThrowIfCancellationRequested();
                                    var gatherResult = (GatherResult)taskInfo.Result;
                                    Logit.LogGatherResult(gatherResult);
                                    var imageTasks = GenerateImageTasks(gatherResult, token);
                                    Parallel.ForEach(imageTasks, imageResult =>
                                    {
                                    });
                                    Task.WaitAll(imageTasks.ToArray());
                                }
                                catch (Exception exc)
                                {
                                    Logit.LogError(exc);
                                }
                            })
                            );
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// GENERATE IMAGE TASKS
        /// Given a list of raw camera images, generate a list of associated published images.
        /// </summary>
        /// <param name="gatherResult"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<Task> GenerateImageTasks(GatherResult gatherResult, CancellationToken token)
        {
            var processor = new ImageProcessor();
            var pubImages = DbRepo.ListPubImages(gatherResult);
            var items = new List<Task>();
            foreach (var pubImage in pubImages)
            {
                items.Add(Task<GenerateImageResult>.Factory.StartNew(()
                    => processor.GeneratePubImage(pubImage), token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                    .ContinueWith(taskInfo =>
                    {
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            var pubImageResult = (GenerateImageResult)taskInfo.Result;
                            Logit.LogGenerateImageResult(pubImageResult);
                            var publishTasks = GeneratePublishTasks(pubImageResult, token);
                            Parallel.ForEach(publishTasks, publishResult =>
                            {
                            });
                            Task.WaitAll(publishTasks.ToArray());
                        }
                        catch (Exception exc)
                        {
                            Logit.LogError(exc);
                        }
                    })
                    );
            }
            return items;
        }

        /// <summary>
        /// GENERATE PUBLISH TASKS
        /// Given a published image, send it to all associated destinations.
        /// </summary>
        /// <param name="imageResult"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public List<Task> GeneratePublishTasks(GenerateImageResult imageResult, CancellationToken token)
        {
            var processor = new PublishProcessor();
            var destinations = DbRepo.ListDestinations(imageResult);
            var items = new List<Task>();
            foreach (var dest in destinations)
            {
                items.Add(Task<PublishResult>.Factory.StartNew(() => processor.Publish(dest), token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                    .ContinueWith(taskInfo => Logit.LogPublishResult(taskInfo.Result))
                    );
            }
            return items;
        }
    }
}
