using Arklens;

namespace Contracts;

public static class CharacterEndpoints
{
    
}

public record CharacterModel(
    Gender Gender,
    string Race,
    string Name,
    string Class,
    string Subclass,
    string Alignment,
    CharacteristicsModel Characteristics,
    FileIdentifier Portrait,
    FileIdentifier Minifigure);
    
public record CharacteristicsModel(
    int Strength,
    int Dexterity,
    int Constitution,
    int Intelligence,
    int Wisdom,
    int Charisma);