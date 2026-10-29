namespace Draveling.API.Localization;

public sealed record Language(string Code, string NativeName);

/// <summary>
/// The languages the site is published in. English lives at the site root,
/// every other language under its own URL prefix such as /hy/ or /ru/.
///
/// To add a language:
///   1. Add it to <see cref="All"/>.
///   2. Add an AppStrings.{code}.resx file next to AppStrings.resx.
///   3. Add a "@page" route with the new prefix to every page.
///   4. Add the code to the supported list in wwwroot/index.html.
/// </summary>
public static class Languages
{
    public const string Default = "en";

    public static readonly IReadOnlyList<Language> All =
    [
        new("en", "English"),
        new("hy", "Հայերեն"),
        new("ru", "Русский"),
    ];

    public static bool IsSupported(string code) => All.Any(l => l.Code == code);

    /// <summary>
    /// Splits a base-relative path such as "hy/trip-budget-calculator" into the
    /// language ("hy") and the rest of the path ("trip-budget-calculator").
    /// Paths without a known prefix belong to the default language.
    /// </summary>
    public static (string Language, string Path) Split(string baseRelativePath)
    {
        var slash = baseRelativePath.IndexOf('/');
        var first = slash < 0 ? baseRelativePath : baseRelativePath[..slash];

        if (first != Default && IsSupported(first))
        {
            return (first, slash < 0 ? string.Empty : baseRelativePath[(slash + 1)..]);
        }

        return (Default, baseRelativePath);
    }

    /// <summary>Prefixes a base-relative path with the language, e.g. ("trip-budget-calculator", "hy") → "hy/trip-budget-calculator".</summary>
    public static string LocalizePath(string path, string language)
    {
        if (language == Default)
        {
            return path;
        }

        return path.Length == 0 ? language : $"{language}/{path}";
    }
}
