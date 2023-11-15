using XwingTurnRunner.XWingStateMachine.Obstacles;
using XwingTurnRunner.XWingStateMachine.Phases;
using XwingTurnRunner.XWingStateMachine.Pilots;

namespace XwingTurnRunner.XWingStateMachine;

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
    public int ObstacleCount => 6;
    public List<Obstacle> ObstaclePool { get; set; }
}

public class Game
{
    private readonly GameContext _context;
    private readonly SetupPhase _setup;
    private readonly PlanningPhase _planning;
    private readonly MovementPhase _movement;
    private readonly CombatPhase _combat;
    private readonly CleanupPhase _cleanup;

    public Game(
        GameContext context,
        SetupPhase setup,
        PlanningPhase planning,
        MovementPhase movement,
        CombatPhase combat,
        CleanupPhase cleanup)
    {
        _context = context;
        _setup = setup;
        _planning = planning;
        _movement = movement;
        _combat = combat;
        _cleanup = cleanup;
    }

    public async Task RunGame()
    {
        await _setup.Run();
        while (_context.Players.Any(x => x.Ships.Any(y => y.HullRemaining > 0)))
        {
            await _planning.Run();
            await _movement.Run();
            await _combat.Run();
            await _cleanup.Run();
        }
    }
}