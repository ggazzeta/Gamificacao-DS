using Assets.Scripts.DTOs;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SQLCon : MonoBehaviour
{

    public string host, database, user, password, charset;
    private string connectionString;
    public bool pooling = true;
    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;

    public GameObject tarefaLinha;
    public GameObject tarefaPropostaLinha;
    public Transform headerTarefas;
    public Transform headerPropostaTarefas;
    public GameObject AbaTime;
    private GerirTime gerirTime;
    public GameObject AbaCurso;
    private GerirCursos gerirCursos;
    public GameObject canvas;
    public TelaPrincipalController telaPrincipal;



    private void Start()
    {
        gerirTime = AbaTime.GetComponent<GerirTime>();
        gerirCursos = AbaCurso.GetComponent<GerirCursos>();
        telaPrincipal = canvas.GetComponent<TelaPrincipalController>();
        RequisitarTarefasUsuario();
        RequisitarCodigoNomeUsuariosTime();
        RequisitarTarefasTerminadas();
        RequisitarCursosUsuario();
        RequisitarTarefasPropostas();
        RequisitarMonstroETarefas();
    }

    public void RequisitarCursosUsuario()
    {
        try
        {
            UsuarioAtual.cursosUsuario = new List<CURSO>();

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);
            int usuario = UsuarioAtual.usuarioLogado.Codigo;

            string sql = @$"SELECT * FROM CURSOS WHERE USUARIO = @USUARIO";
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@USUARIO", MySqlDbType.Int32) { Value = usuario });

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                List<string> listaObjetos = new List<string>();

                if (rdr[0] == null)
                {
                    return;
                }

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    listaObjetos.Add(rdr[i]?.ToString());
                }

                CURSO cursoUsuario = new CURSO();

                cursoUsuario = Funcoes.MapearCursos(listaObjetos);

                UsuarioAtual.cursosUsuario.Add(cursoUsuario);
            }

            gerirCursos.PreencheCursosUsuario();

            rdr.Close();

        }
        catch (Exception ex)
        {
            rdr.Close();
            Debug.Log(ex.Message);
            return;
        }
    }

    public void RequisitarCodigoNomeUsuariosTime()
    {
        if (UsuarioAtual.usuarioLogado.Funcao == 1)
        {
            try
            {
                connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
                con = new MySqlConnection(connectionString);
                int timeGestor = UsuarioAtual.usuarioLogado.Time;

                if (timeGestor <= 0)
                {
                    return;
                }

                string sql = @$"SELECT CONCAT(ID,'-', NOME) FROM USUARIO WHERE TIME = @TIME";
                con.Open();
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 10;
                cmd.Parameters.Add(new MySqlParameter("@TIME", MySqlDbType.Int32) { Value = timeGestor });

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    List<string> listaObjetos = new List<string>();

                    if (rdr[0] == null)
                    {
                        return;
                    }

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        listaObjetos.Add(rdr[i]?.ToString());
                    }

                    string codigoNome = listaObjetos[0];
                    UsuarioAtual.usuariosTimeGestor.Add(codigoNome);
                }

                rdr.Close();

            }
            catch (Exception ex)
            {
                rdr.Close();
                Debug.Log(ex.Message);
                return;
            }
        }
    }

    public void RequisitarTarefasTerminadas()
    {
        try
        {
            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            var usuarioTime = UsuarioAtual.usuarioLogado.Time;

            string sql = @$"SELECT COUNT(*) FINALIZADAS, (SELECT COUNT(*) FROM TAREFA WHERE COALESCE(TIME,0) = @TIME) TOTAL FROM TAREFA WHERE COALESCE(FINALIZADA,0) = 1 AND COALESCE(TIME,0) = @TIME AND COALESCE(TAREFAFALHOU,0) <> 1";
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@TIME", MySqlDbType.Int32) { Value = usuarioTime });

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                List<string> listaObjetos = new List<string>();

                if (rdr[0] == null)
                {
                    return;
                }

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    listaObjetos.Add(rdr[i]?.ToString());
                }

                if (!string.IsNullOrEmpty(listaObjetos[0]) && !string.IsNullOrEmpty(listaObjetos[1]))
                {
                    UsuarioAtual.quantidadeTarefasTerminadas = Convert.ToInt32(listaObjetos[0]);
                    UsuarioAtual.quantidadeTarefasTotais = Convert.ToInt32(listaObjetos[1]);
                }

            }

            sql = @$"SELECT COUNT(*) FROM TAREFA WHERE COALESCE(FINALIZADA,0) = 1 AND COALESCE(TIME,0) = @TIME AND COALESCE(TAREFAFALHOU,0) = 1";
            con.Close();
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@TIME", MySqlDbType.Int32) { Value = usuarioTime });

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                List<string> listaObjetos = new List<string>();

                if (rdr[0] == null)
                {
                    return;
                }

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    listaObjetos.Add(rdr[i]?.ToString());
                }

                if (!string.IsNullOrEmpty(listaObjetos[0]))
                {
                    UsuarioAtual.quantidadeTarefasFalhas = Convert.ToInt32(listaObjetos[0]);
                }

            }

            gerirTime.PreencheAreaTime();

        }

        catch (Exception ex)
        {
            rdr.Close();
            Debug.Log(ex.Message);
            return;
        }
    }


    public void RequisitarTarefasUsuario()
    {
        try
        {

            UsuarioAtual.tarefasAtuais = new List<TAREFASDTO>();

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            var usuarioCodigo = UsuarioAtual.usuarioLogado.Codigo;
            var usuarioTime = UsuarioAtual.usuarioLogado.Time;

            if (usuarioCodigo <= 0)
            {
                return;
            }

            string sql = @$"SELECT ID, NOME, DESCRICAO, FINALIZADA, USUARIO, DATAENTREGA, TIME, TAREFAFALHOU, XPTAREFA FROM TAREFA WHERE (USUARIO = @USER OR TIME = @USERTIME) AND COALESCE(FINALIZADA,0) = 0 AND COALESCE(TAREFAFALHOU,0) <> 1";
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@USER", MySqlDbType.Int32) { Value = usuarioCodigo });
            cmd.Parameters.Add(new MySqlParameter("@USERTIME", MySqlDbType.Int32) { Value = usuarioTime });

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                List<string> listaObjetos = new List<string>();

                if (rdr[0] == null)
                {
                    return;
                }

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    listaObjetos.Add(rdr[i]?.ToString());
                }

                TAREFASDTO tarefa = new TAREFASDTO();

                tarefa = Funcoes.MapearTarefas(listaObjetos);
                UsuarioAtual.tarefasAtuais.Add(tarefa);
            }

            if (UsuarioAtual.tarefasAtuais != null && UsuarioAtual.tarefasAtuais.Any())
            {
                foreach (var tarefa in UsuarioAtual.tarefasAtuais)
                {
                    GameObject go = Instantiate(tarefaLinha, headerTarefas);
                    var preencheTarefas = go.GetComponent<PreencheTarefas>();
                    var objetoTarefa = preencheTarefas.tarefa;
                    if (objetoTarefa != null)
                    {
                        objetoTarefa.CODIGO = tarefa.Codigo;
                        objetoTarefa.NOMETAREFA = tarefa.NomeTarefa;
                        objetoTarefa.DESCRICAOTAREFA = tarefa.DescricaoTarefa;
                        objetoTarefa.DATAFINAL = tarefa.DataTarefa;
                        objetoTarefa.FINALIZADA = Funcoes.ConverteBoolParaInt(tarefa.Finalizada);
                        objetoTarefa.USUARIO = tarefa.UsuarioTarefa.GetValueOrDefault(0);
                        objetoTarefa.TIME = tarefa.Time.GetValueOrDefault(0);
                        objetoTarefa.XPTarefa = tarefa.XPTarefa;
                        preencheTarefas.PreencheTarefaObjeto();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            rdr.Close();
            Debug.Log(ex.Message);
            return;
        }
    }

    public void RequisitarTarefasPropostas()
    {
        try
        {

            UsuarioAtual.tarefasPropostaTime = new List<TAREFASPROPOSTA>();

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            var usuarioTime = UsuarioAtual.usuarioLogado.Time;

            string sql = @$"SELECT PT.ID, PT.NOME, PT.DESCRICAO, PT.ACEITA, U.NOME, PT.TIME FROM PROPOSTATAREFA PT
                            INNER JOIN USUARIO U ON PT.USUARIO = U.ID WHERE PT.TIME = @USERTIME AND COALESCE(PT.ACEITA,0) <> 1";
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@USERTIME", MySqlDbType.Int32) { Value = usuarioTime });

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                List<string> listaObjetos = new List<string>();

                if (rdr[0] == null)
                {
                    return;
                }

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    listaObjetos.Add(rdr[i]?.ToString());
                }

                TAREFASPROPOSTA tarefa = new TAREFASPROPOSTA();

                tarefa = Funcoes.MapearPropostaTarefas(listaObjetos);
                UsuarioAtual.tarefasPropostaTime.Add(tarefa);
            }

            if (UsuarioAtual.tarefasPropostaTime != null && UsuarioAtual.tarefasPropostaTime.Any())
            {
                foreach (var tarefa in UsuarioAtual.tarefasPropostaTime)
                {
                    GameObject go = Instantiate(tarefaPropostaLinha, headerPropostaTarefas);
                    var preencheTarefas = go.GetComponent<ControlaTelaScroll>();
                    preencheTarefas.PreencherTarefasProposta(tarefa);
                }

            }

            telaPrincipal.ChecaTarefasPropostas();
        }
        catch (Exception ex)
        {
            rdr.Close();
            Debug.Log(ex.Message);
            return;
        }
    }

    public void InsertTarefa(TAREFASDTO tarefaCadastro)
    {
        try
        {
            telaPrincipal.AbreLoadingScreen();
            string sbWhere = string.Empty;


            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "INSERT INTO TAREFA (NOME, DESCRICAO, FINALIZADA, USUARIO, DATAENTREGA, TIME, XPTAREFA) VALUES (@NOME, @DESCRICAO, 0, @USUARIO, @DATAENTREGA, @TIME, @XP)";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@NOME", MySqlDbType.VarChar) { Value = tarefaCadastro.NomeTarefa });
            cmd.Parameters.Add(new MySqlParameter("@DESCRICAO", MySqlDbType.VarChar) { Value = tarefaCadastro.DescricaoTarefa });
            cmd.Parameters.Add(new MySqlParameter("@USUARIO", MySqlDbType.Int32) { Value = tarefaCadastro.UsuarioTarefa });
            cmd.Parameters.Add(new MySqlParameter("@DATAENTREGA", MySqlDbType.DateTime) { Value = tarefaCadastro.DataTarefa });
            cmd.Parameters.Add(new MySqlParameter("@TIME", MySqlDbType.Int32) { Value = tarefaCadastro.Time });
            cmd.Parameters.Add(new MySqlParameter("@XP", MySqlDbType.Int32) { Value = tarefaCadastro.XPTarefa });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                StartCoroutine(EsperaSegundosVoltarTela(2));
            }
            else
            {
                Debug.Log("Falha");
                telaPrincipal.FechaLoadingScreen();
            }
        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
        }
    }

    public void InsertTarefaProposta(TAREFASPROPOSTA tarefaCadastro)
    {
        try
        {
            telaPrincipal.AbreLoadingScreen();

            string sbWhere = string.Empty;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "INSERT INTO PROPOSTATAREFA (NOME, DESCRICAO, ACEITA, USUARIO, TIME) VALUES (@NOME, @DESCRICAO, 0, @USUARIO, @TIME)";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@NOME", MySqlDbType.VarChar) { Value = tarefaCadastro.NomeTarefa });
            cmd.Parameters.Add(new MySqlParameter("@DESCRICAO", MySqlDbType.VarChar) { Value = tarefaCadastro.DescricaoTarefa });
            cmd.Parameters.Add(new MySqlParameter("@ACEITA", MySqlDbType.Int32) { Value = tarefaCadastro.Aceita });
            cmd.Parameters.Add(new MySqlParameter("@USUARIO", MySqlDbType.Int32) { Value = tarefaCadastro.UsuarioTarefa });
            cmd.Parameters.Add(new MySqlParameter("@TIME", MySqlDbType.Int32) { Value = tarefaCadastro.Time });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                StartCoroutine(EsperaSegundosVoltarTela(2));
            }
            else
            {
                Debug.Log("Falha");
                telaPrincipal.FechaLoadingScreen();
            }

        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
        }
    }

    public void UpdatePropostaAceita(TAREFASPROPOSTA tarefaAtual)
    {
        try
        {
            string sbWhere = string.Empty;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "UPDATE PROPOSTATAREFA SET ACEITA = 1 WHERE ID = @ID";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32) { Value = tarefaAtual.Codigo });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {

            }
            else
            {
            }
        }

        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public bool InsertCurso(CURSO cursoInclusao)
    {
        try
        {
            telaPrincipal.AbreLoadingScreen();
            string sbWhere = string.Empty;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "INSERT INTO CURSOS (NOME, DESCRICAO, AREACURSO, MINUTOS, DATATERMINO, USUARIO) VALUES (@NOME, @DESCRICAO, @AREACURSO, @MINUTOS, @DATATERMINO, @USUARIO)";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@NOME", MySqlDbType.VarChar) { Value = cursoInclusao.NomeCurso });
            cmd.Parameters.Add(new MySqlParameter("@DESCRICAO", MySqlDbType.VarChar) { Value = cursoInclusao.DescricaoCurso });
            cmd.Parameters.Add(new MySqlParameter("@AREACURSO", MySqlDbType.VarChar) { Value = cursoInclusao.AreaCurso });
            cmd.Parameters.Add(new MySqlParameter("@MINUTOS", MySqlDbType.Int32) { Value = cursoInclusao.Minutos });
            cmd.Parameters.Add(new MySqlParameter("@DATATERMINO", MySqlDbType.DateTime) { Value = cursoInclusao.DataTermino });
            cmd.Parameters.Add(new MySqlParameter("@USUARIO", MySqlDbType.Int32) { Value = UsuarioAtual.usuarioLogado.Codigo });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                telaPrincipal.FechaLoadingScreen();
                return true;
            }
            else
            {
                Debug.Log("Falha");
                telaPrincipal.FechaLoadingScreen();
                return false;
            }
        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
            return false;
        }
    }

    public async Task GravarXP(int XPNovo)
    {
        try
        {
            telaPrincipal.AbreLoadingScreen();
            var usuarioAlteracao = new USUARIO();
            string sbWhere = string.Empty;
            usuarioAlteracao = UsuarioAtual.usuarioLogado;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "UPDATE USUARIO SET XP = @XP WHERE ID = @ID";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@XP", MySqlDbType.Int32) { Value = XPNovo });
            cmd.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32) { Value = usuarioAlteracao.Codigo });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                telaPrincipal.FechaLoadingScreen();
                RequisitarCursosUsuario();
                //telaPrincipal.MudarAbas(3);
            }
            else
            {
                telaPrincipal.FechaLoadingScreen();
            }
        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
        }
    }

    public async Task GravarXP(int XPNovo, USUARIO usuarioXP)
    {
        try
        {
            telaPrincipal.AbreLoadingScreen();
            var usuarioAlteracao = new USUARIO();
            string sbWhere = string.Empty;
            usuarioAlteracao = usuarioXP;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "UPDATE USUARIO SET XP = @XP WHERE ID = @ID";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@XP", MySqlDbType.Int32) { Value = XPNovo });
            cmd.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32) { Value = usuarioAlteracao.Codigo });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                telaPrincipal.FechaLoadingScreen();
                RequisitarCursosUsuario();
            }
            else
            {
                telaPrincipal.FechaLoadingScreen();
            }
        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
        }
    }

    public async void RequisitarMonstroETarefas()
    {
        try
        {

            UsuarioAtual.tarefasMonstro = new List<TAREFASDTO>();
            UsuarioAtual.usuarioMonstro = new USUARIO();

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            var monstroCodigo = 50;


            string sql = @$"SELECT ID, NOME, DESCRICAO, FINALIZADA, USUARIO, DATAENTREGA, TIME, TAREFAFALHOU, XPTAREFA FROM TAREFA WHERE COALESCE(FINALIZADA,0) = 1 AND COALESCE(TAREFAFALHOU,0) = 1";
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandTimeout = 10;

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                List<string> listaObjetos = new List<string>();

                if (rdr[0] == null)
                {
                    return;
                }

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    listaObjetos.Add(rdr[i]?.ToString());
                }

                TAREFASDTO tarefa = new TAREFASDTO();

                tarefa = Funcoes.MapearTarefas(listaObjetos);
                UsuarioAtual.tarefasMonstro.Add(tarefa);
            }

            sql = @$"SELECT * FROM USUARIO WHERE ID = @CODIGO";
            con.Close();
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@CODIGO", MySqlDbType.Int32) { Value = monstroCodigo });

            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {

                List<string> listaObjetos = new List<string>();

                if (rdr[0] == null)
                {
                    return;
                }

                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    listaObjetos.Add(rdr[i]?.ToString());
                }

                USUARIO usuarioMonstro = new USUARIO();

                usuarioMonstro = Funcoes.MapearUsuario(listaObjetos);
                UsuarioAtual.usuarioMonstro = usuarioMonstro;
            }
        }
        catch (Exception ex)
        {
            rdr.Close();
            Debug.Log(ex.Message);
            return;
        }
    }

    public void GravarNivel(int levelAtual)
    {
        try
        {
            telaPrincipal.AbreLoadingScreen();
            var usuarioAlteracao = new USUARIO();
            string sbWhere = string.Empty;
            usuarioAlteracao = UsuarioAtual.usuarioLogado;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "UPDATE USUARIO SET NIVEL = @NIVEL WHERE ID = @ID";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@NIVEL", MySqlDbType.Int32) { Value = levelAtual });
            cmd.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32) { Value = usuarioAlteracao.Codigo });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                telaPrincipal.FechaLoadingScreen();
                RequisitarCursosUsuario();
                telaPrincipal.MudarAbas(3);
            }
            else
            {
                telaPrincipal.FechaLoadingScreen();
            }
        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
        }
    }

    public void GravarNivel(int levelAtual, USUARIO usuarioNivel)
    {
        try
        {
            telaPrincipal.AbreLoadingScreen();
            var usuarioAlteracao = new USUARIO();
            string sbWhere = string.Empty;
            usuarioAlteracao = usuarioNivel;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "UPDATE USUARIO SET NIVEL = @NIVEL WHERE ID = @ID";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@NIVEL", MySqlDbType.Int32) { Value = levelAtual });
            cmd.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32) { Value = usuarioAlteracao.Codigo });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                telaPrincipal.FechaLoadingScreen();
            }
            else
            {
                telaPrincipal.FechaLoadingScreen();
            }
        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
        }
    }

    public async Task GravarXPTime(int xpGanho, int timeXP)
    {
        try
        {
            telaPrincipal.AbreLoadingScreen();
            string sbWhere = string.Empty;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "UPDATE USUARIO SET XP = XP + @XP WHERE TIME = @TIME";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@XP", MySqlDbType.Int32) { Value = xpGanho });
            cmd.Parameters.Add(new MySqlParameter("@TIME", MySqlDbType.Int32) { Value = timeXP });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                telaPrincipal.FechaLoadingScreen();
                Start();
                telaPrincipal.MudarAbas(3);
            }
            else
            {
                telaPrincipal.FechaLoadingScreen();
            }
        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
        }
    }

    public bool UpdateMetaUsuario(int quantidadeHoras)
    {
        try
        {
            telaPrincipal.AbreLoadingScreen();
            var usuarioAlteracao = new USUARIO();
            string sbWhere = string.Empty;

            usuarioAlteracao = UsuarioAtual.usuarioLogado;
            usuarioAlteracao.MetaUsuario = quantidadeHoras;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "UPDATE USUARIO SET METAUSUARIO = @METAUSUARIO WHERE ID = @ID";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@METAUSUARIO", MySqlDbType.Int32) { Value = quantidadeHoras });
            cmd.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32) { Value = usuarioAlteracao.Codigo });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                telaPrincipal.FechaLoadingScreen();
                RequisitarCursosUsuario();
                telaPrincipal.MudarAbas(2);
                return true;
            }
            else
            {
                telaPrincipal.FechaLoadingScreen();
                return false;
            }
        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
            return false;
        }
    }


    public bool UpdateTarefaFinalizar(TAREFASDTO tarefaUpdate)
    {
        telaPrincipal.AbreLoadingScreen();
        try
        {
            var usuarioAlteracao = new USUARIO();
            string sbWhere = string.Empty;

            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "UPDATE TAREFA SET TAREFAFALHOU = @TAREFAFALHOU, FINALIZADA = 1 WHERE ID = @ID";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@TAREFAFALHOU", MySqlDbType.Int32) { Value = tarefaUpdate.TarefaFalhou });
            cmd.Parameters.Add(new MySqlParameter("@ID", MySqlDbType.Int32) { Value = tarefaUpdate.Codigo });

            int linhasAfetadas = cmd.ExecuteNonQuery();

            if (linhasAfetadas > 0)
            {
                telaPrincipal.FechaLoadingScreen();
                RequisitarTarefasUsuario();
                return true;
            }
            else
            {
                telaPrincipal.FechaLoadingScreen();
                return false;
            }
        }

        catch (Exception ex)
        {
            telaPrincipal.FechaLoadingScreen();
            Debug.Log(ex.Message);
            return false;
        }
    }

    IEnumerator EsperaSegundosVoltarTela(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        gerirTime.BotaoSairProposta();
        gerirTime.BotaoSairTarefa();
        telaPrincipal.FechaLoadingScreen();
        UsuarioAtual.quantidadeTarefasFalhas = 0;
        UsuarioAtual.quantidadeTarefasTerminadas = 0;
        UsuarioAtual.quantidadeTarefasTotais = 0;
        UsuarioAtual.usuariosTimeGestor = new List<string>();
        UsuarioAtual.cursosUsuario = new List<CURSO>();
        UsuarioAtual.tarefasAtuais = new List<TAREFASDTO>();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
