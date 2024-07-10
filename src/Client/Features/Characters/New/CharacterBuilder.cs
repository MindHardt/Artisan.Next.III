using Arklens;
using Arklens.Classes;
using Arklens.Races;

namespace Client.Features.Characters.New;

public record CharacterBuilder
{
    public CharacterBuilder()
    {
        Characteristics = new CharacteristicsBuilder(this);
    }

    public string? Name { get; set; }

    private Race? _race;
    public Race? Race
    {
        get => _race;
        set
        {
            _race = value;
            _raceImpactsOverride = null;
        }
    }

    private RaceImpacts? _raceImpactsOverride;
    private Class? _class;

    public Class? Class
    {
        get => _class;
        set
        {
            Subclass = null;
            _class = value;
        }
    }

    public RaceImpacts? RaceImpacts
    {
        get => Race?.Impacts ?? _raceImpactsOverride;
        set
        {
            if (Race?.Impacts is not null)
            {
                return;
            }

            _raceImpactsOverride = value;
        }
    }

    public Gender? Gender { get; set; }
    public Alignment? Alignment { get; set; }

    public IReadOnlyCollection<Alignment> AllowedAlignments =>
        Class?.AllowedAlignments ??
        Subclass?.AllowedAlignments ??
        Alignment.AllValues;

    public Subclass? Subclass { get; set; }

    public int? CharacteristicLimit { get; set; }
    public string? AvatarBase64 { get; set; }
    public CharacteristicsBuilder Characteristics { get; }

    public bool IsClassSkill(ClassSkills skill) =>
        Class?.ClassSkills.HasFlag(skill) is true;
}