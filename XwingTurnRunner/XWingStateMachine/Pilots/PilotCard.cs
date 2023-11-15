using XwingTurnRunner.XWingStateMachine.Dials;

namespace XwingTurnRunner.XWingStateMachine.Pilots;

public record PilotCard(int Skill, int Points);

public record ShipCard(PilotCard Pilot, ShipType Type, Dial Dial, int MaxHull, int MaxShields);

public enum ShipType
{
    T65,
    T70,
    Z95
}