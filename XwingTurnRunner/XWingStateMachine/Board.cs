using System.Drawing;
using XwingTurnRunner.XWingStateMachine.Obstacles;

namespace XwingTurnRunner.XWingStateMachine;

public record Bounds(List<Point> Points);
public record Piece<T>(Bounds Bounds, T Value);

public record Board(
    Bounds BoardEdge,
    List<Piece<Obstacle>> Obstacles,
    List<Piece<ShipModel>> Ships,
    List<Piece<Bomb>> Bombs,
    List<Piece<Mine>> Mines);

    public record Bomb;
    public record Mine;