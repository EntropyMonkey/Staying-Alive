using UnityEngine;
using System.Collections;

public class Signpost : MonoBehaviour {
    private TextMesh textMesh;

	// Use this for initialization
	void Start () {
        textMesh = GetComponentInChildren<TextMesh>();
        if (textMesh == null)
        {
            Debug.LogWarning("Text mesh wasn't found on sign post");
        }
        else
        {
            textMesh.gameObject.active = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == GlobalNames.TAG.Player)
        {
            textMesh.gameObject.active = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == GlobalNames.TAG.Player)
        {
            textMesh.gameObject.active = false;
        }
    }
}
