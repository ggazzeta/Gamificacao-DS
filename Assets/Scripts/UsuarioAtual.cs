using Assets.Scripts.DTOs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UsuarioAtual
{

    public static USUARIO usuarioLogado;

    public static USUARIO usuarioMonstro;

    public static List<TAREFASDTO> tarefasAtuais = new List<TAREFASDTO>();

    public static List<TAREFASDTO> tarefasMonstro = new List<TAREFASDTO>();

    public static List<TAREFASPROPOSTA> tarefasPropostaTime = new List<TAREFASPROPOSTA>();

    public static TAREFASDTO tarefaSelecionada;

    public static List<string> usuariosTimeGestor = new List<string>();

    public static List<CURSO> cursosUsuario = new List<CURSO>();

    public static int quantidadeTarefasTerminadas = 0;

    public static int quantidadeTarefasTotais = 0;

    public static int quantidadeTarefasFalhas = 0;

}
