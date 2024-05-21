using System.Collections;
using Arklens.Races;

namespace Client.Features.Characters.New;

public record CharacteristicsBuilder(CharacterBuilder _character) : IEnumerable<KeyValuePair<string, int>>
{
    private static readonly (int Min, int Max) DefaultRange = (-3, 3);
    private static readonly string[] Emojis =
    [
        "💪", "🏃", "🧡", "🧠", "🦉", "👄"
    ];
    
    private readonly CharacterBuilder _character = _character;
    private readonly int[] _values = new int[6];

    public int this[string emoji]
    {
        get => _values[Array.IndexOf(Emojis, emoji)];
        set => _values[Array.IndexOf(Emojis, emoji)] = value;
    }

    public int? PointsLimit => _character.CharacteristicLimit;
    public int? PointsLeft => PointsLimit - _values.Sum();

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