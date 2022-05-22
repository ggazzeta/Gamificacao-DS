using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEditor;
using System.Globalization;
using Assets.Scripts.DTOs;

public class TelaPrincipalController : MonoBehaviour
{
    public TextMeshProUGUI AbaAtual;
    public GameObject MensagemAlerta;
    public GameObject AbaObjetivos;
    public GameObject AbaGerirTime;
    public GameObject AbaMonstro;
    public GameObject botaoIncluirTarefa;
    public GameObject botaoFinalizar, botaoFalhar;
    public GameObject PainelObservacoesTarefas;
    public TextMeshProUGUI NomeTarefa;
    public TextMeshProUGUI DescricaoTarefa;
    public TextMeshProUGUI DataTarefa;
    public GameObject loadingScreen;
    public GameObject AreaMeta;
    public GameObject AreaNaoCadastrado;
    public GameObject AbaCursos;
    public GameObject TelaPropostas;
    public GameObject BotaoPropostas;
    public GameObject BotaoPropor;
    public TextMeshProUGUI Header;

    public TarefaPainelObservacao tarefaDoPainel;

    private void Start()
    {
        botaoFalhar.SetActive(false);
        botaoFinalizar.SetActive(false);
        loadingScreen.SetActive(false);
        MensagemAlerta.SetActive(false);
        AbaObjetivos.SetActive(true);
        AbaGerirTime.SetActive(false);
        AbaMonstro.SetActive(false);
        botaoIncluirTarefa.SetActive(false);
        PainelObservacoesTarefas.SetActive(false);
        AreaNaoCadastrado.SetActive(false);
        AreaMeta.SetActive(false);
        AbaCursos.SetActive(false);
        TelaPropostas.SetActive(false);
        bool setActive = UsuarioAtual.usuarioLogado.Funcao != 1 ? true : false;
        BotaoPropor.SetActive(setActive);
    }

    private void Update()
    {

        //if (Input.GetMouseButtonUp(0))
        //{
        //    PointerEventData cursor = new PointerEventData(EventSystem.current);
        //    cursor.position = Input.mousePosition;
        //    List<RaycastResult> objectsHit = new List<RaycastResult>();
        //    EventSystem.current.RaycastAll(cursor, objectsHit);
        //    var listaObjetosTarefa = objectsHit?.Where(w => w.gameObject.tag == "Tarefa");
        //    foreach (var item in listaObjetosTarefa)
        //    {
        //        var teste = item.gameObject.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        //    }
        //}

    }

    public void MudarAbas(int aba)
    {
        AbaAtual.text = Funcoes.MapearAba(aba);
        if (aba == 1)
        {
            AbaObjetivos.SetActive(true);
            AbaGerirTime.SetActive(false);
            AbaMonstro.SetActive(false);
            AbaCursos.SetActive(false);
        }
        else if (aba == 2)
        {
            AbaObjetivos.SetActive(false);
            AbaGerirTime.SetActive(false);
            AbaMonstro.SetActive(false);
            AbaCursos.SetActive(true);

            if(UsuarioAtual.usuarioLogado.MetaUsuario > 0)
            {
                AreaMeta.SetActive(true);
                AreaNaoCadastrado.SetActive(false);
            }
            else
            {
                AreaMeta.SetActive(false);
                AreaNaoCadastrado.SetActive(true);
            }
        }
        else if (aba == 3)
        {
            AbaObjetivos.SetActive(false);
            AbaGerirTime.SetActive(true);
            AbaMonstro.SetActive(false);
            AbaCursos.SetActive(false);
            if (UsuarioAtual.usuarioLogado.Funcao == 1)
            {
                botaoIncluirTarefa.SetActive(true);
            }
        }
        else if (aba == 4)
        {
            AbaObjetivos.SetActive(false);
            AbaGerirTime.SetActive(false);
            AbaMonstro.SetActive(true);
            AbaCursos.SetActive(false);
        }
    }

    public void AbreMensagemAlerta()
    {
        MensagemAlerta.SetActive(true);
    }

    public void FechaMensagemAlerta()
    {
        MensagemAlerta.SetActive(false);
    }

    public void VoltaTelaMenu()
    {
        UsuarioAtual.tarefasAtuais = new List<TAREFASDTO>();
        UsuarioAtual.usuariosTimeGestor = new List<string>();
        UsuarioAtual.tarefasPropostaTime = new List<TAREFASPROPOSTA>();
        UsuarioAtual.cursosUsuario = new List<CURSO>();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ReloadScene()
    {
        UsuarioAtual.tarefasAtuais = new List<TAREFASDTO>();
        UsuarioAtual.usuariosTimeGestor = new List<string>();
        UsuarioAtual.tarefasPropostaTime = new List<TAREFASPROPOSTA>();
        UsuarioAtual.cursosUsuario = new List<CURSO>();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChecaTarefasPropostas()
    {
        BotaoPropostas.SetActive(false);

        if (UsuarioAtual.usuarioLogado.Funcao == 1 && UsuarioAtual.tarefasPropostaTime.Count() > 0)
        {
            BotaoPropostas.SetActive(true);
        }
    }

    public void AbreTelaTarefas(TAREFASDTO tarefa)
    {
        NomeTarefa.text = tarefa.NomeTarefa;
        DescricaoTarefa.text = tarefa.DescricaoTarefa;
        CultureInfo idioma = new CultureInfo("pt-BR");
        DataTarefa.text = tarefa.DataTarefa.ToString("dddd, dd MMMM yyyy", idioma);
        tarefaDoPainel.Codigo = tarefa.Codigo;
        tarefaDoPainel.DataTarefa = tarefa.DataTarefa;
        tarefaDoPainel.DescricaoTarefa = tarefa.DescricaoTarefa;
        tarefaDoPainel.NomeTarefa = tarefa.NomeTarefa;
        tarefaDoPainel.UsuarioTarefa = tarefa.UsuarioTarefa;
        tarefaDoPainel.TimeTarefa = tarefa.Time;
        tarefaDoPainel.XPTarefa = tarefa.XPTarefa;
        tarefaDoPainel.TarefaFinalizada = Funcoes.ConverteBoolParaInt(tarefa.Finalizada);
        if (UsuarioAtual.usuarioLogado.Funcao == 3 && (tarefaDoPainel.UsuarioTarefa != null && tarefaDoPainel.UsuarioTarefa > 0 && tarefaDoPainel.UsuarioTarefa == UsuarioAtual.usuarioLogado.Codigo))
        {
            botaoFalhar.SetActive(true);
            botaoFinalizar.SetActive(true);
        }

        else if (UsuarioAtual.usuarioLogado.Funcao == 1)
        {
            botaoFalhar.SetActive(true);
            botaoFinalizar.SetActive(true);
        }
        else
        {
            botaoFalhar.SetActive(false);
            botaoFinalizar.SetActive(false);
        }

        PainelObservacoesTarefas.SetActive(true);

    }

    public void FechaTelaTarefasPropostas()
    {
        TelaPropostas.SetActive(false);
    }

    public void AbreTelaTarefasPropostas()
    {
        TelaPropostas.SetActive(true);
        Header = GameObject.FindGameObjectWithTag("HeaderScroller").GetComponent<TextMeshProUGUI>();
        Header.text = "Objetivos Propostos";
    }

    public void FechaTelaTarefas()
    {
        PainelObservacoesTarefas.SetActive(false);
    }

    public void AbreLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }
    public void FechaLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }
}
