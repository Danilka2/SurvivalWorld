using Survival_World;

namespace SurvivalWorld
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Survival World";
            Console.ForegroundColor = ConsoleColor.DarkGray;

            bool exit = false;
            while (!exit)
            {
                Game Game = new Game();
                exit = Game.StartGame();
                Console.ForegroundColor = ConsoleColor.DarkGray;
            } // Инициализация игры
        }

        class Game
        {
            private Player Player; // Глобальная переменная игрока
            private Events Events; // Глобальная переменная событий

            public Game() {}

            private void createPlayer() // Создание персонажа
            {
                string bufferErr = ""; // Переменная для буфферизации ошибки (чтобы при перерендеринге ее выводить)
                while (true)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("--   Регистрация    --");
                    if (bufferErr != "") // Если ошибка не пустая - значит она произошла. Её нужно вывести и обнулить
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("! " + bufferErr);
                        bufferErr = "";
                    } 
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Введите никнейм: ");

                    try
                    {
                        string nickname = Console.ReadLine();
                        if (String.IsNullOrWhiteSpace(nickname)) throw new Exception("Никнейм не может быть пустым");
                        Player = new Player(nickname);
                        break;
                    }
                    catch (Exception e)
                    {
                        bufferErr = e.Message; // Если словили ошибку - записываем ее в буфер и при перерендеринге она выведется
                    }
                }
            }
            public void initEvent()
            {
                Events = new Events(Player); // Инициализируем и передаём пользователя чтобы события могли с ним взаимодействовать
            }
            public bool StartGame()
            {
                createPlayer();
                initEvent();
                int eventCount = 0; // Кол-во пройденных событий

                while (true)
                {
                    if (Player.Health <= 0) break;

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"          {Player.Nickname}");

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(" Опыт: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(Player.Experience);

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write(" Жизни: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(Player.Health);

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(" Монеты: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(Player.Money + "\n");

                    Console.ForegroundColor = ConsoleColor.White;
                    Events.SummonEvent( RandomIndexOfEvent() ); // Генерируем рандомное событие
                    eventCount += 1;
                }

                int score = Player.Money + Player.Experience + eventCount;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("         GAME OVER       ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($" + {Player.Money} за накопленные монеты");
                Console.WriteLine($" + {Player.Experience} за опыт");
                Console.WriteLine($" + {eventCount} пройденных событий");
                Console.WriteLine($"    Всего очков: {score} \n");

                Console.Write(" Хочешь начать снова? \n -");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                if (Console.ReadLine().ToLower() == "да") return false;
                return true;
            }
            private int RandomIndexOfEvent()
            {
                return new Random().Next(0, Events.GetCountEvent()); // Генерируем случайный индекс события
            }
        }
    }
}