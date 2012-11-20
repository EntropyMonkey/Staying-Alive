using UnityEngine;
using System.Collections;

public class Timer {
    public bool IsActive
    {
        get;
        private set;
    }

    public bool IsTimeUp
    {
        get;
        private set;
    }

    private float elapsedTime;
    private float shouldRunFor;

    public void Start(float time)
    {
        IsActive = true;
        IsTimeUp = false;
        shouldRunFor = time;
        elapsedTime = 0;
    }

    public void Stop()
    {
        IsActive = false;
        IsTimeUp = false;
    }

	// Update is called once per frame
	public void Update (float dtime) {
        if (IsActive)
        {
            elapsedTime += dtime;
            if (elapsedTime > shouldRunFor)
            {
                IsActive = false;
                IsTimeUp = true;
            }
        }
	}
}
