using MediatR;
using MoreLinq;

namespace XwingTurnRunner.XWingStuff;

public record Player(
    int Id,
    List<Ship> Ships)
{
    public int TotalPoints => Ships.Select(x => x.Pilot.Points).Sum();
}

public class GameContext
{
    public Player Player1 { get; }
    public Player Player2 { get; }
    public Player PlayerWithInitiave { get; set; }
    public List<Obstacle> Obstacles { get; set; }
}

public class Game
{
    private readonly Random _random;
    private readonly IMediator _mediator;
    private readonly GameContext _context;

    public Game(Random random, IMediator mediator, GameContext context)
    {
        _random = random;
        _mediator = mediator;
        _context = context;
    }

    public async Task RunGame()
    {
        await SetupGame();
    }

    // Phases:
    // Start
    // Planning
    // Movement
    // Combat
    private async Task SetupGame()
    {
        _context.PlayerWithInitiave = await SelectInitiative();
        await PlaceRocks();
        await PlaceShips();

    }
    private async Task<Player> SelectInitiative()
    {
        var lowestPointPlayer = new[] { _context.Player1, _context.Player2 }
            .Shuffle()
            .OrderBy(x => x.TotalPoints)
            .First();

        var player = await _mediator.Send(new GetPlayerWithInitiativeRequest(lowestPointPlayer));
        return player;
    }

    private async Task PlaceRocks() => throw new NotImplementedException();
    private async Task PlaceShips() => throw new NotImplementedException();
}

public record GetPlayerWithInitiativeRequest(Player SelectingPlayer) : IRequest<Player>;