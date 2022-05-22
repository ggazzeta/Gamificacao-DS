using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class SelectableText
    : MonoBehaviour,
    IEventSystemHandler,
    ISelectHandler,
    IDeselectHandler,
    IPointerDownHandler
{
    public TelaPrincipalController controller;

    private void Start()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Selection tracking
        EventSystem.current.SetSelectedGameObject(gameObject, eventData);
    }


    public void OnSelect(BaseEventData eventData)
    {
        TAREFASDTO tarefaSelecionada = new TAREFASDTO();
        //base.OnSelect(eventData);
        UnityEngine.Debug.Log("Selected");
        var Textos = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        var isSelecionada = gameObject.GetComponentInChildren<Toggle>()?.isOn;
        //var teste = gameObject.GetComponent<PreencheTarefas>().tarefa;

        tarefaSelecionada.Finalizada = isSelecionada.GetValueOrDefault(false);

        if (Textos != null)
        {
            foreach (var texto in Textos)
            {
                if (texto.tag == "Tarefa" && texto.name.Contains("TextoDescricao"))
                {
                    tarefaSelecionada.DescricaoTarefa = texto.text;
                }
                if (texto.tag == "Tarefa" && texto.name.Contains("TextoData"))
                {
                    tarefaSelecionada.DataTarefa = Convert.ToDateTime(texto.text);
                }
            }
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //base.OnDeselect(eventData);
        UnityEngine.Debug.Log("De-Selected");
    }

    public void SelecionouTarefa()
    {
        TAREFASDTO tarefaSelecionada = new TAREFASDTO();
        //base.OnSelect(eventData);
        UnityEngine.Debug.Log("Selected");
        //var teste = this.gameObject.GetComponent<PreencheTarefas>().tarefa;
        var Textos = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        var isSelecionada = gameObject.GetComponentInChildren<Toggle>()?.isOn;

        tarefaSelecionada.Finalizada = isSelecionada.GetValueOrDefault(false);

        if (tarefaSelecionada.Finalizada)
        {
            UsuarioAtual.tarefasAtuais.Add(tarefaSelecionada);
        }

        if (!tarefaSelecionada.Finalizada)
        {
            UsuarioAtual.tarefasAtuais = UsuarioAtual.tarefasAtuais.Where(w => w.DescricaoTarefa != tarefaSelecionada.DescricaoTarefa).ToList();
        }
    }

    public void ClicouTarefa()
    {
        var tarefaInstancia = gameObject.GetComponent<PreencheTarefas>().tarefaDTO;
        var teste = GameObject.FindWithTag("TelaTarefas");
        controller = GameObject.FindGameObjectWithTag("Canvas").GetComponent<TelaPrincipalController>();
        controller.AbreTelaTarefas(tarefaInstancia);
    }
}