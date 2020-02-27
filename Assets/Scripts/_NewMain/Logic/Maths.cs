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
            gameObject.transform.rotation *= Quaternion.AngleAxis(angle - gameObject.transform.rotation.eulerAngles.y, new Vector3(0, 1, 0));
        }

        public static void SetObjectPitch(GameObject gameObject, float angle)
        {
            Debug.Log("Inside before: " + gameObject.transform.rotation.eulerAngles);
            gameObject.transform.rotation *= Quaternion.AngleAxis(angle - gameObject.transform.rotation.eulerAngles.z, new Vector3(0, 0, 1));
            Debug.Log("Inside after: " + gameObject.transform.rotation.eulerAngles);
        }

        public static void SetObjectRoll(GameObject gameObject, float angle)
        {
            gameObject.transform.rotation *= Quaternion.AngleAxis(angle - gameObject.transform.rotation.eulerAngles.x, new Vector3(1, 0, 0));
        }
    }
}