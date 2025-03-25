using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using LS = LocalizationSupports;

public class LocalizationSupports
{
    private static Dictionary<string, Locale> _locales;
    public static Dictionary<string, Locale> Locales
    {
        get
        {
            if (_locales == null)
            {
                _locales = new Dictionary<string, Locale>();
                foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
                {
                    _locales.Add(locale.Identifier.Code, locale);
                }
            }
            return _locales;
        }
    }

    public static void SetLocale(string localeCode)
    {
        if (Locales.Keys.Contains(localeCode))
        {
            LocalizationSettings.SelectedLocale = Locales.GetValueOrDefault(localeCode);
        }
        else
        {
            // TODO: raise error
            Debug.LogWarning($"The locale `{localeCode}` is not available.");
        }
    }
    public static string GetLocalizedString(string tableName, string key)
    {
        LocalizedString stringRef = new LocalizedString() { TableReference = tableName, TableEntryReference = key };
        return stringRef.GetLocalizedString();
    }

    public static string __(string t, string k) => GetLocalizedString(t, k);
}
