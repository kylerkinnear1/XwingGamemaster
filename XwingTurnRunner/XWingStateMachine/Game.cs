using XwingTurnRunner.Infrastructure;
using XwingTurnRunner.XWingStateMachine.Obstacles;
using XwingTurnRunner.XWingStateMachine.Phases;
using XwingTurnRunner.XWingStateMachine.Pilots;

namespace XwingTurnRunner.XWingStateMachine;

public interface IGameState
{
    IEnumerable<Player> Players { get; }
    IEnumerable<Obstacle> Obstacles { get; }
    IEnumerable<ShipCard> Ships { get; }
    Board Board { get; }
}

// TODO: Clean this up. Not a fan currently.
public record GameState(GameContext Context) : IGameState
{
    public IEnumerable<Player> Players => Context.Players;
    public IEnumerable<Obstacle> Obstacles => Context.Obstacles;
    public IEnumerable<ShipCard> Ships => Context.Ships;
    public Board Board => Context.Board;
}

public class GameContext
{
    public Player[] Players { get; set; }
    public List<Obstacle> Obstacles { get; set; } = new();
    public List<ShipCard> Ships { get; set; } = new();
    public int ObstacleCount => 6;

    public Board Board { get; set; }
    
    public GameContext(Player player1, Player player2, Board? board = null)
    {
        Players = new [] {player1, player2};
        Board = board ?? new();
    }
}

public interface IGame
{
    IBus Bus { get; }
    IGameState State { get; }
}

public class Game : IGame
{
    public IBus Bus { get; }
    public IGameState State { get; }
    public GameContext Context { get; }
    
    private readonly SetupPhase _setup;
    private readonly PlanningPhase _planning;
    private readonly MovementPhase _movement;
    private readonly CombatPhase _combat;
    private readonly CleanupPhase _cleanup;

    public Game(
        IBus bus,
        GameContext context,
        SetupPhase setup,
        PlanningPhase planning,
        MovementPhase movement,
        CombatPhase combat,
        CleanupPhase cleanup)
    {
        Bus = bus;
        Context = context;
        State = new GameState(context);
        _setup = setup;
        _planning = planning;
        _movement = movement;
        _combat = combat;
        _cleanup = cleanup;

        Bus.Subscribe<NewGameRequestedEvent>(Run);
    }

    public async Task Run(NewGameRequestedEvent evnt)
    {
        await _setup.Run();
        while (Context.Players.Any(x => x.Ships.Any(y => y.HullRemaining > 0)))
        {
            await _planning.Run();
            await _movement.Run();
            await _combat.Run();
            await _cleanup.Run();
        }
    }
}

public record NewGameRequestedEvent;

public interface IGameFactory
{
    Game StartNew(GameContext context);
}

public class GameFactory : IGameFactory
{
    public Game StartNew(GameContext context)
    {
        var gameBus = new Bus();
        return new(
            gameBus,
            context,
            new(context, gameBus),
            new(context, gameBus),
            new(),
            new(),
            new());
    }
}