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
        Sharpness,
        ShieldEnemy,
        BossEnemy,
    }

    public static class CardTypes
    {
        public static readonly CardType[] FreelySpawnable =
            new[] {CardType.Enemy, CardType.Event, CardType.Pickup, CardType.Health, CardType.Sharpness, CardType.ShieldEnemy, CardType.BossEnemy};
    }
}