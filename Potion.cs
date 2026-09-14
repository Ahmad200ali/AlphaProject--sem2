public class Potion : Item
{
    public int AmountToHeal;
    public int UsesLeft;
    public bool OnlyInFight;

    public Potion(int id, string name, int amountToHeal, int usesLeft, bool onlyInFight) : base(id, name)
    {
        AmountToHeal = amountToHeal;
        UsesLeft = usesLeft;
        OnlyInFight = onlyInFight;
    }
}
