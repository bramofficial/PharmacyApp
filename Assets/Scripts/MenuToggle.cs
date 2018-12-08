using VRTK;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToggle : MonoBehaviour {

    public VRTK_ControllerEvents controllerEvents;
    public GameObject menu;

    bool menuState = false;
    
    private void OnEnable()
    {
	Debug.Log("Enabled");
        controllerEvents.TouchpadReleased += ControllerEvents_TouchpadReleased;
        controllerEvents.TouchpadPressed += ControllerEvents_TouchpadPressed;
    }

    private void OnDisable()
    {
	Debug.Log("Disabled");
        controllerEvents.TouchpadReleased -= ControllerEvents_TouchpadReleased;
        controllerEvents.TouchpadPressed -= ControllerEvents_TouchpadPressed;
    }

    private void ControllerEvents_TouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
	    Debug.Log("Called controllerEvents_Touchpad");
        menuState = !menuState;
        menu.SetActive(menuState);
    }

    private void ControllerEvents_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("HJDFHDFJHDFFDJDFJKFJ");
    }


    // Load the scene with the desired game mode
    // First we will load a loading screen scene
    public void loadingScreen(int gameMode)
    {
        //This saves the game mode, so the Pharmacy scene can load the right game
        PlayerPrefs.SetInt("gameMode", gameMode);
        SceneManager.LoadScene(1);
    }
}
