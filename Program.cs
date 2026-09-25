Console.WriteLine("Welcome to the game!");
Console.Write("What is your name? ");
string? nameInput = Console.ReadLine();

string playerName = "Hero";
if (nameInput != null && nameInput.Trim() != "")
{
    playerName = nameInput.Trim();
}

Player player = new Player(playerName, 20);

// Starting gear
player.AddItem(World.WeaponByID(World.WEAPON_ID_RUSTY_SWORD));
player.SwitchWeapon("Rusty sword");
player.AddItem(World.PotionByID(World.POTION_ID_HEALING_POTION));

Console.WriteLine();
Console.WriteLine($"Welcome, {player.Name}! Monsters are bothering the town.");
Console.WriteLine("Clear the alchemist's garden and the farmer's field, then the guard lets you cross the bridge.");
Console.WriteLine("Collect the spider silk in the spider forest to win the game.");

ShowHelp();
player.ShowLocation();

bool playing = true;
while (playing)
{
    Console.WriteLine();
    Console.Write("> ");
    string? input = Console.ReadLine();

    if (input == null)
    {
        break;
    }

    input = input.Trim().ToLower();
    if (input == "")
    {
        continue;
    }

    // first word is the command, the rest is what it applies to
    string command = input;
    string argument = "";
    int spaceIndex = input.IndexOf(' ');
    if (spaceIndex != -1)
    {
        command = input.Substring(0, spaceIndex);
        argument = input.Substring(spaceIndex + 1).Trim();
    }

    if (command == "help")
    {
        ShowHelp();
    }
    else if (command == "look")
    {
        player.ShowLocation();
    }
    else if (command == "north" || command == "east" || command == "south" || command == "west")
    {
        Move(command);
    }
    else if (command == "go")
    {
        if (argument == "")
        {
            Console.WriteLine("Go where? Use north, east, south or west.");
        }
        else
        {
            Move(argument);
        }
    }
    else if (command == "inventory" || command == "i")
    {
        player.ShowInventory();
    }
    else if (command == "health")
    {
        player.ShowHealth();
    }
    else if (command == "gold")
    {
        player.ShowGold();
    }
    else if (command == "shop")
    {
        player.ShowShop();
    }
    else if (command == "buy")
    {
        if (argument == "")
        {
            Console.WriteLine("Buy what? For example: buy monster sword");
        }
        else
        {
            player.BuyItem(argument);
        }
    }
    else if (command == "equip")
    {
        if (argument == "")
        {
            Console.WriteLine("Equip what? For example: equip club");
        }
        else
        {
            player.SwitchWeapon(argument);
        }
    }
    else if (command == "use")
    {
        if (argument == "")
        {
            Console.WriteLine("Use what? For example: use healing potion");
        }
        else
        {
            player.UsePotion(argument);
        }
    }
    else if (command == "take")
    {
        player.TakeItem();
    }
    else if (command == "quest")
    {
        if (player.CurrentLocation.QuestAvailableHere == null)
        {
            Console.WriteLine("There is no quest available here.");
        }
        else
        {
            player.CurrentLocation.QuestAvailableHere.StartQuest(player);
            if (!player.IsDead() && !player.HasWonGame())
            {
                player.ShowLocation();
            }
        }
    }
    else if (command == "quit")
    {
        playing = false;
    }
    else
    {
        Console.WriteLine($"Unknown command '{command}'. Type 'help' to see the commands.");
    }

    if (player.IsDead())
    {
        playing = false;
    }

    if (player.HasWonGame())
    {
        Console.WriteLine("You completed every quest and defeated every monster. You won!");
        playing = false;
    }
}

Console.WriteLine("Thanks for playing!");

void Move(string direction)
{
    Location oldLocation = player.CurrentLocation;
    player.MoveTo(direction);

    // ask to start the quest when you arrive at a quest giver
    Quest? quest = player.CurrentLocation.QuestAvailableHere;
    if (player.CurrentLocation == oldLocation || quest == null || quest.IsCompleted)
    {
        return;
    }

    string startOrContinue = quest.IsActive ? "continue" : "start";
    Console.Write($"Do you want to {startOrContinue} the quest '{quest.Name}'? (yes/no) ");
    string? answer = Console.ReadLine();
    if (answer != null && (answer.Trim().ToLower() == "yes" || answer.Trim().ToLower() == "y"))
    {
        quest.StartQuest(player);
        if (!player.IsDead() && !player.HasWonGame())
        {
            player.ShowLocation();
        }
    }
    else
    {
        Console.WriteLine("Okay, come back later and type 'quest' to start it.");
    }
}

void ShowHelp()
{
    Console.WriteLine();
    Console.WriteLine("Commands:");
    Console.WriteLine("north, east, south, west - move (or: go north)");
    Console.WriteLine("look - show where you are");
    Console.WriteLine("take - pick up the item here");
    Console.WriteLine("quest - begin or continue the quest at this location");
    Console.WriteLine("inventory - show your items");
    Console.WriteLine("equip <weapon> - switch weapon");
    Console.WriteLine("use <potion> - use a potion");
    Console.WriteLine("health - show your health");
    Console.WriteLine("gold - show your gold");
    Console.WriteLine("shop - see items for sale at the guard post");
    Console.WriteLine("buy <item> - buy an item from the shop");
    Console.WriteLine("help - show the commands");
    Console.WriteLine("quit - stop playing");
}
