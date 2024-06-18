namespace Arklens;

public enum Gender
{
    Undefined = 0,
    Male = 1,
    Female = 2
}

public static class GenderExtensions
{
    public static string GetReadableName(this Gender gender) => gender switch
    {
        Gender.Undefined => "N/A",
        Gender.Male => "М",
        Gender.Female => "Ж",
        _ => "?"
    };
}