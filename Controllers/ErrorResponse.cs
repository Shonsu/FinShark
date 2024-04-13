using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Dto
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string StatusPhrase { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public DateTime TimeStamp { get; set; }
        public static IActionResult GenerateErrorResponse(ActionContext context)
        {
            var apiError = new ErrorResponse();
            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.TimeStamp = DateTime.Now;
            var errors = context.ModelState.AsEnumerable();
            foreach (var error in errors)
            {
                foreach (var inner in error.Value!.Errors)
                {
                    apiError.Errors.Add(String.Format("{0}: {1}", error.Key, inner.ErrorMessage));
                }
            }
            return new BadRequestObjectResult(apiError);
        }
    }
}