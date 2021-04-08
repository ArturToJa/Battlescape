using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattlescapeLogic
{

    public enum MatchTypes { Online, HotSeat, Singleplayer, None }

    public class Global : MonoBehaviour
    {
        static Global _instance;
        public static Global instance
        {
            get
            {
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
        [SerializeField] BattlescapeGraphics.Skybox skybox;
        [SerializeField] ArmySavingManager _armySavingManager;
        public ArmySavingManager armySavingManager
        {
            get
            {
                return _armySavingManager;
            }
            private set
            {
                _armySavingManager = value;
            }
        }
        public Map currentMap { get; private set; }
        [SerializeField] List<Map> maps;
        public List<PlayerTeam> playerTeams { get; private set; }
        public MatchTypes matchType { get; set; }

        //THIS IS TEMPORARY!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public List<PlayerBuilder> playerBuilders = new List<PlayerBuilder>();



        public AbstractMovement[] movementTypes = new AbstractMovement[4];
        public IActiveEntity currentEntity { get; set; }
        public int playerCount { get; set; }

        [SerializeField] List<Sprite> _flags;
        public List<Sprite> flags
        {
            get
            {
                return _flags;
            }
        }
        [SerializeField] List<Sprite> _emblems;
        public List<Sprite> emblems
        {
            get
            {
                return _emblems;
            }
        }
        [SerializeField] BattlescapeGraphics.Colours _colours;

        public BattlescapeGraphics.Colours colours
        {
            get
            {
                return _colours;
            }
        }

        void Awake()
        {            
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
            movementTypes[(int)MovementTypes.Ground] = new GroundMovement();
            movementTypes[(int)MovementTypes.Flying] = new FlyingMovement();
            movementTypes[(int)MovementTypes.Teleport] = new TeleportationMovement();
            movementTypes[(int)MovementTypes.None] = null;
            playerTeams = new List<PlayerTeam>();
            PlayerTeam teamLeft = new PlayerTeam(0, 1);
            PlayerTeam teamRight = new PlayerTeam(1, 1);
            playerTeams.Add(teamLeft);
            playerTeams.Add(teamRight);
            matchType = MatchTypes.None;
            playerCount = 2;
            SceneManager.sceneLoaded += OnSceneLoaded;
            GameRound.instance.Setup();
        }

        

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            BattlescapeSound.SoundManager.instance.PlayTheme();

            if (scene.name.Contains("_GameScene_"))
            {
                SetMap(scene.name);
                skybox.SetSkyboxToRandom();
            }
            if (scene.name.Contains("_ManagementScene"))
            {
                BattlescapeUI.ArmyManagementScreens.instance.OnStart();
            }
        }



        public PlayerBuilder GetCurrentPlayerBuilder()
        {
            return playerBuilders[0];
        }


        public bool IsCurrentPlayerLocal()
        {
            foreach (PlayerBuilder playerBuilder in playerBuilders)
            {
                if (playerBuilder.type == PlayerType.Local && playerBuilder.team == GameRound.instance.currentPlayer.team && playerBuilder.index == GameRound.instance.currentPlayer.index)
                {
                    return true;
                }
            }

            return (GameRound.instance.currentPlayer.type == PlayerType.Local);
        }

        public List<Unit> GetAllUnits()
        {
            List<Unit> returnList = new List<Unit>();
            foreach (PlayerTeam team in playerTeams)
            {
                foreach (Player player in team.players)
                {
                    returnList.AddRange(player.playerUnits);
                }
            }
            return returnList;
        }

        public List<Obstacle> GetAllObstacles()
        {
            List<Obstacle> obstacles = new List<Obstacle>();
            foreach (Tile tile in currentMap.board)
            {
                Obstacle obstacle = tile.GetMyObject<Obstacle>();

                if (obstacle != null)
                {
                    obstacles.Add(obstacle);
                }
            }
            return obstacles;

        }

        public List<DestructibleObstacle> GetAllDestructibles()
        {
            List<DestructibleObstacle> destructibles = new List<DestructibleObstacle>();
            foreach (Tile tile in currentMap.board)
            {
                DestructibleObstacle destructible = tile.GetMyObject<DestructibleObstacle>();

                if (destructible != null)
                {
                    destructibles.Add(destructible);
                }
            }
            return destructibles;
        }

        //Doesn't count the observers
        public int GetActivePlayerCount()
        {
            int count = 0;
            foreach (PlayerBuilder playerBuilder in playerBuilders)
            {
                if (playerBuilder != null && playerBuilder.isObserver == false)
                {
                    count++;
                }
            }
            {
                foreach (PlayerTeam team in playerTeams)
                {
                    foreach (Player player in team.players)
                    {
                        if (player.isObserver == false)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        public static List<IMouseTargetable> FindAllTargetablesInLine(Vector3 start, Vector3 end)
        {
            var VectorToTarget = -start + end;

            Ray ray = new Ray(start, VectorToTarget);
            RaycastHit[] hits = Physics.RaycastAll(ray, Vector3.Distance(start, end));

            var list = new List<IMouseTargetable>();

            foreach (var hit in hits)
            {
                foreach (IMouseTargetable item in hit.transform.GetComponentsInChildren<IMouseTargetable>())
                {
                    list.Add(item);
                }                
            }
            return list;
        }

        void SetMap(string mapName)
        {
            foreach (Map map in maps)
            {
                if (map.mapName == mapName)
                {
                    currentMap = map;
                    currentMap.OnSetup();

                }
            }
        }

        public Race GetLocalRace()
        {
            foreach (PlayerTeam team in Global.instance.playerTeams)
            {
                foreach (Player player in team.players)
                {
                    if (player.type == PlayerType.Local)
                    {
                        return player.race;
                    }
                }
            }
            Debug.LogError("No local player found!");
            return Race.Neutral;
        }

        public bool HaveAllPlayersChosenRace()
        {
            foreach (PlayerBuilder playerBuilder in playerBuilders)
            {
                if (playerBuilder.race == Race.Neutral)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetStillPlayingTeamsCount()
        {
            int returnInt = 0;
            foreach (PlayerTeam team in playerTeams)
            {
                if (team.HasLost() == false)
                {
                    returnInt++;
                }
            }
            return returnInt;
        }
    }
}
