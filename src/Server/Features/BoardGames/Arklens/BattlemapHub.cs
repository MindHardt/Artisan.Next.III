using Client.Features.BoardGames.Arklens.BattleMap;
using Microsoft.AspNetCore.SignalR;

namespace Server.Features.BoardGames.Arklens;

public class BattleMapHub : Hub<IBattleMapClient>
{
    public async Task AddCharacter(BattleMapCharacterModel character)
    {
        await Clients.All.AddCharacter(character);
    }
}