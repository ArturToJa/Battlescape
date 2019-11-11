using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class UnitColours : MonoBehaviour
{

    [SerializeField] List<Material> playerMaterials;
    Unit myUnit;

    void Start()
    {
        myUnit = GetComponentInParent<Unit>();
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject.tag == "Coloured")
            {
                renderer.material = playerMaterials[(int)myUnit.owner.colour];
            }

        }
    }
}
