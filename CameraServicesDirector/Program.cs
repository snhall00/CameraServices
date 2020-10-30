using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ODOT.ITS.CSS.BusinessLogic.Logging;
using ODOT.ITS.CSS.BusinessLogic.Processors;

namespace ODOT.ITS.CSS.Director
{
    public class Program
    {
        /// <summary>
        /// MAIN
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            var _tokenSource = new CancellationTokenSource();
            Logit.LogInformation("CssDirector starting...");
            CssDispatcher.RunTopLevelTasks(DateTime.Now.Minute, _tokenSource.Token);
            var elapsedTime = String.Format("{0:0.0}", (DateTime.Now - startTime).TotalMilliseconds / 1000);
            Logit.LogInformation("CssDirector finished in " + elapsedTime + " seconds.");
            Console.Write("Type any key to pause...");
            if (ConsoleReadPauseWithTimeout(3))
            {
                Console.Write("\nType any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// CONSOLE READ KEY WITH TIMEOUT
        /// </summary>
        /// <param name="timeoutSec"></param>
        /// <returns></returns>
        public static bool ConsoleReadPauseWithTimeout(int timeoutSec)
        {
            var timeout = TimeSpan.FromSeconds(3);
            Task<ConsoleKeyInfo> task = Task.Factory.StartNew(Console.ReadKey);
            Task.WaitAny(new Task[] { task }, timeout);

            var result = Task.WaitAny(new Task[] { task }, timeout) == 0;
            return result;
        }
    }
}
