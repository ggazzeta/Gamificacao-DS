using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapearUsuario : MonoBehaviour
{
    //MAIN CAMERA
    public USUARIO usuarioLogado;
    public int abaAtual;
    public TextMeshProUGUI nomeUsuario;
    public TextMeshProUGUI nomeTimeFuncao;
    public TextMeshProUGUI AbaAtual;
    public Sprite[] spriteUsuario;
    public Sprite[] spritesFuncao;
    public Image imagemUsuario;
    public Image imagemArcoFuncao;

    // Start is called before the first frame update
    void Start()
    {
        usuarioLogado = UsuarioAtual.usuarioLogado;
        nomeUsuario.text = usuarioLogado.Nome;
        nomeTimeFuncao.text = Funcoes.ConverteIntParaTime(usuarioLogado.Time) + " (" + Funcoes.ConverteIntParaFuncao(usuarioLogado.Funcao) + ")";
        AbaAtual.text = Funcoes.MapearAba(abaAtual);
        MudarImagens();
    }

    private void MudarImagens()
    {
        if (usuarioLogado.Time == 1)
        {
            imagemUsuario.sprite = spriteUsuario[0];
        }
        else if (usuarioLogado.Time == 2)
        {
            imagemUsuario.sprite = spriteUsuario[1];
        }

        if (usuarioLogado.Funcao == 1) // GESTOR
        {
            imagemArcoFuncao.sprite = spritesFuncao[0];
        }
        else if (usuarioLogado.Funcao == 2) // CRIADOR
        {
            imagemArcoFuncao.sprite = spritesFuncao[1];
        }
        else if (usuarioLogado.Funcao == 3) // FUNCIONARIO
        {
            imagemArcoFuncao.sprite = spritesFuncao[2];
        }

    }

}
