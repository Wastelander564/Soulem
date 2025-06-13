using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Image backgroundImage;

    // This method assigns the card data to the UI elements
    public void SetCard(SoulCards card)
    {
        if (card == null)
        {
            ClearSlot();
            return;
        }

        cardNameText.text = card.cardName;
        backgroundImage.color = GetColorForRarity(card.rarity);
    }

    // Clears the slot UI to empty
    public void ClearSlot()
    {
        cardNameText.text = "";
        backgroundImage.color = Color.clear;
    }

    // Returns a color based on rarity for visual feedback
    private Color GetColorForRarity(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => new Color(187f/255f, 187f/255f, 187f/255f),       // light gray
            Rarity.Rare => new Color(125f/255f, 191f/255f, 0f),               // greenish
            Rarity.VeryRare => new Color(65f/255f, 65f/255f, 255f/255f),      // blue
            Rarity.Epic => new Color(146f/255f, 65f/255f, 243f/255f),         // purple
            Rarity.Mythical => new Color(235f/255f, 211f/255f, 32f/255f),     // yellow
            _ => Color.white,
        };
    }


}
