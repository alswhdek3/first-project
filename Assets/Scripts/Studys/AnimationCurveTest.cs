using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveTest : MonoBehaviour
{
    public float time = 2f;

    public AnimationCurve animationCurve;

    public GameObject monster;

    float currenttime = 0f;
    float percent = 0f;

    private void Update()
    {
        currenttime += Time.deltaTime;
        transform.position = new Vector2(animationCurve.Evaluate(currenttime), 0f);

        if (currenttime >= time)
            currenttime -= Time.deltaTime;

        if (currenttime <= 0)
            currenttime += Time.deltaTime;

    }
}
