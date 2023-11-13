using XwingTurnRunner.XWingStateMachine.Cards;

namespace XwingTurnRunner.XWingStateMachine;

public record Ship(ShipCard Card, int ShieldsRemaining, int HullRemaining, int StressAssigned);