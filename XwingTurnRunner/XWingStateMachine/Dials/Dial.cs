namespace XwingTurnRunner.XWingStateMachine.Dials;

public record Dial(IEnumerable<Maneuver> Maneuvers);

public enum Direction { Left, Right, Straight }
public enum Bearing { Hard, Bank, KTurn, Sloop, Stop, Reverse }
public record Maneuver(int Speed, Direction Direction, Bearing Bearing);
