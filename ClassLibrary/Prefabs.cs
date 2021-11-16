using System.Collections.Generic;

namespace ELEKSUNI
{
    class Prefabs
    {
        public Clothes simpleClothes = new Clothes(Keys.SimpleClothes, 5, 2, Keys.Equip, 0, 10);
        public Clothes heavyClothes = new Clothes(Keys.HeavyClothes, 40, 3.5, Keys.Equip, 20, 30);

        public Weapon sharpStick = new Weapon(Keys.SharpStick, 2, 0.7, Keys.Equip, 15);
        public Weapon knife = new Weapon(Keys.Knife, 15, 0.2, Keys.Equip, 30);
        public Weapon sharpKnife = new Weapon(Keys.SharpKnife, 25, 0.2, Keys.Equip, 45);
        public Weapon oldAxe = new Weapon(Keys.OldAxe, 20, 3.0, Keys.Equip, 40);
        public Weapon axe = new Weapon(Keys.Axe, 55, 3.3, Keys.Equip, 60);

        public NPC witch = new NPC(Keys.Witch, 250, 10, 35, false, new List<Item>() { Consumable.CreateCure(15, 0.2)});
        public NPC forester = new NPC(Keys.Forester, 100, 30, 20, false, new List<Item>() { Consumable.CreateBandage(10, 0.2) , new Weapon(Keys.Axe, 55, 3.3, Keys.Equip, 60), new Clothes(Keys.HeavyClothes, 40, 3.5, Keys.Equip, 20, 30) } );
        public NPC leprechaun = new NPC(Keys.Leprechaun, 50, 10, 10, false);
        public NPC bear = new NPC(Keys.Bear, 500, 80, 80, true);
        public NPC wolf = new NPC(Keys.Wolf, 70, 25, 30, true);
        private List<Spot> spots;
        public List<Spot> GetPrefabs()
        {
            return spots;
        }
        private void GenerateSpotPrefabs()
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
        public Prefabs()
        {
            GenerateSpotPrefabs();
        }
    }
}