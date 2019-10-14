using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoGui : MonoBehaviour
{
    [SerializeField] Text infoText;

    public void UpdateInfo(IHaveInfo info)
    {
        if (info == null)
            infoText.text = "";
        else
            infoText.text = info.GetInfoText();
    }
}
