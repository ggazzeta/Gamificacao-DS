using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreencheTarefas : MonoBehaviour
{
    public Tarefas tarefa;
    public TextMeshProUGUI nomeTarefa;
    public TextMeshProUGUI dataTarefa;
    public TextMeshProUGUI descricaoTarefa;
    public TextMeshProUGUI codigoTarefa;
    public TextMeshProUGUI usuarioTarefa;
    public TAREFASDTO tarefaDTO = new TAREFASDTO();

    // Start is called before the first frame update
    void Start()
    {

    }
    
    public void PreencheTarefaObjeto()
    {
        codigoTarefa.text = tarefa.CODIGO.ToString();
        descricaoTarefa.text = tarefa.DESCRICAOTAREFA;
        usuarioTarefa.text = tarefa.USUARIO.ToString();
        nomeTarefa.text = tarefa.NOMETAREFA;
        dataTarefa.text = tarefa.DATAFINAL.ToString("dd/MM/yyyy");

        #region PreencheDTO
        tarefaDTO.Codigo = tarefa.CODIGO;
        tarefaDTO.DescricaoTarefa = tarefa.DESCRICAOTAREFA;
        tarefaDTO.Finalizada = Funcoes.ConverteIntParaBool(tarefa.FINALIZADA);
        tarefaDTO.UsuarioTarefa = tarefa.USUARIO;
        tarefaDTO.Time = tarefa.TIME;
        tarefaDTO.NomeTarefa = tarefa.NOMETAREFA;
        tarefaDTO.DataTarefa = tarefa.DATAFINAL;
        tarefaDTO.XPTarefa = tarefa.XPTarefa;
        #endregion
    }

}
