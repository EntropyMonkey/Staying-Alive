using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    // Code inspired by http://forum.unity3d.com/threads/36866-How-to-make-a-main-menu-in-Unity

    public int buttonType = 0; // 1 = play, 2 = quit, 3 = settings

    void OnMouseEnter()
    {
        renderer.material.color = Color.green; // color chosen box's text green
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.white; // color chosen box's text white
    }

    void OnMouseDown()
    {
        if (buttonType == 1)
        {
            // start playing the game
            Debug.Log("Loading level!");
            Application.LoadLevel(0);
        }
        else if (buttonType == 2)
        {
            // Quit the game
            Debug.Log("Exiting!");
            Application.Quit();
        }
    }
}
