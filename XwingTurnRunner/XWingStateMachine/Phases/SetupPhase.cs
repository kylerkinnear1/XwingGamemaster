using System.Drawing;
using MediatR;
using MoreLinq;
using XwingTurnRunner.XWingStateMachine.Obstacles;

namespace XwingTurnRunner.XWingStateMachine.Phases;

public class SetupPhase
{
    private readonly GameContext _context;
    private readonly IMediator _mediator;

    public SetupPhase(GameContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
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

        var player = await _mediator.Send(new SelectInitiativeRequest(lowestPointPlayer));
        return player;
    }

    private async Task PlaceObstacles()
    {
        var selectingPlayer = _context.Players.First();
        for (int i = 0; i < _context.ObstacleCount; i++)
        {
            var placement = await _mediator.Send(new PlaceObstacleRequest(selectingPlayer, _context.ObstaclePool));
            _context.ObstaclePool.Remove(placement.PlacedObstacle);
            _context.Obstacles.Add(placement.PlacedObstacle);
            selectingPlayer = _context.Players.Single(x => x != selectingPlayer);
        }
    }

    private async Task PlaceShips()
    {
        throw new NotImplementedException();
    }
}

public record SelectInitiativeRequest(Player SelectingPlayer) : IRequest<Player>;

public record ObstaclePlacement(Obstacle PlacedObstacle, Point Location);
public record PlaceObstacleRequest(Player SelectingPlayer, List<Obstacle> ObstaclePool) : IRequest<ObstaclePlacement>;