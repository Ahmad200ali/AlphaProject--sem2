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
            while(true )
            {
                Console.WriteLine($"{monster.Name} : {monster.CurrentHitPoints}/{monster.MaximumHitPoints}");
                Console.WriteLine($"Player : {player.CurrentHitPoints}/{player.MaximumHitPoints}");

                string choice = "";
                Random random = new Random();
                int chance; 
                int damage = 0;
                string typeofattack = "";
    
                while (choice != "1" && choice != "2"  && choice != "3" && choice != "4" ){
                Console.WriteLine("Choose a way to attack:");
                Console.WriteLine($"[1] Fast attack with {player.CurrentWeapon.Name} with a chance of 80 % with damage of {player.CurrentWeapon.MaximumDamage}  ");
                Console.WriteLine($"[2] Normal attack with {player.CurrentWeapon.Name} with a chance of 60 % with damage of {2 * player.CurrentWeapon.MaximumDamage}  ");
                Console.WriteLine($"[3] Hard attack with {player.CurrentWeapon.Name} with a chance of 40 % with damage of {4 * player.CurrentWeapon.MaximumDamage}  ");
                Console.WriteLine($"[4] Flee from {monster.Name}");

                choice = Console.ReadLine();

                chance = random.Next(1, 101);
            
                typeofattack = choice switch
                    {
                        "1" => "Fast attack",
                        "2" => "Normal attack",
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
                    // monster.CurrentHitPoints = monster.MaximumHitPoints;
                    //player.CurrentHitPoints = player.MaximumHitPoints
                    return false;
                }
                else if(choice != "1" && choice != "2"  && choice != "3" && choice != "4" )
                    {
                        Console.WriteLine("Not a valid input!");
                        continue;
                    }

                }
                if (damage == 0)
                {
                    Console.WriteLine($"Player tries a {typeofattack} but misses {monster.Name}");
                }
                else
                {
                    Console.WriteLine($"Player does a {typeofattack} with {damage} damage to {monster.Name}");
                }
                monster.CurrentHitPoints -= damage;
                if (monster.CurrentHitPoints <= 0 )
                {
                    
                    return true;
                }

                Console.WriteLine($"{monster.Name} does {monster.MaximumDamage} damage to Player");
                player.CurrentHitPoints -= monster.MaximumDamage;

                if (player.CurrentHitPoints <= 0 )
                {
                    player.CurrentHitPoints = 0;
                    return false;
                }

                    
                
            }



        }


    }