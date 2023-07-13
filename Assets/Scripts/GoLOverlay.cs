using UnityEngine;
using TMPro;

public class GoLOverlay : MonoBehaviour
{
    public TMP_Text phCel;
    public TMP_Text phIte;

    public HUD hud;

    public void quitDialog()
    {
        hud.isActive = false;
        gameObject.SetActive(false);
    }

    public void updateAll(int numeroCelulas, int numeroIteraciones)
    {
        updateCelulas(numeroCelulas);
        updateIteraciones(numeroIteraciones);
    }

    public void updateCelulas(int numeroCeluals)
    {
        phCel.text = numeroCeluals.ToString();
    }

    public void updateIteraciones(int numeroIteraciones)
    {
        phIte.text = numeroIteraciones.ToString();
    }
}
