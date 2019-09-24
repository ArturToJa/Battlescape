using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsManager
{
    Color GetObjectColor(GameObject target)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if(renderer)
            return renderer.material.color;
        Debug.LogError(target.name + "object does not have renderer");
        return new Color(0.0f,0.0f,0.0f,0.0f);
    }
    void SetObjectColor(GameObject target, Color color)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer)
            renderer.material.color = color;
        Debug.LogError(target.name + "object does not have renderer");
        return;
    }

    float GetObjectAnimationSpeed(GameObject target)
    {
        Animator animator = target.GetComponent<Animator>();
        if (animator)
            return animator.speed;
        Debug.LogError(target.name + "object does not have animator");
        return 1.0f;
    }
    void SetObjectAnimationSpeed(GameObject target, float speed)
    {
        Animator animator = target.GetComponent<Animator>();
        if(animator)
            animator.speed = speed;
        Debug.LogError(target.name + "object does not have animator");
        return;
    }

    void ResetObjectAnimationSpeed(GameObject target)
    {
        SetObjectAnimationSpeed(target, 1.0f);
    }
}
