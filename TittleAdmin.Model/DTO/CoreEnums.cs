using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TittleAdmin.Model.DTO
{
    public class EnumExtended
    {
        /// <summary>
        /// Generic function that obtains value .Neme property of [Display] attribute
        /// on a supplied enum value. Can be used with any enum, not just 'Industry'
        /// in this example
        /// </summary>
        public static string GetEnumDisplayName<T>(T value) where T : struct
        {
            // Get the MemberInfo object for supplied enum value
            var memberInfo = value.GetType().GetMember(value.ToString());
            if (memberInfo.Length != 1)
                return null;

            // Get DisplayAttibute on the supplied enum value
            var displayAttribute = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (displayAttribute == null || displayAttribute.Length != 1)
                return null;

            return displayAttribute[0].Name;
        }
    }

    public enum PromoTypes
    {
        [Display(Name = "Event")]
        Event = 1,
        [Display(Name = "Offer")]
        Offer = 2,
        [Display(Name = "Internal")]
        Internal = 3,
        [Display(Name = "Test")]
        Test = 4,
        [Display(Name = "Partner")]
        Partner = 5
    }

    public enum PromoValueType
    {
        [Display(Name = "SGD")]
        FlatRate = 1,
        [Display(Name = "%")]
        Percentage = 2
    }

    public enum NotificationTypes
    {
        [Display(Name = "One Time")]
        OneTime = 1,
        [Display(Name = "Daily")]
        Daily = 2,
        [Display(Name = "Weekly")]
        Weekly = 3,
        [Display(Name = "Monthly")]
        Monthly = 4
    }

    public enum NotificationStatus
    {
        [Display(Name = "Draft")]
        Draft = 1,
        [Display(Name = "Published")]
        Published = 2
    }
}