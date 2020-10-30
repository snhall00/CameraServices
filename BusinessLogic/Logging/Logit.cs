using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ODOT.ITS.CSS.BusinessLogic.BusinessEntities;

namespace ODOT.ITS.CSS.BusinessLogic.Logging
{
    public static class Logit
    {
        /// <summary>
        /// LOG GATHER RESULT
        /// </summary>
        /// <param name="gatherResult"></param>
        /// <returns></returns>
        public static GatherResult LogGatherResult(GatherResult gatherResult)
        {
            LogVerbose("Gather success for camera: " + gatherResult.Camera.Name
                + ", raw filename: " + gatherResult.RawImageName + " (thread " + Thread.CurrentThread.ManagedThreadId.ToString() + ")");
            return gatherResult;
        }

        /// <summary>
        /// LOG GATHER RESULTS
        /// </summary>
        /// <param name="gatherResults"></param>
        /// <returns></returns>
        public static List<GatherResult> LogGatherResults(List<GatherResult> gatherResults)
        {
            if (gatherResults.Count > 0)
            {
                LogVerbose("Ftp Gather success for server: " + gatherResults[0].ServerName + " (thread " + Thread.CurrentThread.ManagedThreadId.ToString() + ")");
            }
            foreach (var result in gatherResults)
            {
                LogGatherResult(result);
            }
            return gatherResults;
        }

        /// <summary>
        /// LOG GENERATE IMAGE RESULT
        /// </summary>
        /// <param name="generateImageResult"></param>
        /// <returns></returns>
        public static GenerateImageResult LogGenerateImageResult(GenerateImageResult generateImageResult)
        {
            LogVerbose("Published Image generated for camera: " + generateImageResult.Camera.Name
                + ", published image filename: " + generateImageResult.ImageName + " (thread " + Thread.CurrentThread.ManagedThreadId.ToString() + ")");
            return generateImageResult;
        }

        /// <summary>
        /// LOG PUBLISH RESULT
        /// </summary>
        /// <param name="publishResult"></param>
        /// <returns></returns>
        public static PublishResult LogPublishResult(PublishResult publishResult)
        {
            LogVerbose("Publish success for camera: " + publishResult.Camera.Name
                + ", destination: " + publishResult.DestinationName + " (thread " + Thread.CurrentThread.ManagedThreadId.ToString() + ")");
            return publishResult;
        }

        /// <summary>
        /// LOG INFORMATION
        /// </summary>
        /// <param name="msg"></param>
        public static void LogInformation(string msg)
        {
            Console.WriteLine("(information) " + msg);
        }

        /// <summary>
        /// LOG VERBOSE
        /// </summary>
        /// <param name="msg"></param>
        public static void LogVerbose(string msg)
        {
            Console.WriteLine("(verbose) " + msg);
        }

        /// <summary>
        /// LOG ERROR
        /// </summary>
        /// <param name="exc"></param>
        public static void LogError(Exception exc)
        {
            if (exc.GetType() != typeof(AggregateException))
            {
                Console.WriteLine("(" + exc.GetType().ToString() + ") " + exc.Message);
            }
            Exception test = exc;
            while (test.InnerException != null)
            {
                if (test.InnerException.GetType() != typeof(AggregateException))
                {
                    Console.WriteLine("(" + test.InnerException.GetType().ToString()
                        + ") " + test.InnerException.Message);
                }
                test = test.InnerException;
            }
        }
    }
}

