using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private ElementalAffinity elementalAffinity;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;

    // Element colors
    public Color fireColor = Color.red;
    public Color iceColor = Color.cyan;
    public Color waterColor = Color.blue;
    public Color lightningColor = Color.yellow;
    public Color necroticColor = Color.green;

    void Awake()
    {
        elementalAffinity = GetComponent<ElementalAffinity>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();

        if (elementalAffinity == null)
        {
            Debug.LogError("Missing ElementalAffinity on " + gameObject.name);
            return;
        }

        UpdateColor();
    }

    private void UpdateColor()
    {
        Color selectedColor;

        switch (elementalAffinity.Element)
        {
            case ElementalAffinity.States.Fire:
                selectedColor = fireColor;
                break;
            case ElementalAffinity.States.Ice:
                selectedColor = iceColor;
                break;
            case ElementalAffinity.States.Water:
                selectedColor = waterColor;
                break;
            case ElementalAffinity.States.Lightning:
                selectedColor = lightningColor;
                break;
            case ElementalAffinity.States.Necrotic:
                selectedColor = necroticColor;
                break;
            default:
                selectedColor = Color.white;
                break;
        }

        // Apply to SpriteRenderer
        if (spriteRenderer != null)
        {
            spriteRenderer.color = selectedColor;
        }

        // Apply to TrailRenderer
        if (trailRenderer != null)
        {
            trailRenderer.startColor = selectedColor;
            trailRenderer.endColor = selectedColor;

            // Optional: use gradient fade
            // Gradient gradient = new Gradient();
            // gradient.SetKeys(
            //     new GradientColorKey[] {
            //         new GradientColorKey(selectedColor, 0.0f),
            //         new GradientColorKey(selectedColor, 1.0f)
            //     },
            //     new GradientAlphaKey[] {
            //         new GradientAlphaKey(1.0f, 0.0f),
            //         new GradientAlphaKey(0.0f, 1.0f)
            //     }
            // );
            // trailRenderer.colorGradient = gradient;
        }
    }

    // Optional method to call when element changes during runtime
    public void RefreshColor()
    {
        UpdateColor();
    }
}
