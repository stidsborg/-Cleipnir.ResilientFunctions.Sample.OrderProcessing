// See https://aka.ms/new-console-template for more information

using Npgsql;

const string connectionString = "Server=localhost;Port=5432;Userid=postgres;Password=Pa55word!;Database=presentation;";
var conn = new NpgsqlConnection(connectionString);
conn.Open();
var command = new NpgsqlCommand("TRUNCATE TABLE orders; TRUNCATE TABLE rfunctions;", conn);
command.ExecuteNonQuery();
Console.WriteLine("Tables truncated successfully");