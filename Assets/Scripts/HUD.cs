using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public SaveDialog saveDialog;
    public LoadDialog loadDialog;
    public ColorDialog colorDialog;
    public SalirDialog salirDialog;
    public GoLOverlay goLOverlay;
    public SizeDialog sizeDialog;

    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        saveDialog.gameObject.SetActive(false);
        loadDialog.gameObject.SetActive(false);
        colorDialog.gameObject.SetActive(false);
        salirDialog.gameObject.SetActive(false);
        sizeDialog.gameObject.SetActive(true);
        showOverlayDialog();
        // goLOverlay.gameObject.SetActive(false);
    }

    public void showSaveDialog()
    {
        saveDialog.gameObject.SetActive(true);
        isActive = true;
    }

    public void showLoadDialog()
    {
        loadDialog.gameObject.SetActive(true);
        isActive  = true;
    }

    public void showColorDialog()
    {
        colorDialog.gameObject.SetActive(true);
        isActive = true;
    }

    public void showSalirDialog()
    {
        salirDialog.gameObject.SetActive(true);
        isActive = true;
    }

    public void showOverlayDialog()
    {
        goLOverlay.gameObject.SetActive(true);
        // isActive = true;
    }

    public void showSizeDialog()
    {
        sizeDialog.gameObject.SetActive(true);
        isActive = true;
    }

    public void actualizarTodo(int numeroCelulas, int numeroIteraciones)
    {
        goLOverlay.updateAll(numeroCelulas, numeroIteraciones);
    }

    public void actualizarCelulas(int numeroCelulas)
    {
        goLOverlay.updateCelulas(numeroCelulas);
    }

    public void actualizarIteraciones(int numeroIteraciones)
    {
        goLOverlay.updateIteraciones(numeroIteraciones);
    }
}
