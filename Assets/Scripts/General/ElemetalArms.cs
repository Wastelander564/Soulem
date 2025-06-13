using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalArms : MonoBehaviour
{
    public ElementalAffinity Element;  // Reference to the ElementalAffinity script
    public Sprite NecroticArm;  // Sprite for the Necrotic arm
    public Sprite LightningArm;  // Sprite for the Lightning arm
    public Sprite FireArm;  // Sprite for the Fire arm
    public Sprite IceArm;  // Sprite for the Ice arm
    public Sprite WaterArm;  // Sprite for the Water arm

    private SpriteRenderer spriteRenderer;  // SpriteRenderer to update the arm sprite

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get the SpriteRenderer component
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component not found on this GameObject!");
        }

        ArmsChange();  // Initialize the arm sprite based on the Element
    }

    public void ArmsChange()
    {
        // Check the element and assign the appropriate sprite
        if (Element != null)
        {
            switch (Element.Element)
            {
                case ElementalAffinity.States.Lightning:
                    spriteRenderer.sprite = LightningArm;  // Change to Lightning arm
                    break;

                case ElementalAffinity.States.Necrotic:
                    spriteRenderer.sprite = NecroticArm;  // Change to Necrotic arm
                    break;

                case ElementalAffinity.States.Water:
                    spriteRenderer.sprite = WaterArm;  // Change to Water arm
                    break;

                case ElementalAffinity.States.Fire:
                    spriteRenderer.sprite = FireArm;  // Change to Fire arm
                    break;

                case ElementalAffinity.States.Ice:
                    spriteRenderer.sprite = IceArm;  // Change to Ice arm
                    break;

                default:
                    Debug.LogWarning("Unknown element, defaulting to Necrotic arm.");
                    spriteRenderer.sprite = NecroticArm;  // Default to Necrotic if unknown
                    break;
            }
        }
    }
}
