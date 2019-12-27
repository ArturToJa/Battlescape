using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AI_Movement : AI_Base_Movement
{
    static List<BattlescapeLogic.Unit> unitsSkippingTurn;

    public AI_Movement(int ID) : base(ID)
    {
        base.CallTheConstructor(ID);
        if (unitsSkippingTurn == null)
        {
            unitsSkippingTurn = new List<BattlescapeLogic.Unit>();
        }
    }

    protected override Queue<BattlescapeLogic.Unit> GetPossibleUnits()
    {

        if (GetUnitsWithPriority() == null)
        {
            //GameStateManager.NextPhase();
            return null;
        }

        Queue<BattlescapeLogic.Unit> myUnitsToMove = new Queue<BattlescapeLogic.Unit>(GetUnitsWithPriority());
        BattlescapeLogic.Unit firstUnit = myUnitsToMove.Peek();
        while (unitsSkippingTurn.Contains(firstUnit) == true || firstUnit.statistics.movementPoints > 0 == false)
        {
            myUnitsToMove.Dequeue();
            if (myUnitsToMove.Count == 0)
            {
                if (GetUnitsWithPriority() == null)
                {
                    //GameStateManager.NextPhase();
                }
                return null;
            }
            else
            {
                firstUnit = myUnitsToMove.Peek();
            }
        }
        return myUnitsToMove;
    }

    List<Unit> GetUnitsWithPriority()
    {
        foreach (Unit ally in allyList)
        {
            if (ally.IsRanged() && ((ally is Hero) == false) && ally.CanStillMove() && unitsSkippingTurn.Contains(ally) == false)
            {
                return GetAllShooters();
            }
        }
        foreach (Unit ally in allyList)
        {
            if (((ally is Hero) == false) && ally.CanStillMove() && unitsSkippingTurn.Contains(ally) == false)
            {
                return GetAllMelee();
            }

        }

        foreach (Unit ally in allyList)
        {
            if (ally.CanStillMove() && unitsSkippingTurn.Contains(ally) == false)
            {
                return GetAllHeroes();
            }
        }


        return null;
    }

    List<Unit> GetAllHeroes()
    {
        List<BattlescapeLogic.Unit> temp = new List<BattlescapeLogic.Unit>();
        foreach (BattlescapeLogic.Unit ally in allyList)
        {
            if (ally is Hero && ally.IsAlive())
            {
                temp.Add(ally);
            }

        }
        return temp;
    }

    List<BattlescapeLogic.Unit> GetAllMelee()
    {
        List<BattlescapeLogic.Unit> temp = new List<BattlescapeLogic.Unit>();
        foreach (BattlescapeLogic.Unit ally in allyList)
        {
            if (ally.IsRanged() == false && ((ally is Hero) == false) && ally.IsAlive())
            {
                temp.Add(ally);
            }

        }
        return temp;
    }

    List<BattlescapeLogic.Unit> GetAllShooters()
    {
        List<BattlescapeLogic.Unit> temp = new List<BattlescapeLogic.Unit>();
        foreach (BattlescapeLogic.Unit ally in allyList)
        {
            if (ally.IsRanged() && ((ally is Hero) == false) && ally.IsAlive())
            {
                temp.Add(ally);
            }

        }
        return temp;
    }

    public override float EvaluateTile(BattlescapeLogic.Unit currentUnit, Tile tile, int enemiesDirection, Dictionary<BattlescapeLogic.Unit, List<Tile>> EnemyMovementRanges)
    {
        return 0;
        /*
        float Evaluation = 0f;
        //First, i want to introduce simple evaluation concepts. No tricks and special stuff, only very basic basics.

        // Lets check, if WE are a SHOOTER or a HERO. If yes, we will want to act totally diferently than the rest.

        // THEN if we are a HERO, lets stay in the second line. Lets defend shooters, cause currently we dont have MAGES yet, they will act differently. Lets not want to attack THAT much and lets REALLY not want to get hit. Lets stay close to friends, too!

        // If we are a shooter, lets first not want to get to combat. Lets try to get into shooting position. Lets try REALLY hard not to get hit.

        // SHOOTERS DONT want to stay next to other shooters. Obviously.

        // If we are NEITHER, lets try to first attack if good situation (prio on hero and shooters). Then lets try to protecc shooters and heroes. Also, bonus points for staying together (not only for real purpose, also cause it looks much more pro for player's eyes).
        List<Tile> neighbours = tile.neighbours;

        if (currentUnitis Hero)
        {
            // we ARE a hero.
            // 1. stay close to friends
            // 2. defend shooterz
            // 3. away from enemies
            // 4. attack (especially when singe enemy, weaker, shooter).

            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i].myUnit != null && neighbours[i].myUnit.owner == currentUnit.PlayerID && neighbours[i].myUnit != currentUnit)
                {
                    // there is an ally on next tile. Give like +0.1f for it.
                    Evaluation += 0.1f;
                    //  Debug.Log("I: " + i + " Tile: " + tile.transform.position.x + " " + tile.transform.position.z + " Added 0.1 for ally on tile: " + tile.neighbours[i]);
                    if (neighbours[i].myUnit.IsRanged() && IsDefendingAllyIfStandingHere(tile.transform.position, neighbours[i].myUnit.transform.position, enemiesDirection))
                    {
                        //   Debug.Log("Added 0.1 for shooter: " + tile.neighbours[i].myUnit);
                        Evaluation += 0.2f;
                        foreach (BattlescapeLogic.Unit uniterino in neighbours[i].myUnit.AllyList)
                        {
                            if (uniterino.IsRanged() == false && IsDefendingAllyIfStandingHere(uniterino.transform.position, neighbours[i].myUnit.transform.position, enemiesDirection))
                            {
                                // someone already defends this unit, lets deduct like 0.05f for it..
                                Evaluation -= 0.05f;
                            }
                        }
                    }
                    else if (neighbours[i].myUnit.IsRanged() == false && IsDefendingAllyIfStandingHere(neighbours[i].myUnit.transform.position, tile.transform.position, enemiesDirection))
                    {
                        Evaluation += 0.1f;
                    }
                }
            }



            int enemiesInRange = 0;

            foreach (var enemy in EnemyMovementRanges)
            {
                if (enemy.Value.Contains(tile))
                {
                    float hate = CheckHowMuchIHateToBeAttackedByThisUnit(currentUnit, enemy.Key.GetComponent<BattlescapeLogic.Unit>());
                    Evaluation -= hate;
                    if (hate > 0)
                    {
                        //   Debug.Log("Tile: " + tile + "'s evaluation got lowered by: " + hate + " because of: " + enemy.Key.gameObject);
                    }
                    enemiesInRange++;
                }
            }

            if (enemiesInRange > 2)
            {
                Evaluation -= enemiesInRange * 0.15f;
                //  Debug.Log("Tile: " + tile + "'s evaluation got lowered by: " + enemiesInRange * 0.1f + " because of " + enemiesInRange + " enemies in range of the tile.");
            }



            /*if (tile.IsProtectedByEnemyOf(ID))
            {
                // Here the hero is attacking an enemy. He will like it, if a) there is only one enemy b) enemy is low on HP c) enemy is shooter. He will not like it, if enemies are 3+, if they are strong and full health etc.

                Evaluation = EvaluateCombatTile(currentUnit, tile, Evaluation);
            }

        }
        else if (currentUnit.IsRanged())
        {
            // we ARE shooter.
            // 1. we DONT want to stay close to other shooters
            // 2. we dont want to get attacked
            // 3. we absolutely do not want to get into melee combat like EVER?
            // 4. we want to find the best shooting target and we want to have him in range - especially in "good" range if possibru
            float penaltyForNearbyShooters = 0;
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i].myUnit != null && neighbours[i].myUnit.owner == currentUnit.PlayerID && neighbours[i].myUnit != currentUnit && neighbours[i].myUnit.IsRanged())
                {
                    // there is an allied SHOOTER on next tile. Give like -0.2f for it. We do not really like this type of positions.
                    penaltyForNearbyShooters -= 0.4f;
                    // Debug.Log(" Tile: " + tile.transform.position.x + " " + tile.transform.position.z + " Subtracted 0.2 for allied shooter on tile: " + tile.neighbours[i]);
                }
            }
            Evaluation += penaltyForNearbyShooters;

            /*int enemiesInRange = 0;
            float penaltyForEnemiesInRange = 0;

            foreach (var enemy in EnemyMovementRanges)
            {
                enemiesInRange++;
            }

            if (enemiesInRange > 2)
            {
                penaltyForEnemiesInRange -= enemiesInRange * 0.03f;
                // Debug.Log("Tile: " + tile + "'s evaluation got lowered by: " + enemiesInRange * 0.1f + " because of " + enemiesInRange + " enemies in range of the tile.");
                foreach (var enemy in EnemyMovementRanges)
                {
                    if (enemy.Value.Contains(tile))
                    {
                        float hate = CheckHowMuchIHateToBeAttackedByThisUnit(currentUnit, enemy.Key.GetComponent<BattlescapeLogic.Unit>());
                        penaltyForEnemiesInRange -= hate;
                        if (hate > 0)
                        {
                            // Debug.Log("Tile: " + tile + "'s evaluation got lowered by: " + hate + " because of: " + enemy.Key.gameObject);
                        }
                    }
                }

            }
            Evaluation += penaltyForEnemiesInRange;
            
            if (tile.IsProtectedByEnemyOf(ID))
            {
                // we are entering melee combat. Let's FOR NOW give it a -0.5f just for that. We just DO NOT want our shooters to enter melee combat right now.. Then maybe we will give some situations where it will be an OK move?

                if (GameStateManager.IsGreenActive() == (VictoryLossChecker.GreenScore < VictoryLossChecker.RedScore + 5))
                {
                    //we are heavily loosing, and maybe there is a heavily wounded enemy in there?
                }

                foreach (ClickableTile clit in tile.neighbours)
                {
                    if (clit.myUnit!= null && clit.myUnitis Hero && clit.myUnit.isGreen != GameStateManager.IsGreenActive())
                    {
                        //there IS an enemy hero in there AND he is heavily wounded.. We MIGHT think about getting in combat maybe?
                    }
                }

                Evaluation -= 5f;
            }

            //NOW LETS find the best shooting target possible! And give some value to it XD?
            float GoodBoiBonus = 0f;
            float NoTargetButGoodTileForFutureBonus = 0f;
            BattlescapeLogic.Unit bestTarget = ShootingAITool.FindBestTarget(currentUnit.GetComponent<ShootingScript>(), tile.transform.position, currentUnit.GetComponent<ShootingScript>().theUnit.statistics.GetCurrentAttackRange());
            if (bestTarget != null)
            {
                // so if the best target is not null, so it is possible to shoot at SOMEONE from the Tile being evaluated, then..
                if (CombatController.Instance.WouldItBePossibleToShoot(currentUnit.GetComponent<ShootingScript>(), tile.transform.position, ShootingAITool.FindBestTarget(currentUnit.GetComponent<ShootingScript>(), tile.transform.position, currentUnit.GetComponent<ShootingScript>().theUnit.statistics.GetCurrentAttackRange()).transform.position).Value == true)
                {
                    // if it is in "bad range"
                    var temp = 0.25f + 0.5f * ShootingAITool.EvaluateAsATarget(currentUnit, bestTarget.currentPosition);
                    // Debug.Log("We just incremented our evaluation for tile: " + tile + " by " + temp + "for this target in bad range: " + bestTarget);
                    GoodBoiBonus += temp;
                }
                else
                {
                    // it is not "bad range"
                    var temp = 0.7f + ShootingAITool.EvaluateAsATarget(currentUnit, bestTarget.currentPosition);
                    // Debug.Log("We just incremented our evaluation for tile: " + tile + " by " + temp + "for this target in good range: " + bestTarget);
                    GoodBoiBonus += temp;
                }
            }
            else
            {
                // we DONT have any shooting target.. SO we have to not be a pussy and go forward. NOTE that if we wanted AI to play good we would probably build this different way :P
                // HERE i am "missusing" the function. Originally function is for checking if im "in front" of an ally in relation to "most" of the enemies. Here, I am checkign if im "in front" of.. MYSELF, if im on TheTile. 
                // So i am checking if going to the Tile is moving towards enemies.
                if (IsDefendingAllyIfStandingHere(tile.transform.position, currentUnit.transform.position, enemiesDirection))
                {
                    NoTargetButGoodTileForFutureBonus += 0.1f;
                }

                // and finally here we want to get to shooting range in 2 turns. Is it good - idk, but it "feels" right xD
                BattlescapeLogic.Unit NextTurnBestTarget = ShootingAITool.FindBestTarget(currentUnit.GetComponent<ShootingScript>(), tile.transform.position, currentUnit.GetComponent<ShootingScript>().theUnit.statistics.GetCurrentAttackRange() + currentUnit.GetComponent<BattlescapeLogic.Unit>().GetCurrentMoveSpeed(false));
                if (NextTurnBestTarget != null)
                {
                    // we do not care if it is bad range cause we are a dumb AI
                    var temp = 0.25f + 0.25f * ShootingAITool.EvaluateAsATarget(currentUnit, NextTurnBestTarget.currentPosition);
                    // Debug.Log("We just incremented our evaluation for tile: " + tile + " by " + temp + "for this target in next turn range: " + bestTarget);
                    NoTargetButGoodTileForFutureBonus += temp;
                }


            }
            Evaluation += NoTargetButGoodTileForFutureBonus;
            Evaluation += GoodBoiBonus;
        }
        else
        {
            // we are MELELELEE unit. We wan to:
            // 1. FIGHT! (Much more than Hero)
            // 2. Defend shooters and hero (but less than we want to fight)
            // 3. Go "forwarderino" more likely than back.
            if (currentUnit.CheckIfIsInCombat())
            {
                // we are checking the QC.
                foreach (var enemy in currentUnit.EnemyList)
                {
                    if (enemy.IsRanged())
                    {
                        Evaluation -= 10f;
                    }
                }
            }
            if (tileIsProtectedByEnemyOf(ID))
            {
                // Here the unit is attacking an enemy. For now, he "wants to fight" the same as the Hero XD. We will fine-tune it all with a UnitAIData script which will contain all the data needed for this ;)
                Evaluation = EvaluateCombatTile(currentUnit, tile, Evaluation);
            }
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (neighbours[i].myUnit != null && neighbours[i].myUnit.owner == currentUnit.PlayerID && neighbours[i].myUnit != currentUnit)
                {
                    // there is an ally on next tile. Give like +0.01f for it.
                    Evaluation += 0.01f;
                    //  Debug.Log("I: " + i + " Tile: " + tile.transform.position.x + " " + tile.transform.position.z + " Added 0.01 for ally on tile: " + tile.neighbours[i]);
                    if (neighbours[i].myUnit.IsRanged() && IsDefendingAllyIfStandingHere(tile.transform.position, neighbours[i].myUnit.transform.position, enemiesDirection))
                    {
                        //   Debug.Log("Added 0.1 for shooter: " + tile.neighbours[i].myUnit);
                        Evaluation += 0.2f;
                        foreach (BattlescapeLogic.Unit uniterino in neighbours[i].myUnit.AllyList)
                        {
                            if (uniterino.IsRanged() == false && uniterino.GetComponent<HeroScript>() == null && IsDefendingAllyIfStandingHere(uniterino.transform.position, neighbours[i].myUnit.transform.position, enemiesDirection))
                            {
                                // someone already defends this unit, lets deduct like 0.01f for it..
                                Evaluation -= 0.01f;
                            }
                        }
                    }
                }
            }

            // HERE i am "missusing" the function. Originally function is for checking if im "in front" of an ally in relation to "most" of the enemies. Here, I am checkign if im "in front" of.. MYSELF, if im on TheTile. 
            // So i am checking if going to the Tile is moving towards enemies.
            if (IsDefendingAllyIfStandingHere(tile.transform.position, currentUnit.transform.position, enemiesDirection))
            {
                Evaluation += 0.1f;
            }

        }



        if (tile.name == "Tile_15_9")
        {
            //            Debug.Log("Right Corner's evaluation: " + Evaluation);
        }
        return Evaluation;
    */
    }

    private static float EvaluateCombatTile(BattlescapeLogic.Unit currentUnit, Tile tile, float Evaluation)
    {
        int enemiesOnNextTiles = 0;
        List<Tile> neighbours = tile.neighbours;
        foreach (Tile t in neighbours)
        {
            if (t.myUnit != null && t.myUnit.owner != currentUnit.owner)
            {
                enemiesOnNextTiles++;
                if (t.myUnit.statistics.healthPoints <= 2 && t.myUnit.statistics.healthPoints < currentUnit.statistics.healthPoints)
                {
                    Evaluation += 0.2f;
                    // Debug.Log("Evaluation of the tile: " + tile + " got increased by: 0.15f because of weaker enemy on the tile: " + t);
                    if (t.myUnit.statistics.GetCurrentDefence() < currentUnit.statistics.GetCurrentAttack())
                    {
                        Evaluation += 0.1f;
                        //     Debug.Log("Even more increase - defennce lower than attacker's attack");
                    }
                }

                if (t.myUnit.statistics.healthPoints >= 4 || t.myUnit.statistics.healthPoints > currentUnit.statistics.healthPoints + 1 || t.myUnit.statistics.GetCurrentAttack() > currentUnit.statistics.GetCurrentDefence())
                {
                    Evaluation -= 0.1f;
                    // Debug.Log("Evaluation of the tile: " + tile + " decreased by 0.2f because of strong enemy: " + t.myUnit + " on tile " + t);
                }
                if (t.myUnit.IsRanged())
                {
                    Evaluation += 0.3f;
                    //  Debug.Log("Evaluation of the tile: " + tile + " got increased by: 0.25f because of shooter on the tile: " + t);
                }

                if (enemiesOnNextTiles == 1)
                {
                    Evaluation += 0.2f;
                    //   Debug.Log("Evaluation of the tile: " + tile + " got increased by: 0.2f because of a single enemy only.");
                }
                if (enemiesOnNextTiles >= 3)
                {
                    Evaluation -= 0.2f * enemiesOnNextTiles;
                    //   Debug.Log("Evaluation of the tile: " + tile + " got decreased by: " + (0.2f * enemiesOnNextTiles) + " because of " + enemiesOnNextTiles + " enemies on neighbour tiles");
                }
            }
        }
        foreach (BattlescapeLogic.Unit unit in Object.FindObjectsOfType<BattlescapeLogic.Unit>())
        {
            if (unit.owner == currentUnit.owner && Mathf.Abs(tile.transform.position.x - unit.transform.position.x) <= unit.statistics.movementPoints + 1 && Mathf.Abs(tile.transform.position.z - unit.transform.position.z) <= unit.statistics.movementPoints + 1)
            {
                Evaluation += 0.01f;
            }
            if (unit.owner == currentUnit.owner && unit.currentPosition.neighbours.Contains(tile))
            {
                Evaluation += 0.2f;
            }
        }

        return Evaluation;
    }

    float CheckHowMuchIHateToBeAttackedByThisUnit(BattlescapeLogic.Unit currentUnit, BattlescapeLogic.Unit enemy)
    {
        // here we need to find a formula for how much an enemy is "scary" for us.
        float value = 0f;
        if (currentUnit.statistics.GetCurrentDefence() < enemy.statistics.GetCurrentAttack())
        {
            value += enemy.statistics.GetCurrentAttack() - currentUnit.statistics.GetCurrentDefence();
        }
        if (currentUnit.statistics.healthPoints < currentUnit.statistics.maxHealthPoints)
        {
            value = value + 0.25f * value * (currentUnit.statistics.maxHealthPoints - currentUnit.statistics.healthPoints);
        }
        if (currentUnit.IsRanged())
        {
            value += 0.01f;
            value = value * 1.2f;
        }
        //Debug.Log("Unit: " + currentUnit + " is afraid of: " + enemy + " this much: " + 0.1f*value);

        return 0.1f * value;
    }

    protected override void PerformTheAction(BattlescapeLogic.Unit currentUnit, KeyValuePair<Tile, float> target)
    {
        AI_Controller.tilesAreEvaluated = false;
        Tile theTile = target.Key;

        if (theTile != null)
        {
            //PathCreator.Instance.AddSteps(currentUnit.currentPosition,theTile);
            Networking.instance.SendCommandToMove(currentUnit, theTile);
            unitsSkippingTurn.Clear();
            Debug.Log("Chosen tile is: " + theTile);
        }
        else
        {
            Debug.Log("No tile available.");
            unitsSkippingTurn.Add(currentUnit);
        }
    }

    bool IsDefendingAllyIfStandingHere(Vector3 tile, Vector3 ally, int direction)
    {
        switch (direction)
        {
            case 0:
                return (tile.z > ally.z);
            case 1:
                return (tile.x > ally.x);
            case 2:
                return (tile.z < ally.z);
            case 3:
                return (tile.x < ally.x);
            default:
                Debug.Log("What?");
                return false;
        }
    }
}
