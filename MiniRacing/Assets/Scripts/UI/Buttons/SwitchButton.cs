using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : BaseButton
{
    [SerializeField] private GameObject gameObjectToActivate;
    [SerializeField] private GameObject gameObjectToDeactivate;

    protected override void OnClick()
    {
        gameObjectToActivate.SetActive(true);
        gameObjectToDeactivate.SetActive(false);
    }
}
