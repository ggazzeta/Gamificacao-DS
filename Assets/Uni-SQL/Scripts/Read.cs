using UnityEngine;
using TMPro;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using Assets.Scripts.DTOs;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Read : MonoBehaviour
{
    public string host, database, user, password, charset, query;
    public bool pooling = true;

    #region Tela Login
    public TMP_InputField TextLogin;
    public TMP_InputField TextSenha;
    public TextMeshProUGUI TextSucessFail;
    #endregion

    #region Tela Cadastro
    public TMP_InputField TextNomeCadastro;
    public TMP_InputField TextUsuarioCadastro;
    public TMP_InputField TextSenhaCadastro;
    public TMP_Dropdown ddTimeCadastro;
    public TMP_Dropdown ddFuncaoCadastro;
    public TextMeshProUGUI TextStatusLogin;
    #endregion

    private string connectionString;
    public FuncoesCanvas canvas;
    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;
    public List<USUARIO> listaUsuarios = new List<USUARIO>();
    public Toggle salvarSenha;

    void Start()
    {
        TextStatusLogin.text = "";
        if (PlayerPrefs.HasKey("salvarSenha"))
            salvarSenha.isOn = PlayerPrefs.GetInt("salvarSenha") == 1 ? true : false;
        if (PlayerPrefs.HasKey("loginText"))
            TextLogin.text = PlayerPrefs.GetString("loginText");
        if (PlayerPrefs.HasKey("senhaText"))
            TextSenha.text = PlayerPrefs.GetString("senhaText");
    }

    public void InsertUsuario()
    {
        try
        {
            canvas.AbreLoadingScreen();

            var usuarioCadastro = new USUARIO();

            if (VerificaCamposCadastro())
            {
                usuarioCadastro.Nome = TextNomeCadastro.text;
                usuarioCadastro.Login = TextUsuarioCadastro.text;
                usuarioCadastro.Senha = TextSenhaCadastro.text;
                usuarioCadastro.Nivel = 0;
                usuarioCadastro.Time = Funcoes.ConverteTimeParaInt(ddTimeCadastro.options[ddTimeCadastro.value].text);
                usuarioCadastro.Funcao = Funcoes.ConverteFuncaoParaInt(ddFuncaoCadastro.options[ddFuncaoCadastro.value].text);

                connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
                con = new MySqlConnection(connectionString);

                string query = "INSERT INTO USUARIO (NOME, LOGIN, SENHA, NIVEL, TIME, FUNCAO, XP) VALUES (@NOME, @LOGIN, @SENHA, @NIVEL, @TIME, @FUNCAO, 0)";

                con.Open();
                cmd = new MySqlCommand(query, con);
                cmd.CommandTimeout = 10;
                cmd.Parameters.Add(new MySqlParameter("@NOME", MySqlDbType.VarChar) { Value = usuarioCadastro.Nome });
                cmd.Parameters.Add(new MySqlParameter("@LOGIN", MySqlDbType.VarChar) { Value = usuarioCadastro.Login });
                cmd.Parameters.Add(new MySqlParameter("@SENHA", MySqlDbType.VarChar) { Value = usuarioCadastro.Senha });
                cmd.Parameters.Add(new MySqlParameter("@NIVEL", MySqlDbType.Int32) { Value = usuarioCadastro.Nivel });
                cmd.Parameters.Add(new MySqlParameter("@TIME", MySqlDbType.Int32) { Value = usuarioCadastro.Time });
                cmd.Parameters.Add(new MySqlParameter("@FUNCAO", MySqlDbType.Int32) { Value = usuarioCadastro.Funcao });

                int linhasAfetadas = cmd.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    TextStatusLogin.text = "Inclusão Realizada";
                    TextStatusLogin.color = Color.green;
                    StartCoroutine(EsperaSegundosVoltarTela(2));
                }
                else
                {
                    TextStatusLogin.text = "Inclusão Falhou";
                    TextStatusLogin.color = Color.red;
                }

            }
        }
        catch (Exception ex)
        {
            canvas.FechaLoadingScreen();
        }


    }


    public bool UsuarioExiste()
    {
        connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
        con = new MySqlConnection(connectionString);

        string sql = @$"SELECT NOME FROM USUARIO WHERE LOGIN = @LOGIN";
        con.Open();
        cmd = new MySqlCommand();
        cmd.Connection = con;
        cmd.CommandText = sql;
        cmd.CommandTimeout = 10;
        cmd.Parameters.Add(new MySqlParameter("@LOGIN", MySqlDbType.VarChar) { Value = TextUsuarioCadastro.text });

        rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            return true;
        }

        return false;
    }

    public bool VerificaCamposCadastro()
    {

        if (string.IsNullOrEmpty(TextNomeCadastro.text))
        {
            TextStatusLogin.text = "Nome obrigatório.";
            return false;
        }

        if (string.IsNullOrEmpty(TextUsuarioCadastro.text))
        {
            TextStatusLogin.text = "Usuario obrigatório.";
            return false;
        }

        if (string.IsNullOrEmpty(TextSenhaCadastro.text))
        {
            TextStatusLogin.text = "Senha obrigatória.";
            return false;
        }

        if (string.IsNullOrEmpty(ddTimeCadastro.itemText.text))
        {
            TextStatusLogin.text = "Time obrigatório.";
            return false;
        }

        if (string.IsNullOrEmpty(ddFuncaoCadastro.itemText.text))
        {
            TextStatusLogin.text = "Funcao obrigatória.";
            return false;
        }

        if (!string.IsNullOrEmpty(TextSenhaCadastro.text) && TextSenhaCadastro.text.Length < 3)
        {
            TextStatusLogin.text = "Senha precisa ter pelo menos 3 caracteres.";
            return false;
        }

        if (UsuarioExiste())
        {
            TextStatusLogin.text = "Usuario já cadastrado";
            return false;
        }

        return true;
    }

    public void InsertLog(LOG logAdicionar)
    {
        try
        {
            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);

            string query = "INSERT INTO LOG (DATA, DESCRICAO, USUARIO) VALUES (@DATA, @DESCRICAO, @USUARIO)";

            con.Open();
            cmd = new MySqlCommand(query, con);
            cmd.CommandTimeout = 10;
            cmd.Parameters.Add(new MySqlParameter("@DATA", MySqlDbType.DateTime) { Value = logAdicionar.Data });
            cmd.Parameters.Add(new MySqlParameter("@DESCRICAO", MySqlDbType.VarChar) { Value = logAdicionar.Descricao });
            cmd.Parameters.Add(new MySqlParameter("@USUARIO", MySqlDbType.Int32) { Value = logAdicionar.Usuario });

            int linhasAfetadas = cmd.ExecuteNonQuery();
            con.Close();

        }
        catch (Exception ex)
        {
            con.Close();
        }
    }

    public void RequisitarLogin()
    {
        try
        {
            canvas.AbreLoadingScreen();
            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= " + charset + ";";
            con = new MySqlConnection(connectionString);
            var logged = false;

            if (ValidaCamposDigitados())
            {

                string login, senha;

                login = TextLogin.text.Trim();
                senha = TextSenha.text.Trim();

                if (salvarSenha.isOn)
                {
                    PlayerPrefs.SetInt("salvarSenha", 1);
                    PlayerPrefs.SetString("loginText", login);
                    PlayerPrefs.SetString("senhaText", senha);
                }
                else
                {
                    PlayerPrefs.SetInt("salvarSenha", 0);
                    PlayerPrefs.SetString("loginText", "");
                    PlayerPrefs.SetString("senhaText", "");
                }

                //LOG logLogin = new LOG()
                //{
                //    Data = DateTime.Now,
                //    Descricao = "Requisitou Login usuario: " + login + ", senha: " + senha,
                //    Usuario = 1
                //};

                //InsertLog(logLogin);

                if ((string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha)))
                {
                    return;
                }

                string sql = @$"SELECT ID, NOME, LOGIN, SENHA, NIVEL, TIME, FUNCAO, XP, METAUSUARIO FROM USUARIO WHERE LOGIN = @LOGIN AND SENHA = @SENHA";
                con.Open();
                cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 10;
                cmd.Parameters.Add(new MySqlParameter("@LOGIN", MySqlDbType.VarChar) { Value = login });
                cmd.Parameters.Add(new MySqlParameter("@SENHA", MySqlDbType.VarChar) { Value = senha });

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    List<string> listaObjetos = new List<string>();

                    if (rdr[0] == null)
                    {
                        TextSucessFail.text = "Login Falhou. Usuário ou senha inválidos.";
                        TextSucessFail.color = Color.red;
                        return;
                    }

                    for (int i = 0; i < rdr.FieldCount; i++)
                    {
                        listaObjetos.Add(rdr[i]?.ToString());
                    }

                    USUARIO usuarioAtual = new USUARIO();

                    usuarioAtual = Funcoes.MapearUsuario(listaObjetos);
                    UsuarioAtual.usuarioLogado = usuarioAtual;

                    listaUsuarios.Add(usuarioAtual);
                    logged = true;

                }

                if (listaUsuarios == null || !listaUsuarios.Any())
                {
                    TextLogin.text = "";
                    TextSenha.text = "";
                    TextLogin.Select();
                    TextLogin.ActivateInputField();
                    TextSucessFail.text = "Login Falhou. Usuário ou senha inválidos.";
                    TextSucessFail.color = Color.red;
                    canvas.FechaLoadingScreen();
                    StartCoroutine(EsperaSegundosApagarFail(3));
                }
                rdr.Close();
                listaUsuarios = new List<USUARIO>();

                if (logged)
                {
                    canvas.FechaLoadingScreen();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }

        }
        catch (Exception ex)
        {
            rdr.Close();
            canvas.FechaLoadingScreen();
            return;
        }
    }

    private bool ValidaCamposDigitados()
    {
        if (string.IsNullOrEmpty(TextLogin.text))
        {
            TextSucessFail.text = "Login Falhou. Usuário ou senha inválidos.";
            TextSucessFail.color = Color.red;
            canvas.FechaLoadingScreen();
            return false;
        }
        if (string.IsNullOrEmpty(TextSenha.text))
        {
            TextSucessFail.text = "Login Falhou. Usuário ou senha inválidos.";
            TextSucessFail.color = Color.red;
            canvas.FechaLoadingScreen();
            return false;
        }

        return true;
    }

    IEnumerator EsperaSegundosVoltarTela(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        canvas.FechaTelaCadastro();
        canvas.FechaLoadingScreen();
    }

    IEnumerator EsperaSegundosApagarFail(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        TextSucessFail.text = "";
    }
}