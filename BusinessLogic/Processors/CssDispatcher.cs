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
    public static class CssDispatcher
    {
        public static void RunTopLevelTasks(int currentMinute, CancellationToken token)
        {
            List<EntityServer> servers = null;
            try
            {
                token.ThrowIfCancellationRequested();
                servers = DbRepo.ListServersByMinute(currentMinute);
                Logit.LogInformation("Processing " + servers.Count.ToString() + " servers...");
            }
            catch (Exception exc)
            {
                var errMsg = "Dispatcher Error: " + exc.Message;
                if (exc.InnerException != null)
                {
                    errMsg += "-- " + exc.InnerException.Message;
                }
                Logit.LogError(new ApplicationException(errMsg));
            }
            try
            {
                token.ThrowIfCancellationRequested();
                var processor = new TaskProcessor();
                var topLevelTasks = processor.GenerateTopLevelTasks(servers, token);
                Parallel.ForEach(topLevelTasks, topLevelResult =>
                {
                });
                Task.WaitAll(topLevelTasks.ToArray()); //needed to catch aggregate exceptions
            }
            catch (Exception exc)
            {
                Logit.LogError(exc);
            }
        }
    }
}
