﻿/**
 * Loot
 * 
 * Original game mady by Trent Nagy
 */
using System;
using System.Collections.Generic;
using System.IO;

namespace loot
{
    class Program
    {
        public static List<string> playerInventory = new List<string> { "sword" };
        public static List<string> possibleLoot = new List<string> { "health crystal", "potion" };
        public static Player player = new Player();

        public static int enemyHealth = 0;

        //----Stats----
        public static int explorationsLasted = 0;
        public static int enemiesSlain = 0;
        public static int potionsDrank = 0;
        public static int crystalsUsed = 0;
        public static int goldObtained = 0;
        //----Stats----

        /**
         *  Main 
         */
        static void Main(string[] args)
        {
            MainMenu();
        }

        /**
         * Fancy menu screen
         */
        public static void MainMenu()
        {
            Console.Clear();
            player.Health = 5;
            player.MaxHealth = 5;
            explorationsLasted = 0;
            enemiesSlain = 0;
            potionsDrank = 0;
            crystalsUsed = 0;
            playerInventory.Clear();
            playerInventory.Add("sword");

            string title = "==============================================\n" +
                               "||||||||||||||||||||||||||||||||||||||||||||||\n" +
                               "==============================================\n\n" +
                               "  L           OOO          OOO       TTTTTTT  \n" +
                               "  L          O   O        O   O         T     \n" +
                               "  L         O     O      O     O        T     \n" +
                               "  L         O     O      O     O        T     \n" +
                               "  L          O   O        O   O         T     \n" +
                               "  LLLLLL      OOO          OOO          T     \n\n" +
                               "==============================================\n" +
                               "||||||||||||||||||||||||||||||||||||||||||||||\n" +
                               "==============================================\n";

            Console.WriteLine(title);

            PromptMenu();
        }

        /**
         * Handles menu selection
         */
        public static void PromptMenu()
        {
            Console.WriteLine("New Game");
            Console.WriteLine("Continue");
            Console.WriteLine("Exit\n");

            string menuChoice = Console.ReadLine();

            switch (menuChoice.ToLower())
            {
                //New Game
                case "new game":
                case "new":
                    Console.Clear();
                    Console.WriteLine("While eavesdropping on a conversation in town, you hear of The Dungeon that contains" +
                    "\ntreasure of immeasurable wealth. With the last few gold you have, you buy a sword and armor." +
                    "\nHaving nothing to lose, you enter The Dungeon whilist clutching your sword close to you.\n");

                    PromptUser();
                    break;
                //Exit game
                case "exit":
                    Environment.Exit(0);
                    break;
                case "continue":
                    try
                    {
                        BinaryReader reader = new BinaryReader(new FileStream("savestate", FileMode.Open));
                        explorationsLasted = reader.ReadInt32();
                        enemiesSlain = reader.ReadInt32();
                        potionsDrank = reader.ReadInt32();
                        crystalsUsed = reader.ReadInt32();
                        player.MaxHealth = reader.ReadInt32();
                        player.Health = reader.ReadInt32();

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            playerInventory.Add(reader.ReadString());
                        }

                        Console.Clear();
                        playerInventory.Remove("sword");
                        reader.Close();
                        PromptUser();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.Read();
                    }

                    break;
                //The program will break if this isn't here.
                case "":
                    Console.Clear();
                    MainMenu();
                    break;
                //Unknown selection
                default:
                    Console.WriteLine("\nThat feature hasn't been put in yet!\n");
                    PromptMenu();
                    break;
            }
        }

