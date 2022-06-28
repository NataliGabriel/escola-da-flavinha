using System;
using System.Data;

namespace GestaoMusical_Web.Models
{
    public class Login
    {
        public Int64 IdAlunas { get; set; }
        public string usuario { get; set; }
        
        public string anotacao { get; set; }


        public List<Login> ListaAlunas()
        {
            List<Login> leituras = new List<Login>();
            var sql = "select * from sc_alunas.tb_alunas ORDER BY nome_completo";
            Banco banco = new Banco();
            DataTable Registros = banco.SelecionaDados(sql);

            if (Registros.Rows.Count != 0)
            {
                for (int Registro = 0; Registro < Registros.Rows.Count; Registro++)
                {
                    leituras.Add(new Login
                    {
                        IdAlunas = Convert.ToInt64(Registros.Rows[Registro]["id_alunas"]),
                        usuario = Convert.ToString(Registros.Rows[Registro]["nome_completo"]),
                    }
                    );
                }
            }
            return leituras;
        }

        public Login PesquisarAlunaId(int IDaluna)
        {
            Login leituras = new Login();
            var scriptSQL = "SELECT " +
                    "tba.anotacao " +
                    "FROM sc_alunas.tb_anotacao as tba " +
                    "INNER JOIN " +
                    "sc_alunas.tb_alunas " +
                    "ON " +
                    "tba.id_alunas = sc_alunas.tb_alunas.id_alunas " +
                    "WHERE " +
                    "tba.id_alunas =" + IDaluna;
            Banco banco = new Banco();
            DataTable Registros = banco.SelecionaDados(scriptSQL);

            if (Registros.Rows.Count != 0)
            {
                for (int Registro = 0; Registro < Registros.Rows.Count; Registro++)
                {
                //  leituras.IdAlunas = Convert.ToInt64(Registros.Rows[Registro]["id_alunas"]);
                    leituras.usuario = Convert.ToString(Registros.Rows[Registro]["id_alunas"]);
                    leituras.anotacao = Convert.ToString(Registros.Rows[Registro]["anotacao"]);


                }
            }
            return leituras;

        }
    }
}
