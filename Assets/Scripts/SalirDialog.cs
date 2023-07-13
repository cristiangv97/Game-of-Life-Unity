using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalirDialog : MonoBehaviour
{
    public HUD hud;

    public void salirAplicacion()
    {
        EventManager.TriggerEvent("SalirDialog");

        hud.isActive = false;
        gameObject.SetActive(false);
    }

    public void quitDialog()
    {
        hud.isActive = false;
        gameObject.SetActive(false);
    }
}
