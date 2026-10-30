namespace Draveling.API.Features.PackingList;

public enum PackingCategory { Documents, Clothing, Toiletries, Electronics, Health, Misc }

/// <summary>
/// One possible item. <see cref="Key"/> is also the resource key suffix ("Item_{Key}") for its name.
/// <see cref="When"/> decides whether the item applies to a trip; <see cref="Quantity"/> gives a count or null.
/// </summary>
public sealed record PackingItem(
    string Key,
    PackingCategory Category,
    Func<PackingListInput, bool> When,
    Func<PackingListInput, int?> Quantity);

/// <summary>Every item the generator knows about, in display order.</summary>
public static class PackingCatalog
{
    public static readonly IReadOnlyList<PackingItem> Items =
    [
        // Documents
        Item("IdCard", PackingCategory.Documents),
        Item("Passport", PackingCategory.Documents, i => i.Abroad),
        Item("Visa", PackingCategory.Documents, i => i.Abroad),
        Item("TravelInsurance", PackingCategory.Documents, i => i.Abroad),
        Item("Bookings", PackingCategory.Documents),
        Item("BankCards", PackingCategory.Documents),
        Item("LocalCurrency", PackingCategory.Documents, i => i.Abroad),
        Item("DocumentCopies", PackingCategory.Documents, i => i.Abroad),
        Item("DrivingLicence", PackingCategory.Documents, i => i.Car),
        Item("ChildDocuments", PackingCategory.Documents, i => i.Children),
        Item("EmergencyContacts", PackingCategory.Documents),

        // Clothing
        Item("Underwear", PackingCategory.Clothing, quantity: PerDay(cap: 7)),
        Item("Socks", PackingCategory.Clothing, quantity: PerDay(cap: 7)),
        Item("TShirts", PackingCategory.Clothing, i => i.Weather != Weather.Cold, PerDay(cap: 7)),
        Item("LongSleeves", PackingCategory.Clothing, i => i.Weather != Weather.Hot, EveryDays(2, cap: 4)),
        Item("Trousers", PackingCategory.Clothing, quantity: EveryDays(3, cap: 3)),
        Item("Shorts", PackingCategory.Clothing, i => i.Weather == Weather.Hot || i.TripType == TripType.Beach, EveryDays(3, cap: 3)),
        Item("Sweater", PackingCategory.Clothing, i => i.Weather != Weather.Hot, i => i.Weather == Weather.Cold ? 2 : 1),
        Item("LightJacket", PackingCategory.Clothing, i => i.Weather == Weather.Mild),
        Item("WarmJacket", PackingCategory.Clothing, i => i.Weather == Weather.Cold && i.TripType != TripType.Winter),
        Item("SkiJacket", PackingCategory.Clothing, i => i.TripType == TripType.Winter),
        Item("SkiTrousers", PackingCategory.Clothing, i => i.TripType == TripType.Winter),
        Item("ThermalUnderwear", PackingCategory.Clothing, i => i.TripType == TripType.Winter || i.Weather == Weather.Cold, _ => 2),
        Item("RainJacket", PackingCategory.Clothing, i => i.TripType == TripType.Outdoor || i.Weather == Weather.Mild),
        Item("Swimwear", PackingCategory.Clothing, i => i.TripType == TripType.Beach || i.Weather == Weather.Hot, _ => 2),
        Item("Sandals", PackingCategory.Clothing, i => i.TripType == TripType.Beach || i.Weather == Weather.Hot),
        Item("WalkingShoes", PackingCategory.Clothing),
        Item("HikingBoots", PackingCategory.Clothing, i => i.TripType == TripType.Outdoor),
        Item("BusinessOutfits", PackingCategory.Clothing, i => i.TripType == TripType.Business, PerDay(cap: 5)),
        Item("DressShoes", PackingCategory.Clothing, i => i.TripType == TripType.Business),
        Item("Belt", PackingCategory.Clothing, i => i.TripType == TripType.Business),
        Item("Sleepwear", PackingCategory.Clothing, quantity: i => i.Days > 7 ? 2 : 1),
        Item("SunHat", PackingCategory.Clothing, i => i.Weather == Weather.Hot || i.TripType == TripType.Beach),
        Item("WarmAccessories", PackingCategory.Clothing, i => i.Weather == Weather.Cold || i.TripType == TripType.Winter),
        Item("SkiGloves", PackingCategory.Clothing, i => i.TripType == TripType.Winter),
        Item("Sunglasses", PackingCategory.Clothing, i => i.Weather == Weather.Hot || i.TripType is TripType.Beach or TripType.Winter or TripType.Outdoor),
        Item("LaundryBag", PackingCategory.Clothing, i => i.Days > 7),

        // Toiletries
        Item("Toothbrush", PackingCategory.Toiletries),
        Item("Deodorant", PackingCategory.Toiletries),
        Item("Shampoo", PackingCategory.Toiletries),
        Item("Razor", PackingCategory.Toiletries),
        Item("Hairbrush", PackingCategory.Toiletries),
        Item("NailClippers", PackingCategory.Toiletries, i => i.Days > 3),
        Item("HandSanitizer", PackingCategory.Toiletries),
        Item("Sunscreen", PackingCategory.Toiletries, i => i.Weather == Weather.Hot || i.TripType is TripType.Beach or TripType.Outdoor or TripType.Winter),
        Item("AfterSun", PackingCategory.Toiletries, i => i.TripType == TripType.Beach),
        Item("LipBalm", PackingCategory.Toiletries, i => i.Weather == Weather.Cold || i.TripType is TripType.Winter or TripType.Outdoor),
        Item("Moisturizer", PackingCategory.Toiletries, i => i.Weather == Weather.Cold || i.TripType == TripType.Winter),
        Item("InsectRepellent", PackingCategory.Toiletries, i => i.Weather == Weather.Hot || i.TripType is TripType.Beach or TripType.Outdoor),
        Item("WetWipes", PackingCategory.Toiletries, i => i.Children || i.TripType == TripType.Outdoor),

        // Electronics
        Item("PhoneCharger", PackingCategory.Electronics),
        Item("PowerBank", PackingCategory.Electronics),
        Item("Headphones", PackingCategory.Electronics),
        Item("PlugAdapter", PackingCategory.Electronics, i => i.Abroad),
        Item("Camera", PackingCategory.Electronics, i => i.TripType is TripType.Beach or TripType.CityBreak or TripType.Outdoor),
        Item("Laptop", PackingCategory.Electronics, i => i.Work),
        Item("Headset", PackingCategory.Electronics, i => i.Work),
        Item("CarCharger", PackingCategory.Electronics, i => i.Car),

        // Health
        Item("Medications", PackingCategory.Health),
        Item("FirstAidKit", PackingCategory.Health),
        Item("Painkillers", PackingCategory.Health),
        Item("BlisterPlasters", PackingCategory.Health, i => i.TripType is TripType.Outdoor or TripType.CityBreak),
        Item("MotionSickness", PackingCategory.Health, i => i.Car),
        Item("StomachRemedy", PackingCategory.Health, i => i.Abroad),
        Item("AllergyMedicine", PackingCategory.Health, i => i.TripType == TripType.Outdoor),
        Item("ChildMedicine", PackingCategory.Health, i => i.Children),
        Item("PrescriptionCopies", PackingCategory.Health, i => i.Abroad),

        // Other essentials
        Item("DayBackpack", PackingCategory.Misc, i => i.TripType is TripType.CityBreak or TripType.Outdoor),
        Item("WaterBottle", PackingCategory.Misc),
        Item("Snacks", PackingCategory.Misc, i => i.Children || i.TripType == TripType.Outdoor),
        Item("TravelPillow", PackingCategory.Misc, i => i.Abroad),
        Item("Umbrella", PackingCategory.Misc, i => i.Weather == Weather.Mild),
        Item("BeachTowel", PackingCategory.Misc, i => i.TripType == TripType.Beach),
        Item("DryBag", PackingCategory.Misc, i => i.TripType == TripType.Beach),
        Item("KidsEntertainment", PackingCategory.Misc, i => i.Children),
        Item("Stroller", PackingCategory.Misc, i => i.Children),
        Item("WorkDocuments", PackingCategory.Misc, i => i.Work),
        Item("NotebookPen", PackingCategory.Misc, i => i.TripType == TripType.Business),
        Item("BusinessCards", PackingCategory.Misc, i => i.TripType == TripType.Business),
        Item("LuggageLocks", PackingCategory.Misc, i => i.Abroad),
        Item("SkiGoggles", PackingCategory.Misc, i => i.TripType == TripType.Winter),
        Item("SkiPass", PackingCategory.Misc, i => i.TripType == TripType.Winter),
        Item("TrekkingPoles", PackingCategory.Misc, i => i.TripType == TripType.Outdoor),
        Item("OfflineMaps", PackingCategory.Misc, i => i.TripType == TripType.Outdoor),
        Item("Headlamp", PackingCategory.Misc, i => i.TripType == TripType.Outdoor),
        Item("LaundryDetergent", PackingCategory.Misc, i => i.Days > 7),
    ];

    private static PackingItem Item(
        string key,
        PackingCategory category,
        Func<PackingListInput, bool>? when = null,
        Func<PackingListInput, int?>? quantity = null)
        => new(key, category, when ?? (_ => true), quantity ?? (_ => null));

    /// <summary>One per day, up to <paramref name="cap"/>. Longer trips assume laundry is possible.</summary>
    private static Func<PackingListInput, int?> PerDay(int cap) => i => Math.Min(i.Days ?? 1, cap);

    /// <summary>One per <paramref name="days"/> days, rounded up, up to <paramref name="cap"/>.</summary>
    private static Func<PackingListInput, int?> EveryDays(int days, int cap)
        => i => Math.Min((int)Math.Ceiling((i.Days ?? 1) / (double)days), cap);
}
