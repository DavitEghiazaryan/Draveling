using System.ComponentModel.DataAnnotations;

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

    [Required(ErrorMessage = "Enter the number of travelers.")]
    [Range(1, MaxTravelers, ErrorMessage = "Travelers must be between 1 and 100.")]
    public int? Travelers { get; set; } = 2;

    [Required(ErrorMessage = "Enter the number of trip days.")]
    [Range(1, MaxDays, ErrorMessage = "Trip days must be between 1 and 365.")]
    public int? Days { get; set; } = 7;

    [Range(0, MaxCost, ErrorMessage = "Accommodation cost cannot be negative.")]
    public decimal? AccommodationPerNight { get; set; } = 120;

    [Range(0, MaxCost, ErrorMessage = "Food cost cannot be negative.")]
    public decimal? FoodPerPersonPerDay { get; set; } = 40;

    [Range(0, MaxCost, ErrorMessage = "Transportation cost cannot be negative.")]
    public decimal? TransportationPerDay { get; set; } = 20;

    [Range(0, MaxCost, ErrorMessage = "Activities cost cannot be negative.")]
    public decimal? ActivitiesPerPersonPerDay { get; set; } = 30;

    [Range(0, MaxCost, ErrorMessage = "Flight cost cannot be negative.")]
    public decimal? FlightPerPerson { get; set; } = 500;

    [Range(0, MaxCost, ErrorMessage = "Other expenses cannot be negative.")]
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
