using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Elves_Fencer_WideSwing : Ability_Basic
{
    [SerializeField] int Damage;
    UnitScript previousMousovered;
    List<GameObject> colouredUnits = new List<GameObject>();

    protected override void OnStart()
    {
        Target = myUnit.myTile;
    }

    protected override void OnUpdate()
    {
        if (isBeingUsed && MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.MouseoveredUnit != previousMousovered && MouseManager.Instance.MouseoveredUnit.PlayerID != myUnit.PlayerID)
        {
            previousMousovered = MouseManager.Instance.MouseoveredUnit;
            ColourPotentialTargets();
        }
        if (isBeingUsed && MouseManager.Instance.MouseoveredUnit == null)
        {
            UncolourPotentialTargets();
            previousMousovered = null;
        }
    }


    protected override bool IsUsableNow()
    {
        return true;
    }

    protected override void Use()
    {
        /*ColourTiles();*/
    }

    protected override void CancelUse()
    {
        /* TileColouringTool.UncolourAllTiles();*/
    }

    void ColourPotentialTargets()
    {
        foreach (Tile neighbour in myUnit.myTile.neighbours)
        {
            if (neighbour.myUnit != null && MouseManager.Instance.MouseoveredUnit.myTile.neighbours.Contains(neighbour))
            {
                PaintObject(neighbour.myUnit.gameObject, Color.red);
                colouredUnits.Add(neighbour.myUnit.gameObject);
            }
        }
    }
    void PaintObject(GameObject Object, Color color)
    {
        Renderer[] rs = Object.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            Material m = r.material;
            m.color = color;
            r.material = m;
        }
    }
    void UncolourPotentialTargets()
    {
        foreach (GameObject go in colouredUnits)
        {
            PaintObject(go, Color.white);
        }
    }



    protected override bool ActivationRequirements()
    {
        return
            MouseManager.Instance.mouseoveredTile != null &&
            MouseManager.Instance.mouseoveredTile.myUnit != null &&
            myUnit.EnemyList.Contains(MouseManager.Instance.mouseoveredTile.myUnit);
    }

    public override void Activate()
    {
        StartCoroutine(WideSwing());
    }

    IEnumerator WideSwing()
    {
        CreateVFXOn(transform, BasicVFX.transform.rotation);
        myUnit.GetComponent<UnitMovement>().LookAtTheTarget(Target.transform.position, 0/*myUnit.GetComponentInChildren<BodyTrigger>().RotationInAttack*/);
        GetComponent<AnimController>().SpecialAttack();
        Log.SpawnLog("Fencer uses Wide Swing, swinging his blade recklessly around him");
        //LOGIC
        UnitScript Enemy = Target.myUnit;
        // Hit for Target
        HitForSwing(Enemy);
        foreach (Tile neighbour in myUnit.myTile.neighbours)
        {
            if (neighbour.myUnit != null && Enemy.myTile.neighbours.Contains(neighbour))
            {
                HitForSwing(neighbour.myUnit);
            }
        }
        // for mutual neighbours.
        // add debuff
        PassiveAbility_Buff.AddBuff(gameObject, 2, 0, -myUnit.CurrentDefence, 0, 0, true, "FencerDebuff", null, 0, false, true, false);
        myUnit.hasAttacked = true;
        yield return null;
        FinishUsing();
    }

    void HitForSwing(UnitScript target)
    {
        target.DealDamage(Damage - target.CurrentDefence, true, false, false);
        PopupTextController.AddParalelPopupText("-" + (Damage - target.CurrentDefence), PopupTypes.Damage);
    }



    public override void AI_Activate(GameObject Target)
    {
        AlreadyUsedThisTurn = true;
        StartCoroutine(WideSwing());
    }

    public override GameObject AI_ChooseTarget()
    {
        // no idea really, on what criteria this AI should choose correct unit - but maybe You, dear reader, will know any better ;/ For now Random will do.
        return myUnit.EnemyList[Random.Range(0, myUnit.AllyList.Count)].gameObject;
    }

    public override bool AI_IsGoodToUseNow()
    {
        return myUnit.EnemyList.Count - myUnit.AllyList.Count >= 3;
    }





    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }


}