        /**
         * Asks the player what they want to do.
         */
        public static void PromptUser()
        {
            if (player.Health >= 1)
            {
                Console.WriteLine("You have " + player.Health + " health\n");
                Console.WriteLine("1) View Inventory\n" +
                                  "2) Explore\n" +
                                  "3) View Stats\n" +
                                  "4) Save\n" +
                                  "5) Exit\n");
                string input = Console.ReadLine();

                //Show the inventory, and ask player  if they want to use an item.
                switch (input)
                {
                    //Show the player's inventory, ask if they want to use an item
                    case "1":
                        // Catch an exception if there's a problem with printing out the inventory list.
                        try
                        {
                            Console.WriteLine("\n-----Inventory-----");
                            for (int i = 0; i < playerInventory.Count; i++)
                                Console.WriteLine(playerInventory[i]);

                            Console.WriteLine("-------------------\n");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine("\nSomething bad happened when showing the inventory. \nPlease Contact Trent about this.");
                        }

                        Console.WriteLine("Use item? (y/n)");
                        string useItemYorN = Console.ReadLine();

                        if (useItemYorN.ToLower() == "y")
                        {
                            Console.WriteLine("\nWhich one? (use the item name)");
                            string itemChoice = Console.ReadLine();
                            Console.Clear();

                            switch (itemChoice.ToLower())
                            {
                                //Potion
                                case "potion":
                                    Console.WriteLine("You drink the potion, and feel your wounds begin to heal immediately." +
                                                  "\nYour health is restored to " + player.MaxHealth + "\n");
                                    player.Health = player.MaxHealth;
                                    playerInventory.Remove("potion");
                                    potionsDrank++;
                                    break;
                                //Health crystal
                                case "health crystal":
                                case "crystal":
                                    Console.WriteLine("The crystal responds to your desire for greatness.");
                                    Console.WriteLine("Your max health is increased by 1.\n");
                                    player.MaxHealth++;
                                    playerInventory.Remove("health crystal");
                                    crystalsUsed++;
                                    break;
                                //Sword
                                case "sword":
                                    Console.WriteLine("You swing the sword at the air, hoping to hit something. Nothing happens.\n");
                                    break;
                                //Unknown
                                default:
                                    Console.WriteLine("You look, but cannot find " + itemChoice + " in your inventory.");
                                    break;
                            }
                        }
                        else if (useItemYorN.ToLower() == "n")
                        {
                            Console.Clear();
                            Console.WriteLine("You decide not to use anything.");
                        }

                        PromptUser();
                        break;
                    //Explore the dungeon
                    case "2":
                        Console.WriteLine("\nYou explore the dungeon further.\n");
                        explorationsLasted++;

                        Random rand = new Random();
                        int chance = rand.Next(50);

                        if (chance >= 0 && chance <= 20)
                            FindChest();
                        else if (chance >= 21 && chance <= 40)
                            FindEnemy();
                        else if (chance >= 41 && chance <= 50)
                            FindTrap();

                        PromptUser();
                        break;
                    //Show player stats
                    case "3":
                        Console.WriteLine("\nExplorations lasted: " + explorationsLasted +
                                      "\nEnemies slain: " + enemiesSlain +
                                      "\nPotions Drank: " + potionsDrank +
                                      "\nCrystals used: " + crystalsUsed + "\n");
                        PromptUser();
                        break;
                    //Save the game
                    case "4":
                        try
                        {
                            Console.Clear();
                            Console.WriteLine("You scribble down your adventure so far onto a piece of parchment...");
                            BinaryWriter writer = new BinaryWriter(new FileStream("savestate", FileMode.Create));
                            writer.Write(explorationsLasted);
                            writer.Write(enemiesSlain);
                            writer.Write(potionsDrank);
                            writer.Write(crystalsUsed);
                            writer.Write(player.MaxHealth);
                            writer.Write(player.Health);
                            foreach (string item in playerInventory)
                            {
                                writer.Write(item);
                            }
                            Console.WriteLine("Game saved successfully!\n");
                            writer.Close();
                            PromptUser();
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine(ex.Message + "\n Cannot create file.");
                            Console.Read();
                        }
                        break;
                    //Return to menu
                    case "5":
                        Console.WriteLine("\nYou decide to retire your days of adventuring.");
                        Console.WriteLine("\n\nPress enter to leave the dungeon.");
                        Console.Read();
                        MainMenu();
                        break;
                    //Unknown
                    default:
                        Console.WriteLine("You don't think \"" + input + "\" is a viable option.\n");
                        PromptUser();
                        break;
                }
            }
            else
            {
                Player.Die();
            }
        }

