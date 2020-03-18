using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;


public class Ability_Human_Catapult_ShootToObstacle : Ability_Basic
{
    protected override void OnStart()
    {
        return;
    }

    protected override void OnUpdate()
    {
        return;
        /*if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(isBeingUsed.ToString() + ", " + (MouseManager.Instance.mouseoveredTile != null).ToString() + ", " + CombatController.Instance.WouldItBePossibleToShoot(this.GetComponent<ShootingScript>(), this.transform.position, MouseManager.Instance.mouseoveredTile.transform.position).Key.ToString() + ", " + UsesLeft.ToString());
        }*/
    }

    protected override void Use()
    {
        return;
    }

    protected override void CancelUse()
    {
        return;
    }

    protected override void SetTarget()
    {
        Target = null; // MouseManager.Instance.mouseoveredTile;
    }

    protected override bool IsUsableNow()
    {
        return true;
    }

    public override void Activate()
    {
       StartCoroutine(ShootToObstacle());
    }

    public override bool ActivationRequirements()
    {
        return true; //MouseManager.Instance.mouseoveredTile != null && MouseManager.Instance.mouseoveredTile.hasObstacle == true && CombatController.Instance.WouldItBePossibleToShoot(myUnit, this.transform.position, MouseManager.Instance.mouseoveredTile.transform.position) && Helper.FindChildWithTag(MouseManager.Instance.mouseoveredTile.gameObject, "Dice") != null;
    }

   

    IEnumerator ShootToObstacle()
    {
        Log.SpawnLog("Catapult shoots at an obstacle, destroying it completely!");
        myUnit.statistics.numberOfAttacks = 0;        
        GameObject objecto = null;
        foreach (Transform item in Target.transform)
        {
            if (item.gameObject.tag == "Dice")
            {
                objecto = item.gameObject;
            }
        }
        if (objecto == null)
        {
            Debug.LogError("WHY NULL?");
        }
        //StartCoroutine(CombatController.Instance.ShootToObstacle(myShootingScript,objecto.transform.position));
        yield return null;
        FinishUsing();
        
        
    }
    //protected override void ColourTiles()
    //{
    //    foreach (Tile tile in Global.instance.map.board)
    //    {
    //        if (tile.hasObstacle && CombatController.Instance.WouldItBePossibleToShoot(myUnit, this.transform.position, tile.transform.position) && Helper.FindChildWithTag(tile.gameObject, "Dice") != null)
    //        {
    //            BattlescapeGraphics.ColouringTool.ColourObject(tile, Color.red);
    //        }
    //    }
    //}


    /////////////////////// AI segment



    public override bool AI_IsGoodToUseNow()
    {
        // I dont know if AI should use this ability at all. Anyway currently it does nto even have this unit afaik, so ill just make it not use it.
        return false;
    }

    public override void AI_Activate(GameObject Target)
    {
        Debug.LogError("WTF? Why use this ability?");
        Log.SpawnLog("AI used an ability it should not have used. Tell Dogo about it.");
        return;
    }

    public override GameObject AI_ChooseTarget()
    {
        Debug.Log("this already is bad");
        return null;
    }
}



