using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    protected Button button;

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    protected virtual void OnClick()
    {
        Debug.Log("Base button clicked");
    }
}
