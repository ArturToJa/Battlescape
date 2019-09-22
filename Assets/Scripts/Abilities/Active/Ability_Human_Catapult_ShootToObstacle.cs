using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(ShootingScript))]
public class Ability_Human_Catapult_ShootToObstacle : Ability_Basic
{
    ShootingScript myShootingScript;

    protected override void OnStart()
    {
        myShootingScript = myUnit.GetComponent<ShootingScript>();
    }

    protected override void OnUpdate()
    {
        return;
        /*if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(isBeingUsed.ToString() + ", " + (MouseManager.Instance.mouseoveredTile != null).ToString() + ", " + ShootingScript.WouldItBePossibleToShoot(this.GetComponent<ShootingScript>(), this.transform.position, MouseManager.Instance.mouseoveredTile.transform.position).Key.ToString() + ", " + UsesLeft.ToString());
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
        Target = MouseManager.Instance.mouseoveredTile;
    }

    protected override bool IsUsableNow()
    {
        return true;
    }

    public override void Activate()
    {
       StartCoroutine(ShootToObstacle());
    }

    protected override bool ActivationRequirements()
    {
        return MouseManager.Instance.mouseoveredTile != null && MouseManager.Instance.mouseoveredTile.hasObstacle == true && ShootingScript.WouldItBePossibleToShoot(this.GetComponent<ShootingScript>(), this.transform.position, MouseManager.Instance.mouseoveredTile.transform.position).Key && Helper.FindChildWithTag(MouseManager.Instance.mouseoveredTile.gameObject, "Dice") != null;
    }

   

    IEnumerator ShootToObstacle()
    {
        Log.SpawnLog("Catapult shoots at an obstacle, destroying it completely!");
        myUnit.GetComponent<ShootingScript>().hasAlreadyShot = true;
        myUnit.GetComponent<ShootingScript>().myTarget = Target.transform.position;
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
        StartCoroutine(CombatController.Instance.ShootToObstacle(myShootingScript,objecto.transform.position));
        yield return null;
        FinishUsing();
        
        
    }
    protected override void ColourTiles()
    {
        foreach (Tile tile in Map.Board)
        {
            if (tile.hasObstacle && ShootingScript.WouldItBePossibleToShoot(myShootingScript, this.transform.position, tile.transform.position).Key && Helper.FindChildWithTag(tile.gameObject, "Dice") != null)
            {
                tile.TCTool.ColourTile(Color.red);
            }
        }
    }


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



