using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ExemploCrud
{
    class Program
    {
        static void Main(string[] args)
        {
            // Variavél recebe uma string vazia utilizando função extensiva.
            var opcaoSelecionada = string.Empty;

            // Laço de repetição programado para sair do loop apenas quando o caractere digitado pelo usuário for 5
            do
            {
                // Impressão de menu na tela de comando do windows.
                Console.WriteLine("Digite uma das opções abaixo:");
                Console.WriteLine("1 - Inserir um novo registro.");
                Console.WriteLine("2 - Exibir registros cadastrados.");
                Console.WriteLine("3 - Editar um registro.");
                Console.WriteLine("4 - Excluir um registro.");
                Console.WriteLine("5 - Encerrar sistema.");

                // Leitura de caractere inserido pelo usuário.
                opcaoSelecionada = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine("");

                // Tomada de decisão a partir do caractere inserido pelo usuário.
                if (opcaoSelecionada == "1")
                    InserirNovoRegistro();
                else if (opcaoSelecionada == "2")
                    ExibirRegistrosCadastrados();
                else if (opcaoSelecionada == "3")
                    EditarRegistro();
                else if (opcaoSelecionada == "4")
                    ExcluirRegistro();

                Console.WriteLine("");
            } while (opcaoSelecionada != "5");
        }

        public static SqlConnection Connection()
        {
            var connectionString = @"data source=DESKTOP-UC20QVS;Initial Catalog=pocFramework;Integrated Security=SSPI;";
            var sqlConnection = new SqlConnection(connectionString);
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();

            return sqlConnection;
        }

        public static void Select(string sql, IEnumerable<SqlParameter> parametros)
        {
            using (var connection = Connection())
            {
                var command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.Text;

                if (parametros != null)
                    command.Parameters.AddRange(parametros.ToArray());

                var dados = command.ExecuteReader();
                Console.WriteLine("Id\tNome");
                while (dados.Read())
                {
                    Console.WriteLine($"{dados["id"]}\t{dados["nome"]}");
                }
                connection.Close();
            }
        }

        public static void Execute(string sql, IEnumerable<SqlParameter> parametros)
        {
            using (var connection = Connection())
            {
                var command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.Text;

                if (parametros != null)
                    command.Parameters.AddRange(parametros.ToArray());

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private static void InserirNovoRegistro()
        {
            Console.WriteLine("Informe o nome que deseja cadastrar com o maximo de 55 caracteres:");
            var nome = Console.ReadLine();
            if (nome.Length > 55)
            {
                Console.WriteLine("O nome informado contem mais que 55 caracteres.");
            }
            else
            {
                var sql = "insert into pessoas(nome) values(@nome)";
                var parametros = new List<SqlParameter>() { new SqlParameter("@nome", nome) };
                Execute(sql, parametros);

                Console.WriteLine("Pessoa cadastrada com sucesso.");
            }
        }

        private static void ExibirRegistrosCadastrados()
        {
            var sql = "select id, nome from pessoas";

            Select(sql, null);
        }

        private static void EditarRegistro()
        {
            Console.WriteLine("Informe o código do registro que deseja editar:");
            var id = Console.ReadLine();
            Console.WriteLine("Informe o novo nome para o código informado:");
            var nome = Console.ReadLine();
            var sql = "update pessoas set nome = @nome where id = @id";
            var parametros = new List<SqlParameter>()
            {
                new SqlParameter("@nome", nome) ,
                new SqlParameter("@id", id)
            };
            Execute(sql, parametros);

            Console.WriteLine("Pessoa atualizada com sucesso.");
        }

        private static void ExcluirRegistro()
        {
            Console.WriteLine("Informe o código do registro que deseja excluir:");
            var id = Console.ReadLine();
            var sql = "delete pessoas where id = @id";
            var parametros = new List<SqlParameter>() { new SqlParameter("@id", id) };
            Execute(sql, parametros);

            Console.WriteLine("Pessoa excluida com sucesso.");
        }
    }
}
