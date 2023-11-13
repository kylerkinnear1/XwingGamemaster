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

        var player = await _mediator.Send(new GetPlayerWithInitiativeRequest(lowestPointPlayer));
        return player;
    }

    private async Task<List<Obstacle>> PlaceObstacles()
    {
        throw new NotImplementedException();
    }

    private async Task PlaceShips()
    {
        throw new NotImplementedException();
    }
}

public record GetPlayerWithInitiativeRequest(Player SelectingPlayer) : IRequest<Player>;