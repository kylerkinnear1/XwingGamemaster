using XwingTurnRunner.XWingStateMachine.Pilots;

namespace XwingTurnRunner.XWingStateMachine;

public record ShipModel(ShipCard Card, int ShieldsRemaining, int HullRemaining, int StressAssigned);