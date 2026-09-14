public class Player
{
    public string Name;
    public int CurrentHitPoints;
    public int MaximumHitPoints;
    public List<Item> Inventory = new List<Item>();
    public Weapon? CurrentWeapon;
    public bool InFight;

    public Player(string name, int maximumHitPoints)
    {
        Name = name;
        MaximumHitPoints = maximumHitPoints;
        CurrentHitPoints = maximumHitPoints;
    }

    public void AddItem(Item item)
    {
        Inventory.Add(item);
        Console.WriteLine($"{item.Name} was added to your inventory.");
    }

    public void ShowInventory()
    {
        if (Inventory.Count == 0)
        {
            Console.WriteLine("Your inventory is empty.");
            return;
        }

        Console.WriteLine("Inventory:");
        foreach (Item item in Inventory)
        {
            if (item == CurrentWeapon)
            {
                Console.WriteLine($"- {item.Name} (equipped)");
            }
            else
            {
                Console.WriteLine($"- {item.Name}");
            }
        }
    }

    public void SwitchWeapon(string weaponName)
    {
        if (InFight)
        {
            Console.WriteLine("You can't switch weapons during a fight.");
            return;
        }

        foreach (Item item in Inventory)
        {
            if (item is Weapon weapon && weapon.Name.ToLower() == weaponName.ToLower())
            {
                CurrentWeapon = weapon;
                Console.WriteLine($"You are now using the {weapon.Name}.");
                return;
            }
        }

        Console.WriteLine($"You don't have a weapon called {weaponName}.");
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
