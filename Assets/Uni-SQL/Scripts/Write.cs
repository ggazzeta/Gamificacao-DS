using UnityEngine;
using System;
using System.Data;
using System.Text;

using System.Collections;
using System.Collections.Generic;

using MySql.Data;
using MySql.Data.MySqlClient;

public class Write : MonoBehaviour
{
    public string host, database, user, password, charset, query;
    public bool pooling = true;

    private string connectionString;
    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    
    void Start()
    {
        connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";Charset= "+ charset +";Pooling=";
        if (pooling) {
            connectionString += "True";
        } else {
            connectionString += "False";
        }
        
        con = new MySqlConnection(connectionString);
        try
        {
            con.Open();

            string sql = query;
            cmd = new MySqlCommand(sql, con);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        con.Close();
    }
}
