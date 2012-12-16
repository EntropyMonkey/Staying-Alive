using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
    public GameObject HighlightedChild;
    public float ChildExtraScale = 1;

    private MeshRenderer renderer;
   
	// Use this for initialization
	void Start () {
        renderer = transform.GetComponent<MeshRenderer>();
        renderer.material.mainTexture.wrapMode = TextureWrapMode.Clamp;

        if (HighlightedChild != null)
        {
            HighlightedChild.GetComponent<MeshRenderer>().material.mainTexture.wrapMode = TextureWrapMode.Clamp;
            HighlightedChild.transform.localScale *= ChildExtraScale;
            HighlightedChild.active = false;
        }
	}

    void OnMouseEnter()
    {
        if (HighlightedChild != null)
        {
            renderer.enabled = false;
            HighlightedChild.active = true;
        }
    }

    void OnMouseExit()
    {
        if (HighlightedChild != null)
        {
            HighlightedChild.active = false;
            renderer.enabled = true;
        }
    }
}
