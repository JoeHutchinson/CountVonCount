using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CountVonCount.Infrastructure
{
    public sealed class SQLDBStore : IStorage
    {
        public Dictionary<string, int> Get()
        {
            using (var connection = CreateSqlConnection())
            {
                var result = new Dictionary<string, int>();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"SELECT TOP 100 w.Name, w.Count FROM WordMetrics w ORDER BY w.Count DESC";

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Add(reader[0].ToString(), int.Parse(reader[1].ToString()));
                            }
                        }
                    }
                }
                return result;
            }
        }

        public void Save(IEnumerable<KeyValuePair<string, int>> words)
        {
            using (var connection = CreateSqlConnection())
            {
                var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                BulkUpload(connection, transaction, words);
                UpdateMetrics(connection, transaction);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Merge uploaded words into the main table and clear temporary table
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        private static void UpdateMetrics(SqlConnection connection, SqlTransaction transaction)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                MERGE WordMetrics AS TARGET
                USING UpdatedWordMetrics AS SOURCE 
                ON (TARGET.Id = SOURCE.Id)
                WHEN MATCHED THEN 
                UPDATE SET TARGET.Count = TARGET.Count + SOURCE.Count
                WHEN NOT MATCHED BY TARGET THEN 
                INSERT (Id, Name, Count)
                VALUES (SOURCE.Id, SOURCE.Name, SOURCE.Count);
                TRUNCATE TABLE UpdatedWordMetrics;";
                cmd.Connection = connection;
                cmd.Transaction = transaction;
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Upload all words to tmp table
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="words"></param>
        private static void BulkUpload(SqlConnection connection, SqlTransaction transaction, IEnumerable<KeyValuePair<string, int>> words)
        {
            using(var dt = CreateDataTable(words))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO UpdatedWordMetrics SELECT Id, Name, Count FROM @dt";
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;

                var param = cmd.Parameters.AddWithValue("@dt", dt);
                param.TypeName = "WordType";
                param.SqlDbType = SqlDbType.Structured;

                cmd.ExecuteNonQuery();
            }
        }

        private static DataTable CreateDataTable(IEnumerable<KeyValuePair<string, int>> words)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Count", typeof(int));

            foreach (var word in words)
            {
                var row = dt.NewRow();
                row["Id"] = word.Key;       // Salt
                row["Name"] = word.Key;     // Encrypt
                row["Count"] = word.Value;
                dt.Rows.Add(row);
            }

            return dt;
        }

        private static SqlConnection CreateSqlConnection()
        {
            var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString);
            connection.Open();
            return connection;
        }
    }
}