public class Player
{
    public string Name;
    public int CurrentHitPoints;
    public int MaximumHitPoints;
    public List<Item> Inventory = new List<Item>();
    public Weapon? CurrentWeapon;
    public bool InFight;
    public Location? CurrentLocation;
    public List<int> CompletedQuestIDs = new List<int>();
    public List<int> KilledMonsterIDs = new List<int>();

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

    public void UsePotion(string potionName)
    {
        foreach (Item item in Inventory)
        {
            if (item is Potion potion && potion.Name.ToLower() == potionName.ToLower())
            {
                if (potion.OnlyInFight && !InFight)
                {
                    Console.WriteLine($"You can only use the {potion.Name} during a fight.");
                    return;
                }

                Heal(potion.AmountToHeal);
                potion.UsesLeft--;

                if (potion.UsesLeft == 0)
                {
                    Inventory.Remove(potion);
                    Console.WriteLine($"The {potion.Name} is used up.");
                }
                else
                {
                    Console.WriteLine($"The {potion.Name} has {potion.UsesLeft} uses left.");
                }
                return;
            }
        }

        Console.WriteLine($"You don't have a potion called {potionName}.");
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

    public void CompleteQuest(Quest quest)
    {
        if (!CompletedQuestIDs.Contains(quest.ID))
        {
            CompletedQuestIDs.Add(quest.ID);
        }
    }

    public void AddKilledMonster(Monster monster)
    {
        if (!KilledMonsterIDs.Contains(monster.ID))
        {
            KilledMonsterIDs.Add(monster.ID);
        }
    }

    public bool HasWonGame()
    {
        // all quests done
        foreach (Quest quest in World.Quests)
        {
            if (!CompletedQuestIDs.Contains(quest.ID))
            {
                return false;
            }
        }

        // every monster killed at least once
        foreach (Monster monster in World.Monsters)
        {
            if (!KilledMonsterIDs.Contains(monster.ID))
            {
                return false;
            }
        }

        // and you have to be in the spider forest
        return CurrentLocation != null && CurrentLocation.ID == World.LOCATION_ID_SPIDER_FIELD;
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
