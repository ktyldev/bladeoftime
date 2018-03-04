using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostFXHandler : MonoBehaviour
{
    private Camera _cam;
    private PostProcessVolume _volume;
    private ColorGrading _grading;

    private bool _fade = false;

    private static PostFXHandler Instance { get; set; }

    void Start()
    {
        _grading = ScriptableObject.CreateInstance<ColorGrading>();
        _grading.enabled.Override(true);
        _grading.saturation.Override(1f);

        _volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, _grading);

        Instance = this;
    }

    void Update()
    {
        if (!_fade)
            return;

        _grading.saturation.value = Mathf.Lerp(
            _grading.saturation.value,
            -70f,
            0.02f);
    }

    void Destroy()
    {
        RuntimeUtilities.DestroyVolume(_volume, true);
    }

    public static void DesaturateFade()
    {
        Instance._fade = true;
    }

    void FadeToBlack()
    {
        StartCoroutine(FadeToBlackCoroutine());
    }

    private IEnumerator FadeToBlackCoroutine()
    {
        while (_grading.brightness.value > 0f)
        {
            _grading.brightness.Override(_grading.brightness.value - .1f);
            yield return new WaitForFixedUpdate();
        }
    }
}