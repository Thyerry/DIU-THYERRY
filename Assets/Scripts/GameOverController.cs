using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField]
    GameObject GameOverScreen;

    [SerializeField]
    Button restart;

    [SerializeField]
    AudioSource AudioGameOver;

    [SerializeField]
    AudioSource AudioSelect;
    public void Restart()
    {
        AudioSelect.Play();
        Invoke("TrueRestart", 1f);
    }
    private void TrueRestart()
    {
        SceneManager.LoadScene("Fase 1");
    }

    public void Exit()
    {
        AudioSelect.Play();
        Invoke("TrueExit", 1f);
    }

    private void TrueExit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowGameOverScreen()
    {
        AudioGameOver.Play();
        GameOverScreen.SetActive(true);
        restart.Select();
    }
}
