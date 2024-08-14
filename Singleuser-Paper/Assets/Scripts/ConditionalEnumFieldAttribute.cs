using UnityEngine;

public class ConditionalEnumFieldAttribute : PropertyAttribute
{
    public string EnumFieldName;
    public int EnumValue;

    public ConditionalEnumFieldAttribute(string enumFieldName, int enumValue)
    {
        this.EnumFieldName = enumFieldName;
        this.EnumValue = enumValue;
    }
}
