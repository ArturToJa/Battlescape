﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{

    public enum MatchTypes { Online, HotSeat, Singleplayer, None }

    public class Global : MonoBehaviour
    {
        public static Global instance { get; private set; }
        public Map currentMap { get; private set; }
        [SerializeField] List<Map> maps;
        public List<PlayerTeam> playerTeams { get; private set; }
        public MatchTypes matchType { get; set; }

        //THIS IS TEMPORARY!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public List<PlayerBuilder> playerBuilders = new List<PlayerBuilder>();



        public AbstractMovement[] movementTypes = new AbstractMovement[3];
        public IActiveEntity currentEntity { get; set; }
        public int playerCount { get; set; }

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
            DontDestroyOnLoad(this.gameObject);
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
            movementTypes[(int)MovementTypes.Ground] = new GroundMovement();
            movementTypes[(int)MovementTypes.Flying] = new FlyingMovement();
            movementTypes[(int)MovementTypes.None] = null;
            playerTeams = new List<PlayerTeam>();
            PlayerTeam teamLeft = new PlayerTeam(0, 1);
            PlayerTeam teamRight = new PlayerTeam(1, 1);
            playerTeams.Add(teamLeft);
            playerTeams.Add(teamRight);
            matchType = MatchTypes.None;
            playerCount = 2;
            GameRound.instance.Setup();
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
                    foreach (Unit unit in player.playerUnits)
                    {
                        returnList.Add(unit);
                    }
                }
            }
            return returnList;
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

        public static List<GameObject> FindAllObjectsInLine(Vector3 start, Vector3 end, float lineYMultiplyer = 1)
        {
            var VectorToTarget = -start + end;

            Ray ray = new Ray(start + Vector3.up* lineYMultiplyer, VectorToTarget+Vector3.up* lineYMultiplyer);
            RaycastHit[] hits = Physics.RaycastAll(ray, Vector3.Distance(start, end));
            var list = new List<GameObject>();

            foreach (var hit in hits)
            {
                    list.Add(hit.transform.gameObject);
            }


            return list;
        }

        public void SetMap(string mapName)
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
    }
}
