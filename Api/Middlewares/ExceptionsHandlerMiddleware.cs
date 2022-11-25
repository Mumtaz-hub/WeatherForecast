using System;
using System.Net;
using System.Threading.Tasks;
using Common;
using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Api.Middlewares
{
    public class ExceptionsHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionsHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (DbUpdateException ex)
            {
                await HandleDatabaseException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static async Task HandleDatabaseException(HttpContext context, DbUpdateException ex)
        {
            await WriteException(context, PopulateDataBaseExceptionInfo(ex), HttpStatusCode.InternalServerError);
        }

        private static async Task HandleException(HttpContext context, Exception ex)
        {
            await WriteException(context, ex.Message, HttpStatusCode.InternalServerError);
        }

        private static async Task WriteException(HttpContext context, string exceptionMessage, HttpStatusCode statusCode)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new FailureResult(exceptionMessage);
            response.StatusCode = (int)statusCode;
            var result = responseModel.ToJson();
            await response.WriteAsync(result);
        }

        private static string PopulateDataBaseExceptionInfo(Exception exception)
        {
            var errorMessage = string.Empty;
            var rootException = exception.GetBaseException();

            var sqlException = rootException as SqlException;
            var exceptionMessage = rootException.Message;

            if (sqlException == null)
                return exceptionMessage;

            const string sqlErrorMessage = "Cannot insert duplicate record in {0}";

            // cannot insert duplicate record
            switch (sqlException.Number)
            {
                case 515:
                case 547:
                    errorMessage = sqlException.Message;
                    break;
                case 2601:
                    var startPos = exceptionMessage.IndexOf(@"with unique index '", StringComparison.Ordinal);
                    var endPos = exceptionMessage.IndexOf(@"'.", startPos, StringComparison.Ordinal);
                    startPos += "with unique index '".Length;
                    var indexName = exceptionMessage.Substring(startPos, (endPos - startPos));
                    var qualifiedIndexName = indexName.Substring(indexName.IndexOf('_') + 1).Replace('_', ' ');
                    errorMessage = string.Format(sqlErrorMessage, qualifiedIndexName);
                    break;
            }

            return errorMessage;
        }
    }
}
