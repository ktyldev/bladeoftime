﻿using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private IControlMode _input;
    private static GameOver Instance { set; get; }
    bool _gameOver = false;
    bool _canReload = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        this.Find<Health>(GameTags.Player).Death.AddListener(EndGame);
        _input = this.Find<PlayerController>(GameTags.Player).Input;
    }

    private void Update()
    {
        if (_canReload && _input.AnyButtonPressed)
            StartCoroutine(ReloadGameCoroutine());
    }

    private void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
    }
    
    public static bool IsEnded()
    {
        return Instance._gameOver;
    }

    private IEnumerator EndGameCoroutine()
    {
        _gameOver = true;
        MusicManager.SetFilter(true);
        PostFXHandler.DesaturateFade();
        yield return new WaitForSecondsRealtime(1.5f);
        _canReload = true;
    }

    private IEnumerator ReloadGameCoroutine()
    {
        MusicManager.FadeOut();
        yield return new WaitForSecondsRealtime(.7f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
