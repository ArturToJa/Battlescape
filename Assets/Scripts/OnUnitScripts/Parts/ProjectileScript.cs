using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public bool isNormalProjectile;
    public Vector3 Target;
    public float speed;
    public float time;
    float angle;
    float currentTime;
    public ShootingScript myShooter;
    [SerializeField] GameObject Explosion;
    [SerializeField] Sound HitSound;
    [SerializeField] bool isHailOfArrows;

    private void Start()
    {
        
        if (isNormalProjectile == false)
        {
            return;
        }
        GetFullTime();
        currentTime = 0;
    }

    private void Update()
    {
        if (transform.position.y <= 0)
        {
            Destroy(this.gameObject, 0.1f);
        }
        if (isNormalProjectile == false)
        {
            return;
        }
        currentTime += Time.deltaTime;
        if (currentTime >= 0.9f * time && myShooter != null)
        {
            myShooter.PlayRandomHitShot();
        }
        CountAngle();
        transform.Rotate(Vector3.left, angle, Space.Self);
        GetComponent<Rigidbody>().velocity = transform.forward * speed;        
    }

    private void CountAngle()
    {
        angle = Mathf.Lerp(0, -90, (1 / time) * Time.deltaTime);
    }

    float CountArcLength(float distance)
    {
        return ( (distance*Mathf.PI)/(2*Mathf.Sqrt(2)));
    }

    float CountTimeToLanding(float distance)
    {
        return distance / speed;
    }
    float CountSimpleDistance()
    {
        return Vector3.Distance(Target, transform.position);
    }

    public void GetFullTime()
    {
        time = CountTimeToLanding(CountArcLength(CountSimpleDistance()));       
    }

    public float GetFullestTime()
    {
        return CountTimeToLanding(CountArcLength(CountSimpleDistance()));
    }
    /*private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("WOOF");
    }*/

    private void OnDestroy()
    {
        if (Explosion != null)
        {
            GameObject explosion = null;
            if (isHailOfArrows)
            {
                explosion = Instantiate(Explosion, transform.position + new Vector3(0, 1, 0), Explosion.transform.rotation);
            }
            else
            {
                explosion = Instantiate(Explosion, Target + new Vector3(0, 0.5f, 0), Explosion.transform.rotation);
            }
            AudioSource explosionSound = explosion.AddComponent<AudioSource>();
            explosionSound.clip = HitSound.clip;
            explosionSound.volume = HitSound.volume;
            explosionSound.Play();
        }
    }   
}
