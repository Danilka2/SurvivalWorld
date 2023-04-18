namespace Survival_World
{
    public class Events
    {
        private List<Event> EventsList;
        private Player Player;

        public Events(Player player)
        {
            if (player == Player) throw new ArgumentException("Пользователь не передан. (init Events)");
            Player = player; // Получаем игрока для дальнейшего взаимодействия

            EventsList = new List<Event>() // Список событий игры
            {
                new Event // Создаём событие (все атрибуты обязательно иницивлизировать, но garantEffect, variantsList, positiveAnswerEffect и negativeAnswerEffect можно оставлять пустыми)
                (
                    player: Player, // Передаётся обязательно чтобы потом внутри можно было с ним взаимодействовать
                    nameEvent: "Случайная встреча",
                    descEvent: "Вы встретили купцов",

                    garantEffect: new Dictionary<string, int>() // Список гарантированных эффектов после события
                    {
                        { "Experience", 100 } // В таком формате передаются параметры для изменения { "Что", На сколько }. Существующие параметры можно посмотреть в switch на строке 111
                    },

                    variantsList: new List<Variant>()
                    {
                        new Variant
                        (
                            player: Player, // Передаётся обязательно чтобы потом внутри можно было с ним взаимодействовать
                            descVariant: "-Хочешь денег, бедолага?",
                            positiveAnswerEffect: new Dictionary<string, int>()
                            {
                                { "Money", 100 }
                            },
                            negativeAnswerEffect: new Dictionary<string, int>()
                            {
                                { "Health", -10 }
                            }
                        )
                    }
                )
            };
        }

        public void CreateEvent(Player player, string name, string description, Dictionary<string, int> garantEffect, List<Variant> variantsList) // Передаются все необходимые параметры для создания события
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("В наименование события передана пустая строка\n(create Event)");
            if (String.IsNullOrWhiteSpace(description)) throw new ArgumentNullException("В описание события передана пустая строка\n(create Event)");

            EventsList.Add(new Event(player, name, description, garantEffect, variantsList));
        }
        public void SummonEvent(int index) // Стартует событие по индексу
        {
            if (index < 0 || index > EventsList.Count) throw new ArgumentException("Индекс вызываемого события выходит за рамки списка событий (StartEvent)\n(событие по переданному индексу не существует)");
            
            EventsList[index].StartEvent();
        }
        public int GetCountEvent()
        {
            return EventsList.Count;
        }

        // КЛАСС СОБЫТИЯ
        private class Event
        {
            private Player Player; // Игрок обязателен чтобы с ним можно было взаимодействовать в событии
            public string Name { get; init; }
            public string Description { get; init; }
            public Dictionary<string, int> GarantEffect { get; init; } // Список гарантированных эффектов после события
            public List<Variant> VariantsList { get; init; }

            public Event(Player player, string nameEvent, string descEvent, Dictionary<string, int> garantEffect, List<Variant> variantsList) // Конструктор
            {
                if (player == Player) throw new ArgumentException("Пользователь не передан. (init Events)");
                if (String.IsNullOrWhiteSpace(nameEvent)) throw new ArgumentNullException("В наименование события передана пустая строка (init Event)");
                if (String.IsNullOrWhiteSpace(descEvent)) throw new ArgumentNullException("В описание события передана пустая строка (init Event)");
                if (garantEffect == GarantEffect) throw new ArgumentException("Не переданы свойства события (init Event)");

                Player = player;
                Name = nameEvent;
                Description = descEvent;
                GarantEffect = garantEffect;
                VariantsList = variantsList;
            }
             
            public void StartEvent() // Начинает событие
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(" ===== ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{Name}");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(" =====");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"   {Description}");

                for (int i = 0; i < VariantsList.Count; i++) // Выводит все переданные развилки события
                {
                    VariantsList[i].ShowVariant();
                }

                SummonEffect(GarantEffect); // После прохождения всего события применяет гарантированный эффект события

                Console.Write(" \n ");
                Console.ReadLine(); // Это чтобы консолька не стерлась после события и игрок мог перед перерендерингом все еще раз просмотреть, а потом что-либо ввести и перекинет на другое событие
            }

            public void SummonEffect(Dictionary<string, int> Effects) // Применяет эффекты
            {
                foreach (KeyValuePair<string, int> Effect in Effects) // Перебирает все переданные эффекты
                {
                    switch (Effect.Key)
                    {
                        case "Money":
                            Console.WriteLine($" {Effect.Value} монет");
                            Player.EditMoney(Effect.Value);
                            break;
                        case "Experience":
                            Console.WriteLine($" {Effect.Value} опыта");
                            Player.EditExperience(Effect.Value);
                            break;
                        case "Health":
                            Console.WriteLine($" {Effect.Value} хп");
                            Player.EditHealth(Effect.Value);
                            break;
                    }
                }
            }
        }

        // КЛАСС ВАРИАНТОВ ДЛЯ СОБЫТИЯ
        public class Variant
        {
            private Player Player; // Игрок обязателен чтобы с ним можно было взаимодействовать в событии
            public string DescVariant { get; init; }
            public new Dictionary<string, int> PositiveAnswerEffect { get; init; } // Эффекты от положительного ответа
            public new Dictionary<string, int> NegativeAnswerEffect { get; init; } // Эффекты от отрицаиельного ответа

            public Variant(Player player, string descVariant, Dictionary<string, int> positiveAnswerEffect, Dictionary<string, int> negativeAnswerEffect) // Конструктор
            {
                if (player == Player) throw new ArgumentException("Пользователь не передан. (init Variant)");
                if (String.IsNullOrWhiteSpace(descVariant)) throw new ArgumentException("Передано пустое описание варианта (init Variant)");

                Player = player;
                DescVariant = descVariant;
                PositiveAnswerEffect = positiveAnswerEffect;
                NegativeAnswerEffect = negativeAnswerEffect;
            }

            public void ShowVariant() // Выводит развилку в консоль
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($" {DescVariant} \n -");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string getAnswer = Console.ReadLine().ToLower();
                if (String.IsNullOrWhiteSpace(getAnswer)) Console.WriteLine(" Что-ж, нормально отвечать ты не хочешь..."); // Если игрок ничего не ввёл (по сути промолчал) - выведет это
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                if (getAnswer == "да")
                {
                    SummonEffect(PositiveAnswerEffect); // Применяются полжительные эффекты после ответа ДА
                } else
                {
                    SummonEffect(NegativeAnswerEffect); // Применяютмя отрицательные эффекты если он ввёл что-либо отличающееся то ДА (по сути отрицательный ответ)
                }
            }

            public void SummonEffect(Dictionary<string, int> Effects) // Применяет эффекты
            {
                foreach (KeyValuePair<string, int> Effect in Effects) // Перебирает переданные эффекты
                {
                    switch (Effect.Key)
                    {
                        case "Money":
                            Console.WriteLine($" {Effect.Value} монет");
                            Player.EditMoney(Effect.Value);
                            break;
                        case "Experience":
                            Console.WriteLine($" {Effect.Value} опыта");
                            Player.EditExperience(Effect.Value);
                            break;
                        case "Health":
                            Console.WriteLine($" {Effect.Value} хп");
                            Player.EditHealth(Effect.Value);
                            break;
                    }
                }
            }
        }

    }
}
