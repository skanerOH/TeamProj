using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PersonalBlog.BLL.Validation;
using PersonalBlog.PL.Models.ResponseModels;
using PersonalBlog.PL.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Extentions
{
    /// <summary>
    /// Custom Exception middleware
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next">RequestDelegate</param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Main middleware method
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BlogsException be)
            {
                httpContext.Response.ContentType = "application/json; charset=utf-8";
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseModel
                {
                    responseCode = ResponseModelCode.Error,
                    responseMessage = "blog logic errors",
                    data = new List<string>(be.errorMessages)
                }));
            }
            catch (ModelStateException mse)
            {
                httpContext.Response.ContentType = "application/json; charset=utf-8";
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseModel
                {
                    responseCode = ResponseModelCode.Error,
                    responseMessage = "invalid input",
                    data = new List<string>(mse.errorMessages)
                }));
            }
            catch (Exception e)
            {
                httpContext.Response.ContentType = "application/json; charset=utf-8";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseModel
                {
                    responseCode = ResponseModelCode.Error,
                    responseMessage = "general errors",
                    data = new List<string> { e.Message }
                }));
            }
        }
    }
}
