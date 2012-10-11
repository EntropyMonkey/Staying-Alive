using UnityEngine;
using System.Collections;

public class KalimbaTest : MonoBehaviour {
    private float durationBetweenBangs = 2F;
    private float sinceLastBang = 2F;

	// Use this for initialization
	void Start () {
        KalimbaPd.Init();
        KalimbaPd.OpenFile("sinewave_test.pd","C:\\Users\\Peter\\Documents\\Games\\KalimbaUnity\\PD");        
	}
	
	// Update is called once per frame
	void Update () {
        if (sinceLastBang > durationBetweenBangs)
        {
            sinceLastBang -= durationBetweenBangs;
            KalimbaPd.SendBangToReceiver("sine_on");
        }
        sinceLastBang += Time.deltaTime;
	}
}
