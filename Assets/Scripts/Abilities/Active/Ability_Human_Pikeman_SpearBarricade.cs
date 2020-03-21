using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Human_Pikeman_SpearBarricade : Ability_Basic
{
    [SerializeField] GameObject BarricadePrefab;
    [SerializeField] Vector3[] Positions;
    [SerializeField] GameObject ClonePrefab;


    protected override void OnStart()
    {
        return;
    }

    protected override void OnUpdate()
    {
        return;
    }





    protected override bool IsUsableNow()
    {
        return
            true;
    }

    protected override void Use()
    {
        return;
    }

    protected override void CancelUse()
    {
        return;
    }








    public override bool ActivationRequirements()
    {
        return true;
            //MouseManager.Instance.mouseoveredTile != null &&
            //MouseManager.Instance.mouseoveredTile.IsWalkable() == true &&
            //MouseManager.Instance.mouseoveredTile.IsWalkable() &&
            //MouseManager.Instance.mouseoveredTile.neighbours.Contains(myUnit.currentPosition);
    }

    public override void Activate()
    {
        StartCoroutine(SpearBarricade());
    }

    IEnumerator SpearBarricade()
    {
        Log.SpawnLog("Pikeman uses Spear Barricade!");
        yield return null;
        FinishUsing();
        //myUnit.LookAtTheTarget(Target.transform.position, myUnit.GetComponentInChildren<BodyTrigger>().RotationInAttack);
        //myUnit.//myUnit.GetComponent<AnimController>().SpecialAttack();
        List<GameObject> visuals = new List<GameObject>();
        foreach (Vector3 position in Positions)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject copy = Instantiate(ClonePrefab, position + transform.position, ClonePrefab.transform.rotation, transform);
            visuals.Add(copy);
            Animator a = copy.GetComponent<Animator>();
            a.SetTrigger("SpecialAttack");
        }
        yield return new WaitForSeconds(1f - (Positions.Length*0.1f));
        GameObject barricade = Instantiate(BarricadePrefab, Target.transform.position, BarricadePrefab.transform.rotation, Target.transform);
        Target.myObstacle = barricade.GetComponent<Obstacle>();
        CreateVFXOn(barricade.transform, BasicVFX.transform.rotation);
        foreach (GameObject disposable in visuals)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(disposable);
            }
            else
            {
                Destroy(disposable);
            }

        }

    }







    public override void AI_Activate(GameObject Target)
    {
        SendCommandForActivation();
        Debug.Log("Activated Spear Barricade - hwy?");
    }

    public override GameObject AI_ChooseTarget()
    {
        return null;
    }

    public override bool AI_IsGoodToUseNow()
    {
        return false;
    }







    protected override void SetTarget()
    {
        Target = null;// MouseManager.Instance.mouseoveredTile;
    }
    protected override void ColourTiles()
    {
        foreach (Tile tile in myUnit.currentPosition.neighbours)
        {
            if (tile.IsWalkable())
            {
                tile.highlighter.TurnOn(Color.green);
            }
        }
    }

}
