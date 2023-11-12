using MediatR;
using MoreLinq;

namespace XwingTurnRunner.XWingStuff;

public record Player(
    int Id,
    List<Ship> Ships)
{
    public int TotalPoints => Ships.Select(x => x.Pilot.Points).Sum();
}

// TODO: Maybe make a reusable scripting engine for future Tactics RPG Game Engine:
// Try to keep logic and display separate
// Use 'EventBus' (reusing Mediator for now) to communicate between UI and Logic Engine
// Request inputs via script
// Use Subscriptions specified in Game Context? Figure out 'Game' subscribes to use input.
// Could also do ISubscribe<T>. See what feels good when developing. Add an end game eventually.
// IPhase Run might be part or the scripting engine?
// Maybe change 'Phase' to 'State' and the scripting engine is a state machine scripting engine.
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
    private readonly SetupPhase _setup;

    public async Task RunGame()
    {
        await _setup.RunSetup();
        await SetupGame();
    }

    // Phases:
    // Setup

    // Start
    // Planning
    // Movement
    // Combat
    private async Task SetupGame()
    {
        // Loop through phases?
    }
}

public class SetupPhase
{
    private readonly GameContext _context;
    private readonly IMediator _mediator;

    public async Task RunSetup()
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


public class PlanningPhase
{
    public async Task RunPlanning() => throw new NotImplementedException();
}

public class MovementPhase
{
    public async Task RunPlanning() => throw new NotImplementedException();
}

public record GetPlayerWithInitiativeRequest(Player SelectingPlayer) : IRequest<Player>;