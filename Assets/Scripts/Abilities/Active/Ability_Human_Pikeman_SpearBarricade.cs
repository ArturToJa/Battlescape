﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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








    protected override bool ActivationRequirements()
    {
        return
            MouseManager.Instance.mouseoveredTile != null &&
            MouseManager.Instance.mouseoveredTile.isWalkable == true &&
            MouseManager.Instance.mouseoveredTile.IsLegalTile() &&
            MouseManager.Instance.mouseoveredTile.GetNeighbours().Contains(myUnit.myTile);
    }

    public override void Activate()
    {
        StartCoroutine(SpearBarricade());
    }

    IEnumerator SpearBarricade()
    {
        Log.SpawnLog("Pikeman uses Spear Barricade!");
        Target.hasObstacle = true;
        yield return null;
        FinishUsing();
        myUnit.GetComponent<UnitMovement>().LookAtTheTarget(Target.transform.position, myUnit.GetComponentInChildren<BodyTrigger>().RotationInAttack);
        myUnit.GetComponent<AnimController>().SpecialAttack();
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
        GameObject Barricade = Instantiate(BarricadePrefab, Target.transform.position, BarricadePrefab.transform.rotation, Target.transform);        
        CreateVFXOn(Barricade.transform, BasicVFX.transform.rotation);
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
        Target = MouseManager.Instance.mouseoveredTile;
    }
    protected override void ColourTiles()
    {
        foreach (Tile tile in myUnit.myTile.GetNeighbours())
        {
            if (tile.IsLegalTile())
            {
                tile.TCTool.ColourTile(Color.green);
            }
        }
    }

}
