using Assets.Scripts.DTOs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlaTelaScroll : MonoBehaviour
{

    public TextMeshProUGUI NomeTarefa;
    public TextMeshProUGUI Usuario;
    public TAREFASPROPOSTA tarefaProposta;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PreencherTarefasProposta(TAREFASPROPOSTA tarefaPropostaAtual)
    {
        this.tarefaProposta = tarefaPropostaAtual;
        NomeTarefa.text = tarefaProposta.NomeTarefa;
        Usuario.text = tarefaProposta.UsuarioTarefa;
    }

    public void AbrirTelaInclusao()
    {
        GameObject objetoGerirTime = GameObject.Find("AbaGerirTime");
        GerirTime gerirTime = objetoGerirTime.GetComponent<GerirTime>();
        gerirTime.IncluirNovaTarefa(true);
        gerirTime.tarefaPropostaAberta = tarefaProposta;
        gerirTime.nomeTarefa.text = this.tarefaProposta.NomeTarefa;
        gerirTime.descricaoTarefa.text = this.tarefaProposta.DescricaoTarefa;
    }
}
