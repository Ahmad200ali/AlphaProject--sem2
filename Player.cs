public class Player
{
    public string Name;
    public int CurrentHitPoints;
    public int MaximumHitPoints;

    public Player(string name, int maximumHitPoints)
    {
        Name = name;
        MaximumHitPoints = maximumHitPoints;
        CurrentHitPoints = maximumHitPoints;
    }

    public void ShowHealth()
    {
        Console.WriteLine($"Health: {CurrentHitPoints}/{MaximumHitPoints}");
    }

    public void TakeDamage(int damage)
    {
        CurrentHitPoints -= damage;
        if (CurrentHitPoints < 0)
        {
            CurrentHitPoints = 0;
        }

        Console.WriteLine($"You lost {damage} health.");
        ShowHealth();

        if (IsDead())
        {
            Console.WriteLine("Your health is 0. Game over!");
        }
    }

    public bool IsDead()
    {
        return CurrentHitPoints == 0;
    }

    public void Heal(int amount)
    {
        CurrentHitPoints += amount;
        if (CurrentHitPoints > MaximumHitPoints)
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        Console.WriteLine($"You recovered {amount} health.");
        ShowHealth();
    }
}
