using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace ASPMyCompany.Service
{
    public class AdminAreaAuthorisation : IControllerModelConvention
    {
        private readonly string area;
        private readonly string policy;

        public AdminAreaAuthorisation(string area, string policy)
        {
            this.area = area;
            this.policy = policy;
        }

        public void Apply(ControllerModel controller)
        {
            if (controller.Attributes.Any(a =>
                    a is AreaAttribute attr && attr.RouteValue.Equals(area, StringComparison.OrdinalIgnoreCase)
                                         )
             || controller.RouteValues.Any(r =>
                    r.Key.Equals("area", StringComparison.OrdinalIgnoreCase)
                &&  r.Key.Equals( area , StringComparison.OrdinalIgnoreCase)
                                          )
               )
            {
                controller.Filters.Add(new AuthorizeFilter(policy));
            }
        }
    }
}
