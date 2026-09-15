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
    public Weapon? Reward;
    public Monster MonsterTarget;

    public Quest(int id, string name, string description, int fightingLocationID, Monster monsterTarget, Weapon? reward = null)
    {
        ID = id;
        Name = name;
        Description = description;
        FightingLocationID = fightingLocationID;
        MonsterTarget = monsterTarget;
        Reward = reward;
    }

    public void StartQuest(Player player)
    {
        if (IsCompleted) { Console.WriteLine("This quest has already been completed!"); return; }
        if (!IsActive)
        {
            IsActive = true;
            player.CurrentLocation = World.LocationByID(FightingLocationID);
            Console.WriteLine($"\n=== QUEST: {Name} ===");
            Console.WriteLine(Description);
            Console.WriteLine($"Your goal: Kill {RequiredKillCount} {MonsterTarget.Name}(s)");
            Console.WriteLine($"Location: {player.CurrentLocation.Name}");
        }
        ContinueQuest(player);
    }

    private void ContinueQuest(Player player)
    {
        while (KillCount < RequiredKillCount && !player.IsDead())
        {
            Monster enemy = new Monster(MonsterTarget.ID, MonsterTarget.Name, MonsterTarget.MaximumDamage, MonsterTarget.MaximumHitPoints, MonsterTarget.MaximumHitPoints);
            Console.WriteLine($"\nA wild {enemy.Name} appears! Type 'attack' or 'flee'.");
            // wating for Batle class fight method
            if ()
            {
                if (!player.IsDead()) 
                {
                    Console.WriteLine("You fled from the monster. Quest paused.");
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
        if (Reward != null)
        {
            player.AddItem(Reward);
            player.CurrentWeapon = Reward;
            Console.WriteLine($"You received and equipped {Reward.Name} ({Reward.MaximumDamage} damage)!");
        }
    }
}
