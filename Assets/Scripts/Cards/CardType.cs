namespace Cards
{
    public enum CardType
    {
        Enemy,
        Event,
        Pickup,
        Health,
        Trap,
        Door,
        Player,
    }

    public static class CardTypes
    {
        public static readonly CardType[] FreelySpawnable =
            new[] {CardType.Enemy, CardType.Event, CardType.Pickup, CardType.Health};
    }
}