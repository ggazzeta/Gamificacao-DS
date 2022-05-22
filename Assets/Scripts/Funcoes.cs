using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using Assets.Scripts.DTOs;

public static class Funcoes
{
    public static USUARIO MapearUsuario(List<string> listaObjetos)
    {

        if (listaObjetos != null)
        {

            USUARIO usuario = new USUARIO()
            {
                Codigo = Convert.ToInt32(listaObjetos[0]),
                Nome = string.IsNullOrEmpty(listaObjetos[1]) ? "" : listaObjetos[1],
                Login = string.IsNullOrEmpty(listaObjetos[2]) ? "" : listaObjetos[2],
                Senha = string.IsNullOrEmpty(listaObjetos[3]) ? "" : listaObjetos[3],
                Nivel = string.IsNullOrEmpty(listaObjetos[4]) ? 0 : Convert.ToInt32(listaObjetos[4]),
                Time = string.IsNullOrEmpty(listaObjetos[5]) ? 0 : Convert.ToInt32(listaObjetos[5]),
                Funcao = string.IsNullOrEmpty(listaObjetos[6]) ? 0 : Convert.ToInt32(listaObjetos[6]),
                XP = string.IsNullOrEmpty(listaObjetos[7]) ? 0 : Convert.ToInt32(listaObjetos[7]),
                MetaUsuario = string.IsNullOrEmpty(listaObjetos[8]) ? 0 : Convert.ToInt32(listaObjetos[8])
            };

            return usuario;
        }

        return new USUARIO();
    }

    public static TAREFASDTO MapearTarefas(List<string> listaObjetos)
    {

        if (listaObjetos != null)
        {

            TAREFASDTO usuario = new TAREFASDTO()
            {
                Codigo = Convert.ToInt32(listaObjetos[0]),
                NomeTarefa = string.IsNullOrEmpty(listaObjetos[1]) ? "" : listaObjetos[1],
                DescricaoTarefa = string.IsNullOrEmpty(listaObjetos[2]) ? "" : listaObjetos[2],
                Finalizada = string.IsNullOrEmpty(listaObjetos[3]) ? false : ConverteIntParaBool(listaObjetos[4]),
                UsuarioTarefa = string.IsNullOrEmpty(listaObjetos[4]) ? 0 : Convert.ToInt32(listaObjetos[4]),
                DataTarefa = string.IsNullOrEmpty(listaObjetos[5]) ? DateTime.Today : Convert.ToDateTime(listaObjetos[5]),
                Time = string.IsNullOrEmpty(listaObjetos[6]) ? 0 : Convert.ToInt32(listaObjetos[6]),
                TarefaFalhou = string.IsNullOrEmpty(listaObjetos[7]) ? 0 : Convert.ToInt32(listaObjetos[7]),
                XPTarefa = string.IsNullOrEmpty(listaObjetos[8]) ? 0 : Convert.ToInt32(listaObjetos[8])
            };

            return usuario;
        }

        return new TAREFASDTO();
    }

    public static TAREFASPROPOSTA MapearPropostaTarefas(List<string> listaObjetos)
    {

        if (listaObjetos != null)
        {

            TAREFASPROPOSTA tarefasProposta = new TAREFASPROPOSTA()
            {
                Codigo = Convert.ToInt32(listaObjetos[0]),
                NomeTarefa = string.IsNullOrEmpty(listaObjetos[1]) ? "" : listaObjetos[1],
                DescricaoTarefa = string.IsNullOrEmpty(listaObjetos[2]) ? "" : listaObjetos[2],
                Aceita = string.IsNullOrEmpty(listaObjetos[3]) ? 0 : Convert.ToInt32(listaObjetos[3]),
                UsuarioTarefa = string.IsNullOrEmpty(listaObjetos[4]) ? "" : listaObjetos[4],
                Time = string.IsNullOrEmpty(listaObjetos[5]) ? 0 : Convert.ToInt32(listaObjetos[5]),
            };

            return tarefasProposta;
        }

        return new TAREFASPROPOSTA();
    }

