using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTheme : MonoBehaviour
{
    AudioSource mainTheme;

    private void Start()
    {
        mainTheme = GetComponent<AudioSource>();
    }
    public void StopPlaying()
    {
        mainTheme.Stop();
    }
}
