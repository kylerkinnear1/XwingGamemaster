﻿using XwingTurnRunner.XWingStateMachine.Obstacles;
using XwingTurnRunner.XWingStateMachine.Pilots;

namespace XwingTurnRunner.XWingStateMachine;

public record Player(
    List<ShipCard> Cards,
    List<ShipModel> Ships,
    List<Obstacle> ObstaclePool)
{
    public int TotalPoints => Cards.Select(x => x.Pilot.Points).Sum();
    public IEnumerable<ShipModel> AliveShips => Ships.Where(x => x.HullRemaining > 0);
}