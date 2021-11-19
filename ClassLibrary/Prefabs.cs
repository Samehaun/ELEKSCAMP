using System.Collections.Generic;

namespace ELEKSUNI
{
    class Prefabs
    {
        public Clothes simpleClothes;
        public Clothes heavyClothes;

        public Weapon sharpStick;
        public Weapon knife;
        public Weapon sharpKnife;
        public Weapon oldAxe;
        public Weapon axe;

        public Item meteor;
        public Item wolfSkin;
        public Item wolfTeeth;
        public Item leftShoe;
        public Item flint;
        public Item harePaw;
        public Item purse;

        public Consumable antidote;
        public Consumable bondage;
        public Consumable meat;
        public Consumable berries;
        public Consumable poisonBerries;
        public Consumable mushrooms;
        public Consumable poisonMushrooms;
        public Consumable trap;
        public Consumable hornetNest;

        public NPC witch;
        public NPC forester;
        public NPC leprechaun;
        public NPC bear;
        public NPC wolf;
        public NPC hare;

        private List<Spot> spots;
        public List<Spot> GetPrefabs()
        {
            return spots;
        }
        public void GenerateSpotPrefabs()
        {
            spots = new List<Spot>()
            {
            { new Spot(Keys.Clearing) },
            { new Spot(Keys.OrdinaryForest) },
            { new Spot(Keys.OrdinaryForest) },
            { new Spot(Keys.OrdinaryForest) },
            { new Spot(Keys.Oak) },
            { new Spot(Keys.Oak) },
            { new Spot(Keys.Berries) },
            { new Spot(Keys.Berries) },
            { new Spot(Keys.Berries) },
            { new Spot(Keys.Oak) },
            { new Spot(Keys.Cave) },
            { new Spot(Keys.Lake, bear) },
            { new Spot(Keys.Ravine) },
            { new Spot(Keys.DryRiver) },
            { new Spot(Keys.Creek, wolf) },
            { new Spot(Keys.Crater) },
            { new Spot(Keys.Cave) },
            { new Spot(Keys.ThermalSprings) },
            { new Spot(Keys.Stump, leprechaun) },
            { new Spot(Keys.SwordInStone) },
            { new Spot(Keys.Swamp) },
            { new Spot(Keys.Burrow) },
            { new Spot(Keys.ForesterHouse, forester) },
            { new Spot(Keys.Glade) },
            { new Spot(Keys.WolfPit, sharpStick) },
            { new Spot(Keys.PitHouse) },
            { new Spot(Keys.GingerbreadHouse, witch) },
            { new Spot(Keys.Pine) },
            { new Spot(Keys.Hanged) },
            { new Spot(Keys.Monolith) },
            };
        }
        public void GenerateTestSpotPrefabs()
        {
            spots = new List<Spot>()
            {
            { new Spot(Keys.Glade, poisonMushrooms) },
            { new Spot(Keys.Crater, meteor) },
            { new Spot(Keys.Creek, wolf) },
            { new Spot(Keys.OrdinaryForest, trap) },
            { new Spot(Keys.Hanged, flint) },
            { new Spot(Keys.WolfPit, sharpStick) },
            { new Spot(Keys.ForesterHouse, forester) },
            { new Spot(Keys.Oak, purse) },
            { new Spot(Keys.SwordInStone, mushrooms) },
            { new Spot(Keys.Stump, leprechaun) },
            { new Spot(Keys.Burrow) },
            { new Spot(Keys.Oak, hornetNest) },
            { new Spot(Keys.Lake, bear) },
            { new Spot(Keys.Berries, berries) },
            { new Spot(Keys.Berries, poisonBerries) },
            { new Spot(Keys.GingerbreadHouse, witch) },
            };
        }
        public Prefabs()
        {
            //Clothes
            simpleClothes = new Clothes(Keys.SimpleClothes, 5, 2, Keys.Equip, 5, 10);
            heavyClothes = new Clothes(Keys.HeavyClothes, 20, 3.5, Keys.Equip, 20, 30);
            //Weapons
            sharpStick = new Weapon(Keys.SharpStick, 2, 0.7, Keys.Equip, 15);
            knife = new Weapon(Keys.Knife, 10, 0.2, Keys.Equip, 30);
            sharpKnife = new Weapon(Keys.SharpKnife, 20, 0.2, Keys.Equip, 45);
            oldAxe = new Weapon(Keys.OldAxe, 17, 3.0, Keys.Equip, 40);
            axe = new Weapon(Keys.Axe, 27, 3.3, Keys.Equip, 60);
            //Items
            meteor = new Item(Keys.Meteor, 150, 12.5);
            wolfSkin = new Item(Keys.WolfSkin, 15, 1.5);
            wolfTeeth = new Item(Keys.WolfTeeth, 3, 0.2);
            leftShoe = new Item(Keys.Shoe, 2, 0.5);
            flint = new Item(Keys.Flint, 12, 0.1, Keys.Fire);
            harePaw = new Item(Keys.HarePaw, 7, 0.1);
            purse = new Item(Keys.Purse, 6, 0.2, Keys.Open);
            //Consumables
            antidote = Consumable.CreateCure(9, 0.2);
            bondage = Consumable.CreateBandage(4, 0.2);
            meat = Consumable.CreateFood(Keys.Meat, 5, 1.7, 150);
            berries = Consumable.CreateFood(Keys.Berries, 1, 0.3, 20);
            poisonBerries = Consumable.CreatePoisoned(Keys.Berries, 1, 0.3);
            mushrooms = Consumable.CreateFood(Keys.Mushrooms, 2, 0.3, 40);
            poisonMushrooms = Consumable.CreatePoisoned(Keys.Mushrooms, 2, 0.3);
            trap = Consumable.CreateTrap(40);
            hornetNest = Consumable.CreateHornetNest(30);
            //NPC
            witch = new NPC(Keys.Witch, 250, 10, 35, false, new List<Item>() { antidote });
            forester = new NPC(Keys.Forester, 100, 30, 20, false, new List<Item>() { axe, heavyClothes, bondage });
            leprechaun = new NPC(Keys.Leprechaun, 50, 10, 10, false);
            bear = new NPC(Keys.Bear, 500, 80, 80, true);
            wolf = new NPC(Keys.Wolf, 70, 25, 30, true, new List<Item>() { wolfSkin });
            hare = new NPC(Keys.Hare, 0, 0, 0, false, new List<Item>() { meat, harePaw } );
            

        }
    }
}

