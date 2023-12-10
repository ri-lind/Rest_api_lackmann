using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System.Collections;
using Newtonsoft.Json;
using LackmannApi;
using ScottPlot;

namespace LackmannApi
{
    public class DatabaseManager
    {
        public static ArrayList GetAllDocuments()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=lackmann";
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM marketdocument");
            command.Connection = connection;
            NpgsqlDataReader reader = command.ExecuteReader();
            ArrayList documents = new ArrayList();
            while(reader.Read())
            {
                documents.Add(CreateDocument(reader));

            }
            connection.Close();
            return documents;
        }



        public static MarketDocument FetchFromDatabase(int id)
        {
            string selectStatement = $"SELECT * FROM marketDocument WHERE id = {id}";
            string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=lackmann";

            using NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            using NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            cmd.CommandText = selectStatement;
            
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            
            MarketDocument marketDocument = new MarketDocument();
            while(dataReader.Read())
            {
                marketDocument = CreateDocument(dataReader);
                marketDocument.Points = FetchPoints(marketDocument);
            }
            con.Close();
            return marketDocument;
        }


        public static MarketDocument CreateDocument(NpgsqlDataReader dataReader)
        {
            string mrid = dataReader.GetString(1);
            int revisionNumber = dataReader.GetInt32(2);
            string type = dataReader.GetString(3);
            string senderMRID = dataReader.GetString(4);
            string receiverMRID = dataReader.GetString(5);
            DateTime createdTime = DateTime.Parse(dataReader.GetString(6));
            return new MarketDocument(mrid, revisionNumber, type, senderMRID, receiverMRID, createdTime);

        }

        public static ArrayList FetchPoints(MarketDocument marketDocument)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=lackmann";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            string selectStatement = $"SELECT position, quantity FROM Point WHERE fk_mrid = '{marketDocument.MRID}'";

            cmd.CommandText = selectStatement;
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            ArrayList points = new ArrayList();

            while(dataReader.Read())
            {
                int position = dataReader.GetInt32(0);
                int quantity = dataReader.GetInt32(1);

                points.Add(new Point(position, quantity));
            }
            
            return points;

        }
    }
}