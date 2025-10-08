using Mono.Data.Sqlite;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Numerics;
using UnityEngine;

public class DatabaseController : MonoBehaviour
{
    SqliteConnection dbConnection;
    private string DatabaseName = "/MinigamesDB.db";
    private string dbPath;
    void Start()
    {
        dbPath = "URI=file:" + Application.dataPath + "/StreamingAssets" + DatabaseName;
        print(dbPath);
    }   

    //private void CreateDatabase()
    //{
    //    using (dbConnection = new SqliteConnection(dbPath))
    //    {
    //        dbConnection.Open();
    //        using (SqliteCommand command = dbConnection.CreateCommand())
    //        {
    //            command.CommandText = "CREATE TABLE IF NOT EXISTS Planets (planetId INTEGER PRIMARY KEY, planetName TEXT NOT NULL, planetDescription TEXT NOT NULL)";
    //            command.ExecuteNonQuery();
    //        }
    //        dbConnection.Close();   
    //    }
    //}

    public DataTable SelectData(string selectQuery)
    {
        DataSet resultSet = new DataSet();
        using (dbConnection = new SqliteConnection(dbPath))
        {
            dbConnection.Open();
            
            SqliteDataAdapter adapter = new SqliteDataAdapter(selectQuery, dbConnection);

            adapter.Fill(resultSet);
            adapter.Dispose();
            dbConnection.Close();
        }
        return resultSet.Tables[0];
    }
}
