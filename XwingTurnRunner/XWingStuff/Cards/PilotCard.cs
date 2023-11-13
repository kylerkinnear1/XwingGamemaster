using static XwingTurnRunner.XWingStuff.Cards.PilotCard;

namespace XwingTurnRunner.XWingStuff.Cards;

public record PilotCard(int Skill, int Points);
public record ShipCard(PilotCard Pilot, ShipType Type, int MaxHull, int MaxShields);
public enum ShipType
{
    T65, T70, Z95
}

public record CriticalHitCard(Type Type);

public enum CriticalHitType
{
    DirectDamage
}