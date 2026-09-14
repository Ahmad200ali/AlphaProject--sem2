public class Weapon : Item
{
    public int MaximumDamage;

    public Weapon(int id, string name, int maximumDamage) : base(id, name)
    {
        MaximumDamage = maximumDamage;
    }
}
