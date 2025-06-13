using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSwap : MonoBehaviour
{
    public ElementalAffinity element;
    public ElementalArms arm;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            element.SetElementToNecrotic();
            arm.ArmsChange();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            element.SetElementToFire();
            arm.ArmsChange();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            element.SetElementToWater();
            arm.ArmsChange();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            element.SetElementToIce();
            arm.ArmsChange();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            element.SetElementToLightning();
            arm.ArmsChange();
        }
    }
}
