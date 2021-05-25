using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStart : MonoBehaviour
{
    [SerializeField]
    Button start;
    new AudioSource audio;
    private void Start()
    {
        start.Select();
        audio = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        audio.Play();
        Invoke("StartGame", 1);
    }
    private void StartGame()
    {
        
        SceneManager.LoadScene("Fase 1");
    }
}
