using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using TMPro;

public class SaveDialog : MonoBehaviour
{

    public TMP_InputField patternName;
    public HUD hud;

    public void savePattern()
    {
        EventManager.TriggerEvent("SavePattern");

        hud.isActive = false;
        gameObject.SetActive(false);
    }

    public void quitDialog()
    {
        hud.isActive = false;
        gameObject.SetActive(false);
    }
}
