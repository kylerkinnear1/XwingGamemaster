namespace XwingTurnRunner.XWingStuff;

// Phases:
// Start
// Planning
// Movement
// Combat

public class GameContext
{
}

public class Game
{

}

public class Turn
{
    public StartPhase Start { get; }
    public PlanningPhase Planning { get; }
    public MovementPhase Movement { get; }
    public CombatPhase Combat { get; }
    public CleanupPhase Cleanup { get; }

}

public interface IPhase
{
    void Run(GameContext context);
}

public class StartPhase
{

}

public class PlanningPhase
{

}

public class MovementPhase
{

}

public class CombatPhase
{

}

public class CleanupPhase
{
}
