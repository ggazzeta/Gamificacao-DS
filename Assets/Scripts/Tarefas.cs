using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nova Tarefa", menuName = "Tarefa", order = 1)]
public class Tarefas : ScriptableObject
{
    public int CODIGO;

    public string NOMETAREFA;

    public string DESCRICAOTAREFA;

    public DateTime DATAFINAL;

    public int FINALIZADA;

    public int TIME;

    public int USUARIO;

    public int XPTarefa;
}
