using XwingTurnRunner.XWingStuff.Cards;

namespace XwingTurnRunner.XWingStuff.Ships;

public record Ship(ShipCard Card, int ShieldsRemaining, int HullRemaining, int StressAssigned);