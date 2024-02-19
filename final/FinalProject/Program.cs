using System;

// Character class
public class Character
{
    // Properties
    public string Name { get; set; }
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public int Defense { get; set; }

    // Constructor
    public Character(string name, int health, int attackPower, int defense)
    {
        Name = name;
        Health = health;
        AttackPower = attackPower;
        Defense = defense;
    }

    // Methods
    public virtual void Attack(Character target)
    {
        int damage = AttackPower - target.Defense;
        if (damage < 0)
            damage = 0;

        target.TakeDamage(damage);
        Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage.");
    }

    public virtual void Defend()
    {
        // Implementation for character defend
        Console.WriteLine($"{Name} defends against the attack.");
    }

    public void TakeDamage(int damage)
    {
        // Implementation for character take damage
        Health -= damage;
        if (Health < 0)
            Health = 0;

        Console.WriteLine($"{Name} takes {damage} damage. Remaining health: {Health}");
    }
}

// Derived classes: Warrior, Mage, Rogue
public class Warrior : Character
{
    // Constructor
    public Warrior(string name, int health, int attackPower, int defense)
        : base(name, health, attackPower, defense)
    {
        // Additional constructor logic for Warrior class
    }

    // Additional methods or overrides specific to Warrior class
}

public class Mage : Character
{
    // Constructor
    public Mage(string name, int health, int attackPower, int defense)
        : base(name, health, attackPower, defense)
    {
        // Additional constructor logic for Mage class
    }

    // Additional methods or overrides specific to Mage class
}

public class Rogue : Character
{
    // Constructor
    public Rogue(string name, int health, int attackPower, int defense)
        : base(name, health, attackPower, defense)
    {
        // Additional constructor logic for Rogue class
    }

    // Additional methods or overrides specific to Rogue class
}

// Enemy class
public class Enemy : Character
{
    // Constructor
    public Enemy(string name, int health, int attackPower, int defense)
        : base(name, health, attackPower, defense)
    {
    }

    // Methods
    public override void Attack(Character target)
    {
        int damage = AttackPower - target.Defense;
        if (damage < 0)
            damage = 0;

        target.TakeDamage(damage);
        Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage.");
    }

    public override void Defend()
    {
        // Implementation for enemy defend
        Console.WriteLine($"{Name} defends against the attack.");
    }
}

// Battle class
public class Battle
{
    // Methods
    public void StartBattle(Character player, Character enemy)
    {
        // Implementation for starting a battle
        Console.WriteLine($"A battle starts between {player.Name} and {enemy.Name}!");

        while (player.Health > 0 && enemy.Health > 0)
        {
            player.Attack(enemy);
            if (enemy.Health <= 0)
            {
                Console.WriteLine($"{enemy.Name} has been defeated!");
                break;
            }

            enemy.Attack(player);
            if (player.Health <= 0)
            {
                Console.WriteLine($"{player.Name} has been defeated!");
                break;
            }
        }
    }

    public void EndBattle()
    {
        // Implementation for ending a battle
        Console.WriteLine("The battle has ended.");
    }

    public bool CheckVictory(Character player, Character enemy)
    {
        // Implementation for checking victory condition
        return player.Health > 0 && enemy.Health <= 0;
    }
}

// Game class
public class Game
{
    // Methods
    public void StartGame()
    {
        // Implementation for starting the game
        Console.WriteLine("Welcome to the Text-Based RPG!");
    }

    public void MainMenu()
    {
        // Implementation for displaying the main menu
        Console.WriteLine("Main Menu:");
        Console.WriteLine("1. Start Game");
        Console.WriteLine("2. Exit");
        Console.Write("Enter your choice: ");
        int choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
                CharacterSelection();
                break;
            case 2:
                Console.WriteLine("Exiting game...");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                MainMenu();
                break;
        }
    }

    public void CharacterSelection()
    {
        // Implementation for character selection
        Console.WriteLine("Character Selection:");
        Console.WriteLine("1. Warrior");
        Console.WriteLine("2. Mage");
        Console.WriteLine("3. Rogue");
        Console.Write("Choose your character: ");
        int choice = Convert.ToInt32(Console.ReadLine());

        Battle battle = new Battle(); // Create an instance of the Battle class
        switch (choice)
        {
            case 1:
                Warrior playerWarrior = new Warrior("Warrior", 100, 20, 10);
                battle.StartBattle(playerWarrior, GenerateEnemy()); // Call StartBattle from the Battle instance
                break;
            case 2:
                Mage playerMage = new Mage("Mage", 80, 25, 5);
                battle.StartBattle(playerMage, GenerateEnemy()); // Call StartBattle from the Battle instance
                break;
            case 3:
                Rogue playerRogue = new Rogue("Rogue", 90, 15, 15);
                battle.StartBattle(playerRogue, GenerateEnemy()); // Call StartBattle from the Battle instance
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                CharacterSelection();
                break;
        }
    }

    private Enemy GenerateEnemy()
    {
        Random rand = new Random();
        int enemyType = rand.Next(1, 4);
        switch (enemyType)
        {
            case 1:
                return new Enemy("Goblin", 50, 10, 5);
            case 2:
                return new Enemy("Orc", 70, 15, 8);
            case 3:
                return new Enemy("Dragon", 100, 25, 15);
            default:
                return new Enemy("Skeleton", 40, 8, 3);
        }
    }
}

// Main program
class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.StartGame();
        game.MainMenu();
    }
}