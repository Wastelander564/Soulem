using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string RoundCountKey = "RoundCount";
    private const string SoulsKey = "PlayerSouls";

    public void SaveRoundCount(int roundCount)
    {
        PlayerPrefs.SetInt(RoundCountKey, roundCount);
        PlayerPrefs.Save();
        Debug.Log("RoundCount saved: " + roundCount);
    }

    public int LoadRoundCount(int defaultRound = 1)
    {
        if (PlayerPrefs.HasKey(RoundCountKey))
        {
            int loadedRound = PlayerPrefs.GetInt(RoundCountKey);
            Debug.Log("RoundCount loaded: " + loadedRound);
            return loadedRound;
        }
        else
        {
            Debug.Log("No saved RoundCount found, starting at default: " + defaultRound);
            return defaultRound;
        }
    }

    public void ClearSaveOnPlayerDeath()
    {
        Debug.Log("Player died. Clearing saved round count and souls.");
        PlayerPrefs.DeleteKey(RoundCountKey);
        PlayerPrefs.DeleteKey(SoulsKey); // FIXED LINE
        PlayerPrefs.Save();
    }
}
