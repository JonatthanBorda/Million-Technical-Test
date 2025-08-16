using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Million.Domain.Common;

namespace Million.WebApi.Middleware
{
    /// <summary>
    /// Middleware global que traduce excepciones a ProblemDetails.
    /// Maneja ValidationException, DomainException y genéricas.
    /// </summary>
    public sealed class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException vex)
            {
                var errors = vex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                var problem = Results.ValidationProblem(
                    errors: errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray()),
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Solicitud inválida",
                    type: "https://million/errors/validation");

                await problem.ExecuteAsync(context);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                var problem = Results.Problem(
                    type: "https://million/errors/conflict",
                    title: "Datos duplicados",
                    detail: "Ya existe un registro con el mismo CodeInternal para este propietario.",
                    statusCode: StatusCodes.Status409Conflict);
                await problem.ExecuteAsync(context);
            }
            catch (Exception)
            {
                var problem = Results.Problem(
                    type: "https://million/errors/unexpected",
                    title: "Error inesperado",
                    detail: "Ha ocurrido un error inesperado. Intenta nuevamente.",
                    statusCode: StatusCodes.Status500InternalServerError);
                await problem.ExecuteAsync(context);
            }
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex) =>
            ex.InnerException is SqlException sql && (sql.Number is 2627 or 2601);
    }
}
