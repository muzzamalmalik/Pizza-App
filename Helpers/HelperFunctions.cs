
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PizzaOrder.Helpers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerNameAttribute : Attribute
    {
        public string Name { get; }

        public ControllerNameAttribute(string name)
        {
            Name = name;
        }
    }
    public class ControllerNameAttributeConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNameAttribute = controller.Attributes.OfType<ControllerNameAttribute>().SingleOrDefault();
            if (controllerNameAttribute != null)
            {
                controller.ControllerName = controllerNameAttribute.Name;
            }
        }
    }
    public static class HelperFunctions
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }
        public static string ToDate(this DateTime dateString)
        {
            var PKZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var Now = TimeZoneInfo.ConvertTimeFromUtc(dateString, PKZone);
            var dt = Now.Date.ToString("ddd dd-MM-yyyy");
            return dt;
        }
        public static string ToDate(this DateTime? dateString)
        {
            if (dateString != null)
            {
                var PKZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
                var Now = TimeZoneInfo.ConvertTimeFromUtc(dateString.Value, PKZone);
                var dt = Now.Date.ToString("ddd dd-MM-yyyy");
                return dt;
            }
            else
            {
                return "";
            }

        }
        public static string ToDate_ForEdit(this DateTime dateString)
        {
            //var getDate = Convert.ToDateTime(dateString);
            var dt = dateString.Date.ToString("yyyy-MM-dd");
            return dt;
        }
        public static string ToDate_ForEdit(this DateTime? dateString)
        {
            if (dateString != null)
            {
                var dt = dateString.Value.Date.ToString("yyyy-MM-dd");
                return dt;
            }
            else
            {
                return "";
            }

        }
        public static string ToDateTime(this DateTime dateString)
        {
            var PKZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var Now = TimeZoneInfo.ConvertTimeFromUtc(dateString, PKZone);
            var dt = Now.ToString($"ddd dd-MMM-yyyy HH:mm");
            return dt;
        }
        public static string ToDateTime(this DateTime? dateString)
        {
            if (dateString != null)
            {
                var PKZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
                var Now = TimeZoneInfo.ConvertTimeFromUtc(dateString.Value, PKZone);
                var dt = Now.ToString($"ddd dd-MMM-yyyy HH:mm");
                return dt;
            }
            else
            {
                return "";
            }
        }
        public static DateTime ToDateTime_Parse(this string dateString)
        {
            DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime);
            return dateTime;
        }
        public static DateTime? ToDateTime_Parse_ToNullable(this string dateString)
        {
            if (!dateString.IsNullOrEmpty())
            {
                DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime);
                return dateTime;
            }
            else
            {
                return null;
            }
        }
        public static string ToDateTime_WithSeconds(this DateTime dateString)
        {
            var PKZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var Now = TimeZoneInfo.ConvertTimeFromUtc(dateString, PKZone);
            var dt = Now.ToString("dd-MM-yyyy hh:mm:ss tt");
            return dt;
        }
        public static string ToTime(this TimeSpan timeSpan)
        {
            DateTime time = DateTime.UtcNow.Date.Add(timeSpan);
            var PKZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            var Now = TimeZoneInfo.ConvertTimeFromUtc(time, PKZone);
            string dt = Now.ToString("hh:mm:ss tt");
            return dt;
        }
        //public static string ToDateTime_WithSeconds(this DateTime dateString)
        //{
        //    var PKZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
        //    var Now = TimeZoneInfo.ConvertTimeFromUtc(dateString, PKZone);
        //    var dt = Now.ToString("dd-MM-yyyy hh:mm:ss tt");
        //    return dt;
        //}
        public static string To24HRTime(this TimeSpan timeSpan)
        {
            string shortForm = "";
            shortForm += $"{timeSpan.Hours:00}";
            shortForm += ":" + $"{timeSpan.Minutes:00}";
            shortForm += ":" + $"{timeSpan.Seconds:00}";
            return shortForm;
        }
        public static int ToInt32(this string Value)
        {
            int intValue = Convert.ToInt32(Value);
            return intValue;
        }
        public static int ToNotNull_Int(this int? Value)
        {
            if (Value != null)
            {
                int intValue = Convert.ToInt32(Value);
                return intValue;
            }
            else
            {
                return 0;
            }
        }
        public static bool ToBoolean(this string Value)
        {
            bool boolValue = Convert.ToBoolean(Value);
            return boolValue;
        }
        public static double ToDouble(this int? Value)
        {
            if (Value != null)
            {
                var doubleValue = Convert.ToDouble(Value);
                return doubleValue;
            }
            else
            {
                return 0;
            }

        }
        public static double ToNotNull_Double(this double? Value)
        {
            if (Value != null)
            {
                var doubleValue = Convert.ToDouble(Value);
                return doubleValue;
            }
            else
            {
                return 0;
            }

        }

        public static string GetDescription(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Description ?? "Description Not Found";
        }


        public static string ImageConverted(byte[] imageFile = null)
        {
            
            if (imageFile != null && imageFile.Length > 0)
            {
                return "data: image / png; base64," +  Convert.ToBase64String(imageFile, 0, imageFile.Length);            
            }

            return string.Empty;
        }

        //internal static string GetTNANames(List<TnaAction> list, string tnaIds)
        //{
        //    if (list.Count > 0 && !string.IsNullOrEmpty(tnaIds))
        //    {
        //        var ids = Array.ConvertAll(tnaIds.Split(","), Convert.ToInt32);
        //        var names = list.Where(m => ids.Contains(m.Id)).Select(m => m.Name).ToList();

        //        return string.Join(',', names);
        //    }
        //    else
        //        return "";
        //}
        internal static List<int> FromStrToIntArray(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var ids = Array.ConvertAll(str.Split(","), Convert.ToInt32).ToList();

                return ids;
            }
            else
            {
                return new List<int>();
            }
        }
        internal static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }


        public static string SetNotificationDesc(string[] ValuesToShow, string From)
        {
            var details = string.Join(" ", ValuesToShow);
            return $"{ details }";// { Environment.NewLine }From: { From }";
        }
        public static string SetNullTo_EmptyString(this string Value)
        {
            if (Value == null)
            {
                return "";
            }
            else
                return Value;
        }




        public static void CheckUserLoggedIn()
        {
            var loggedUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirstValue(Enums.ClaimType.UserId.ToString()));
            if (loggedUserId == 0)
                throw new Exception(CustomMessage.UserNotLoggedIn);
        }
    }
}
