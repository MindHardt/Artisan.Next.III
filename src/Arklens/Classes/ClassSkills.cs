namespace Arklens.Classes;

[Flags]
public enum ClassSkills : short
{
    Acrobatics = 1 << 0,
    HorseRiding = 1 << 1,
    Survival = 1 << 2,
    Diplomacy = 1 << 3,
    KnowledgeMagic = 1 << 4,
    KnowledgeWorld = 1 << 5,
    KnowledgeReligion = 1 << 6,
    KnowledgeDungeons = 1 << 7,
    KnowledgeNature = 1 << 8,
    Climbing = 1 << 9,
    Mechanics = 1 << 10,
    FirstAid = 1 << 11,
    Swimming = 1 << 12,
    Stealth = 1 << 13,
}