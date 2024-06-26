using Arklens.Alids;

namespace Arklens.Items;

[AlidDomain]
public abstract record Item
{
    public abstract Money? Price { get; }
}