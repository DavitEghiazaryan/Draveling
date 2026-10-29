using Microsoft.AspNetCore.Components;

namespace Draveling.API.Localization;

/// <summary>
/// The language of the page currently shown, derived from the URL.
/// Builds links that stay in the current language or switch to another one.
/// </summary>
public sealed class AppLanguage(NavigationManager navigation)
{
    /// <summary>Language code of the current page, e.g. "en", "hy" or "ru".</summary>
    public string Code => Split().Language;

    /// <summary>The current page path without the language prefix, e.g. "trip-budget-calculator".</summary>
    public string PathWithoutLanguage => Split().Path;

    /// <summary>Base-relative link to <paramref name="path"/> in the current language.</summary>
    public string Href(string path) => Languages.LocalizePath(path, Code);

    /// <summary>Base-relative link to the current page in another language.</summary>
    public string HrefFor(string language) => Languages.LocalizePath(PathWithoutLanguage, language);

    /// <summary>Absolute URL of the current page in another language, for hreflang and canonical tags.</summary>
    public string AbsoluteUrlFor(string language) => navigation.BaseUri + HrefFor(language);

    private (string Language, string Path) Split()
    {
        var relative = navigation.ToBaseRelativePath(navigation.Uri);
        var cut = relative.IndexOfAny(['?', '#']);
        if (cut >= 0)
        {
            relative = relative[..cut];
        }

        return Languages.Split(relative.TrimEnd('/'));
    }
}
