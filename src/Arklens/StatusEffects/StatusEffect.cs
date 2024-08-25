using Arklens.Alids;

namespace Arklens.StatusEffects;

[AlidDomain]
public abstract record StatusEffect : IAlidEntity
{
    //public abstract void Apply(Creature creature);
    //public abstract void Dispel(Creature creature);
    public Alid Alid { get; }

    protected StatusEffect()
    {
        Alid = Alid.CreateOwnFor<StatusEffect>(GetType().Name);
    }
}