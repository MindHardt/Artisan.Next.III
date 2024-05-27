using System.Security.Claims;
using Arklens;
using Arklens.Classes;
using Arklens.Races;

namespace Contracts;

public static class CharacterEndpoints
{
    
}

public record CharacterModel(
    Gender Gender,
    Race Race,
    string Name,
    Class Class,
    Subclass? Subclass,
    Alignment Alignment,
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