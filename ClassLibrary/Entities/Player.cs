using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    class Player
    {
        private double staminaRegenRate;
        private double stamina;
        private double hunger;
        public Keys? recentlyUsedItemEffect;
        public string Name { get; private set; }
        public int Health { get; private set; }
        public Weapon CurrentWeapon { get; set; }
        public Clothes CurrentClothes { get; set; }
        public List<Keys> Effects { get; private set; }
        private Inventory inventory;
        private double hungerModifier;
        public Player()
        {
            staminaRegenRate = 3;
            Health = 100;
            stamina = 100;
            hunger = 0;
            inventory = new Inventory();
            Effects = new List<Keys>();
        }
        public Player(string playerName) : this()
        {
            Name = playerName;
        }
        public void AddEffect(Keys effect)
        {
            if (!Effects.Contains(effect))
            {
                Effects.Add(effect);
            }
        }
        public double Rest()
        {
            double time = CalculateStaminaNeededToTravel() / staminaRegenRate;
            if (stamina < 100)
            {
                stamina += CalculateStaminaNeededToTravel();
            }
            hungerModifier = 1.3;
            InnerStateProcess(time);
            return time;
        }
        public double Sleep()
        {
            stamina = 100;
            double time = 100 / (staminaRegenRate * 3);
            hungerModifier = 1;
            InnerStateProcess(time);
            return time;
        }
        public double CalculateTimeNeededToTravel()
        {
            double time = (int)MainQuestConfig.BaseTimeToChangeLocation * ((int)MainQuestConfig.BasePlayerSpeed / Speed());
            return time;
        }
        public double CalculateStaminaNeededToTravel()
        {
            return CalculateTimeNeededToTravel() * (int)MainQuestConfig.BasePlayerStaminaConsuption;
        }
        public void RecaculateStateDueToTraveling(int additionalStaminaConsumptionModifier = 1)
        {
            stamina -= CalculateStaminaNeededToTravel() * additionalStaminaConsumptionModifier;
            hungerModifier = 2;
            InnerStateProcess(CalculateTimeNeededToTravel() * additionalStaminaConsumptionModifier);
        }
        private double Speed()
        {
            double speed = (int)MainQuestConfig.BasePlayerSpeed;
            double maxWeight = (int)MainQuestConfig.MaxWeightPlayerCanCarry;
            if (maxWeight < inventory.Weight)
            {
                speed *= Math.Sqrt(maxWeight / inventory.Weight);
            }
            return speed;
        }
        public void Equip()
        {
            if (inventory.CurrentItem is Weapon)
            {
                CurrentWeapon = (Weapon)inventory.CurrentItem;
            }
            else
            {
                CurrentClothes = (Clothes)inventory.CurrentItem;
            }
        }
        public void InnerStateProcess(double time)
        {
            hunger += time * hungerModifier;
            if(hunger >= 100)
            {
                AddEffect(Keys.Starve);
            }
            Health -= (hunger < 100) ? 0 : (int) (hunger / 100 * time);
        }
        public void TakeHit(int attack)
        {
            if(CurrentClothes == null)
            {
                Health -= attack;
            }
            else if(CurrentClothes.Defence < attack)
            {
                Health -= (attack - CurrentClothes.Defence);
            }
        }
        public void Eat(bool isPoisoned)
        {
            if (!isPoisoned)
            {
                Consumable food = (Consumable)inventory.CurrentItem;
                hunger -= food.EffectPower;
                if(hunger < 100)
                {
                    Effects.Remove(Keys.Starve);
                }
            }
            else
            {
                AddEffect(Keys.IsPoisoned);
                recentlyUsedItemEffect = Keys.GetPoisonMessage;
            }
            inventory.DropSelected();
        }
        public void TakeAntidote()
        {
            Effects.Remove(Keys.IsPoisoned);
            recentlyUsedItemEffect = Keys.CurePoison;
            inventory.DropSelected();
        }
        public void Unequip()
        {
            if (CurrentClothes == inventory.CurrentItem)
            {
                CurrentClothes = null;
            }
            else if (CurrentWeapon == inventory.CurrentItem)
            {
                CurrentWeapon = null;
            }
        }
        public void Drop()
        {
            Unequip();
            inventory.DropSelected();
        }
        public void Sell(int itemToSellIndex, NPC buyer)
        {
            Item itemToSell = inventory.GetItem(itemToSellIndex);
            inventory.AddMoney(itemToSell.Price);
            inventory.CurrentItem = itemToSell;
            Unequip();
            buyer.Buy(itemToSell);
            inventory.DropSelected();
        }
        public void Buy(int itemToBuyIndex, NPC owner)
        {
            Item itemToBuy = owner.GetItem(itemToBuyIndex);
            if (inventory.Coins >= itemToBuy.Price)
            {
                inventory.AddMoney(-itemToBuy.Price);
                Take(itemToBuyIndex, owner);
            }
        }
        public void Take(int itemToTakeIndex, NPC owner)
        {
            Item itemToTake = owner.GetItem(itemToTakeIndex);
            inventory.Add(itemToTake);
            owner.Drop(itemToTake);
        }
        public void Steal(int itemToStealIndex, NPC owner)
        {
            Take(itemToStealIndex, owner);
            owner.IsHostile = true;
        }
        public void PickItem(Item item)
        {
            inventory.Add(item);
        }
        public int HowMuchCoins()
        {
            return inventory.Coins;
        }
        public Item GetActiveItem()
        {
            return inventory.CurrentItem;
        }
        public List<Item> GetListOfItemsInInventory()
        {
            return inventory.GetItemList();
        }
        public void SelectItemToOperate(int index)
        {
            inventory.CurrentItem = inventory.GetItem(index);
        }
        public void Open()
        {
            inventory.AddMoney(inventory.CurrentItem.Price * 2);
            inventory.DropSelected();
        }
        public bool HasItem(Item item)
        {
            return inventory.GetItemList().Contains(item);
        }
        public PlayerSave Save()
        {
            return new PlayerSave(this, inventory, stamina, hunger);
        }
        public void Load(PlayerSave save, Prefabs prefabs)
        {
            stamina = save.Stamina;
            hunger = save.Hunger;
            Name = save.Name;
            Health = save.Health;
            if(save.CurrentClothes != null)
            {
                CurrentClothes = (Clothes)prefabs.GetItemByKey((Keys)save.CurrentClothes);
            }
            if(save.CurrentWeapon != null)
            {
                CurrentWeapon = (Weapon)prefabs.GetItemByKey((Keys)save.CurrentWeapon);
            }
            Effects.AddRange(save.Effects);
            inventory.Load(save.Inventory, prefabs);
        }
    }
    struct PlayerSave
    {
        public double Stamina { get; set; }
        public double Hunger { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public Keys? CurrentWeapon { get; set; }
        public Keys? CurrentClothes { get; set; }
        public List<Keys> Effects { get; set; }
        public InventorySave Inventory { get; set; }
        public PlayerSave(Player player, Inventory inventory, double stamina, double hunger)
        {
            Hunger = hunger;
            Stamina = stamina;
            Name = player.Name;
            Health = player.Health;
            if (player.CurrentClothes != null)
            {
                CurrentClothes = player.CurrentClothes.Name;
            }
            else
            {
                CurrentClothes = null;
            }
            if(player.CurrentWeapon != null)
            {
                CurrentWeapon = player.CurrentWeapon.Name;
            }
            else
            {
                CurrentWeapon = null;
            }
            Effects = new List<Keys>();
            Effects.AddRange(player.Effects);
            Inventory = inventory.Save();
        }
    }
}