using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BattlescapeUI
{
    public class DestructibleObstacleHealthbar : AbstractHealthbar
    {
        BattlescapeLogic.DestructibleObstacle myObstacle;


        protected override void OnStart()
        {
            healthBarFill = GetComponentsInChildren<Image>()[1];
            myObstacle = GetComponentInParent<BattlescapeLogic.DestructibleObstacle>();
            myObject = myObstacle.transform;
            TurnOff();
        }

        protected override float GetPercent()
        {
            return (float)myObstacle.currentHealthPoints / (float)myObstacle.maxHealthPoints;
        }
    }
}