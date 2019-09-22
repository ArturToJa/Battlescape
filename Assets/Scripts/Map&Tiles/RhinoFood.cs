using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoFood : MonoBehaviour
{
    [SerializeField] GameObject vfx;
    GameObject child;
    public static List<RhinoFood> foods = new List<RhinoFood>();


    void Start()
    {
        foods.Add(this);
        child = Instantiate(vfx, transform.position, vfx.transform.rotation, transform);
        SetMeActiveTo(false);
    }
      
    void SetMeActiveTo(bool isActive)
    {
        child.SetActive(isActive);
    }

    public static void SetAllActiveTo(bool isActive)
    {
        foreach (RhinoFood r in foods)
        {
            r.SetMeActiveTo(isActive);
        }
    }
    private void OnDestroy()
    {
        foods.Remove(this);
    }
}
