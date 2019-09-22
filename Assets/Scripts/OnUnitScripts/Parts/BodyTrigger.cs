using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrigger : MonoBehaviour
{
    public float RotationInAttack;
    public void OnTriggerEnter(Collider other)
    {       
        if (other.tag == "Projectile" && other.GetComponentInParent<ProjectileScript>().Target == this.transform.position)
        {
            GetComponentInParent<AnimController>().AnimateWound();
            Destroy(other.gameObject, 0.01f);
        }
    }
}
