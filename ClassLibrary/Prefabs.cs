using System.Collections.Generic;

namespace ELEKSUNI
{
    class Prefabs
    {
        public Clothes simpleClothes = new Clothes(Keys.SimpleClothes, 5, 2, Keys.Equip, 0, 10);
        public Clothes heavyClothes = new Clothes(Keys.HeavyClothes, 40, 3.5, Keys.Equip, 20, 30);
        public Consumable bondage = Consumable.CreateBandage(10, 0.2);

        public NPC witch = new NPC(Keys.Witch, 250, 10, 35, false, new List<Item>() { Consumable.CreateCure(15, 0.2)});
        public NPC forester = new NPC(Keys.Forester, 100, 30, 20, false, new List<Item>() { Consumable.CreateBandage(10, 0.2) });
        public NPC leprechaun = new NPC(Keys.Leprechaun, 50, 20, 10, false);
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
            { new Spot(Keys.Clearing, simpleClothes) },
            { new Spot(Keys.OrdinaryForest, simpleClothes) },
            { new Spot(Keys.OrdinaryForest, simpleClothes) },
            { new Spot(Keys.OrdinaryForest, simpleClothes) },
            { new Spot(Keys.Oak, simpleClothes) },
            { new Spot(Keys.Oak, simpleClothes) },
            { new Spot(Keys.Berries, simpleClothes) },
            { new Spot(Keys.Berries, simpleClothes) },
            { new Spot(Keys.Berries, simpleClothes) },
            { new Spot(Keys.Oak, simpleClothes) },
            { new Spot(Keys.Cave, simpleClothes) },
            { new Spot(Keys.Lake, simpleClothes) },
            { new Spot(Keys.Ravine, simpleClothes) },
            { new Spot(Keys.DryRiver, simpleClothes) },
            { new Spot(Keys.Creek, simpleClothes) },
            { new Spot(Keys.Crater, simpleClothes) },
            { new Spot(Keys.Cave, simpleClothes) },
            { new Spot(Keys.ThermalSprings, simpleClothes) },
            { new Spot(Keys.Stump, simpleClothes) },
            { new Spot(Keys.SwordInStone, simpleClothes) },
            { new Spot(Keys.Swamp, simpleClothes) },
            { new Spot(Keys.Burrow, simpleClothes) },
            { new Spot(Keys.ForesterHouse, simpleClothes) },
            { new Spot(Keys.Glade, simpleClothes) },
            { new Spot(Keys.WolfPit, simpleClothes) },
            { new Spot(Keys.PitHouse, simpleClothes) },
            { new Spot(Keys.GingerbreadHouse, simpleClothes) },
            { new Spot(Keys.Pine, simpleClothes) },
            { new Spot(Keys.Hanged, simpleClothes) },
            { new Spot(Keys.Monolith, simpleClothes) },
            };
        }
        public Prefabs()
        {
            GenerateSpotPrefabs();
        }
    }
}