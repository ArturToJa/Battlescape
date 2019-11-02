using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Elves_Raven_EmpShot_Fireball : Ability_Elves_Raven_EmpoweredShot
{
    UnitScript previousMousovered;
    List<GameObject> colouredUnits = new List<GameObject>();

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
    public override void Activate()
    {
        base.Activate();
        StartCoroutine(Fireball());
    }

    IEnumerator Fireball()
    {

        Log.SpawnLog("Raven empowers his shot with 'Fireball' spell, hitting " + Target.myUnit.name + " and all it's neighbours.");
        foreach (Tile neighbour in Target.neighbours)
        {
            if (neighbour.myUnit != null)
            {
                neighbour.myUnit.DealDamage(Damage + myUnit.statistics.GetCurrentAttack() - neighbour.myUnit.statistics.GetCurrentDefence(), true, false, true);
                PopupTextController.AddParalelPopupText("-" + (Damage + myUnit.statistics.GetCurrentAttack() - neighbour.myUnit.statistics.GetCurrentDefence()), PopupTypes.Damage);
            }
        }
        yield return null;
        FinishUsing();
    }

    void ColourPotentialTargets()
    {
        Tile targeto = MouseManager.Instance.MouseoveredUnit.myTile;
        PaintObject(targeto.myUnit.gameObject, Color.red);
        colouredUnits.Add(targeto.myUnit.gameObject);
        foreach (Tile neighbour in targeto.neighbours)
        {
            if (neighbour.myUnit != null)
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
        colouredUnits.Clear();
    }
}
