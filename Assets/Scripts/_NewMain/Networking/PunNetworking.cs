using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [RequireComponent(typeof(PhotonView))]
    public class PunNetworking : NetworkingBaseClass
    {
        private PhotonView photonView;

        #region fieldsToRemoveAsap
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
        #endregion

        #region override
        protected override void Start()
        {
            base.Start();
            photonView = GetComponent<PhotonView>();
        }

        public override void Connect()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void Disconnect()
        {
            Debug.Log("Disconnected!");
            PhotonNetwork.Disconnect();
        }

        public override bool IsConnected()
        {
            return PhotonNetwork.IsConnected;
        }

        public override void JoinRoom(string roomName)
        {
            if (PhotonNetwork.JoinRoom(roomName))
            {

            }
            else
            {
                Debug.Log("Failed to join the room!");
            }
        }

        public override void SendCommandToAddPlayer(PlayerTeam playerTeam, Player player)
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

        public override void SendCommandToAddObstacles()
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

        public override void SendCommandToDestroyObstacle(Unit sourceUnit, Obstacle myObstacle)
        {
            if (Global.instance.matchType == MatchTypes.Online && PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(
                    "RPCDestroyObstacle",
                    RpcTarget.All,
                    sourceUnit.currentPosition.position.x,
                    sourceUnit.currentPosition.position.z,
                    myObstacle.currentPosition[0].position.x,
                    myObstacle.currentPosition[0].position.z);
            }
            else if (Global.instance.matchType != MatchTypes.Online)
            {
                myObstacle.Destruct(sourceUnit);
            }
        }

        public override void SendCommandToMove(Unit unit, Tile destination)
        {
            PlayerInput.instance.isInputBlocked = true; //this makes sense only on the 'active' PC' that's why I put it here ;)

            if (Global.instance.matchType == MatchTypes.Online)
            {
                int startX = Mathf.RoundToInt(unit.transform.position.x);
                int startZ = Mathf.RoundToInt(unit.transform.position.z);
                int endX = Mathf.RoundToInt(destination.transform.position.x);
                int endZ = Mathf.RoundToInt(destination.transform.position.z);

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

        public override void SendCommandToStartAttack(Unit attackingUnit, IDamageable target)
        {
            PlayerInput.instance.isInputBlocked = true;
            Position targetPosition = new Position(Mathf.RoundToInt(target.GetMyPosition().x), Mathf.RoundToInt(target.GetMyPosition().x));

            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC(
                    "RPCAttack", RpcTarget.All,
                    attackingUnit.currentPosition.position.x,
                    attackingUnit.currentPosition.position.z,
                    targetPosition.x,
                    targetPosition.z);
            }
            else
            {
                attackingUnit.Attack(target);
            }

        }

        public override void SendCommandToGiveChoiceOfRetaliation(Unit retaliatingUnit, Unit target)
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
                GetComponent<PhotonView>().RPC("RPCRetaliation", RpcTarget.All, retaliatingUnit.currentPosition.position.x, retaliatingUnit.currentPosition.position.z, target.currentPosition.position.x, target.currentPosition.position.z);
            }
            else
            {
                RetaliationChoice(retaliatingUnit, target);
            }

        }

        public override void SendCommandToRetaliate(Unit retaliatingUnit, Unit retaliationTarget)
        {
            //note also that AttackTarget is now the guy who retaliates, and AttackingUnit is getting hit.

            if (Global.instance.matchType == MatchTypes.Online)
            {
                int unitX = retaliatingUnit.currentPosition.position.x;
                int unitZ = retaliatingUnit.currentPosition.position.z;
                photonView.RPC("RPCRetaliate", RpcTarget.All, unitX, unitZ);
            }
            else
            {
                retaliatingUnit.RetaliateTo(retaliationTarget);
            }
        }

        public override void SendCommandToHit(Unit source, IDamageable target, int damage = -1)
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
                    source.currentPosition.position.x,
                    source.currentPosition.position.z,
                    targetX,
                    targetZ,
                    damage);
            }
            else
            {
                source.HitTarget(target, damage);
            }
        }

        public override void  SendCommandToNotRetaliate()
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

        public override void SendCommandToEndTurnPhase()
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
        #endregion

        #region private
        void RetaliationChoice(Unit _retaliatingUnit, Unit _retaliationTarget)
        {
            retaliationChoiceUI.TurnOn();
            GameRound.instance.SetPhaseToEnemy();
            retaliatingUnit = _retaliatingUnit;
            retaliationTarget = _retaliationTarget;
        }

        private void PlayerEndedPreGame()
        {
            photonView.RPC("RPCPlayerEndedPreGame", RpcTarget.All);
        }

        private void FinishRetaliation()
        {
            UIManager.SmoothlyTransitionActivity(waitingForRetaliationUI, false, 0.001f);
            GameRound.instance.ResetPhaseAfterEnemy();
        }
        #endregion

        #region RPCs
        [PunRPC]
        void RPCSetHeroName(int ID, string name)
        {
            HeroNames.SetHeroName(ID, name);
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

        [PunRPC]
        public void RPCConnectionLossScreen(string text)
        {
            StartCoroutine(TurnDisconnectionScreenVisible(text));

        }

        [PunRPC]
        void RPCSetSeed(int s)
        {
            StartCoroutine(PutObstaclesWhenPossible(s));
        }

        [PunRPC]
        void RPCDestroyObstacle(int sourceX, int sourceZ, int obstacleX, int obstacleZ)
        {
            Obstacle obstacle = Global.instance.currentMap.board[obstacleX, obstacleZ].myObstacle;
            Unit unit = Global.instance.currentMap.board[obstacleX, obstacleZ].myUnit;
            obstacle.Destruct(unit);
        }

        [PunRPC]
        void RPCDoMovement(int startX, int startZ, int endX, int endZ)
        {
            Tile startTile = Global.instance.currentMap.board[startX, startZ];
            Unit unit = startTile.myUnit;
            if (unit == null)
            {
                Debug.LogError("NoUnit!");
                Log.SpawnLog("NO UNIT TO MOVE!");
                return;
            }
            Tile destination = Global.instance.currentMap.board[endX, endZ];
            unit.Move(destination);
        }

        [PunRPC]
        void RPCAttack(int sourceX, int sourceZ, int targetX, int targetZ)
        {
            Unit attackingUnit = Global.instance.currentMap.board[sourceX, sourceZ].myUnit;
            IDamageable target = Global.instance.currentMap.board[targetX, targetZ].GetMyDamagableObject();
            attackingUnit.Attack(target);
        }

        [PunRPC]
        void RPCRetaliation(int attackerX, int attackerZ, int targetX, int targetZ)
        {

            Unit retaliatingUnit = Global.instance.currentMap.board[attackerX, attackerZ].myUnit;
            if (retaliatingUnit.GetMyOwner().type != PlayerType.Local)
            {
                UIManager.InstantlyTransitionActivity(waitingForRetaliationUI, true);
                GameRound.instance.SetPhaseToEnemy();
                return;
            }
            Unit target = Global.instance.currentMap.board[targetX, targetZ].myUnit;
            RetaliationChoice(retaliatingUnit, target);

        }

        [PunRPC]
        void RPCRetaliate(int attackerX, int attackerZ, int targetX, int targetZ)
        {
            Unit attacker = Global.instance.currentMap.board[attackerX, attackerZ].myUnit;
            Unit target = Global.instance.currentMap.board[targetX, targetZ].myUnit;
            attacker.RetaliateTo(target);
        }

        [PunRPC]
        void RPCHitTarget(int sourceX, int sourceZ, int targetX, int targetZ, int damage)
        {
            Unit source = Global.instance.currentMap.board[sourceX, sourceZ].myUnit;
            IDamageable target = Global.instance.currentMap.board[targetX, targetZ].GetMyDamagableObject();
            source.HitTarget(target, damage);
        }

        [PunRPC]
        void RPCNoRetaliation()
        {
            FinishRetaliation();
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
        #endregion

        #region Coroutines
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
        IEnumerator PutObstaclesWhenPossible(int s)
        {
            while (Global.instance.currentMap.mapVisuals == null)
            {
                yield return new WaitForSeconds(1f);
            }
            Global.instance.currentMap.mapVisuals.GenerateObjects(s);
        }
        #endregion
    }
}
