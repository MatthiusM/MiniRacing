using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : BaseButton
{
    [SerializeField] private GameObject upgrades;

    protected override void OnClick()
    {
        if (SavedData.instance.UseCoins(100) && AreAllChildrenGreen())
        {
            ChangeNextChildColor();
            SavedData.instance.IncreaseMaxSpeed(5);
        }

    }

    private void ChangeNextChildColor()
    {
        int childCount = upgrades.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = upgrades.transform.GetChild(i);

            Debug.Log(child.name);

            if (child.GetComponent<Image>().color != Color.green)
            {
                child.GetComponent<Image>().color = Color.green;
                return;
            }
        }
    }

    private bool AreAllChildrenGreen()
    {
        int childCount = upgrades.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = upgrades.transform.GetChild(i);
            if (child.GetComponent<Image>().color != Color.green)
            {
                return false;
            }
        }
        return true;
    }
}
