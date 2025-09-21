using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private GameEngine engine;
        private Player player;

        public Form1()
        {
            InitializeComponent();

           
            player = new Player(strength: 10, stamina: 8, agility: 6, intellect: 5);
            engine = new GameEngine(player);

            
            engine.AddEnemy(new Enemy("Гоблін", 1, 50, 8, 3, 20, 10));
            engine.AddEnemy(new Enemy("Скелет", 2, 70, 12, 5, 40, 20));

         
            engine.AddEvent(new Event("Попався на пастку!", 10));
            engine.AddEvent(new Event("Знайшов отруйний гриб!", 5));

            //UI here
        }
    }

    
    public class Weapon
    {
        public string Name;
        public int Price;
        public int Attack;

        public Weapon(string name, int price, int attack)
        {
            Name = name;
            Price = price;
            Attack = attack;
        }
    }

    public class Armor
    {
        public string Name;
        public int Price;   
        public int Defense;

        public Armor(string name, int price, int defense)
        {
            Name = name;
            Price = price;
            Defense = defense;
        }
    }


    public class Player
    {
        private int strength;
        private int stamina;
        private int agility;
        private int intellect;

        private int health;
        private int mana;
        private int money;
        private int level;
        private int experience;

        private Weapon weapon;
        private Armor armor;

        public Player(int strength, int stamina, int agility, int intellect)
        {
            this.strength = strength;
            this.stamina = stamina;
            this.agility = agility;
            this.intellect = intellect;
            health = 100;
            mana = 50;
            money = 100;
            level = 1;
            experience = 0;
        }

        public int Health => health;
        public int Mana => mana;
        public int Money => money;
        public int Level => level;
        public int Experience => experience;

        public int CalculateAttack()
        {
            int weaponAttack = weapon != null ? weapon.Attack : 0;
            return strength + weaponAttack;
        }

        public int CalculateDefense()
        {
            int armorDefense = armor != null ? armor.Defense : 0;
            return stamina + armorDefense;
        }

        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health < 0) health = 0;
        }

        public void UseMana(int amount)
        {
            mana -= amount;
            if (mana < 0) mana = 0;
        }

        public void AddExperience(int amount)
        {
            experience += amount;
            if (experience >= level * 100)
            {
                level++;
                experience = 0;
                health += 20;
                mana += 10;
            }
        }

        public void AddMoney(int amount)
        {
            money += amount;
        }

        public void EquipWeapon(Weapon w)
        {
            weapon = w;
        }

        public void EquipArmor(Armor a)
        {
            armor = a;
        }
    }


    public class Enemy
    {
        public string Name;
        public int Level;
        public int Health;
        public int Attack;
        public int Defense;
        public int ExperienceReward;
        public int MoneyReward;

        public Enemy(string name, int level, int health, int attack, int defense, int expReward, int moneyReward)
        {
            Name = name;
            Level = level;
            Health = health;
            Attack = attack;
            Defense = defense;
            ExperienceReward = expReward;
            MoneyReward = moneyReward;
        }

        public int PerformAttack()
        {
            return Attack;
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health < 0) Health = 0;
        }
    }

    public class Event
    {
        public string Description;  
        public int Damage;

        public Event(string description, int damage)
        {
            Description = description;
            Damage = damage;
        }

        public void Apply(Player player)
        {
            player.TakeDamage(Damage);
        }
    }


    public class Shop
    {
        public List<Weapon> Weapons { get; } = new List<Weapon>();
        public List<Armor> Armors { get; } = new List<Armor>();

        public void GenerateItems()
        {
            Weapons.Add(new Weapon("Меч", 50, 10));
            Weapons.Add(new Weapon("Сокира", 70, 15));

            Armors.Add(new Armor("Шкіряна броня", 40, 5));
            Armors.Add(new Armor("Залізна броня", 80, 12));
        }

        public void BuyItem(Player player, Weapon weapon)
        {
            if (player.Money >= weapon.Price)
            {
                player.AddMoney(-weapon.Price);
                player.EquipWeapon(weapon);
            }
        }

        public void BuyItem(Player player, Armor armor)
        {
            if (player.Money >= armor.Price)
            {
                player.AddMoney(-armor.Price);
                player.EquipArmor(armor);
            }
        }
    }

    public class GameEngine
    {
        private Player player;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Event> events = new List<Event>();
        private Shop shop = new Shop();

        public GameEngine(Player player)
        {
            this.player = player;
            shop.GenerateItems();
        }

        public void AddEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        public void AddEvent(Event e)
        {
            events.Add(e);
        }

        public void PlayerTurn(Enemy enemy)
        {
            int damage = player.CalculateAttack() - enemy.Defense;
            if (damage < 0) damage = 0;
            enemy.TakeDamage(damage);
        }

        public void EnemyTurn(Enemy enemy)
        {
            int damage = enemy.PerformAttack() - player.CalculateDefense();
            if (damage < 0) damage = 0;
            player.TakeDamage(damage);
        }

        public void TriggerEvent(Event e)
        {
            e.Apply(player);
        }

        public Shop GetShop()
        {
            return shop;
        }

        public Player GetPlayer()
        {
            return player;
        }

        public List<Enemy> GetEnemies()
        {
            return enemies;
        }

        public List<Event> GetEvents()
        {
            return events;
        }
    }
}
