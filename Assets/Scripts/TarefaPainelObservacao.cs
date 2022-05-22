using Assets.Scripts.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TarefaPainelObservacao : MonoBehaviour
{

    public int Codigo;
    public string DescricaoTarefa;
    public int? TarefaFinalizada;
    public int? UsuarioTarefa;
    public string NomeTarefa;
    public int? TimeTarefa;
    public int XPTarefa;
    public DateTime DataTarefa;
    public SQLCon _sqlConnection;
    public TelaPrincipalController _telaPrincipal;
    public LevelSystem _levelSystem;
    public GerirMonstro _gerirMonstro;

    public async void FinalizaObjetivo()
    {
        TAREFASDTO tarefaFinalizada = new TAREFASDTO()
        {
            Codigo = this.Codigo,
            NomeTarefa = this.NomeTarefa,
            DescricaoTarefa = this.DescricaoTarefa,
            UsuarioTarefa = this.UsuarioTarefa,
            Time = this.TimeTarefa,
            Finalizada = true,
            DataTarefa = this.DataTarefa,
            TarefaFalhou = 0,
            XPTarefa = this.XPTarefa
        };

        if (tarefaFinalizada != null)
        {
            if (_sqlConnection.UpdateTarefaFinalizar(tarefaFinalizada))
            {
                if (tarefaFinalizada.UsuarioTarefa == 0 && tarefaFinalizada.Time > 0)
                {
                    await _levelSystem.GanharXPTime(tarefaFinalizada.XPTarefa, UsuarioAtual.usuarioLogado.Time);
                }
                else
                {
                    await _levelSystem.GanharXP(tarefaFinalizada.XPTarefa);
                }

                _telaPrincipal.AbreLoadingScreen();
                StartCoroutine(EsperaSegundosVoltarTela(2));
                //_telaPrincipal.ReloadScene();
            }
        }

    }

    IEnumerator EsperaSegundosVoltarTela(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        UsuarioAtual.quantidadeTarefasFalhas = 0;
        UsuarioAtual.quantidadeTarefasTerminadas = 0;
        UsuarioAtual.quantidadeTarefasTotais = 0;
        UsuarioAtual.usuariosTimeGestor = new List<string>();
        UsuarioAtual.cursosUsuario = new List<CURSO>();
        UsuarioAtual.tarefasAtuais = new List<TAREFASDTO>();
        _telaPrincipal.FechaLoadingScreen();
        _telaPrincipal.FechaTelaTarefas();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public async void FalhaObjetivo()
    {
        TAREFASDTO tarefaFinalizada = new TAREFASDTO()
        {
            Codigo = this.Codigo,
            NomeTarefa = this.NomeTarefa,
            DescricaoTarefa = this.DescricaoTarefa,
            UsuarioTarefa = this.UsuarioTarefa,
            Time = this.TimeTarefa,
            Finalizada = true,
            DataTarefa = this.DataTarefa,
            TarefaFalhou = 1,
            XPTarefa = this.XPTarefa
        };


        if (tarefaFinalizada != null)
        {
            if (_sqlConnection.UpdateTarefaFinalizar(tarefaFinalizada))
            {
                await _gerirMonstro.GanharXP(tarefaFinalizada.XPTarefa);
                _telaPrincipal.AbreLoadingScreen();
                StartCoroutine(EsperaSegundosVoltarTela(2));
                //_telaPrincipal.ReloadScene();
            }
        }
    }
}
