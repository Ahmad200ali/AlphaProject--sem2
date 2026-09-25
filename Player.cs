public class Player
{
    public string Name;
    public int CurrentHitPoints;
    public int MaximumHitPoints;
    public List<Item> Inventory = new List<Item>();
    public Weapon? CurrentWeapon;
    public bool InFight;
    public Location CurrentLocation;
    public List<int> CompletedQuestIDs = new List<int>();
    public List<int> KilledMonsterIDs = new List<int>();
    public int Gold = 30;

    public Player(string name, int maximumHitPoints)
    {
        Name = name;
        MaximumHitPoints = maximumHitPoints;
        CurrentHitPoints = maximumHitPoints;
        CurrentLocation = World.LocationByID(World.LOCATION_ID_HOME);
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

    public void AddGold(int amount)
    {
        Gold += amount;
        Console.WriteLine($"Gold: {Gold}");
    }

    public void ShowGold()
    {
        Console.WriteLine($"Gold: {Gold}");
    }

    public void ShowShop()
    {
        if (CurrentLocation.ID != World.LOCATION_ID_GUARD_POST)
        {
            Console.WriteLine("The shop is at the guard post.");
            return;
        }

        Weapon monsterSword = World.WeaponByID(World.WEAPON_ID_MONSTER_SWORD);
        Console.WriteLine("Guard post shop:");
        Console.WriteLine($"- {monsterSword.Name}: {World.MONSTER_SWORD_PRICE} gold ({monsterSword.MaximumDamage} damage)");
        ShowGold();
    }

    public void BuyItem(string itemName)
    {
        if (CurrentLocation.ID != World.LOCATION_ID_GUARD_POST)
        {
            Console.WriteLine("You can only buy items at the guard post shop.");
            return;
        }

        Weapon monsterSword = World.WeaponByID(World.WEAPON_ID_MONSTER_SWORD);
        if (itemName.ToLower() != monsterSword.Name.ToLower())
        {
            Console.WriteLine("That item is not for sale. Type 'shop' to see what is available.");
            return;
        }

        if (Inventory.Any(item => item.ID == monsterSword.ID))
        {
            Console.WriteLine("You already own the Monster Sword.");
            return;
        }

        if (Gold < World.MONSTER_SWORD_PRICE)
        {
            Console.WriteLine($"The Monster Sword costs {World.MONSTER_SWORD_PRICE} gold. You have {Gold} gold.");
            return;
        }

        Gold -= World.MONSTER_SWORD_PRICE;
        AddItem(monsterSword);
        CurrentWeapon = monsterSword;
        Console.WriteLine($"You bought and equipped the {monsterSword.Name}! Gold remaining: {Gold}");
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
        return CurrentLocation.ID == World.LOCATION_ID_SPIDER_FIELD;
    }

    public bool HasCompletedQuest(int questID)
    {
        return CompletedQuestIDs.Contains(questID);
    }

    public void ShowLocation()
    {
        Console.WriteLine();
        Console.WriteLine($"You are at: {CurrentLocation.Name}");
        Console.WriteLine(CurrentLocation.Description);

        if (CurrentLocation.ItemLayingHere != null)
        {
            Console.WriteLine($"You see a {CurrentLocation.ItemLayingHere.Name} lying here. Type 'take' to pick it up.");
        }

        if (CurrentLocation.QuestAvailableHere != null && !CurrentLocation.QuestAvailableHere.IsCompleted)
        {
            Console.WriteLine("A quest is available here. Type 'quest' to begin or continue it.");
        }

        ShowExits();
    }

    public void ShowExits()
    {
        Console.WriteLine("You can go:");

        if (CurrentLocation.LocationToNorth != null)
        {
            Console.WriteLine($"- north to {CurrentLocation.LocationToNorth.Name}");
        }
        if (CurrentLocation.LocationToEast != null)
        {
            Console.WriteLine($"- east to {CurrentLocation.LocationToEast.Name}");
        }
        if (CurrentLocation.LocationToSouth != null)
        {
            Console.WriteLine($"- south to {CurrentLocation.LocationToSouth.Name}");
        }
        if (CurrentLocation.LocationToWest != null)
        {
            Console.WriteLine($"- west to {CurrentLocation.LocationToWest.Name}");
        }
    }

    public void MoveTo(string direction)
    {
        if (InFight)
        {
            Console.WriteLine("You can't move during a fight.");
            return;
        }

        Location? newLocation = null;

        if (direction == "north")
        {
            newLocation = CurrentLocation.LocationToNorth;
        }
        else if (direction == "east")
        {
            newLocation = CurrentLocation.LocationToEast;
        }
        else if (direction == "south")
        {
            newLocation = CurrentLocation.LocationToSouth;
        }
        else if (direction == "west")
        {
            newLocation = CurrentLocation.LocationToWest;
        }
        else
        {
            Console.WriteLine($"'{direction}' is not a direction. Use north, east, south or west.");
            return;
        }

        if (newLocation == null)
        {
            Console.WriteLine($"You can't go {direction} from here.");
            ShowExits();
            return;
        }

        if (!CanEnter(newLocation))
        {
            Console.WriteLine("The guard stops you. Clear the farmer's field and the alchemist's garden first.");
            return;
        }

        CurrentLocation = newLocation;
        ShowLocation();
    }

    public bool CanEnter(Location location)
    {
        // the guard lets you over the bridge (and to the spider forest) when both quests are done
        if (location.ID == World.LOCATION_ID_BRIDGE || location.ID == World.LOCATION_ID_SPIDER_FIELD)
        {
            if (HasCompletedQuest(World.QUEST_ID_CLEAR_FARMERS_FIELD) && HasCompletedQuest(World.QUEST_ID_CLEAR_ALCHEMIST_GARDEN))
            {
                return true;
            }
            return false;
        }

        return true;
    }

    public void TakeItem()
    {
        if (CurrentLocation.ItemLayingHere == null)
        {
            Console.WriteLine("There is nothing here to pick up.");
            return;
        }

        AddItem(CurrentLocation.ItemLayingHere);
        CurrentLocation.ItemLayingHere = null;
    }

    public void Heal(int amount)
    {
        int oldHitPoints = CurrentHitPoints;
        CurrentHitPoints += amount;
        if (CurrentHitPoints > MaximumHitPoints)
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        Console.WriteLine($"You recovered {CurrentHitPoints - oldHitPoints} health.");
        ShowHealth();
    }
}
