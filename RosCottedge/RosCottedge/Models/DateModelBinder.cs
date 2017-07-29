using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RosCottedge.Models
{
    public class DateModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string dateString = controllerContext.HttpContext.Request.QueryString[bindingContext.ModelName];

            if (string.IsNullOrEmpty(dateString))
            {
                dateString = controllerContext.HttpContext.Request.Form[bindingContext.ModelName];
            }
                
            DateTime? nullableDate = null;
            DateTime date;

            var success = DateTime.TryParse(dateString, CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None, out date);

            if (success)
            {
                nullableDate = date;
                return nullableDate;
            }
            else
            {
                return null;
            }
        }
    }
}