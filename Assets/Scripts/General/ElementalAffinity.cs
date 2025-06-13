using UnityEngine;

public class ElementalAffinity : MonoBehaviour
{
    public enum States
    {
        Fire,
        Ice,
        Water,
        Lightning,
        Necrotic
    }

    [SerializeField] private States element;

    public States Element
    {
        get => element;
        set => element = value;
    }

    public void SetElementToFire()
    {
        element = States.Fire;
    }

    public void SetElementToIce()
    {
        element = States.Ice;
    }

    public void SetElementToWater()
    {
        element = States.Water;
    }

    public void SetElementToLightning()
    {
        element = States.Lightning;
    }

    public void SetElementToNecrotic()
    {
        element = States.Necrotic;
    }
}
