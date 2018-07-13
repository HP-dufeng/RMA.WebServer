﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMA.WebServer.Host.Authentication
{
    public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            //var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            //var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            //var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

            //if (isAuthorized && !allowAnonymous)
            //{
            //    if (operation.Parameters == null)
            //        operation.Parameters = new List<IParameter>();

            //    operation.Parameters.Add(new NonBodyParameter
            //    {
            //        Name = "Authorization",
            //        In = "header",
            //        Description = "access token",
            //        Required = true,
            //        Type = "string"
            //    });
            //}

            operation.Parameters = operation.Parameters ?? new List<IParameter>();
            var actionAttributes = context.ControllerActionDescriptor.GetControllerAndActionAttributes(true);
            var isAuthorized = actionAttributes.Any(a => a is Abp.Authorization.AbpAuthorizeAttribute);
            var allowAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "access token",
                    Required = true,
                    Type = "string"
                });
            }

        }
    }
}