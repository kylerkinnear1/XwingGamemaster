using MediatR;
using XwingTurnRunner.XWingStuff.Cards;
using XwingTurnRunner.XWingStuff.Phases;
using XwingTurnRunner.XWingStuff.Ships;

namespace XwingTurnRunner.XWingStuff;

public record Player(
    List<ShipCard> Cards,
    List<Ship> Ships)
{
    public int TotalPoints => Cards.Select(x => x.Pilot.Points).Sum();
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
    public Player[] Players { get; set; }
    public List<Obstacle> Obstacles { get; set; }
    public List<ShipCard> Ships { get; set; }
}

public class Game
{
    private readonly Random _random;
    private readonly IMediator _mediator;
    private readonly GameContext _context;
    private readonly SetupPhase _setup;
    private readonly PlanningPhase _planning;
    private readonly MovementPhase _movement;

    public async Task RunGame()
    {
        await _setup.Run();
        while (_context.Players.Any(x => x.Ships.Any(y => y.HullRemaining > 0)))
        {

        }
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