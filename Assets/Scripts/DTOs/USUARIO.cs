using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USUARIO
{
    public USUARIO()
    {

    }

    public int Codigo { get; set; }

    public string Nome { get; set; }

    public string Login { get; set; }

    public string Senha { get; set; }

    public int Nivel { get; set; }

    public int Time { get; set; }

    public int Funcao { get; set; }

    public int XP { get; set; }
    public int? MetaUsuario { get; set; }
}
