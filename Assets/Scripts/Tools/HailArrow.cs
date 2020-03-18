using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HailArrow : MonoBehaviour
{
    public Sound onHitSound { get; set; }
    public Vector3 target { get; set; }
    Rigidbody rb;
    public float maxSpeed { get; set; }
    public float startingSpeed { get; set; }
    public float acceleration { get; set; }


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * startingSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * Time.deltaTime * acceleration);
        }
        if (Vector3.Distance(transform.position, target) < 0.01f || transform.position.y < -0.5f)
        {
            BattlescapeSound.SoundManager.instance.PlaySound(transform.parent.gameObject, onHitSound);
            Destroy(this.gameObject);
        }
    }
}
