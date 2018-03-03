using Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        this.Find<Health>(GameTags.Player).Death.AddListener(EndGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
