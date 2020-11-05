using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class FakeUnit : MonoBehaviour
{
    [SerializeField] string trigger;
    [SerializeField] float timeTillStart;
    float timeSinceCreated = 0;
    bool alreadyDone = false;
    [SerializeField] float timeToDestroy;
    Unit unit;

    void Start()
    {
        timeTillStart = Random.value * timeTillStart;
        unit = GetComponentInParent<Unit>();
    }

    void Update()
    {
        timeSinceCreated += Time.deltaTime;
        if (timeSinceCreated >= timeTillStart && !alreadyDone)
        {
            transform.rotation = unit.visuals.transform.rotation;
            GetComponent<Animator>().SetTrigger(trigger);
            alreadyDone = true;
        }
        if (timeSinceCreated >= timeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
