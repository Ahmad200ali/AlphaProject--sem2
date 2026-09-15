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
    else if (command == "north")
    {
        player.MoveTo("north");
    }
    else if (command == "east")
    {
        player.MoveTo("east");
    }
    else if (command == "south")
    {
        player.MoveTo("south");
    }
    else if (command == "west")
    {
        player.MoveTo("west");
    }
    else if (command == "go")
    {
        if (argument == "")
        {
            Console.WriteLine("Go where? Use north, east, south or west.");
        }
        else
        {
            player.MoveTo(argument);
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
    Console.WriteLine("help - show the commands");
    Console.WriteLine("quit - stop playing");
}
