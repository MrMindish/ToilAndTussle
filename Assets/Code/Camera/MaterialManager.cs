using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public Material lightsMaterial;
    public Material particleMaterial;

    public bool p1Light;
    public bool p2Light;

    private void Start()
    {
        p1Light = false;
        p2Light = false;

    }

    private void Update()
    {
        if (p1Light)
        {
            Color customColor = new Color(1f, 0.7f, 0f); // Define your custom color here (R, G, B)
            lightsMaterial.SetColor("_Color", customColor);
            particleMaterial.SetColor("_Color", customColor);
        }
        else if (p2Light)
        {
            Color customColor = new Color(1f, 0f, 0f); // Define your custom color here (R, G, B)
            lightsMaterial.SetColor("_Color", customColor);
            particleMaterial.SetColor("_Color", customColor);
        }
        else
        {
            Color customColor = new Color(0.9f, 0.9f, 0.95f); // Define your custom color here (R, G, B)
            lightsMaterial.SetColor("_Color", customColor);
            particleMaterial.SetColor("_Color", customColor);
        }
    }
}
