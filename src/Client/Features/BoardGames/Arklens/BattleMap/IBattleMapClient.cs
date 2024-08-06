namespace Client.Features.BoardGames.Arklens.BattleMap;

public interface IBattleMapClient
{
    public Task AddCharacter(BattleMapCharacterModel character, CancellationToken ct = default);
    public Task StartTurn(string character, CancellationToken ct = default);
}