using System;

namespace Sandbox.Localization;

[Serializable]
public class LocalizedStrings
{
    [Property] public Dictionary<string, string> Strings { get; set; } = new (2);
    
    public static LocalizedStrings FromTuple(params (string, string)[] strings)
    {
        var localizedStrings = new LocalizedStrings();
        foreach (var stringTuple in strings)
        {
            localizedStrings.Strings.Add(stringTuple.Item1, stringTuple.Item2);
        }

        return localizedStrings;
    }
        
    public override string ToString()
    {
        var text = "ERROR: No Localization";
        
        if (SettingsSingleton.Data == null || SettingsSingleton.Data.LanguageKey == null)
            text = "ERROR: Settings Not Found!";
        
        else if (Strings.TryGetValue(SettingsSingleton.Data.LanguageKey, out var localizedString))
            text = localizedString;

        return text;
    }
}