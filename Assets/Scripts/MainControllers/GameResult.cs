namespace BattlescapeLogic
{
    public class GameResult
    {

        static GameResult _instance;
        public static GameResult instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameResult();
                }
                return _instance;
            }
        }

        public PlayerTeam winner;
        public bool isOver;
        public bool isDraw;
    }
}