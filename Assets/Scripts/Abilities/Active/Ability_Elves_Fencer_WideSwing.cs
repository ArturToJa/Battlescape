using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Elves_Fencer_WideSwing : Ability_Basic
{
    [SerializeField] int Damage;
    BattlescapeLogic.Unit previousMousovered;
    List<GameObject> colouredUnits = new List<GameObject>();

    protected override void OnStart()
    {
        Target = myUnit.currentPosition;
    }

    protected override void OnUpdate()
    {
        //if (isBeingUsed && MouseManager.Instance.MouseoveredUnit != null && MouseManager.Instance.MouseoveredUnit != previousMousovered && MouseManager.Instance.MouseoveredUnit.GetMyOwner() != myUnit.GetMyOwner())
        //{
        //    previousMousovered = MouseManager.Instance.MouseoveredUnit;
        //    ColourPotentialTargets();
        //}
        //if (isBeingUsed && MouseManager.Instance.MouseoveredUnit == null)
        //{
        //    UncolourPotentialTargets();
        //    previousMousovered = null;
        //}
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
        /* TileBattlescapeGraphics.ColouringTool.UncolourAllTiles();*/
    }

    void ColourPotentialTargets()
    {
        foreach (Tile neighbour in myUnit.currentPosition.neighbours)
        {
            //if (neighbour.myUnit != null && MouseManager.Instance.MouseoveredUnit.currentPosition.neighbours.Contains(neighbour))
            //{
            //    PaintObject(neighbour.myUnit.gameObject, Color.red);
            //    colouredUnits.Add(neighbour.myUnit.gameObject);
            //}
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



    public override bool ActivationRequirements()
    {
        return
            //MouseManager.Instance.mouseoveredTile != null &&
            //MouseManager.Instance.mouseoveredTile.myUnit != null &&
            //myUnit.currentPosition.neighbours.Contains(MouseManager.Instance.mouseoveredTile.myUnit.currentPosition);
            false;
    }

    public override void Activate()
    {
        StartCoroutine(WideSwing());
    }

    IEnumerator WideSwing()
    {
        CreateVFXOn(transform, BasicVFX.transform.rotation);
        //myUnit.LookAtTheTarget(Target.transform.position, 0/*myUnit.GetComponentInChildren<BodyTrigger>().RotationInAttack*/);
        ////myUnit.GetComponent<AnimController>().SpecialAttack();
        Log.SpawnLog("Fencer uses Wide Swing, swinging his blade recklessly around him");
        //LOGIC
        BattlescapeLogic.Unit Enemy = Target.myUnit;
        // Hit for Target
        HitForSwing(Enemy);
        foreach (Tile neighbour in myUnit.currentPosition.neighbours)
        {
            if (neighbour.myUnit != null && Enemy.currentPosition.neighbours.Contains(neighbour))
            {
                HitForSwing(neighbour.myUnit);
            }
        }
        // for mutual neighbours.
        // add debuff
        PassiveAbility_Buff.AddBuff(gameObject, 2, 0, -myUnit.statistics.GetCurrentDefence(), 0, myUnit.statistics.currentMaxNumberOfRetaliations, "FencerDebuff", null, 0, false, true, false);
        myUnit.statistics.numberOfAttacks = 0;
        yield return null;
        FinishUsing();
    }

    void HitForSwing(BattlescapeLogic.Unit target)
    {
        //target.DealDamage(Damage - target.statistics.GetCurrentDefence(), true, false, false);
        PopupTextController.AddParalelPopupText("-" + (Damage - target.statistics.GetCurrentDefence()), PopupTypes.Damage);
    }



    public override void AI_Activate(GameObject Target)
    {
        AlreadyUsedThisTurn = true;
        StartCoroutine(WideSwing());
    }

    public override GameObject AI_ChooseTarget()
    {
        // no idea really, on what criteria this AI should choose correct unit - but maybe You, dear reader, will know any better ;/ For now Random will do.
        return null; // myUnit.EnemyList[Random.Range(0, myUnit.AllyList.Count)].gameObject;
    }

    public override bool AI_IsGoodToUseNow()
    {
        return false; //myUnit.EnemyList.Count - myUnit.AllyList.Count >= 3;
    }





    protected override void SetTarget()
    {
        Target = //MouseManager.Instance.mouseoveredTile;
        null;
    }


}

