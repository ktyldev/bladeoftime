using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private static GameOver Instance { set; get; }
    bool _gameOver = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        this.Find<Health>(GameTags.Player).Death.AddListener(EndGame);
    }

    private void EndGame()
    {
        _gameOver = true;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public static bool IsEnded()
    {
        return Instance._gameOver;
    }
}
