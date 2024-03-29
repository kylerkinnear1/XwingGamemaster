﻿using XwingTurnRunner.Game.Obstacles;
using XwingTurnRunner.Game.Phases;
using XwingTurnRunner.Game.Pilots;
using XwingTurnRunner.Infrastructure;

namespace XwingTurnRunner.Game;

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
    IGameState State { get; }
}

public class Game : IGame
{
    public IGameState State { get; }
    public GameContext Context { get; }

    private readonly IBus _bus;
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
        _bus = bus;
        Context = context;
        _setup = setup;
        _planning = planning;
        _movement = movement;
        _combat = combat;
        _cleanup = cleanup;

        State = new GameState(context);
        _bus.Subscribe<NewGameRequestedEvent>(Run);
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
    Game Create(GameContext context);
}

public class GameFactory : IGameFactory
{
    private readonly IBus _bus;

    public GameFactory(IBus bus) => _bus = bus;

    public Game Create(GameContext context)
    {
        return new(
            _bus,
            context,
            new(context, _bus),
            new(context, _bus),
            new(),
            new(),
            new());
    }
}