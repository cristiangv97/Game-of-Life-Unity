using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorDialog : MonoBehaviour
{
    public Slider sliderRojo;
    public Slider sliderVerde;
    public Slider sliderAzul;

    public Image colorChange;

    public HUD hud;

    void Start()
    {
        sliderRojo.onValueChanged.AddListener((v) => {
            Color tempColor = colorChange.color;
            colorChange.color = new Color(v, tempColor.g, tempColor.b);
        });

        sliderVerde.onValueChanged.AddListener((v) => {
            Color tempColor = colorChange.color;
            colorChange.color = new Color(tempColor.r, v, tempColor.b);
        });

        sliderAzul.onValueChanged.AddListener((v) => {
            Color tempColor = colorChange.color;
            colorChange.color = new Color(tempColor.r, tempColor.g, v);
        });
    }

    public void CambiarColor()
    {
        EventManager.TriggerEvent("CambiarColor");

        hud.isActive = false;
        gameObject.SetActive(false);
    }

    public void quitDialog()
    {
        hud.isActive = false;
        gameObject.SetActive(false);
    }

}
