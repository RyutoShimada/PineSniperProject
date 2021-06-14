using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] string _gameSceneName = null;
    [SerializeField] string _titleSceneName = null;

    private void Update()
    {
        OnMouse();
    }

    private void OnMouse()
    {
        if (!Input.GetButtonDown("Fire1")) return;

        if (SceneManager.GetActiveScene().name == _titleSceneName)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void Retry()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void Title()
    {
        SceneManager.LoadScene(_titleSceneName);
    }
}
