using System.Collections.Generic;

public class UsuarioRetorno
{
    public int ID { get; set; }
    public string NOME { get; set; }
    public string LOGIN { get; set; }
    public string EMAIL { get; set; }
    public int NIVEL { get; set; }
    public int TIME { get; set; }
    public int FUNCAO { get; set; }
    public int XP { get; set; }
}

public class Root
{
    public List<UsuarioRetorno> listaTarefas { get; set; }
}