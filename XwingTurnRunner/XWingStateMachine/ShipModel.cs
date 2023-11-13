using XwingTurnRunner.XWingStateMachine.Cards;

namespace XwingTurnRunner.XWingStateMachine;

public record ShipModel(ShipCard Card, int ShieldsRemaining, int HullRemaining, int StressAssigned);