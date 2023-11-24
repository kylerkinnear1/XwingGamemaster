using XwingTurnRunner.Infrastructure;

namespace XwingTurnRunner.XWingStateMachine.Phases;

public class PlanningPhase
{
    private readonly GameContext _game;
    private readonly IBus _bus;

    public PlanningPhase(GameContext game, IBus bus)
    {
        _game = game;
        _bus = bus;
    }

    public async Task Run()
    {
        var movementGroups = _game.Players
            .SelectMany(x =>
                x.AliveShips.GroupBy(y => y.Card.Pilot.Skill))
            .OrderBy(x => x.Key);

       
        var alreadyMovedShips = new HashSet<ShipModel>();
        var playerToShips = _game.Players
            .SelectMany(
                player => player.Ships.Select(ship => new { ship, player }))
            .ToLookup(x => x.player, x => x.ship);
        foreach (var group in movementGroups)
        {
            // TODO: This isn't right. Active player should have initiative. Loop until no candidate ships
            // haven't been added to alreadyMovedShips.
            var activePlayer = _game.Players.First();
            var candidates = playerToShips[activePlayer].Where(x => !alreadyMovedShips.Contains(x)).ToList();
            if (!candidates.Any())
            {
                activePlayer = _game.Players.Single(x => x != activePlayer);
                continue;
            }

            var shipChoice = await _bus.Send(new SelectShipToActivateRequest(activePlayer, candidates));
            alreadyMovedShips.Add(shipChoice);
        }

        throw new NotImplementedException();
    }

}

public record SelectShipToActivateRequest(Player ActivePlayer, IEnumerable<ShipModel> CandidateShips) : IRequest<ShipModel>;