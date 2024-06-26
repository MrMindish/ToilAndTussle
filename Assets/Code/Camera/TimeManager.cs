using AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    PlayerAttackManager attackManager;

    private float fixedDeltaTime;

    public bool timeSlowedParry;
    public bool timeSlowedHit;
    public float timeSlowedHitTime;

    void Awake()
    {
        attackManager = GetComponentInChildren<PlayerAttackManager>();

        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        if (timeSlowedParry)
        {
            Time.timeScale = 0.01f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        else if (!timeSlowedParry)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }
}