    public static CURSO MapearCursos(List<string> listaObjetos)
    {
        if (listaObjetos != null)
        {

            CURSO curso = new CURSO()
            {
                Codigo = Convert.ToInt32(listaObjetos[0]),
                NomeCurso = string.IsNullOrEmpty(listaObjetos[1]) ? "" : listaObjetos[1],
                DescricaoCurso = string.IsNullOrEmpty(listaObjetos[2]) ? "" : listaObjetos[2],
                AreaCurso = string.IsNullOrEmpty(listaObjetos[3]) ? "" : listaObjetos[4],
                Minutos = string.IsNullOrEmpty(listaObjetos[4]) ? 0 : Convert.ToInt32(listaObjetos[4]),
                DataTermino = string.IsNullOrEmpty(listaObjetos[5]) ? DateTime.Today : Convert.ToDateTime(listaObjetos[5]),
                Usuario = string.IsNullOrEmpty(listaObjetos[6]) ? 0 : Convert.ToInt32(listaObjetos[6])
            };

            return curso;
        }

        return new CURSO();
    }

    public static bool ConverteIntParaBool(string texto)
    {
        switch (texto)
        {
            case "0":
                return false;
            case "1":
                return true;
            default:
                return false;
        }
    }

    public static bool ConverteIntParaBool(int numero)
    {
        switch (numero)
        {
            case 0:
                return false;
            case 1:
                return true;
            default:
                return false;
        }
    }

    public static int ConverteBoolParaInt(bool validacao)
    {
        switch (validacao)
        {
            case false:
                return 0;
            case true:
                return 1;
            default:
                return 0;
        }
    }

    public static int ConverteTimeParaInt(string time)
    {
        switch (time)
        {
            case "Desenvolvimento":
                return 1;
            case "Suporte":
                return 2;
            default:
                return 1;
        }
    }

    public static string ConverteIntParaTime(int time)
    {
        switch (time)
        {
            case 1:
                return "DESENVOLVIMENTO";
            case 2:
                return "SUPORTE";
            default:
                return "DESENVOLVIMENTO";
        }
    }

    public static int ConverteFuncaoParaInt(string time)
    {
        switch (time)
        {
            case "Gestor":
                return 1;
            case "Criador":
                return 2;
            case "Funcionario":
                return 3;
            default:
                return 3;
        }
    }

    public static string ConverteIntParaFuncao(int time)
    {
        switch (time)
        {
            case 1:
                return "GESTOR";
            case 2:
                return "CRIADOR";
            case 3:
                return "FUNCIONARIO";
            default:
                return "CRIADOR";
        }
    }

    public static string MapearAba(int aba)
    {
        switch (aba)
        {
            case 1:
                return "Objetivos";
            case 2:
                return "Meu Aprendizado";
            case 3:
                return "Progresso Time";
            case 4:
                return "Progresso Monstro";
            default:
                return "Objetivos";
        }
    }

    public static int GetUsuarioTarefa(string usuarioTarefa)
    {
        if (!string.IsNullOrEmpty(usuarioTarefa))
        {
            string[] usuarioStrings = usuarioTarefa.Split('-');
            return Convert.ToInt32(usuarioStrings[0]);
        }

        return 0;
    }

    public static int RetornaTempoEmMinutos(int horas, int minutos)
    {
        var horasEmMinutos = horas * 60;
        return horasEmMinutos + minutos;
    }
}

public static class LevelManager
{
    public const int XP_PARA_NIVEL_UM = 900;
    public const int XP_PARA_NIVEL_DOIS = 1800;
    public const int XP_PARA_NIVEL_TRES = 3600;
    public const int XP_PARA_NIVEL_QUATRO = 7200;
    public const int XP_PARA_NIVEL_CINCO = 14400;
}
