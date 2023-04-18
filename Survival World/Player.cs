namespace Survival_World
{
    public class Player
    {
        public int Experience { get; private set; }
        public int Money { get; private set; }
        public int Health { get; private set; }
        public string Nickname { get; private set; }

        public Player(string nickname) 
        {
            if (String.IsNullOrWhiteSpace(nickname)) throw new Exception("Никнейм не может быть пустым");

            Nickname = nickname;
            Experience = 0;
            Money = 0;
            Health = 100;
        }

        public void Rename(string newNickame)
        {
            if (String.IsNullOrWhiteSpace(newNickame)) throw new Exception($"Переименовать персонажа {Nickname} не вышло. Была передана пустая строка (Rename Player)");
            Nickname = newNickame;
        }
        public void EditExperience(int changeEX)
        {
            Experience += changeEX;
        }
        public void EditHealth(int changeHP)
        {
            Health += changeHP;
        }
        public void EditMoney(int changeMoney)
        {
            Money += changeMoney;
        }
    }
}
