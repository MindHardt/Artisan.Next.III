using System.Text.RegularExpressions;
using Arklens;
using Arklens.Classes;

namespace Client.Features.Characters.New;

public partial class NewCharacter
{
    [GeneratedRegex(@"\$\{(?<NAME>[А-Яа-я]+)\}")]
    private static partial Regex SvgPreparationRegex();

    private string PreparePortrait() =>
        _originalSvg!.Replace(OriginalAvatarBase64, Character.AvatarBase64 ?? OriginalAvatarBase64);
    
    private string PrepareEmptySvg() => SvgPreparationRegex().Replace(PreparePortrait(), string.Empty);
    private string PrepareSvg() => SvgPreparationRegex().Replace(PreparePortrait(), 
        match => match.Groups["NAME"].Value switch
        {
            "ИМЯ" => Character.Name,
            "РАСА" => Character.Race?.Name,
            "КЛАСС" => Character.Class?.Name,
            "ПОЛ" => Character.Gender?.GetReadableName(),
            "МИР" => Character.Alignment?.Name,
            "ПОДКЛАСС" => Character.Subclass?.Name,
            "СИЛ" => Character.Characteristics["💪"].AsModifier(),
            "ЛВК" => Character.Characteristics["🏃"].AsModifier(),
            "ВЫН" => Character.Characteristics["🧡"].AsModifier(),
            "ИНТ" => Character.Characteristics["🧠"].AsModifier(),
            "МДР" => Character.Characteristics["🦉"].AsModifier(),
            "ХАР" => Character.Characteristics["👄"].AsModifier(),
            "АКР" => GetSkillMark(ClassSkills.Acrobatics),
            "ВЕР" => GetSkillMark(ClassSkills.HorseRiding),
            "ВЫЖ" => GetSkillMark(ClassSkills.Survival),
            "ПЕР" => GetSkillMark(ClassSkills.Diplomacy),
            "ЗМА" => GetSkillMark(ClassSkills.KnowledgeMagic),
            "ЗМИ" => GetSkillMark(ClassSkills.KnowledgeWorld),
            "ЗРЕ" => GetSkillMark(ClassSkills.KnowledgeReligion),
            "ЗПО" => GetSkillMark(ClassSkills.KnowledgeDungeons),
            "ЗПР" => GetSkillMark(ClassSkills.KnowledgeNature),
            "ЛАЗ" => GetSkillMark(ClassSkills.Climbing),
            "МЕХ" => GetSkillMark(ClassSkills.Mechanics),
            "МЕД" => GetSkillMark(ClassSkills.FirstAid),
            "ПЛА" => GetSkillMark(ClassSkills.Swimming),
            "СКР" => GetSkillMark(ClassSkills.Stealth),
            _ => null
        } ?? string.Empty);

    private string GetSkillMark(ClassSkills skill) =>
        Character.IsClassSkill(skill) ? "+" : string.Empty;
        
    
    private const string OriginalAvatarBase64 =
        "/9j/4AAQSkZJRgABAQEAYABgAAD/4QBmRXhpZgAATU0AKgAAAAgABAEaAAUAAAABAAAAPgEbAAUAAAABAAAARgEoAAMAAAABAAIAAAExAAIAAAAQAAAATgA" +
        "AAAAAAABgAAAAAQAAAGAAAAABcGFpbnQubmV0IDUuMC45AP/bAEMAAwICAwICAwMDAwQDAwQFCAUFBAQFCgcHBggMCgwMCwoLCw0OEhANDhEOCwsQFhAREx" +
        "QVFRUMDxcYFhQYEhQVFP/bAEMBAwQEBQQFCQUFCRQNCw0UFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFP/AABEIA" +
        "AEAAQMBEgACEQEDEQH/xAAfAAABBQEBAQEBAQAAAAAAAAAAAQIDBAUGBwgJCgv/xAC1EAACAQMDAgQDBQUEBAAAAX0BAgMABBEFEiExQQYTUWEHInEUMoGR" +
        "oQgjQrHBFVLR8CQzYnKCCQoWFxgZGiUmJygpKjQ1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoOEhYaHiImKkpOUlZaXmJmaoqOkpaa" +
        "nqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4eLj5OXm5+jp6vHy8/T19vf4+fr/xAAfAQADAQEBAQEBAQEBAAAAAAAAAQIDBAUGBwgJCgv/xAC1EQ" +
        "ACAQIEBAMEBwUEBAABAncAAQIDEQQFITEGEkFRB2FxEyIygQgUQpGhscEJIzNS8BVictEKFiQ04SXxFxgZGiYnKCkqNTY3ODk6Q0RFRkdISUpTVFVWV1hZW" +
        "mNkZWZnaGlqc3R1dnd4eXqCg4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2dri4+Tl5ufo6ery8/T19vf4+fr/" +
        "2gAMAwEAAhEDEQA/AP1TooA//9k=";
}