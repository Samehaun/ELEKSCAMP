using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELEKSUNI
{
    enum Keys
    {
        EN, RU, UA, Equiped,
        Bleeding, Poison, Injure, Sturve, Cold,
        Weight, Coins, Attack, Defence, Equip, Eat, Drop, Drink,
        Inventory, Buy, Sell, Trade, Search,
        Clearing,
        OrdinaryForest,
        Oak,
        Berries,
        Pine,
        Creek,
        DryRiver,
        Cave,
        PitHouse,
        Glade,
        Ravine,
        GingerbreadHouse,
        ForesterHouse,
        Burrow,
        Lake,
        Stump,
        SwordInStone,
        WolfPit,
        Crater,
        Hanged,
        Swamp,
        Monolith,
        ThermalSprings,
        North,
        South,
        East,
        West,
        Rest,
        Sleep,
        NightTime,
        RestNeeded,
        NextZone,
        DirectionDialogMessage,
        InitialMessage,
        StaminaRecovered,
        DayTimeSleep,
        WakeUp,
        Exit,
        Cancel,
        Travel
    }
    static class Data
    {
        private static Dictionary<Keys, string> enText = new Dictionary<Keys, string>()
        {
            {Keys.Coins, "coins" }, {Keys.Weight, "Kg" }, {Keys.Attack, "attack" }, {Keys.Defence, "def" },
            {Keys.Equip, "Equip" }, {Keys.Drop, "Drop" }, {Keys.Eat, "Eat" }, {Keys.Drink, "Drink" },
            {Keys.Inventory, "Inventory" }, {Keys.Buy, "Buy" }, {Keys.Sell, "Sell" }, {Keys.Trade, "Trade" }, {Keys.Search, "Search" },
            {Keys.Bleeding, "bleeding" }, {Keys.Poison, "poisoned" }, {Keys.Injure, "injured" }, {Keys.Sturve, "sturving" }, {Keys.Cold, "freeze" },
            {Keys.North, "North"}, {Keys.South, "South"}, {Keys.East, "East"}, {Keys.West, "West"}, {Keys.Rest, "Rest"}, {Keys.Sleep, "Sleep"},
            {Keys.NightTime, "Too dark to go"}, {Keys.RestNeeded, "You are falling off your feet. Can't go any further"}, {Keys.NextZone, "You have reached new zone"}, {Keys.DirectionDialogMessage, "Choose direction"},
            {Keys.StaminaRecovered, "You rested a bit"}, {Keys.DayTimeSleep, "Sleep during the day?! What are you going to do at night?"}, {Keys.WakeUp, "You are full of energy"},
            {Keys.InitialMessage, "You have come to your senses in an unfamiliar place. It is unknown how you got here, but at least you are safe and sound ... for now"},
            {Keys.Clearing, "Clearing. Many fallen trees"},
            {Keys.OrdinaryForest, "Ordinary forest. Nothing to look at except fallen leaves"},
            {Keys.Oak, "Old oak. There is a big hollow, it seems that you can get to it"},
            {Keys.Berries, "Mixed forest. Lots of shrubs with berries"},
            {Keys.Pine, "Pine trees predominate, it is pleasant to breathe deeply"},
            {Keys.Creek, "Great luck you found the creek"},
            {Keys.DryRiver, "Overgrown dry river bed" },
            {Keys.Cave, "Cave. Looks pretty big"},
            {Keys.PitHouse, "Abandoned pit-house. The door looks functional, but the lock has fallen apart"},
            {Keys.Glade, "Big glade"},
            {Keys.Ravine, "Ravine"},
            {Keys.GingerbreadHouse, "Gingerbread house"},
            {Keys.ForesterHouse, "Forester's house"},
            {Keys.Burrow, "A hill with a burrow. There is definitely someone living in it"},
            {Keys.Lake, "Forest lake"},
            {Keys.Stump, "You have never seen a stump of such a huge size. It is even more surprising that there is a door on the side, and smoke comes from a small pipe. О.о"},
            {Keys.SwordInStone, "A huge boulder with a sword stuck in"},
            {Keys.WolfPit, "Wolf pit. It's good that you are not the first one to find it."},
            {Keys.Crater, "Large crater of unknown origin"},
            {Keys.Hanged, "Hanged. Looks creepy"},
            {Keys.Swamp, "Swamp"},
            {Keys.Monolith, "Absolutely black monolith. It looks like metal, but warm."},
            {Keys.ThermalSprings, "Thermal springs"},
            {Keys.Exit, "Road! It will lead somewhere. You got out!" },
            {Keys.Cancel, "Cancel" }, {Keys.Travel, "Walk"}
        };
        private static Dictionary<Keys, string> ruText = new Dictionary<Keys, string>()
        {
            {Keys.North, "Север"}, {Keys.South, "Юг"}, {Keys.East, "Восток"}, {Keys.West, "Запад"}, {Keys.Rest, "Отдыхать"}, {Keys.Sleep, "Спать"},
            {Keys.NightTime, "Слишком темно чтобы идти"}, {Keys.RestNeeded, "Вы слишком устали, нужен отдых"}, {Keys.NextZone, "Вы благополучно добрались до следующей зоны"}, {Keys.DirectionDialogMessage, "Выберите направление"},
            {Keys.StaminaRecovered, "Вы немного отдохнули"}, {Keys.DayTimeSleep, "Спать днем?! А что собираетесь делать ночью?"}, {Keys.WakeUp, "Вы полны энергии"},
            {Keys.InitialMessage, "Вы пришли в себя в незнакомом месте. Неизвестно как вы зесь оказались, но, по крайней мере Вы живы и здоровы...пока"},
            {Keys.Clearing, "Просека. Много поваленных деревьев"},
            {Keys.OrdinaryForest, "Лес как лес. Кроме опавшей листвы не видно ничего интересного"},
            {Keys.Oak, "Старый дуб. Есть большое дупло, кажется, до него можно добраться"},
            {Keys.Berries, "Смешанный лес. Много кустарника, есть ягоды"},
            {Keys.Pine, "Преобладает хвоя приятно дышать полной грудью"},
            {Keys.Creek, "Большая удача вы нашли ручей"},
            {Keys.DryRiver, "Засохшее русло высохшей реки" },
            {Keys.Cave, "Пещера. Выглядит довольно большой"},
            {Keys.PitHouse, "Покинутая землянка. Дверь выглядит функционирующей, но замок рассыпался"},
            {Keys.Glade, "Большая поляна"},
            {Keys.Ravine, "Овраг"},
            {Keys.GingerbreadHouse, "Избушка на курьих ножках"},
            {Keys.ForesterHouse, "Домик лесника"},
            {Keys.Burrow, "Холм с норой. В ней определенно кто-то живет"},
            {Keys.Lake, "Лесное озеро"},
            {Keys.Stump, "Никогда вы не видели пня таких огромных размеров. Еще более удивительно, что сбоку есть дверца, а из маленькой трубы идет дым О.о"},
            {Keys.SwordInStone, "Огромный валун из которого торчит меч"},
            {Keys.WolfPit, "Волчья яма. Хорошо, что вы не первый, кто ее нашел"},
            {Keys.Crater, "Большая воронка непонятного происхождения"},
            {Keys.Hanged, "Повешенный. Выглядит жутко"},
            {Keys.Swamp, "Болото"},
            {Keys.Monolith, "Абсолютно черный монолит. Похоже на металл, но теплый."},
            {Keys.ThermalSprings, "Термальный источник"},
            {Keys.Exit, "Дорога! Куда-то она да приведет. Вы выбрались!" },
            {Keys.Cancel, "Отмена" }, {Keys.Travel, "Идти"}
        };
        private static Dictionary<Keys, string> uaText = new Dictionary<Keys, string>()
        {
            {Keys.North, "Північ"}, {Keys.South, "Південь"}, {Keys.East, "Схід"}, {Keys.West, "Захід"}, {Keys.Rest, "Відпочити"}, {Keys.Sleep, "Спати"},
            {Keys.NightTime, "Занадто темно задля подорожі"}, {Keys.RestNeeded, "Ви надто втомилися, потрібен відпочинок"}, {Keys.NextZone, "Ви дісталися наступної зони"}, {Keys.DirectionDialogMessage, "Оберіть напрямок"},
            {Keys.StaminaRecovered, "Ви трохи відпочили"}, {Keys.DayTimeSleep, "Спати вдень?! А що збираєтесь робити вночі?"}, {Keys.WakeUp, "Ви сповнені сил"},
            {Keys.InitialMessage, "Ви прийшли до тями в незнайомому місці. Невідомо як ви сюди потрапили, але, принаймні Ви живі і здорові ... поки що"},
            {Keys.Clearing, "Просіка. Багато повалених дерев"},
            {Keys.OrdinaryForest, "Ліс, як ліс. Крім опалого листя не видно нічого цікавого"},
            {Keys.Oak, "Старий дуб. Є велике дупло, здається, до нього можна дістатися"},
            {Keys.Berries, "Змішаний ліс. Багато чагарників, є ягоди"},
            {Keys.Pine, "Переважає хвоя приємно дихати на повні груди"},
            {Keys.Creek, "Великий успіх ви знайшли струмок"},
            {Keys.DryRiver, "Заросле русло висохлої річки" },
            {Keys.Cave, "Печера. Здається досить великою"},
            {Keys.PitHouse, "Покинутая землянка. Дверь выглядит функционирующей, но замок рассыпался"},
            {Keys.Glade, "Большая поляна"},
            {Keys.Ravine, "Яр"},
            {Keys.GingerbreadHouse, "Хатинка на курячих ніжках"},
            {Keys.ForesterHouse, "Будинок лісника"},
            {Keys.Burrow, "Пагорб із норою. У ній безперечно хтось живе"},
            {Keys.Lake, "Лісове озеро"},
            {Keys.Stump, "Ніколи ви не бачили пня таких величезних розмірів. Ще дивніше, що збоку є дверцята, а з маленької труби йде дим О.о"},
            {Keys.SwordInStone, "Величезний валун, з якого стирчить меч"},
            {Keys.WolfPit, "Вовча яма. Добре, що ви не перший, хто її знайшов"},
            {Keys.Crater, "Велика вирва незрозумілого походження"},
            {Keys.Hanged, "Повішений. Виглядає моторошно"},
            {Keys.Swamp, "Болото"},
            {Keys.Monolith, "Абсолютно чорний моноліт. Схоже на метал, але теплий."},
            {Keys.ThermalSprings, "Термальне джерело"},
            {Keys.Exit, "Дорога! Кудись вона та приведе. Ви вибралися!" },
            {Keys.Cancel, "Відміна" }, {Keys.Travel, "Йти"}
        };
        public static string Localize(Keys text, string languageSettings)
        {
            switch (languageSettings)
            {
                case "EN":
                    return enText[text];
                case "RU":
                    return ruText[text];
                case "UA":
                    return uaText[text];
                default:
                    return $"Wrong language settings";
            }
        }
        public static string StateBuilder(Player player, string languageSettings)
        {
            switch (languageSettings)
            {
                case "EN":
                    return $"Player {player.Name} has {player.Health} hp";
                case "RU":
                    return $"У {player.Name} {player.Health} хп";
                case "UA":
                    return $"У {player.Name} {player.Health} хп";
                default:
                    return $"Wrong language settings";
            }
        }
        public static List<string> Localize(List<Keys> options, string languageSettings)
        {
            List<string> localizedOptions = new List<string>();
            foreach (var option in options)
            {
                localizedOptions.Add(Localize(option, languageSettings));
            }
            return localizedOptions;
        }
    }
}