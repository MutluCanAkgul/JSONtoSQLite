using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.IO;


namespace JSONconverttoSQLite
{
    public class Yorum
    {
        public string kulId { get; set; }
        public string lokasyonId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string yorum { get; set; }
        public int puan { get; set; }
    }
    public class YorumlarRepsonse
    {
        public List<Yorum> Yorumlar { get; set; }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string filepath = @"your json file path";
                string json = File.ReadAllText(filepath);

                YorumlarRepsonse response = JsonConvert.DeserializeObject<YorumlarRepsonse>(json);
                string databasePath = @"your sqllite file path";
                string connectionString = $"Data Source={databasePath};Version=3;";
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    foreach (var yorum in response.Yorumlar)
                    {
                        string query = "INSERT INTO Yorumlar (kulId, yorum, lokasyonId, puan, name, surname) VALUES (@kulId, @yorum, @lokasyonId, @puan, @name, @surname)";
                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@kulId", yorum.kulId);
                            command.Parameters.AddWithValue("@lokasyonId", yorum.lokasyonId);
                            command.Parameters.AddWithValue("@name", yorum.name);
                            command.Parameters.AddWithValue("@surname", yorum.surname);
                            command.Parameters.AddWithValue("@yorum", yorum.yorum);
                            command.Parameters.AddWithValue("@puan", yorum.puan);
                            command.ExecuteNonQuery();
                            Console.WriteLine("Yorum Başarıyla Aktarıldı");
                        }
                    }
                }
            } 
            catch(Exception ex)
            {
             Console.WriteLine("Hata : " + ex.ToString());
            }
            Console.ReadKey();
        }
    }
}
