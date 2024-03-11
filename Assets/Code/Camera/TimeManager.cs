using AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    PlayerAttackManager attackManager;

    private float fixedDeltaTime;

    public bool timeSlowed;

    void Awake()
    {
        attackManager = GetComponentInChildren<PlayerAttackManager>();

        // Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        if (timeSlowed)
        {
            Time.timeScale = 0.01f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        else if (!timeSlowed)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Time.timeScale = 0.01f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }
}
