public class Quest
{
    public int ID;
    public string Name;
    public string Description;
    public int KillCount;
    public int RequiredKillCount = 3;
    public bool IsCompleted;
    public bool IsActive;
    public int FightingLocationID;
    public List<Item> Rewards = new List<Item>();
    public Monster MonsterTarget;
    public const int GoldReward = 30;

    public Quest(int id, string name, string description, int fightingLocationID, Monster monsterTarget)
    {
        ID = id;
        Name = name;
        Description = description;
        FightingLocationID = fightingLocationID;
        MonsterTarget = monsterTarget;
    }

    public void StartQuest(Player player)
    {
        if (IsCompleted) { Console.WriteLine("This quest has already been completed!"); return; }

        // only one quest at a time
        foreach (Quest quest in World.Quests)
        {
            if (quest.IsActive && quest != this)
            {
                Console.WriteLine($"Finish the quest '{quest.Name}' first.");
                return;
            }
        }

        if (!IsActive)
        {
            IsActive = true;
            Console.WriteLine($"\n=== QUEST: {Name} ===");
            Console.WriteLine(Description);
            Console.WriteLine($"Your goal: Kill {RequiredKillCount} {MonsterTarget.Name}(s)");
        }
        player.CurrentLocation = World.LocationByID(FightingLocationID);
        Console.WriteLine($"Location: {player.CurrentLocation.Name} ({KillCount}/{RequiredKillCount} killed)");
        ContinueQuest(player);
    }

    private void ContinueQuest(Player player)
    {
        while (KillCount < RequiredKillCount && !player.IsDead())
        {
            Monster enemy = new Monster(MonsterTarget.ID, MonsterTarget.Name, MonsterTarget.MaximumDamage, MonsterTarget.MaximumHitPoints, MonsterTarget.MaximumHitPoints);
            bool PlayerHasWon;
            while (true)
            {
                Console.WriteLine($"\nA wild {enemy.Name} appears! Type 'attack' or 'flee'.");
                string? choice = Console.ReadLine();
                if (choice == null)
                {
                    return;
                }
                choice = choice.Trim().ToLower();
                if(choice == "attack")
                {   
                    break;
                }
                else if(choice == "flee")
                {
                    Console.WriteLine("You fled from the monster. Quest paused, type 'quest' at the quest giver to continue.");
                    return;
                }
                Console.WriteLine("Invalid choice! Type 'attack' or 'flee'.");
            }
            Battle battle = new(player,enemy);
            PlayerHasWon = battle.BattleStart();
            if (PlayerHasWon == false)
            {
                if (!player.IsDead()) 
                {
                    Console.WriteLine("You fled from the monster. Quest paused, type 'quest' at the quest giver to continue.");
                }
                return;
            }
            KillCount++;
            player.AddKilledMonster(enemy);
            Console.WriteLine($"[{enemy.Name} killed: {KillCount}/{RequiredKillCount}]");
        }
        if (KillCount >= RequiredKillCount) CompleteQuest(player);
    }


    public void CompleteQuest(Player player)
    {
        if (IsCompleted) return;
        IsCompleted = true;
        IsActive = false;
        player.CompleteQuest(this);
        if (!World.HowManyQuestCompleted.Contains(this))
        {
            World.HowManyQuestCompleted.Add(this);
        }
        Console.WriteLine($"\n=== QUEST COMPLETED: {Name} ===");
        Console.WriteLine($"You received {GoldReward} gold for completing this quest!");
        player.AddGold(GoldReward);
        foreach (Item reward in Rewards)
        {
            player.AddItem(reward);
            // only switch when the reward is stronger than what you are holding
            if (reward is Weapon weapon && (player.CurrentWeapon == null || weapon.MaximumDamage > player.CurrentWeapon.MaximumDamage))
            {
                player.CurrentWeapon = weapon;
                Console.WriteLine($"You equipped the {weapon.Name} ({weapon.MaximumDamage} damage)!");
            }
        }
    }
}
