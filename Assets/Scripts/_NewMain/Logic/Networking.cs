﻿using System.Collections;
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
            photonView = GetComponent<PhotonView>();
        }

        //Some STUPID stuff in old code, somewhere, IDK
        [PunRPC]
        void RPCSetHeroName(int ID, string name)
        {
            HeroNames.SetHeroName(ID, name);
        }

        public void SendCommandToAddPlayer(PlayerTeam playerTeam, Player player)
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC("RPCAddPlayer", PhotonTargets.Others, player.index, PlayerType.Network, player.isObserver, player.playerName, (int)player.colour, (int)player.race, player.team.index);
                playerTeam.AddNewPlayer(player);
            }
            else
            {
                playerTeam.AddNewPlayer(player);
            }
        }

        [PunRPC]
        void RPCAddPlayer(int playerIndex, int playerType, bool isObserver, string playerName, int colour, int race, int teamIndex)
        {
            PlayerTeam playerTeam = Global.instance.playerTeams[teamIndex];

            PlayerBuilder builder = new PlayerBuilder();

            builder.index = playerIndex;
            builder.type = (PlayerType)playerType;
            builder.isObserver = isObserver;
            builder.playerName = playerName;
            builder.colour = (PlayerColour)colour;
            builder.race = (Faction)race;
            builder.team = playerTeam;

            Player newPlayer = new Player(builder);
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
            if (Global.instance.matchType == MatchTypes.Online && PhotonNetwork.isMasterClient)
            {
                photonView.RPC("RPCSetSeed", PhotonTargets.All, Random.Range(0, 99999));
            }
            else if (Global.instance.matchType != MatchTypes.Online)
            {
                FindObjectOfType<MapVisuals>().RandomlyPutObstacles(Random.Range(0, 99999));
            }
        }

        [PunRPC]
        void RPCSetSeed(int s)
        {
            StartCoroutine(PutObstaclesWhenPossible(s));
        }

        IEnumerator PutObstaclesWhenPossible(int s)
        {
            while (FindObjectOfType<MapVisuals>() == null)
            {
                yield return new WaitForSeconds(1f);
            }
            FindObjectOfType<MapVisuals>().RandomlyPutObstacles(s);
        }


        /// <summary>
        /// Used to either perform movement in offline modes or send an RPC in online mode.
        /// </summary>
        /// <param name="unit"> Unit to be moved to the last tile in Path made by PathCreator</param>
        public void SendCommandToMove(Unit unit, Tile destination)
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
                    PhotonTargets.All,
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
            Tile startTile = Map.Board[startX, startZ];
            Unit unit = startTile.myUnit;
            if (unit == null)
            {
                Debug.LogError("NoUnit!");
                Log.SpawnLog("NO UNIT TO MOVE!");
                return;
            }
            Tile destination = Map.Board[endX, endZ];
            unit.Move(destination);
        }

        /// <summary>
        /// Used to either perform an attack in offline modes or send an RPC in online mode.
        /// </summary>
        /// <param name="Attacker"></param>
        /// <param name="Defender"></param>
        public void SendCommandToStartAttack(Unit Attacker, Unit Defender)
        {
            PlayerInput.instance.isInputBlocked = true;
            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC(
                    "RPCAttack", PhotonTargets.All,
                    Mathf.RoundToInt(Attacker.transform.position.x),
                    Mathf.RoundToInt(Attacker.transform.position.z),
                    Mathf.RoundToInt(Defender.transform.position.x),
                    Mathf.RoundToInt(Defender.transform.position.z));
            }
            else
            {
                Attacker.Attack(Defender);
            }

        }

        [PunRPC]
        void RPCAttack(int AttackerX, int AttackerZ, int DefenderX, int DefenderZ)
        {
            Unit Attacker = Map.Board[AttackerX, AttackerZ].myUnit;
            Unit Defender = Map.Board[DefenderX, DefenderZ].myUnit;
            Attacker.Attack(Defender);
        }


        public void SendCommandToGiveChoiceOfRetaliation(Unit retaliatingUnit, Unit target)
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
                GetComponent<PhotonView>().RPC("RPCRetaliation", PhotonTargets.All, retaliatingUnit.currentPosition.position.x, retaliatingUnit.currentPosition.position.z, target.currentPosition.position.x, target.currentPosition.position.z);
            }
            else
            {
                RetaliationChoice(retaliatingUnit, target);
            }

        }
        [PunRPC]
        void RPCRetaliation(int attackerX, int attackerZ, int targetX, int targetZ)
        {

            Unit retaliatingUnit = Map.Board[attackerX, attackerZ].myUnit;
            if (retaliatingUnit.owner.type != PlayerType.Local)
            {
                UIManager.InstantlyTransitionActivity(waitingForRetaliationUI, true);
                GameRound.instance.SetPhaseToEnemy();
                return;
            }
            Unit target = Map.Board[targetX, targetZ].myUnit;
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
                int unitX = retaliatingUnit.currentPosition.position.x;
                int unitZ = retaliatingUnit.currentPosition.position.z;
                photonView.RPC("RPCRetaliate", PhotonTargets.All, unitX, unitZ);
            }
            else
            {
                retaliatingUnit.RetaliateTo(retaliationTarget);
            }
        }

        [PunRPC]
        void RPCRetaliate(int attackerX, int attackerZ, int targetX, int targetZ)
        {
            Unit attacker = Map.Board[attackerX, attackerZ].myUnit;
            Unit target = Map.Board[targetX, targetZ].myUnit;
            attacker.RetaliateTo(target);
        }

        ////THIS is the actual damaging part!
        //public void SendCommandToHit(Unit source, Unit target)
        //{
        //   if(Global.instance.matchType == MatchTypes.Online)
        //    {
        //        photonView.RPC
        //            ("RPCHitTarget", 
        //            PhotonTargets.All,
        //            source.currentPosition.position.x, 
        //            source.currentPosition.position.z,
        //            target.currentPosition.position.x, 
        //            target.currentPosition.position.z, 
        //            DamageCalculator.CalculateBasicDamage(source, target));
        //    }
        //    else
        //    {                
        //        source.HitTarget(target, DamageCalculator.CalculateBasicDamage(source,target));
        //    }
        //}

        public void SendCommandToHit(Unit source, Unit target, int damage = -1)
        {
            if (damage == -1)
            {
                damage = DamageCalculator.CalculateBasicDamage(source, target);
            }
            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC
                    ("RPCHitTarget",
                    PhotonTargets.All,
                    source.currentPosition.position.x,
                    source.currentPosition.position.z,
                    target.currentPosition.position.x,
                    target.currentPosition.position.z,
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
            Unit source = Map.Board[sourceX, sourceZ].myUnit;
            Unit target = Map.Board[targetX, targetZ].myUnit;
            source.HitTarget(target, damage);
        }

        public void SendCommandToNotRetaliate()
        {
            if (Global.instance.matchType == MatchTypes.Online)
            {
                photonView.RPC("RPCNoRetaliation", PhotonTargets.All);
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
                photonView.RPC("RPCEndTurnPhase", PhotonTargets.All);
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
            photonView.RPC("RPCPlayerEndedPreGame", PhotonTargets.All);
        }

    }
}