using XwingTurnRunner.Game.Obstacles;
using XwingTurnRunner.Game.Pilots;

namespace XwingTurnRunner.Game;

public record Player(
    List<ShipCard> Cards,
    List<ShipModel> Ships,
    List<Obstacle> ObstaclePool)
{
    public int TotalPoints => Cards.Select(x => x.Pilot.Points).Sum();
    public IEnumerable<ShipModel> AliveShips => Ships.Where(x => x.HullRemaining > 0);
}