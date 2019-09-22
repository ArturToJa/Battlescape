using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllPlacedUnits : MonoBehaviour
{

    public void DestroyAllThings()
    {
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("UnitSlot"))
        {
            if (Application.isEditor)
            {
                DestroyImmediate(thing);

            }
            else
            {
                Destroy(thing);
            }

        }
    }
}
