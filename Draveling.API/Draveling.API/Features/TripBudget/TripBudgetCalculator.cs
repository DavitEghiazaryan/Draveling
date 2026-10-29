namespace Draveling.API.Features.TripBudget;

/// <summary>Itemized result of a trip budget estimate. All values are in USD.</summary>
public sealed record TripBudgetResult(
    int Travelers,
    int Days,
    int Nights,
    decimal Accommodation,
    decimal Food,
    decimal Transportation,
    decimal Activities,
    decimal Flights,
    decimal Other)
{
    public decimal Total => Accommodation + Food + Transportation + Activities + Flights + Other;

    public decimal PerTraveler => Total / Travelers;
}

/// <summary>Pure calculation logic for the Trip Budget Calculator. No UI or I/O dependencies.</summary>
public static class TripBudgetCalculator
{
    /// <summary>
    /// Calculates the itemized budget. Travelers and days must be at least 1.
    /// Empty or negative cost values are treated as zero.
    /// </summary>
    public static TripBudgetResult Calculate(TripBudgetInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var travelers = input.Travelers ?? 0;
        var days = input.Days ?? 0;

        ArgumentOutOfRangeException.ThrowIfLessThan(travelers, 1, nameof(input.Travelers));
        ArgumentOutOfRangeException.ThrowIfLessThan(days, 1, nameof(input.Days));

        // Initial version: the number of nights equals the number of trip days.
        var nights = days;

        return new TripBudgetResult(
            Travelers: travelers,
            Days: days,
            Nights: nights,
            Accommodation: Cost(input.AccommodationPerNight) * nights,
            Food: Cost(input.FoodPerPersonPerDay) * days * travelers,
            Transportation: Cost(input.TransportationPerDay) * days,
            Activities: Cost(input.ActivitiesPerPersonPerDay) * days * travelers,
            Flights: Cost(input.FlightPerPerson) * travelers,
            Other: Cost(input.OtherExpenses));
    }

    private static decimal Cost(decimal? value) => value is > 0 ? value.Value : 0m;
}
