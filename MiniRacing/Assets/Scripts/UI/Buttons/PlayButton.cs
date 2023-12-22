using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : BaseButton
{
    protected override void OnClick()
    {
        Play();
    }

    private void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
