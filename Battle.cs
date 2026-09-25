    public class Battle
    {
        private Monster monster;
        private Player player;

        public Battle(Player pplayer,Monster mmonster)
        {
            monster = mmonster;
            player = pplayer;
        }

        public bool BattleStart()
        {
            Console.WriteLine($"Battle has started with {monster.Name}");
            player.InFight = true;
            bool playerWon = Fight();
            player.InFight = false;
            return playerWon;
        }

        private bool Fight()
        {
            Random random = new Random();
            while(true )
            {
                Console.WriteLine();
                Console.WriteLine($"{monster.Name} : {monster.CurrentHitPoints}/{monster.MaximumHitPoints}");
                Console.WriteLine($"{player.Name} : {player.CurrentHitPoints}/{player.MaximumHitPoints}");

                string? choice = "";
                int chance;
                int damage = 0;
                string typeofattack = "";

                while (choice != "1" && choice != "2"  && choice != "3" ){
                Console.WriteLine("Choose a way to attack:");
                Console.WriteLine($"[1] Normal attack with {player.CurrentWeapon.Name} with a chance of 80 % with damage of {player.CurrentWeapon.MaximumDamage}  ");
                Console.WriteLine($"[2] Fast attack with {player.CurrentWeapon.Name} with a chance of 60 % with damage of {2 * player.CurrentWeapon.MaximumDamage}  ");
                Console.WriteLine($"[3] Hard attack with {player.CurrentWeapon.Name} with a chance of 40 % with damage of {4 * player.CurrentWeapon.MaximumDamage}  ");
                Console.WriteLine($"[4] Flee from {monster.Name}");
                Console.WriteLine("[5] Use a potion");

                choice = Console.ReadLine();
                if (choice == null)
                {
                    return false;
                }
                choice = choice.Trim();

                chance = random.Next(1, 101);

                typeofattack = choice switch
                    {
                        "1" => "Normal attack",
                        "2" => "Fast attack",
                        "3" => "Hard attack",
                        _ => ""
                    };

                if (choice == "1" &&  chance <= 80)
                {
                    damage = player.CurrentWeapon.MaximumDamage;

                }
                else if (choice == "2" &&  chance <= 60)
                {
                    damage = player.CurrentWeapon.MaximumDamage * 2 ;
                }
                else if (choice == "3" &&  chance <= 40)
                {
                    damage = player.CurrentWeapon.MaximumDamage * 4;
                }
                else if (choice == "4" )
                {
                    return false;
                }
                else if (choice == "5")
                {
                    // using a potion does not cost a turn
                    Console.Write("Which potion? ");
                    string? potionName = Console.ReadLine();
                    if (potionName != null && potionName.Trim() != "")
                    {
                        player.UsePotion(potionName.Trim());
                    }
                }
                else if(choice != "1" && choice != "2"  && choice != "3" )
                    {
                        Console.WriteLine("Not a valid input!");
                        continue;
                    }

                }
                if (damage == 0)
                {
                    Console.WriteLine($"{player.Name} tries a {typeofattack} but misses {monster.Name}");
                }
                else
                {
                    Console.WriteLine($"{player.Name} does a {typeofattack} with {damage} damage to {monster.Name}");
                }
                monster.CurrentHitPoints -= damage;
                if (monster.CurrentHitPoints <= 0 )
                {
                    Console.WriteLine($"You defeated the {monster.Name}!");
                    return true;
                }

                Console.WriteLine($"{monster.Name} does {monster.MaximumDamage} damage to {player.Name}");
                player.CurrentHitPoints -= monster.MaximumDamage;

                if (player.CurrentHitPoints <= 0 )
                {
                    player.CurrentHitPoints = 0;
                    Console.WriteLine($"The {monster.Name} defeated you. Your health is 0. Game over!");
                    return false;
                }
            }
        }
    }
