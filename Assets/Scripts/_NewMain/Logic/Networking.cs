using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattlescapeLogic
{
    //Maybe this class requires SOME refactors :D it's mostly copy-pasted from old classes being deleted. COntains ALL RPCs and their Commands.
    //Also - if this class stays as is, it is NOT used only for networked games, but for ALL

    public class Networking : MonoBehaviour
    {


        public PhotonView photonView { get; private set; }
        public static Networking instance { get; set; }

        //THIS will get changed when we have NEW UI stuff ;>
        RetaliationButtonsScript _retaliationChoiceUI;
        RetaliationButtonsScript retaliationChoiceUI
        {
            get
            {
                if (_retaliationChoiceUI == null)
                {
                    _retaliationChoiceUI = FindObjectOfType<RetaliationButtonsScript>();
                }
                return _retaliationChoiceUI;
            }
        }

        //THIS will get changed when we have NEW UI stuff ;>
        GameObject _waitingForRetaliationUI;
        GameObject waitingForRetaliationUI
        {
            get
            {
                if (_waitingForRetaliationUI == null)
                {
                    _waitingForRetaliationUI = GameObject.FindGameObjectWithTag("InfoRetal");
                }
                return _waitingForRetaliationUI;
            }
        }



        public Unit retaliatingUnit { get; private set; }
        public Unit retaliationTarget { get; private set; }
        public int playersWhoAlreadyEndedPregame { get; private set; }

        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            photonView = GetComponent<PhotonView>();
        }

        public void SendCommandToLoadScene(string scene)
        {
            if (PhotonNetwork.IsConnected)
            {
                GetComponent<PhotonView>().RPC("RPCLoadScene", RpcTarget.All, scene);
            }
            else
            {
                FindObjectOfType<LevelLoader>().LoadScene(scene);
            }
        }

        [PunRPC]
        void RPCLoadScene(string scene)
        {
            FindObjectOfType<LevelLoader>().LoadScene(scene);
        }


        public void SendCommandToSetHeroName(int teamIndex, int playerIndex, string heroName)
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC("RPCSetHeroName", Photon.Pun.RpcTarget.All, teamIndex, playerIndex, heroName);
            }
            else
            {
                SetHeroName(teamIndex, playerIndex, heroName);
            }
        }

        [PunRPC]
        void RPCSetHeroName(int team, int indexInTeam, string heroName)
        {
            SetHeroName(team, indexInTeam, heroName);
        }

        void SetHeroName(int team, int indexInTeam, string heroName)
        {
            Player player = Global.instance.playerTeams[team].players[indexInTeam];
            player.SetHeroName(heroName);
        }

        public void SendCommandToAddPlayer(PlayerTeam playerTeam, Player player)
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC("RPCAddPlayer", RpcTarget.Others, player.index, player.isObserver, player.playerName, (int)player.colour, (int)player.race, player.team.index);
                playerTeam.AddNewPlayer(player);
            }
            else
            {
                playerTeam.AddNewPlayer(player);
            }
        }

        [PunRPC]
        void RPCAddPlayer(int playerIndex, bool isObserver, string playerName, int colour, int race, int teamIndex)
        {
            PlayerTeam playerTeam = Global.instance.playerTeams[teamIndex];

            PlayerBuilder playerBuilder = new PlayerBuilder();
            playerBuilder.index = playerIndex;
            playerBuilder.type = PlayerType.Network;
            playerBuilder.isObserver = isObserver;
            playerBuilder.playerName = playerName;
            playerBuilder.colour = (PlayerColour)colour;
            playerBuilder.race = (Race)race;
            playerBuilder.team = playerTeam;

            Player newPlayer = new Player(playerBuilder);
            playerTeam.AddNewPlayer(newPlayer);
        }

        //When u get disconnected, opponents will see this
        [PunRPC]
        public void RPCConnectionLossScreen(string text)
        {
            StartCoroutine(TurnDisconnectionScreenVisible(text));

        }

        //This belongs in some UI code but we don't have any new yet
        IEnumerator TurnDisconnectionScreenVisible(string text)
        {
            CanvasGroup screen = GameObject.FindGameObjectWithTag("DisconnectedScreen").GetComponent<CanvasGroup>();
            GameObject.FindGameObjectWithTag("DisconnectedText").GetComponent<Text>().text = text;
            while (screen.alpha < 1)
            {
                UIManager.SmoothlyTransitionActivity(screen.gameObject, true, 0.01f);
                yield return null;
            }

        }

        public void SendCommandToAddObstacles()
        {
            if (Global.instance.matchType == MatchTypes.Online && PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("RPCSetSeed", RpcTarget.All, Random.Range(0, 99999));
            }
            else if (Global.instance.matchType != MatchTypes.Online)
            {
                Global.instance.currentMap.mapVisuals.GenerateObjects(Random.Range(0, 99999));
            }
        }

        [PunRPC]
        void RPCSetSeed(int s)
        {
            StartCoroutine(PutObstaclesWhenPossible(s));
        }

        IEnumerator PutObstaclesWhenPossible(int s)
        {
            while (Global.instance.currentMap.mapVisuals == null)
            {
                yield return new WaitForSeconds(1f);
            }
            Global.instance.currentMap.mapVisuals.GenerateObjects(s);
        }

        public void SendCommandToDestroyObstacle(Unit sourceUnit, Obstacle myObstacle)
        {
            if(Global.instance.matchType == MatchTypes.Online && PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(
                    "RPCDestroyObstacle", 
                    RpcTarget.All, 
                    sourceUnit.currentPosition.bottomLeftCorner.position.x, 
                    sourceUnit.currentPosition.bottomLeftCorner.position.z,
                    myObstacle.currentPosition.bottomLeftCorner.position.x,
                    myObstacle.currentPosition.bottomLeftCorner.position.z);
            }
            else if (Global.instance.matchType != MatchTypes.Online)
            {
                myObstacle.Destruct(sourceUnit);
            }
        }

        [PunRPC]
        void RPCDestroyObstacle(int sourceX, int sourceZ, int obstacleX, int obstacleZ)
        {
            Obstacle obstacle = Global.instance.currentMap.board[obstacleX, obstacleZ].GetMyObject<Obstacle>();
            Unit unit = Global.instance.currentMap.board[obstacleX, obstacleZ].GetMyObject<Unit>();
            obstacle.Destruct(unit);
        }


        /// <summary>
        /// Used to either perform movement in offline modes or send an RPC in online mode.
        /// </summary>
        /// <param name="unit"> Unit to be moved to the last tile in Path made by PathCreator</param>
        public void SendCommandToMove(Unit unit, MultiTile destination)
        {
            PlayerInput.instance.isInputBlocked = true; //this makes sense only on the 'active' PC' that's why I put it here ;)

            if (Global.instance.matchType == MatchTypes.Online)
            {
                int startX = unit.currentPosition.bottomLeftCorner.position.x;
                int startZ = unit.currentPosition.bottomLeftCorner.position.z;
                int endX = destination.bottomLeftCorner.position.x;
                int endZ = destination.bottomLeftCorner.position.z;

                photonView.RPC(
                    "RPCDoMovement",
                    RpcTarget.All,
                    startX,
                    startZ,
                    endX,
                    endZ);
            }
            else
            {
                unit.Move(destination);
            }
        }

        [PunRPC]
        void RPCDoMovement(int startX, int startZ, int endX, int endZ)
        {
            Tile bottomLeftCorner = Global.instance.currentMap.board[startX, startZ];
            Unit unit = bottomLeftCorner.GetMyObject<Unit>();
            if (unit == null)
            {
                Debug.LogError("NoUnit!");
                Log.SpawnLog("NO UNIT TO MOVE!");
                return;
            }
            MultiTile destination = MultiTile.Create(Global.instance.currentMap.board[endX, endZ],unit.currentPosition.size);
            unit.Move(destination);
        }

        /// <summary>
        /// Used to either perform an attack in offline modes or send an RPC in online mode.
        /// </summary>
        /// <param name="attackingUnit"></param>
        /// <param name="target"></param>
        public void SendCommandToStartAttack(Unit attackingUnit, IDamageable target)
        {
            PlayerInput.instance.isInputBlocked = true;
            Position targetPosition = new Position(Mathf.RoundToInt(target.GetMyPosition().x), Mathf.RoundToInt(target.GetMyPosition().x));
           
            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC(
                    "RPCAttack", RpcTarget.All,
                    attackingUnit.currentPosition.bottomLeftCorner.position.x,
                    attackingUnit.currentPosition.bottomLeftCorner.position.z,
                    targetPosition.x,
                    targetPosition.z);
            }
            else
            {
                attackingUnit.Attack(target);
            }

        }

        [PunRPC]
        void RPCAttack(int sourceX, int sourceZ, int targetX, int targetZ)
        {
            Unit attackingUnit = Global.instance.currentMap.board[sourceX, sourceZ].GetMyObject<Unit>();
            IDamageable target = Global.instance.currentMap.board[targetX, targetZ].GetMyDamagableObject();                   
            attackingUnit.Attack(target);
        }


        public void SendCommandToGiveChoiceOfRetaliation(Unit retaliatingUnit, Unit target)
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
                GetComponent<PhotonView>().RPC
                    ("RPCRetaliation", 
                    RpcTarget.All, 
                    retaliatingUnit.currentPosition.bottomLeftCorner.position.x, 
                    retaliatingUnit.currentPosition.bottomLeftCorner.position.z,
                    target.currentPosition.bottomLeftCorner.position.x, 
                    target.currentPosition.bottomLeftCorner.position.z);
            }
            else
            {
                RetaliationChoice(retaliatingUnit, target);
            }

        }
        [PunRPC]
        void RPCRetaliation(int attackerX, int attackerZ, int targetX, int targetZ)
        {

            Unit retaliatingUnit = Global.instance.currentMap.board[attackerX, attackerZ].GetMyObject<Unit>();
            if (retaliatingUnit.GetMyOwner().type != PlayerType.Local)
            {
                UIManager.InstantlyTransitionActivity(waitingForRetaliationUI, true);
                GameRound.instance.SetPhaseToEnemy();
                return;
            }
            Unit target = Global.instance.currentMap.board[targetX, targetZ].GetMyObject<Unit>();
            RetaliationChoice(retaliatingUnit, target);

        }
        void RetaliationChoice(Unit _retaliatingUnit, Unit _retaliationTarget)
        {
            retaliationChoiceUI.TurnOn();
            GameRound.instance.SetPhaseToEnemy();
            retaliatingUnit = _retaliatingUnit;
            retaliationTarget = _retaliationTarget;
        }

        /// <summary>
        /// Used to either perform retaliation in offline modes or send an RPC in online mode.
        /// </summary>
        /// <param name="retaliatingUnit"></param>
        /// <param name="retaliationTarget"></param>
        public void SendCommandToRetaliate(Unit retaliatingUnit, Unit retaliationTarget)
        {
            //note also that AttackTarget is now the guy who retaliates, and AttackingUnit is getting hit.

            if (Global.instance.matchType == MatchTypes.Online)
            {
                int unitX = retaliatingUnit.currentPosition.bottomLeftCorner.position.x;
                int unitZ = retaliatingUnit.currentPosition.bottomLeftCorner.position.z;
                photonView.RPC("RPCRetaliate", RpcTarget.All, unitX, unitZ);
            }
            else
            {
                retaliatingUnit.RetaliateTo(retaliationTarget);
            }
        }

        [PunRPC]
        void RPCRetaliate(int attackerX, int attackerZ, int targetX, int targetZ)
        {
            Unit attacker = Global.instance.currentMap.board[attackerX, attackerZ].GetMyObject<Unit>();
            Unit target = Global.instance.currentMap.board[targetX, targetZ].GetMyObject<Unit>();
            attacker.RetaliateTo(target);
        }       

        public void SendCommandToHit(Unit source, IDamageable target, int damage = -1)
        {
            if (damage == -1)
            {
                damage = DamageCalculator.CalculateDamage(source, target);
            }
            if (Global.instance.matchType == MatchTypes.Online)
            {
                int targetX = Mathf.RoundToInt(target.GetMyPosition().x);
                int targetZ = Mathf.RoundToInt(target.GetMyPosition().z);
                photonView.RPC
                    ("RPCHitTarget",
                    RpcTarget.All,
                    source.currentPosition.bottomLeftCorner.position.x,
                    source.currentPosition.bottomLeftCorner.position.z,
                    targetX,
                    targetZ,
                    damage);
            }
            else
            {
                source.HitTarget(target, damage);
            }
        }

        [PunRPC]
        void RPCHitTarget(int sourceX, int sourceZ, int targetX, int targetZ, int damage)
        {
            Unit source = Global.instance.currentMap.board[sourceX, sourceZ].GetMyObject<Unit>();
            IDamageable target = Global.instance.currentMap.board[targetX, targetZ].GetMyDamagableObject();
            source.HitTarget(target, damage);
        }

        public void SendCommandToNotRetaliate()
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC("RPCNoRetaliation", RpcTarget.All);
            }
            else
            {
                FinishRetaliation();
            }
        }


        [PunRPC]
        void RPCNoRetaliation()
        {
            FinishRetaliation();
        }

        public void FinishRetaliation()
        {
            UIManager.SmoothlyTransitionActivity(waitingForRetaliationUI, false, 0.001f);
            GameRound.instance.ResetPhaseAfterEnemy();
        }

        public void SendCommandToEndTurnPhase()
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC("RPCEndTurnPhase", RpcTarget.All);
            }
            else
            {
                GameRound.instance.EndOfPhase();
            }
        }

        [PunRPC]
        void RPCEndTurnPhase()
        {
            GameRound.instance.EndOfPhase();
        }

        [PunRPC]
        void RPCPlayerEndedPreGame()
        {
            playersWhoAlreadyEndedPregame++;
            //Debug.Log("Players total: " + Global.instance.GetActivePlayerCount());
            //Debug.Log("Players who ended: " + playersWhoAlreadyEndedPregame);
            if (playersWhoAlreadyEndedPregame == Global.instance.GetActivePlayerCount())
            {
                GameRound.instance.StartGame();
            }
        }

        public void PlayerEndedPreGame()
        {
            photonView.RPC("RPCPlayerEndedPreGame", RpcTarget.All);
        }

    }
}
