using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;

namespace GestaoMusical_Web.Models
{
    public class Banco
    {
        private NpgsqlConnection ConexaoBanco = new NpgsqlConnection();
        /// <summary>
        /// Procedure interna de abertura de conexao com banco de dados
        /// </summary>
        private void AbreBanco()
        {
            try
            {
                //    ConexaoBanco = new NpgsqlConnection("Server=localhost"
                //    + ";Port=5432;Database=db_alunas"
                //    + ";User Id=postgres;Password=postgres;");
                //    ConexaoBanco.Open();

                ConexaoBanco = new NpgsqlConnection("Server=ec2-3-214-103-146.compute-1.amazonaws.com"
                + ";Port=5432;Database=d8at45oq8919il"
                + ";User Id=cxasfofusnjzcn; Password=6efd7954ca89b53750773d2e213247dfd72c4076ff0f88083ce6709539ab15dd;");
                ConexaoBanco.Open();
            }
            catch
            { }
        }

        /// <summary>
        /// Procedure interna de fechamendo de conexão com banco
        /// </summary>
        private void FechaBanco()
        {
            try
            {
                ConexaoBanco.Close();
                ConexaoBanco.Dispose();
            }
            catch { }
        }

        /// <summary>
        /// Função de Seleção de Dados e preenchimento em um DataSet
        /// </summary>

        public DataSet SelecionaDadosDataSet(string SQL, string NomeDataSet, string NomeBanco = null)
        {
            DataSet Selecao = new DataSet(NomeDataSet);
            AbreBanco();
            NpgsqlCommand Comando = new NpgsqlCommand(SQL.ToUpper(), ConexaoBanco);
            NpgsqlDataAdapter Adaptador = new NpgsqlDataAdapter(Comando);
            Adaptador.Fill(Selecao, NomeDataSet);
            FechaBanco();
            return Selecao;
        }

        /// <summary>
        /// Função de Seleção de Dados em um DataTable
        /// </summary>

        public DataTable SelecionaDados(string SQL, string NomeBanco = null)
        {
            DataTable Selecao = new DataTable("SELECAO");
            AbreBanco();
            NpgsqlCommand Comando = new NpgsqlCommand(SQL, ConexaoBanco);
            NpgsqlDataAdapter Adaptador = new NpgsqlDataAdapter(Comando);
            Adaptador.Fill(Selecao);
            FechaBanco();
            return Selecao;
        }

        /// <summary>
        /// Função de Seleção de Dados com Passagem de parâmetros no SQL (evitar injeção SQL malicioso)
        /// onde Parâmetros devem ser passados com o separador | (Pipe)
        /// </summary>

        public DataTable SelecionaDados(string SQL, string PARAMETROS, string NomeBanco = null)
        {
            DataTable Selecao = new DataTable("SELECAO");
            try
            {
                AbreBanco();
                NpgsqlCommand Comando = new NpgsqlCommand(SQL.ToUpper(), ConexaoBanco);
                string[] aParametros = PARAMETROS.Split('|');
                foreach (string sParametros in aParametros)
                {
                    if (sParametros != "")
                    {
                        string[] aValor = sParametros.Split(',');
                        Comando.Parameters.Add(new NpgsqlParameter(aValor[0], aValor[1].ToUpper()));
                    }
                }
                NpgsqlDataAdapter Adaptador = new NpgsqlDataAdapter(Comando);
                Adaptador.Fill(Selecao);
            }
            catch { }
            FechaBanco();
            return Selecao;
        }

        /// <summary>
        /// Função geral de Inserção de Dados
        /// </summary>

        public int InsereDados(string SQL, string NomeBanco = null, bool Scalar = false)
        {
            int iRetorno = 0;
            try
            {
                AbreBanco();
                NpgsqlCommand Comando = new NpgsqlCommand(SQL, ConexaoBanco);
                iRetorno = Scalar == false ? Comando.ExecuteNonQuery() : Convert.ToInt32(Comando.ExecuteScalar());
            }
            catch (Exception ex) { }
            FechaBanco();
            return iRetorno;
        }



        /// <summary>
        /// Função geral de Atualização de Dados
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public int AtualizaDados(string SQL, string NomeBanco = null)
        {
            int iRetorno = 0;
            try
            {
                AbreBanco();
                NpgsqlCommand Comando = new NpgsqlCommand(SQL, ConexaoBanco);
                iRetorno = Comando.ExecuteNonQuery();
            }
            catch { }
            FechaBanco();
            return iRetorno;
        }

        /// <summary>
        ///  Função geral de Atualização de Dados
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="PARAMETROS"></param>
        /// <returns></returns>
        public int AtualizaDados(string SQL, string PARAMETROS, string NomeBanco = null)
        {
            int iRetorno = 0;
            try
            {
                AbreBanco();
                NpgsqlCommand Comando = new NpgsqlCommand(SQL.ToUpper(), ConexaoBanco);
                string[] aParametros = PARAMETROS.Split('|');
                foreach (string sParametros in aParametros)
                {
                    if (sParametros != "")
                    {
                        string[] aValor = sParametros.Split(',');
                        if (aValor[1].ToUpper() != "")
                            Comando.Parameters.Add(new NpgsqlParameter(aValor[0], aValor[1].ToUpper()));
                        else
                            Comando.Parameters.Add(new NpgsqlParameter(aValor[0], DBNull.Value));
                    }
                }
                iRetorno = Comando.ExecuteNonQuery();
            }
            catch { }
            FechaBanco();
            return iRetorno;
        }

        /// <summary>
        /// Função geral para Apagar Dados
        /// </summary>
        /// <param name="SQL"></param>
        /// <returns></returns>
        public int ApagarDados(string SQL, string NomeBanco = null)
        {
            int iRetorno = 0;
            try
            {
                AbreBanco();
                NpgsqlCommand Comando = new NpgsqlCommand(SQL, ConexaoBanco);
                iRetorno = Comando.ExecuteNonQuery();
            }
            catch { }
            FechaBanco();
            return iRetorno;
        }
    }
}


