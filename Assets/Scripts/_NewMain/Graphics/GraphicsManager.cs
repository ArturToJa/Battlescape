using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsManager
{

    static GraphicsManager _instance;
    public static GraphicsManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GraphicsManager();
            }
            return _instance;
        }
    }
    public Color GetObjectColor(GameObject target)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer)
        {
            return renderer.material.color;
        }
        Debug.LogError(target.name + "object does not have renderer");
        return new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
    public void SetObjectColor(GameObject target, Color color)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
        Debug.LogError(target.name + "object does not have renderer");
        return;
    }

    public float GetObjectAnimationSpeed(GameObject target)
    {
        Animator animator = target.GetComponent<Animator>();
        if (animator != null)
        {
            return animator.speed;
        }
        Debug.LogError(target.name + "object does not have animator");
        return 1.0f;
    }
    public void SetObjectAnimationSpeed(GameObject target, float speed)
    {
        Animator animator = target.GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = speed;
        }
        else
        {
            Debug.LogError(target.name + " object does not have animator");
        }
        return;
    }

    public void ResetObjectAnimationSpeed(GameObject target)
    {
        SetObjectAnimationSpeed(target, 1.0f);
    }
}
