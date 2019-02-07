using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoRainForumCommon
{
    public class ModelStateValid
    {
        public static string GetErrorMsg(ModelStateDictionary ModelState)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].Errors.Count<1)
                {
                    continue;
                }
                foreach (var error in ModelState[key].Errors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
            }
            return sb.ToString();
        }
    }
}
