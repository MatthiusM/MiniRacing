using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTwoButton : BaseButton
{
    protected override void OnClick()
    {
        SceneManager.LoadScene("SampleScene1");
    }
}
