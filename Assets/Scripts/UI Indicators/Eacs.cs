using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eacs : MonoBehaviour {

    

    
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Tile>())
        {
            other.gameObject.GetComponent<Tile>().isUnderEacs = true;
        }
    }
}
