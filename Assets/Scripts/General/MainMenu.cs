using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuObject;
    public bool MenuActive = false;

    void Start()
    {
        MainMenuObject.SetActive(false); // Make the menu inactive at the start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Check if the "E" key is pressed
        {
            MenuActive = !MainMenuObject.activeSelf;
            MainMenuObject.SetActive(MenuActive);

            if (MenuActive)
            {
                Time.timeScale = 0f; // Pause the game
            }
            else
            {
                Time.timeScale = 1f; // Resume the game
            }
        }
    }
}
