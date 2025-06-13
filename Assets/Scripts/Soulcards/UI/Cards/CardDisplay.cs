using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardRarityAssets rarityAssets;
    public Image backgroundImage;
    public TMP_Text nameText;
    public TMP_Text effectText;
    public GameObject silhouetteOverlay;

    private SoulCards currentCard;

    /// <summary>
    /// Initializes the card display. If ownedCount > 0, it shows the full card details.
    /// If not, it uses the silhouette style and hides the card's actual name and effect.
    /// </summary>
    public void Setup(SoulCards card, int ownedCount, CardRarityAssets rarityAssets)
    {
        this.currentCard = card;
        backgroundImage.sprite = rarityAssets.GetSpriteForRarity(card.rarity);

        if (ownedCount > 0)
        {
            // Fully reveal the card
            nameText.text = card.cardName;
            effectText.text = card.description;
            nameText.color = Color.white;
            effectText.color = Color.white;
            silhouetteOverlay.SetActive(false);
        }
        else
        {
            // Locked version - hide card info
            nameText.text = "???";
            effectText.text = "";
            nameText.color = Color.white;
            effectText.color = Color.white;
            silhouetteOverlay.SetActive(true);

            // Match silhouette appearance to card rarity
            Image silhouetteImage = silhouetteOverlay.GetComponent<Image>();
            if (silhouetteImage != null)
            {
                silhouetteImage.sprite = backgroundImage.sprite;
                silhouetteImage.color = Color.black;
            }
        }
    }

    /// <summary>
    /// Reveals the actual name and effect of the card (used after unlock animation finishes).
    /// </summary>
    public void Reveal()
    {
        if (currentCard != null)
        {
            nameText.text = currentCard.cardName;
            effectText.text = currentCard.description;
            nameText.color = Color.white;
            effectText.color = Color.white;
        }
    }
}
