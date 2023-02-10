﻿using System;
using System.Data;

namespace GestaoMusical_Web.Models
{
    public class Login
    {
        public Int64 IdAlunas { get; set; }
        public string usuario { get; set; }

        public string anotacao { get; set; }
        public string data_anotacao { get; set; }
        public int id_anotacao { get; set; }


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
                        usuario = Convert.ToString(Registros.Rows[Registro]["nome_completo"])
                    }
                    );
                }
            }
            return leituras;
        }
        public List<Login> ListaDatas(int alunaID)
        {
            List<Login> leituras = new List<Login>();
            var sql = "select DISTINCT on (data_anotacao) data_anotacao, id_anotacao from sc_alunas.tb_anotacao WHERE id_alunas = " + alunaID + " ORDER BY data_anotacao";
            Banco banco = new Banco();
            DataTable Registros = banco.SelecionaDados(sql);

            if (Registros.Rows.Count != 0)
            {
                for (int Registro = 0; Registro < Registros.Rows.Count; Registro++)
                {
                    if (Registros.Rows[Registro]["data_anotacao"].ToString() != "")
                    {
                        string[] data_anotacao = Registros.Rows[Registro]["data_anotacao"].ToString().Replace(" 00:00:00", "").Split('/');
                        string data_formatada = data_anotacao[1] + "/" + data_anotacao[0] + "/" + data_anotacao[2];
                        leituras.Add(new Login
                        {
                            id_anotacao = Convert.ToInt32(Registros.Rows[Registro]["id_anotacao"]),
                            data_anotacao = data_formatada
                        }
                        );
                    }
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
