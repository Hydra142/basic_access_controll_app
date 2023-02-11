using System.Collections.Generic;

namespace SafeMessenge.Helpers;
public class ComboBoxOptionHelper
{
    public class ComboBoxOption : ComboBoxOptionHelper
    {
        private readonly string _key;
        private readonly string _value;

        public string Key => _key;

        public string Value => _value;

        public ComboBoxOption(string OptionKey, string OptionValue)
        {
            _key = OptionKey;
            _value = OptionValue;
        }

        public override string ToString()
        {
            return $"key:{_key} text:{_value}";
        }
    }

    public static ComboBoxOption? GetOptionByKey(string? Key, List<ComboBoxOption> options)
    {
        foreach (var helper in options)
        {
            if (helper.Key == Key)
            {
                return helper;
            }
        }
        return null;
    }

    public static ComboBoxOption? GetOptionByValue(string Value, List<ComboBoxOption> options)
    {
        foreach (var helper in options)
        {
            if (helper.Value == Value)
            {
                return helper;
            }
        }
        return null;
    }
}
