using System.Globalization;
using System.Resources;

namespace Draveling.API.Localization;

/// <summary>
/// Localized validation messages for DataAnnotations attributes, which can only
/// read messages from public static string properties.
/// Each property maps to a key in AppStrings.resx.
/// </summary>
public static class ValidationMessages
{
    private static readonly ResourceManager Resources = new(typeof(AppStrings).FullName!, typeof(AppStrings).Assembly);

    public static string TravelersRequired => Get("Validation_TravelersRequired");
    public static string TravelersRange => Get("Validation_TravelersRange");
    public static string DaysRequired => Get("Validation_DaysRequired");
    public static string DaysRange => Get("Validation_DaysRange");
    public static string AccommodationNegative => Get("Validation_AccommodationNegative");
    public static string FoodNegative => Get("Validation_FoodNegative");
    public static string TransportationNegative => Get("Validation_TransportationNegative");
    public static string ActivitiesNegative => Get("Validation_ActivitiesNegative");
    public static string FlightNegative => Get("Validation_FlightNegative");
    public static string OtherNegative => Get("Validation_OtherNegative");

    private static string Get(string key) => Resources.GetString(key, CultureInfo.CurrentUICulture) ?? key;
}
