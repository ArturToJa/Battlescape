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


        // these methods need testing
        public static void SetObjectLocalYaw(GameObject gameObject, float angle)
        {
            gameObject.transform.localRotation *= Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
        }

        public static void SetObjectLocalPitch(GameObject gameObject, float angle)
        {
            gameObject.transform.localRotation *= Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        }

        public static void SetObjectLocalRoll(GameObject gameObject, float angle)
        {
            gameObject.transform.localRotation *= Quaternion.AngleAxis(angle, new Vector3(1, 0, 0));
        }
    }
}