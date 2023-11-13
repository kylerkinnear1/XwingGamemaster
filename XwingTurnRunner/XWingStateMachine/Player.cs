using XwingTurnRunner.XWingStateMachine.Cards;

namespace XwingTurnRunner.XWingStateMachine;

public record Player(
    List<ShipCard> Cards,
    List<Ship> Ships)
{
    public int TotalPoints => Cards.Select(x => x.Pilot.Points).Sum();
}