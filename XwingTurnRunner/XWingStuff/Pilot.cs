using static XwingTurnRunner.XWingStuff.Pilot;

namespace XwingTurnRunner.XWingStuff;

public record Pilot(
    int Skill,
    int Points,
    Assignment Assignments)
{
    public class Assignment
    {
        public int Stress { get; set; } = 0;
        public List<CriticalHit> CriticalHits { get; set; } = new();
    }
}

public record CriticalHit(Type Type);

public enum CriticalHitType
{
    DirectDamage
}

public record Ship(Pilot Pilot, ShipType Type, int Hull, int Shields, int ShieldsRemaining, int HealthRemaining);
public enum ShipType
{
    T65, T70, Z95
}
