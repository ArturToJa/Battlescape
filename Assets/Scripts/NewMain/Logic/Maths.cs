using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class Maths
    {
        public static float Sigmoid(int x, float growthRate)
        {
            return 1.0f / (1.0f + Mathf.Exp(-1.0f * x * growthRate));
        }
    }
}