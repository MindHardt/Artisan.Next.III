using Arklens;
using Vogen;

namespace Contracts;

public record MonsterModel(
    string Name,
    Alignment Alignment,
    CharacteristicsModel Characteristics,
    SavesModel Saves,
    SkillsModel Skills,
    FileIdentifier CoverImage,
    FileIdentifier MinifigureImage);
    
public record CharacteristicsModel(
    Modifier Str,
    Modifier Dex,
    Modifier Con,
    Modifier Int,
    Modifier Wis,
    Modifier Cha);

public record SavesModel(
    Modifier Fort,
    Modifier Reac,
    Modifier Will,
    Modifier Conc,
    Modifier Perc);

public record SkillsModel(
    Modifier Acrobatics,
    Modifier HorseRiding,
    Modifier Survival,
    Modifier Diplomacy,
    Modifier KnowledgeMagic,
    Modifier KnowledgeWorld,
    Modifier KnowledgeReligion,
    Modifier KnowledgeDungeons,
    Modifier KnowledgeNature,
    Modifier Climbing,
    Modifier Mechanics,
    Modifier Medicine,
    Modifier Swimming,
    Modifier Stealth);

[ValueObject<sbyte>]
public readonly partial struct Modifier
{
    public override string ToString() => _value < 0
        ? _value.ToString()
        : $"+{_value}";
}