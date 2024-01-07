using XwingTurnRunner.Game.Pilots;

namespace XwingTurnRunner.Game;

public record ShipModel(ShipCard Card, int ShieldsRemaining, int HullRemaining, int StressAssigned);