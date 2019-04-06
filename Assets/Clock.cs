using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float CurrentTime;
    float startTime;
    public float TimeSpan;
    public bool IsPlaying;
    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = 0;
        TimeSpan = 100;
        IsPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlaying)
        {
            if (CurrentTime < TimeSpan)
            {
                CurrentTime += Time.deltaTime;
            }
        }
    }
}
