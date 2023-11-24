using MoreLinq;
using System.Drawing;
using XwingTurnRunner.Infrastructure;
using XwingTurnRunner.XWingStateMachine.Obstacles;

namespace XwingTurnRunner.XWingStateMachine.Phases;

public class SetupPhase
{
    private readonly GameContext _context;
    private readonly IBus _bus;

    public SetupPhase(GameContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
    }

    public async Task Run()
    {
        var playerWithInitiative = await SelectInitiative();
        _context.Players = new[] { playerWithInitiative, _context.Players.Single(x => x != playerWithInitiative) };
        await PlaceObstacles();
        await PlaceShips();
    }

    private async Task<Player> SelectInitiative()
    {
        var lowestPointPlayer = _context.Players
            .Shuffle()
            .OrderBy(x => x.TotalPoints)
            .First();

        var player = await _bus.Send(new SelectInitiativeRequest(lowestPointPlayer));
        return player;
    }

    private async Task PlaceObstacles()
    {
        var selectingPlayer = _context.Players.First();
        for (var i = 0; i < _context.ObstacleCount; i++)
        {
            var placement = await _bus.Send(new PlaceObstacleRequest(selectingPlayer, _context.ObstaclePool));
            _context.ObstaclePool.Remove(placement.PlacedObstacle);
            _context.Obstacles.Add(placement.PlacedObstacle);
            selectingPlayer = _context.Players.Single(x => x != selectingPlayer);
        }
    }

    private async Task PlaceShips()
    {
        var selectingPlayer = _context.Players.First();
        var shipPool = _context.Players.SelectMany(x => x.Ships).ToHashSet();
        while (shipPool.Any())
        {
            var request = new PlaceShipRequest(selectingPlayer.Ships.Where(shipPool.Contains).ToList(), selectingPlayer);
            var placedShip = await _bus.Send(request);
            _context.Board.Ships.Add(placedShip);
            shipPool.Remove(placedShip);
        }
    }
}

public record SelectInitiativeRequest(Player SelectingPlayer) : IRequest<Player>;

public record ObstaclePlacement(Obstacle PlacedObstacle, Point Location);
public record PlaceObstacleRequest(Player SelectingPlayer, List<Obstacle> ObstaclePool) : IRequest<ObstaclePlacement>;

public record PlaceShipRequest(List<ShipModel> AvailableShips, Player SelectingPlayer) : IRequest<ShipModel>;