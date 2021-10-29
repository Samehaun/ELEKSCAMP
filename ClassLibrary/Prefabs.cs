using System.Collections.Generic;

namespace ELEKSUNI
{
    class Prefabs
    {
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
            { new Spot(Keys.Lake) },
            { new Spot(Keys.Ravine) },
            { new Spot(Keys.DryRiver) },
            { new Spot(Keys.Creek) },
            { new Spot(Keys.Crater) },
            { new Spot(Keys.Cave) },
            { new Spot(Keys.ThermalSprings) },
            { new Spot(Keys.Stump) },
            { new Spot(Keys.SwordInStone) },
            { new Spot(Keys.Swamp) },
            { new Spot(Keys.Burrow) },
            { new Spot(Keys.ForesterHouse) },
            { new Spot(Keys.Glade) },
            { new Spot(Keys.WolfPit) },
            { new Spot(Keys.PitHouse) },
            { new Spot(Keys.GingerbreadHouse) },
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
