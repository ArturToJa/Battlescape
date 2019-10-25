using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class DestructibleScript : MonoBehaviour
{
    [SerializeField] float colorSpeed;
    [SerializeField] float fallingSpeed;
    bool isDying = false;
    [SerializeField] int durability;
    int currDurability;

    private void Start()
    {
        currDurability = durability;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && Application.isEditor && isDying == false)
        {
            GetDestroyed();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Highlight();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            UnHighlight();
        }
    }

    void GetDestroyed()
    {
        StartCoroutine(DestructionRoutine());
        //TODO: it should one day get destroyed in a more cool way.

    }

    public int GetCurrentDurability()
    {
        return currDurability;
    }
    public int GetDurability()
    {
        return durability;
    }


    public void GetDamaged(int dmg)
    {
        currDurability -= dmg;
        if (currDurability <= 0)
        {
            GetDestroyed();
        }
    }

    IEnumerator DestructionRoutine()
    {
        yield return new WaitForSeconds(1f);
        this.transform.parent.GetComponent<Tile>().myObstacle = null;
        isDying = true;
        if (GetComponent<DropScript>() != null)
        {
            GetComponent<DropScript>().Drop();
        }
        while (transform.position.y > -1f)
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                Color color = r.material.color;
                color.a = Mathf.Lerp(color.a, 0, colorSpeed * Time.deltaTime);
                r.material.color = color;
                transform.position += Vector3.down * fallingSpeed * Time.deltaTime;
                yield return null;
            }        
        }
        
        if (Application.isEditor)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Highlight()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = Color.red;
        }
    }

    void UnHighlight()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = Color.white;
        }
    }
}
