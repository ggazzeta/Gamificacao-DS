using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Assets.Scripts.DTOs;

public class GerirTime : MonoBehaviour
{
    [Header("Assigned Objects")]
    public SQLCon _sqlConnection;

    [Header("Painéis")]
    public GameObject painelInclusao;
    public GameObject painelInclusaoProposta;

    [Header("UI Propostas")]
    public TMP_InputField NomeTarefaInput;
    public TMP_InputField DescricaoTarefaInput;

    [Header("UI Tela")]
    public TextMeshProUGUI nomeTime;
    public TMP_InputField nomeTarefa;
    public TMP_InputField descricaoTarefa;
    public TextMeshProUGUI tarefasFinalizadas;
    public TextMeshProUGUI tarefasFalhas;
    public TextMeshProUGUI tarefasTotais;
    public TextMeshProUGUI porcentagemTarefasFeitas;
    public TextMeshProUGUI porcentagemTarefasFalhas;
    public Image barraPorcentagem;
    public Image barraPorcentagemFalhas;
    public TMP_InputField nomeObjetivo;
    public TMP_InputField descricaoObjetivo;
    public TMP_InputField quantidadeXPTarefa;
    public TextMeshProUGUI textoData;
    public TMP_Dropdown usuarioTarefa;
    public TextMeshProUGUI textoErro;

    private bool _veioDaProposta = false;
    public TAREFASPROPOSTA tarefaPropostaAberta = new TAREFASPROPOSTA();

    // Start is called before the first frame update
    void Start()
    {
        painelInclusao.SetActive(false);
        painelInclusaoProposta.SetActive(false);
        textoErro.text = "";
    }

    public void PreencheAreaTime()
    {
        float qtdeFinalizada = UsuarioAtual.quantidadeTarefasTerminadas;
        float qtdeFalhou = UsuarioAtual.quantidadeTarefasFalhas;
        float qtdeTotal = UsuarioAtual.quantidadeTarefasTotais;
        int porcentagemTarefas = 0;
        int porcentagemTarefasFalhasvar = 0;
        if (qtdeTotal <= 0)
        {
            qtdeFalhou = 0;
            qtdeFinalizada = 0;
            porcentagemTarefas = 0;
            barraPorcentagemFalhas.fillAmount = 0;
            barraPorcentagem.fillAmount = 0;
        }
        else
        {
            porcentagemTarefas = Convert.ToInt32(qtdeFinalizada * 100 / qtdeTotal);
            porcentagemTarefasFalhasvar = Convert.ToInt32(qtdeFalhou * 100 / qtdeTotal);
            barraPorcentagem.fillAmount = qtdeFinalizada / qtdeTotal;
            barraPorcentagemFalhas.fillAmount = qtdeFalhou / qtdeTotal;
        }


        nomeTime.text = Funcoes.ConverteIntParaTime(UsuarioAtual.usuarioLogado.Time);
        tarefasFinalizadas.text = qtdeFinalizada.ToString();
        tarefasFalhas.text = qtdeFalhou.ToString();
        tarefasTotais.text = "Total: " + qtdeTotal.ToString();
        porcentagemTarefasFeitas.text = porcentagemTarefas + "%";
        porcentagemTarefasFalhas.text = porcentagemTarefasFalhasvar + "%";
    }

    public void NovaTarefa()
    {
        if (ValidarCamposProposta())
        {
            TAREFASPROPOSTA tarefasProposta = new TAREFASPROPOSTA()
            {
                NomeTarefa = NomeTarefaInput.text,
                DescricaoTarefa = DescricaoTarefaInput.text,
                Aceita = 0,
                UsuarioTarefa = UsuarioAtual.usuarioLogado.Codigo.ToString(),
                Time = UsuarioAtual.usuarioLogado.Time
            };

            _sqlConnection.InsertTarefaProposta(tarefasProposta);
        }
    }

    public void NovoObjetivo()
    {
        textoErro.text = "";
        if (ValidarCamposTarefa())
        {
            TAREFASDTO tarefaCadastro = new TAREFASDTO()
            {
                NomeTarefa = nomeObjetivo.text,
                DescricaoTarefa = descricaoObjetivo.text,
                Finalizada = false,
                XPTarefa = Convert.ToInt32(quantidadeXPTarefa.text),
                DataTarefa = Convert.ToDateTime(textoData.text)
            };

            if (usuarioTarefa.options[usuarioTarefa.value].text.Contains("Todo o time"))
            {
                tarefaCadastro.Time = UsuarioAtual.usuarioLogado.Time;
            }
            else
            {
                tarefaCadastro.UsuarioTarefa = Funcoes.GetUsuarioTarefa(usuarioTarefa.options[usuarioTarefa.value].text);
            }

            _sqlConnection.InsertTarefa(tarefaCadastro);

            if (_veioDaProposta)
            {
                _sqlConnection.UpdatePropostaAceita(tarefaPropostaAberta);
            }
        }
    }


    private bool ValidarCamposTarefa()
    {
        if (string.IsNullOrEmpty(nomeObjetivo.text))
        {
            textoErro.text = "Nome do Objetivo é obrigatório";
            return false;
        }
        if (string.IsNullOrEmpty(descricaoObjetivo.text))
        {
            textoErro.text = "Descrição do Objetivo é obrigatório";
            return false;
        }
        if (string.IsNullOrEmpty(quantidadeXPTarefa.text))
        {
            if (Convert.ToInt32(quantidadeXPTarefa.text) < 0)
            {
                textoErro.text = "Quantidade de XP deve ser positivo";
                return false;
            }
            textoErro.text = "Quantidade de XP é obrigatório";
            return false;
        }
        if (string.IsNullOrEmpty(textoData.text))
        {
            textoErro.text = "Data é obrigatória";
            return false;
        }

        return true;
    }

    public bool ValidarCamposProposta()
    {
        if (string.IsNullOrEmpty(NomeTarefaInput.text?.Trim()))
        {
            textoErro.text = "Nome do Objetivo é obrigatório";
            return false;
        }
        if (string.IsNullOrEmpty(DescricaoTarefaInput.text?.Trim()))
        {
            textoErro.text = "Descrição do Objetivo é obrigatório";
            return false;
        }

        return true;
    }

    public void IncluirNovaTarefa(bool veioDaProposta = false)
    {
        painelInclusao.SetActive(true);
        _veioDaProposta = veioDaProposta;
    }

    public void IncluirNovaProposta()
    {
        painelInclusaoProposta.SetActive(true);
    }

    public void BotaoSairProposta()
    {
        painelInclusaoProposta.SetActive(false);
    }

    public void BotaoSairTarefa()
    {
        textoErro.text = "";
        painelInclusao.SetActive(false);
    }

}
