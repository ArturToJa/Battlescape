using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingArrows : MonoBehaviour
{
   // [SerializeField] GameObject Arrow;
    [SerializeField] int ArrowCount;
    Vector3[] targets;
    [SerializeField] float StartingSpeed;
    [SerializeField] float Acceleration;
    [SerializeField] float Spread;
    [SerializeField] float MaxSpeed;
   // [SerializeField] Sound[] Thunders;
    // Use this for initialization
    void Start()
    {
        SetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb.velocity.magnitude < MaxSpeed)
            {
                rb.AddForce(child.forward * Time.deltaTime * Acceleration);
            }
            else
            {
                Debug.LogError("FASTNESS!");
            }
               
        }       
        
    }

    /*public void InitializeArrows(Vector3[] initialPositions)
    {
        for (int i = 0; i < ArrowCount; i++)
        {
            var temp = Instantiate(Arrow, initialPositions[i], Arrow.transform.rotation, this.transform);
        }
    }*/

    public void SetTarget()
    {
        List<Tile> myNieghbours = GetComponentInParent<Tile>().GetNeighbours();
        targets = new Vector3[myNieghbours.Count];
        float currentSpreadX = Random.Range(-Spread, Spread);
        float currentSpreadZ = Random.Range(-Spread, Spread);
        Vector3 spreadVector = new Vector3(currentSpreadX, 0, currentSpreadZ);
        targets[0] = transform.position + spreadVector;
        transform.GetChild(0).LookAt(targets[0]);
        transform.GetChild(0).GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * StartingSpeed);
       // transform.GetChild(0).GetComponent<ProjectileScript>().HitSound = Thunders[Random.Range(0, Thunders.Length)];
        if (GetComponentInParent<Tile>().myUnit != null)
        {
            transform.GetChild(0).GetComponent<ProjectileScript>().Target = GetComponentInParent<Tile>().myUnit.transform.position;
        }
        for (int i = 1; i < ArrowCount; i++)
        {
            currentSpreadX = Random.Range(-Spread, Spread);
            currentSpreadZ = Random.Range(-Spread, Spread);
            spreadVector = new Vector3(currentSpreadX, 0, currentSpreadZ);
            int targetToFallTo = i % myNieghbours.Count;
            targets[targetToFallTo] = myNieghbours[targetToFallTo].transform.position + spreadVector;
            transform.GetChild(i).LookAt(targets[targetToFallTo]);
            transform.GetChild(i).GetComponent<Rigidbody>().AddForce(transform.GetChild(i).forward * StartingSpeed);
           // transform.GetChild(i).GetComponent<ProjectileScript>().HitSound = Thunders[Random.Range(0, Thunders.Length)];                       
            if (myNieghbours[targetToFallTo].myUnit != null)
            {
                transform.GetChild(i).GetComponent<ProjectileScript>().Target = myNieghbours[targetToFallTo].myUnit.transform.position;
            }
        }
    }   
}
