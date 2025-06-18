using ClassLibrary.DataAccess.Exceptions;
using ClassLibrary.Domain.Domain_Exceptions;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DataAccess.DataAcces_Exceptions
{
    public static class RepositoryExceptionHelper
    {
        public static void HandleException(Exception ex, string contextMessage)
        {
            if (ex is MySqlException sqlEx)
            {
                Log.Error(sqlEx, "Repository: databasefout - {Context}", contextMessage);
                throw new DatabaseException($"Fout bij databasebewerking: {contextMessage}", sqlEx);
            }
            else
            {
                Log.Error(ex, "Repository: onverwachte fout - {Context}", contextMessage);
                throw new RepositoryException($"Onverwachte fout in repositorylaag: {contextMessage}", ex);
            }
        }
    }
}
