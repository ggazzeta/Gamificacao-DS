using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncoesCanvas : MonoBehaviour
{

    public GameObject Canvas;
    public GameObject botaoCadastro;
    public GameObject loadingScreen;
    public GameObject informacoesScreen;

    // Start is called before the first frame update
    void Start()
    {
        loadingScreen.SetActive(false);
        informacoesScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AbreTelaCadastro()
    {
        Canvas.SetActive(true);
        botaoCadastro.SetActive(false);
    }

    public void FechaTelaCadastro()
    {
        Canvas.SetActive(false);
        botaoCadastro.SetActive(true);
    }

    public void AbreLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }
    public void FechaLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    public void AbreInfoJogo()
    {
        if (informacoesScreen.activeInHierarchy)
        {
            informacoesScreen.SetActive(false);
        }
        else
        {
            informacoesScreen.SetActive(true);
        }
    }

    public void FechaJogo()
    {
        Application.Quit();
    }
}
