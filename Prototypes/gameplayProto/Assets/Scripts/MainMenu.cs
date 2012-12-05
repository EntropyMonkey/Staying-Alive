using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    // Code inspired by http://forum.unity3d.com/threads/36866-How-to-make-a-main-menu-in-Unity

    public int buttonType = 0; // different types, do different things

    public GameObject camera;

    void OnMouseEnter()
    {
        renderer.material.color = Color.green; // color chosen box's text white
        /*
        GUITexture[] texture = gameObject.GetComponentsInChildren<GUITexture>();
        GUITexture texture1 = texture[0];
        GUITexture texture2 = texture[1];
        if (texture1.enabled)
        {
            texture1.enabled = false;
            texture2.enabled = true;
        }
        else
        {
            texture2.enabled = false;
            texture1.enabled = true;
        }
         */
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.white; // color chosen box's text white
    }

    void OnMouseDown()
    {
        switch(buttonType)
        {
            case 1:
                // new game: change camera to show The Escape or The Troll Cave
                Vector3 v = new Vector3();
                v.x = 0.2f;
                v.y = 11;
                camera.transform.Translate(v);
                break;
            case 2:
                // the escape : start playing the first level
                Debug.Log("Loading first level!");
                Application.LoadLevel(1);
                break;
            case 3:
                // the troll cave : start playing the second level
                Debug.Log("Loading second level!");
                Application.LoadLevel(2); // WARNING: This may not set the currentLevel variable in the Player correctly!
                break;
            case 4:
                // go back to main menu
                Debug.Log("Going back to main menu");
                Vector3 v2 = new Vector3();
                v2.x = -0.2f;
                v2.y = -11;
                camera.transform.Translate(v2);
                break;
            case 5:
                // config
                Debug.Log("Config - hasn't been implemented yet");
                break;
            case 6:
                // credits
                Debug.Log("Credits - hasn't been implemented yet");
                break;
            case 7:
                // Quit the game
                Debug.Log("Exiting!");
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
