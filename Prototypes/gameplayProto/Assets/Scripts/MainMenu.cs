using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    // Code inspired by http://forum.unity3d.com/threads/36866-How-to-make-a-main-menu-in-Unity

    public enum MenuButtons {NEW_GAME, LEVEL_ONE, LEVEL_TWO, MAIN_MENU,CONFIG, CREDITS,EXIT,MOVEMENT};
    public MenuButtons ButtonType;
    public Vector3 Movement;

    public GameObject camera;

    void OnMouseDown()
    {
        switch(ButtonType)
        {
            case MenuButtons.NEW_GAME:
                // new game: change camera to show The Escape or The Troll Cave
                Vector3 v = new Vector3();
                v.y = 13;
                camera.transform.Translate(v);
                break;
            case MenuButtons.LEVEL_ONE:
                // the escape : start playing the first level
                Debug.Log("Loading first level!");
                Application.LoadLevel(1);
                break;
            case MenuButtons.LEVEL_TWO:
                // the troll cave : start playing the second level
                Debug.Log("Loading second level!");
                Application.LoadLevel(2); 
                break;
            case MenuButtons.MAIN_MENU:
                // go back to main menu
                Debug.Log("Going back to main menu");
                Vector3 v2 = new Vector3();
                v2.y = -13;
                camera.transform.Translate(v2);
                break;
            case MenuButtons.CONFIG:
                // config
                Debug.Log("Config - hasn't been implemented yet");
                break;
            case MenuButtons.CREDITS:
                // credits
                Debug.Log("Credits - hasn't been implemented yet");
                break;
            case MenuButtons.EXIT:
                // Quit the game
                Debug.Log("Exiting!");
                Application.Quit();
                break;
            case MenuButtons.MOVEMENT:
                // Quit the game
                Debug.Log("Moving!");
                camera.transform.position = new Vector3(camera.transform.position.x,camera.transform.position.y+Movement.y,camera.transform.position.z);
                break;
            default:
                break;
        }
    }
}