        /**
         * This method activates if the player finds a chest while exploring
         */
        public static void FindChest()
        {
            Random rand = new Random();
            int chance = rand.Next(50);
            Console.Clear();

            if (chance >= 0 && chance <= 10)
            {
                Console.WriteLine("You find a health crystal!\n");
                playerInventory.Add(possibleLoot[0]);
            }
            else if (chance >= 11)
            {
                Console.WriteLine("You find a potion!\n");
                playerInventory.Add(possibleLoot[1]);
            }
        }

        /**
         * This method activates if the player finds an enemy while exploring
         */
        public static void FindEnemy()
        {
            Console.Clear();
            Random rand = new Random();
            int chance = rand.Next(30);

            if (chance >= 0 && chance <= 10)
                InitiateCombat("Goblin");
            else if (chance >= 11 && chance <= 20)
                InitiateCombat("Spider");
            else if (chance >= 21 && chance <= 30)
                InitiateCombat("Skeleton");
        }

        /**
         * This method activated when the player fights an enemy.
         */
        public static void InitiateCombat(string enemy)
        {
            Console.WriteLine("You start a fight with an enemy " + enemy + "!");
            enemyHealth = 3;

            Random rand = new Random();
            int chance = rand.Next(50);

            if (chance >= 0 && chance <= 25)
            {
                Console.WriteLine("Your natural speed lets you attack first.");
                Player.Hit();
                PromptBattle();
            }
            else
            {
                Console.WriteLine("The enemy's speed gives it an advantage.");
                Console.WriteLine("You take 1 point of damage\n");
                player.Health--;

                if(player.Health <= 0)
                {
                    Player.Die();
                }

                PromptBattle();
            }
        }

        public static void PromptBattle()
        {
            if(enemyHealth > 0)
            {
                if(player.Health <= 0)
                {
                    Player.Die();
                }
                Console.WriteLine("What do you do?\n" +
                              "1) Attack enemy\n" +
                              "2) Run Away\n");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Clear();
                    Random rand = new Random();
                    int chance = rand.Next(100);

                    if (chance > 50)
                    {
                        Player.Hit();

                        chance = rand.Next(100);

                        if (chance > 50)
                        {
                            Console.WriteLine("The enemy hits you for 1 damage.\n");
                            player.Health--;
                            PromptBattle();
                        }
                        else if(enemyHealth < 0)
                        {
                            Console.WriteLine("The enemy is defeated!\n");
                            enemiesSlain++;
                            PromptUser();
                        }
                        else
                        {
                            Console.WriteLine("You evade the enemy's attack.\n");
                            PromptBattle();
                        }
                    }
                    else
                    {
                        Console.WriteLine("The enemy dodges your attack.\n");

                        chance = rand.Next(100);

                        if (chance > 50)
                        {
                            Console.WriteLine("The enemy hits you for 1 damage.\n");
                            player.Health--;
                            PromptBattle();
                        }
                        else
                        {
                            Console.WriteLine("You evade the enemy's attack.\n");
                            PromptBattle();
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Random rand = new Random();
                    int chance = rand.Next(100);

                    if(chance > 50)
                    {
                        Console.WriteLine("You successfully run away.");
                        PromptUser();
                    }
                    else
                    {
                        Console.WriteLine("You failed to escape!");
                        Console.WriteLine("The enemy hits your for 1 damage.");
                        player.Health--;
                        PromptBattle();
                    }
                }
            }
            else
            {
                Console.WriteLine("The enemy is defeated!");
                PromptUser();
            }
        }

        public static void FindTrap()
        {
            Console.Clear();
            Console.WriteLine("You accidentally trip a wire trap! Arrows shoot out and hit you for 2 health.");
            player.Health -= 2;
        }
    }
}