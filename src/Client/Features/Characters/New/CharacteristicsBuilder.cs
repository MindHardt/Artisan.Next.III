using System.Collections;

namespace Client.Features.Characters.New;

public record CharacteristicsBuilder(CharacterBuilder Character) : IEnumerable<KeyValuePair<string, int>>
{
    private static readonly (int Min, int Max) DefaultRange = (-3, 3);
    private static readonly string[] Emojis =
    [
        "💪", "🏃", "🧡", "🧠", "🦉", "👄"
    ];

    private readonly int[] _values = new int[6];

    public int this[string emoji]
    {
        get => _values[Array.IndexOf(Emojis, emoji)];
        set => _values[Array.IndexOf(Emojis, emoji)] = value;
    }

    public int? PointsLimit => Character.CharacteristicLimit;
    public int? PointsLeft => PointsLimit - _values.Sum();

    public int Str { get => _values[0]; set => _values[0] = value; }
    public int Dex { get => _values[1]; set => _values[1] = value; }
    public int Con { get => _values[2]; set => _values[2] = value; }
    public int Int { get => _values[3]; set => _values[3] = value; }
    public int Wis { get => _values[4]; set => _values[4] = value; }
    public int Cha { get => _values[5]; set => _values[5] = value; }

    public bool IncreaseForbidden(string emoji)
        => this[emoji] >= DefaultRange.Max || PointsLeft <= 0;

    public bool DecreaseForbidden(string emoji)
        => this[emoji] <= DefaultRange.Min;

    public void TryIncrease(string emoji)
    {
        if (IncreaseForbidden(emoji) is false)
        {
            this[emoji]++;
        }
    }

    public void TryDecrease(string emoji)
    {
        if (DecreaseForbidden(emoji) is false)
        {
            this[emoji]--;
        }
    }

    public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        => _values.Select((value, index) => KeyValuePair.Create(Emojis[index], value)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}