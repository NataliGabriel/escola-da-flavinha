using GestaoMusical_Web.Models;
using System.Data;

namespace GestaoMusical_Web.Controllers
{
    public class TokenAPI
    {
        Banco banco = new Banco();
        public string GetToken()
        {
            var scriptSQL = "SELECT * FROM tb_api";


            DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);

            var ID = RespostaSQL.Rows[0].Field<string>("token");
            return ID;
        }
    }
}
