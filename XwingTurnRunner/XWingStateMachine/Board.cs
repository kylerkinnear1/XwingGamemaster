using XwingTurnRunner.XWingStateMachine.Obstacles;

namespace XwingTurnRunner.XWingStateMachine;

public record Board(
    List<Obstacle> Obstacles,
    List<ShipModel> Ships,
    List<Bomb> Bombs,
    List<Mine> Mines)
{
    public Board() : this(new(), new(), new(), new()) { }
}

public record Bomb;
public record Mine;