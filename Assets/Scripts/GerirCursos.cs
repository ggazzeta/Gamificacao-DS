using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Assets.Scripts.DTOs;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GerirCursos : MonoBehaviour
{
    public GameObject NovaMetaIncluir;
    public GameObject painelDeInclusao;
    public TMP_InputField quantidadeHoras;
    public SQLCon sqlConnection;
    private int qtdHoras;
    public TextMeshProUGUI statusInclusao;
    public TMP_InputField nomeTreinamento;
    public TMP_InputField descricaoTreinamento;
    public TMP_InputField areaTreinamento;
    public TMP_InputField horaTreinamento;
    public TMP_InputField minutoTreinamento;
    public TextMeshProUGUI dataTermino;
    public TextMeshProUGUI inclusaoFalhou;
    public TextMeshProUGUI horasAtuais;
    public TextMeshProUGUI metaAtual;
    public Image barraProgresso;
    public LevelSystem _levelSystem;

    // Start is called before the first frame update
    void Start()
    {
        NovaMetaIncluir.SetActive(false);
        painelDeInclusao.SetActive(false);
        statusInclusao.enabled = false;
    }

    public void IncluirMeta()
    {
        NovaMetaIncluir.SetActive(true);
    }

    public void AtualizarMetaUsuario()
    {
        var result = 0;
        qtdHoras = Int32.TryParse(quantidadeHoras.text, out result) ? result : 0;
        if (ValidarCamposMeta())
        {
            var alterou = sqlConnection.UpdateMetaUsuario(qtdHoras);
            if (alterou)
            {
                FechaTelaInclusao();
            }
            else
            {
                statusInclusao.enabled = true;
            }
        }
    }

    public async void IncluirNovoCurso()
    {
        CURSO cursoInclusao = new CURSO();
        if (ValidarCamposCurso())
        {
            cursoInclusao.Codigo = 0;
            cursoInclusao.NomeCurso = nomeTreinamento.text;
            cursoInclusao.DescricaoCurso = descricaoTreinamento.text;
            cursoInclusao.AreaCurso = areaTreinamento.text;
            int hora = Convert.ToInt32(horaTreinamento.text);
            int minuto = Convert.ToInt32(minutoTreinamento.text);
            cursoInclusao.Minutos = Funcoes.RetornaTempoEmMinutos(hora, minuto);
            cursoInclusao.DataTermino = Convert.ToDateTime(dataTermino.text);

            var insertCurso = sqlConnection.InsertCurso(cursoInclusao);

            if (insertCurso)
            {
                await _levelSystem.GanharXP(1 * hora);
                FechaPainelInclusao();
                UsuarioAtual.quantidadeTarefasFalhas = 0;
                UsuarioAtual.quantidadeTarefasTerminadas = 0;
                UsuarioAtual.quantidadeTarefasTotais = 0;
                UsuarioAtual.usuariosTimeGestor = new List<string>();
                UsuarioAtual.cursosUsuario = new List<CURSO>();
                UsuarioAtual.tarefasAtuais = new List<TAREFASDTO>();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void PreencheCursosUsuario()
    {
        if (UsuarioAtual.usuarioLogado.MetaUsuario > 0)
        {
            int anoAtual = DateTime.Now.Year;
            DateTime primeiroDia = new DateTime(anoAtual, 1, 1);
            DateTime ultimoDia = new DateTime(anoAtual, 12, 31);
            metaAtual.text = UsuarioAtual.usuarioLogado.MetaUsuario.ToString() + " horas";
            var minutosUsuario = UsuarioAtual.cursosUsuario.Where(w => w.DataTermino >= primeiroDia && w.DataTermino <= ultimoDia).Sum(s => s.Minutos);
            var horasTotais = (float)TimeSpan.FromMinutes(minutosUsuario).TotalHours;
            float fillAmount = horasTotais / UsuarioAtual.usuarioLogado.MetaUsuario.GetValueOrDefault(1);
            horasAtuais.text = horasTotais.ToString("N0");
            barraProgresso.fillAmount = fillAmount;
        }
        else
        {
            metaAtual.text = "0 horas";
            horasAtuais.text = "0";
            barraProgresso.fillAmount = 0;
        }
    }

    public void AbrePainelInclusao()
    {
        painelDeInclusao.SetActive(true);
    }

    public void FechaPainelInclusao()
    {
        painelDeInclusao.SetActive(false);
    }

    public void FechaTelaInclusao()
    {
        NovaMetaIncluir.SetActive(false);
    }

    public bool ValidarCamposMeta()
    {
        if (qtdHoras <= 0)
        {
            return false;
        }
        return true;
    }

    public bool ValidarCamposCurso()
    {
        var resultadoHora = 0;
        var resultadoMinuto = 0;
        
        if (string.IsNullOrEmpty(nomeTreinamento.text))
        {
            inclusaoFalhou.text = "Nome do Treinamento é obrigatório";
            return false;
            // nome é obrigatório
        }
        if (string.IsNullOrEmpty(horaTreinamento.text))
        {
            inclusaoFalhou.text = "Favor preencher a carga horária do treinamento";
            return false;
            // hora obrigatorio
        }
        if (string.IsNullOrEmpty(minutoTreinamento.text))
        {
            inclusaoFalhou.text = "Favor preencher a carga horária do treinamento";
            return false;
            // hora obrigatorio
        }
        if (string.IsNullOrEmpty(dataTermino.text))
        {
            inclusaoFalhou.text = "Data de término é obrigatória";
            return false;
            // data obrigatorio
        }
        if (!Int32.TryParse(horaTreinamento.text, out resultadoHora))
        {
            inclusaoFalhou.text = "Erro. Inclusão não realizada.";
            return false;
            // Nao é int
        }
        if (!Int32.TryParse(minutoTreinamento.text, out resultadoMinuto))
        {
            inclusaoFalhou.text = "Erro. Inclusão não realizada.";
            return false;
            // Nao é int
        }
        return true;
    }
}
