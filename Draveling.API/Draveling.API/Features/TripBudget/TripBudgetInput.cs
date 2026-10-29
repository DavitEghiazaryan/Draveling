using System.ComponentModel.DataAnnotations;
using Draveling.API.Localization;

namespace Draveling.API.Features.TripBudget;

/// <summary>
/// User-entered estimates for a trip. All money values are in USD.
/// Nullable types let the form distinguish "empty" from "zero" so empty inputs can be handled safely.
/// </summary>
public sealed class TripBudgetInput
{
    public const int MaxTravelers = 100;
    public const int MaxDays = 365;
    public const double MaxCost = 1_000_000;

    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.TravelersRequired))]
    [Range(1, MaxTravelers, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.TravelersRange))]
    public int? Travelers { get; set; } = 2;

    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.DaysRequired))]
    [Range(1, MaxDays, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.DaysRange))]
    public int? Days { get; set; } = 7;

    [Range(0, MaxCost, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.AccommodationNegative))]
    public decimal? AccommodationPerNight { get; set; } = 120;

    [Range(0, MaxCost, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.FoodNegative))]
    public decimal? FoodPerPersonPerDay { get; set; } = 40;

    [Range(0, MaxCost, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.TransportationNegative))]
    public decimal? TransportationPerDay { get; set; } = 20;

    [Range(0, MaxCost, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.ActivitiesNegative))]
    public decimal? ActivitiesPerPersonPerDay { get; set; } = 30;

    [Range(0, MaxCost, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.FlightNegative))]
    public decimal? FlightPerPerson { get; set; } = 500;

    [Range(0, MaxCost, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.OtherNegative))]
    public decimal? OtherExpenses { get; set; } = 100;

    /// <summary>Restores the default values in place, so the form keeps the same model instance.</summary>
    public void ResetToDefaults()
    {
        var defaults = new TripBudgetInput();
        Travelers = defaults.Travelers;
        Days = defaults.Days;
        AccommodationPerNight = defaults.AccommodationPerNight;
        FoodPerPersonPerDay = defaults.FoodPerPersonPerDay;
        TransportationPerDay = defaults.TransportationPerDay;
        ActivitiesPerPersonPerDay = defaults.ActivitiesPerPersonPerDay;
        FlightPerPerson = defaults.FlightPerPerson;
        OtherExpenses = defaults.OtherExpenses;
    }
}
