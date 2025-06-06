using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Domain.Domain_Exceptions
{
    public static class ServiceExceptionHelper
    {
        public static void HandleException(Exception ex, string contextMessage)
        {
            var exceptionTypeName = ex.GetType().Name;
            var innerExceptionTypeName = ex.InnerException?.GetType().Name;

            if (exceptionTypeName == "RepositoryException")
            {
                Log.Warning(ex, "Service: repositoryfout - {Context}", contextMessage);
                throw new ServicesException($"Repositoryfout in service: {contextMessage}", ex);
            }
            else if (exceptionTypeName == "DatabaseException" || innerExceptionTypeName == "MySqlException")
            {
                Log.Warning(ex, "Service: databasefout - {Context}", contextMessage);
                throw new ServicesException($"Databasefout in service: {contextMessage}", ex);
            }
            else
            {
                Log.Error(ex, "Service: onverwachte fout - {Context}", contextMessage);
                throw new ServicesException($"Onverwachte fout in service: {contextMessage}", ex);
            }
        }
    }
}