using XwingTurnRunner.Game.Obstacles;

namespace XwingTurnRunner.Game;

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