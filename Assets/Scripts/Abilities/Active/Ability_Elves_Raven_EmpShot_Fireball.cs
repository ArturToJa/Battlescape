using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Elves_Raven_EmpShot_Fireball : Ability_Elves_Raven_EmpoweredShot
{
    BattlescapeLogic.Unit previousMousovered;
    List<GameObject> colouredUnits = new List<GameObject>();

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
    public override void Activate()
    {
        base.Activate();
        StartCoroutine(Fireball());
    }

    IEnumerator Fireball()
    {

        Log.SpawnLog("Raven empowers his shot with 'Fireball' spell, hitting " + Target.GetMyObject<Unit>().name + " and all it's neighbours.");
        foreach (Tile neighbour in Target.neighbours)
        {
            if (neighbour.GetMyObject<Unit>() != null)
            {
                //neighbour.GetMyObject<Unit>().DealDamage(Damage + myUnit.statistics.GetCurrentAttack() - neighbour.GetMyObject<Unit>().statistics.GetCurrentDefence(), true, false, true);
                PopupTextController.AddParalelPopupText("-" + (Damage + myUnit.statistics.GetCurrentAttack() - neighbour.GetMyObject<Unit>().statistics.GetCurrentDefence()), PopupTypes.Damage);
            }
        }
        yield return null;
        FinishUsing();
    }

    void ColourPotentialTargets()
    {
        Tile targeto = //MouseManager.Instance.MouseoveredUnit.currentPosition; 
            null;
        PaintObject(targeto.GetMyObject<Unit>().gameObject, Color.red);
        colouredUnits.Add(targeto.GetMyObject<Unit>().gameObject);
        foreach (Tile neighbour in targeto.neighbours)
        {
            if (neighbour.GetMyObject<Unit>() != null)
            {
                PaintObject(neighbour.GetMyObject<Unit>().gameObject, Color.red);
                colouredUnits.Add(neighbour.GetMyObject<Unit>().gameObject);
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
        colouredUnits.Clear();
    }
}
