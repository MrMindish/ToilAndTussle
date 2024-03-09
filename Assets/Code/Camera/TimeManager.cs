using AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    PlayerHurtboxManager hurtboxManager;

    private float fixedDeltaTime;

    void Awake()
    {
        hurtboxManager = GetComponentInChildren<PlayerHurtboxManager>();

        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        if (hurtboxManager.parried)
        {
            Time.timeScale = 0f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        else if (!hurtboxManager.parried)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }
}
