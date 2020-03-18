using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class HailOfArrowsScript : MonoBehaviour
{
    Tile myTile;
    Vector3[] targets;
    [SerializeField] float spread;
    [SerializeField] float startingSpeed;
    [SerializeField] float maxSpeed;    
    [SerializeField] float acceleration;
    [SerializeField] Sound[] thunders;

    void Start()
    {
        myTile = GetComponentInParent<Tile>();
        SetTarget();
    }

    void SetTarget()
    {
        List<Tile> targetTiles = myTile.neighbours;
        targetTiles.Add(myTile);
        targets = new Vector3[targetTiles.Count];
        for (int i = 0; i < 27; i++)
        {
            float currentSpreadX = Random.Range(-spread, spread);
            float currentSpreadZ = Random.Range(-spread, spread);
            Vector3 spreadVector = new Vector3(currentSpreadX, 0, currentSpreadZ);
            int targetToFallTo = i % targetTiles.Count;
            targets[targetToFallTo] = targetTiles[targetToFallTo].transform.position + spreadVector;
            Transform child = transform.GetChild(i);
            child.LookAt(targets[targetToFallTo]);
            HailArrow arrow = child.GetComponent<HailArrow>();
            arrow.startingSpeed = startingSpeed;
            arrow.onHitSound = thunders[Random.Range(0, thunders.Length)];
            arrow.target = targetTiles[targetToFallTo].transform.position;
            arrow.maxSpeed = maxSpeed;
            arrow.acceleration = acceleration;
        }
    }
}
