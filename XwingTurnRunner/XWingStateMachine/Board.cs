using System.Drawing;
using XwingTurnRunner.XWingStateMachine.Obstacles;

namespace XwingTurnRunner.XWingStateMachine;

public record Board(
    List<Obstacle> Obstacles,
    List<ShipModel> Ships,
    List<Bomb> Bombs,
    List<Mine> Mines);

public record Bomb;
public record Mine;