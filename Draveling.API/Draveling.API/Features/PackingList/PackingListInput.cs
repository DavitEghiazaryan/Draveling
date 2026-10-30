using System.ComponentModel.DataAnnotations;
using Draveling.API.Localization;

namespace Draveling.API.Features.PackingList;

public enum TripType { CityBreak, Beach, Outdoor, Business, Winter }

public enum Weather { Hot, Mild, Cold }

/// <summary>What the visitor tells us about the trip. The list is generated per person.</summary>
public sealed class PackingListInput
{
    public const int MaxDays = 60;

    public TripType TripType { get; set; } = TripType.CityBreak;

    public Weather Weather { get; set; } = Weather.Mild;

    [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.PackDaysRequired))]
    [Range(1, MaxDays, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.PackDaysRange))]
    public int? Days { get; set; } = 7;

    public bool Abroad { get; set; }
    public bool Children { get; set; }
    public bool Car { get; set; }
    public bool Work { get; set; }

    public void ResetToDefaults() => CopyFrom(new PackingListInput());

    /// <summary>Copies all values from another instance, keeping this object bound to the form.</summary>
    public void CopyFrom(PackingListInput other)
    {
        TripType = other.TripType;
        Weather = other.Weather;
        Days = other.Days;
        Abroad = other.Abroad;
        Children = other.Children;
        Car = other.Car;
        Work = other.Work;
    }
}

/// <summary>What is remembered in the visitor's browser between visits.</summary>
public sealed record PackingListState(PackingListInput Input, HashSet<string> Checked);
