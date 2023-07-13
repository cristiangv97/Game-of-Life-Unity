using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SizeDialog : MonoBehaviour
{
    public Slider sliderFilas;
    public Slider sliderColumnas;

    public TMP_Text strFilas;
    public TMP_Text strColumnas;

    public int intFilas = 100;
    public int intColumnas = 100;

    public HUD hud;

    void Start()
    {
        sliderFilas.onValueChanged.AddListener((v) => {
            intFilas = (int) v;
            strFilas.text = intFilas.ToString();
        });

        sliderColumnas.onValueChanged.AddListener((v) => {
            intColumnas = (int)v;
            strColumnas.text = intColumnas.ToString();
        });
    }

    public void SeleccionarSize()
    {
        EventManager.TriggerEvent("CambiarTamano");

        hud.isActive = false;
        gameObject.SetActive(false);
    }

    public void quitDialog()
    {
        hud.isActive = false;
        gameObject.SetActive(false);
    }

}
