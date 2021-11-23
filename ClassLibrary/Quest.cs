using System;
using System.Collections.Generic;

namespace ELEKSUNI
{
    enum MainQuestConfig
    {
        BasePlayerSpeed = 5,
        BasePlayerStaminaConsuption = 5,
        MaxWeightPlayerCanCarry = 10,
        BaseTimeToChangeLocation = 3,
        MapSize = 4,
        MazeDifficulty = 2
    }
    public class Quest
    {
        internal Time time;
        internal Map questMap;
        internal Player player;
        internal InputHandler inputProcessor;
        public bool IsEnded { get; private set; }
        internal Report report;
        public Report Start(string name)
        {
            player = new Player(name);
            questMap = new Map(player);
            time = new Time();
            report = new Report();
            if (player.Name == "Test" || player.Name == "test")
            {
                questMap.SetPlayerLocation((0, 0));
            }
            else
            {
                questMap.SetPlayerLocation(((int)MainQuestConfig.MapSize / 2, (int)MainQuestConfig.MapSize / 2));
            }
            report.SetReportMessage($"Select preferred language:");
            report.ResetOptions(new List<string>() { "EN", "RU", "UA" });
            SetEnglishCommand english = new SetEnglishCommand(this);
            inputProcessor.SetCommand(new List<Command>() { new SetEnglishCommand(this), new SetRussianCommand(this), new SetUkrainianCommand(this) });
            return report;
        }
        public Report ProceedInput(int input)
        {
            inputProcessor.PoceedInput(input);
            return report;
        }
        public Report NonInventoryDialogInputHandler(int input)
        {
            commands[availableCommands[input]].Invoke();
            return report;
        }
        public Report OpenInventoryDialogInputHandler(int input)
        {
            if (input == report.Options.Count - 1)
            {
                Cancel();
            }
            else
            {
                SelectItemInPlayerInventory(input);
                ResetAvailableOptions(player.Inventory.CurrentItem.Options);
                AddUniqueCallInMenueCallHistory(LaunchOpenInventoryDialog);
                ProceedInput = NonInventoryDialogInputHandler;
            }
            return report;
        }
        public Report SellDialogInputHandler(int input)
        {
            if (input == report.Options.Count - 1)
            {
                Cancel();
            }
            else
            {
                Sell(input);
            }
            return report;
        }
        public Report BuyDialogInputHandler(int input)
        {
            if (input == report.Options.Count - 1)
            {
                Cancel();
            }
            else
            {
                TryPurchase(input);
            }
            return report;
        }
        public Report StealDialogInputHandler(int input)
        {
            if (input < report.Options.Count - 1)
            {
                StealOrLoot(input);
                questMap.PlayerSpot.npc.IsHostile = true;
            }
            Cancel();
            return report;
        }
        public Report LootDialogInputHandler(int input)
        {
            if (input < report.Options.Count - 1)
            {
                StealOrLoot(input);
                LaunchLootDialog();
            }
            else
            {
                LaunchMainDialog();
            }
            return report;
        }
        private void Fight()
        {
            while (player.Health > 0 && questMap.PlayerSpot.npc.Health > 0)
            {
                if (player.CurrentWeapon != null)
                {
                    questMap.PlayerSpot.npc.TakeHit(player.CurrentWeapon.Attack);
                }
                player.TakeHit(questMap.PlayerSpot.npc.Attack);
            }
            if (player.Health <= 0)
            {
                EndQuest(Keys.Death);
            }
            else
            {
                LaunchNpcDialog();
            }
        }
        private void LaunchMainDialog()
        {
            AppendStateMessage(questMap.PlayerSpot.Description);
            if (!IsQuestEnded())
            {
                if (questMap.PlayerSpot.npc != null && questMap.PlayerSpot.npc.IsHostile)
                {
                    AppendStateMessage(Keys.Enemy, questMap.PlayerSpot.npc.Name);
                }
                report.PlayerStateOrAdditionalInformation = Data.PlayerStateBuilder(player, language);
                ResetAvailableOptions(questMap.PlayerSpot.GetListOfPossibleOptions());
                ResetMenuCallsChain();
                ProceedInput = NonInventoryDialogInputHandler;
            }
        }
        private void LaunchTravelDialog()
        {
            if (time.DayTime())
            {
                SetStateMessage(Keys.DirectionDialogMessage);
                ResetAvailableOptions(questMap.PlayerSpot.GetAvailableDirections());
            }
            else
            {
                SetStateMessage(Keys.NightTime);
                LaunchMainDialog();
            }
        }
        private void LaunchOpenInventoryDialog()
        {
            SetStateMessage(Keys.Inventory);
            ListInventoryForUseAndLoot(player.Inventory);
            ProceedInput = OpenInventoryDialogInputHandler;
        }
        private void LaunchNpcDialog()
        {
            SetStateMessage(questMap.PlayerSpot.npc.Name);
            ResetAvailableOptions(questMap.PlayerSpot.npc.GetListOfPossibleOptions());
            ProceedInput = NonInventoryDialogInputHandler;
        }
        private void LaunchTradeDialog()
        {
            SetStateMessage(questMap.PlayerSpot.npc.Name);
            ResetAvailableOptions(new List<Keys>() { Keys.Sell, Keys.Buy, Keys.Cancel });
            AddUniqueCallInMenueCallHistory(LaunchNpcDialog);
            ProceedInput = NonInventoryDialogInputHandler;
        }
        private void LaunchSellDialog()
        {
            AddUniqueCallInMenueCallHistory(LaunchTradeDialog);
            SetStateMessage(Keys.Sell);
            ListInventoryForTrading(player.Inventory);
            report.PlayerStateOrAdditionalInformation = $"{ Data.Localize(Keys.Remains, language) } { player.Inventory.Coins } { Data.Localize(Keys.Coins, language) }";
            ProceedInput = SellDialogInputHandler;
        }
        private void LaunchBuyDialog()
        {
            AddUniqueCallInMenueCallHistory(LaunchTradeDialog);
            SetStateMessage(Keys.Buy);
            ListInventoryForTrading(questMap.PlayerSpot.npc.inventory);
            report.PlayerStateOrAdditionalInformation = $"{ Data.Localize(Keys.Remains, language) } { player.Inventory.Coins } { Data.Localize(Keys.Coins, language) }";
            ProceedInput = BuyDialogInputHandler;
        }
        private void LaunchStealDialog()
        {
            AddUniqueCallInMenueCallHistory(LaunchNpcDialog);
            SetStateMessage(questMap.PlayerSpot.npc.Name);
            AppendStateMessage(Keys.Steal);
            ListInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory);
            ProceedInput = StealDialogInputHandler;
        }
        private void LaunchLootDialog()
        {
            SetStateMessage(questMap.PlayerSpot.npc.Name);
            AppendStateMessage(Keys.Loot);
            ListInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory);
            ProceedInput = LootDialogInputHandler;
        }
        internal bool IsQuestEnded()
        {
            if (player.Health > 0 && !questMap.ExitReached)
            {
                return false;
            }
            else if (player.Health <= 0)
            {
                EndQuest(Keys.Death);
            }
            else if (questMap.ExitReached)
            {
                EndQuest(Keys.Exit);
            }
            return true;
        }
        private void EndQuest(Keys result)
        {
            IsEnded = true;
            report.EndingReport(result);
            
        }
        private void Sell(int input)
        {
            SelectItemInPlayerInventory(input);
            UnequipSelectedItem();
            questMap.PlayerSpot.npc.inventory.Add(player.Inventory.CurrentItem);
            player.Inventory.Sell();
            LaunchSellDialog();
        }
        private void TryPurchase(int input)
        {
            SelectItemInNpcInventory(input);
            if (player.Inventory.Coins >= player.Inventory.CurrentItem.Price)
            {
                questMap.PlayerSpot.npc.inventory.Drop();
                player.Inventory.Buy();
                LaunchBuyDialog();
            }
        }
        private void Cancel()
        {
            report.Message = null;
            menuCallChain.Pop().Invoke();
        }
        private void StealOrLoot(int input)
        {
            SelectItemInNpcInventory(input);
            player.Inventory.Add(player.Inventory.CurrentItem);
            questMap.PlayerSpot.npc.inventory.Drop();
        }
        private void Equip()
        {
            if (player.Inventory.CurrentItem is Weapon)
            {
                player.CurrentWeapon = (Weapon)player.Inventory.CurrentItem;
            }
            else
            {
                player.CurrentClothes = (Clothes)player.Inventory.CurrentItem;
            }
            Cancel();
        }
        private void PlayerReachedNewZone(int speedModifier = 1)
        {
            SetStateMessage(Keys.NextZone);
            time.ChangeTime(player.CalculateTimeNeededToTravel());
            player.RecaculateStateDueToTraveling(speedModifier);
            LaunchMainDialog();
        }
        private void Run()
        {
            questMap.Go(questMap.GetRandomAvailableDirection(questMap.PlayerSpot));
            PlayerReachedNewZone(2);
        }
        private void Search()
        {
            if (questMap.PlayerSpot.Description == Keys.Burrow && !questMap.PlayerSpot.searched)
            {
                if (player.CurrentWeapon != null)
                {
                    Hunt();
                }
                else
                {
                    SetStateMessage(Keys.HuntFailed);
                }
                questMap.PlayerSpot.searched = true;
            }
            else
            {
                SearchSpotForHiddenItem();
            }
        }
        private void Hunt()
        {
            SetStateMessage(Keys.HuntSucceed);
            questMap.PlayerSpot.npc = questMap.prefabs.hare;
            ListInventoryForUseAndLoot(questMap.PlayerSpot.npc.inventory);
            ProceedInput = LootDialogInputHandler;
        }
        private void SearchSpotForHiddenItem()
        {
            Item newItem = questMap.PlayerSpot.item;
            if (newItem != null)
            {
                if ((newItem is Consumable) && (newItem as Consumable).AutoConsume)
                {
                    commands[newItem.Use()].Invoke();
                }
                else
                {
                    FoundHiddenItem(newItem);
                }
            }
            else
            {
                SetStateMessage(Keys.NotFound);
            }
            time.ChangeTime(1);
            player.InnerStateProcess(1);
            LaunchMainDialog();
        }

        private void FoundHiddenItem(Item newItem)
        {
            player.Inventory.Add(newItem);
            questMap.PlayerSpot.item = null;
            report.Message = null;
            AppendStateMessage(Keys.Found, newItem.Name);
        }

        private void SelectItemInPlayerInventory(int input)
        {
            player.Inventory.CurrentItem = player.Inventory.Items[input];
            report.PlayerStateOrAdditionalInformation = report.Options[input];
        }
        private void SelectItemInNpcInventory(int input)
        {
            player.Inventory.CurrentItem = questMap.PlayerSpot.npc.inventory.Items[input];
            questMap.PlayerSpot.npc.inventory.CurrentItem = player.Inventory.CurrentItem;
        }
        private void Eat()
        {
            player.Eat(false);
            Cancel();
        }
        private void Poison()
        {
            player.Eat(true);
            ComplexStateReport(Keys.GetPoisonMessage);
            Cancel();
        }
        private void TriggerTrap()
        {
            SetStateMessage(Keys.Trap);
            DisableTrigger(Keys.Bleeding);
        }
        private void TriggerHornetNest()
        {
            SetStateMessage(Keys.HornetNest);
            DisableTrigger(Keys.IsPoisoned);
        }
        private void DisableTrigger(Keys effect)
        {
            Consumable trigger = (Consumable)questMap.PlayerSpot.item;
            player.TakeHit(trigger.EffectPower);
            player.Effects.Add(effect);
            questMap.PlayerSpot.item = null;
        }
        private void CurePoison()
        {
            player.TakeAntidote();
            ComplexStateReport(Keys.CurePoison);
            Cancel();
        }
        private void Open()
        {
            player.Inventory.AddMoney(player.Inventory.CurrentItem.Price * 2);
            player.Inventory.Drop();
            LaunchOpenInventoryDialog();
        }

    }
}