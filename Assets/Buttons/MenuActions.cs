using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActions : MonoBehaviour
{
    public void PlayGame()
    {
        gameObject.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
