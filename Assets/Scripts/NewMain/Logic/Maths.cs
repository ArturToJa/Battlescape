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
        public static void SetObjectYaw(GameObject gameObject, float angle)
        {
            // gameObject.transform.rotation *= Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, angle - gameObject.transform.eulerAngles.z);
            gameObject.transform.rotation = rotation;
        }

        public static void SetObjectPitch(GameObject gameObject, float angle)
        {
            gameObject.transform.rotation *= Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        }

        public static void SetObjectRoll(GameObject gameObject, float angle)
        {
            gameObject.transform.rotation *= Quaternion.AngleAxis(angle, new Vector3(1, 0, 0));
        }
    }
}