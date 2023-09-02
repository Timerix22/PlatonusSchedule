using System;

namespace Platonus.API;

public class PlatonusLanguage
{
    public readonly int Number;
    public readonly string LiteralCode;

    private PlatonusLanguage(int num, string literalCode)
    {
        Number = num;
        LiteralCode = literalCode;
    }

    public static PlatonusLanguage English = new(0, "en");
    public static PlatonusLanguage Russian = new(1, "ru");
    public static PlatonusLanguage Kazakh = new(2, "kz");

    public static PlatonusLanguage Parse(string literalCode) =>
        literalCode.ToLower() switch
        {
            "en" => English,
            "ru" => Russian,
            "kz" => Kazakh,
            _ => throw new Exception($"unknown language code: {literalCode}")
        };
}