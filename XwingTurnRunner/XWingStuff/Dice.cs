namespace XwingTurnRunner.XWingStuff;

public record Dice<TFace>(IEnumerable<TFace> Faces);

public enum AttackDiceFace { Hit, Miss, Crit, Focus }
public enum DefenseDiceFace { Evade, Miss, Focus }

public record Dice
{
    public static readonly Dice<AttackDiceFace> Attack = new(new[]
    {
        AttackDiceFace.Hit, AttackDiceFace.Hit, AttackDiceFace.Miss, AttackDiceFace.Miss, AttackDiceFace.Focus, AttackDiceFace.Crit
    });

    public static readonly Dice<DefenseDiceFace> Defense = new(new[]
    {
        DefenseDiceFace.Evade, DefenseDiceFace.Evade,DefenseDiceFace.Evade,DefenseDiceFace.Miss,DefenseDiceFace.Miss,DefenseDiceFace.Focus,
    });
}