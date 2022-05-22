using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAREFASDTO
{
    public int Codigo { get; set; }

    public string NomeTarefa { get; set; }

    public string DescricaoTarefa { get; set; }

    public DateTime DataTarefa { get; set; }

    public bool Finalizada { get; set; }

    public int? UsuarioTarefa { get; set; }

    public int? Time { get; set; }

    public int? TarefaFalhou { get; set; }

    public int XPTarefa { get; set; }
}
