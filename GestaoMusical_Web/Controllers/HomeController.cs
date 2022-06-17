using GestaoMusical_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data;
using System.Security.Claims;

namespace GestaoMusical_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Login(string usuario, string senha)
        {
            //42808193866
            //11111111111
            var CPF = senha.Substring(0, 3) + "," + senha.Substring(3, 3) +","+ senha.Substring(6,3) + "-" + senha.Substring(9,2);

            Banco banco = new Banco();
            var scriptSQL = "SELECT * FROM sc_alunas.tb_alunas WHERE nome_completo = '"+ usuario+"' AND cpf_aluna = '"+CPF+"' ";


            DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);

            var ID = RespostaSQL.Rows[0].Field<Int64>("id_alunas").ToString();
            if (RespostaSQL.Rows.Count > 0)
            {

                ViewData["Nome"] = usuario;

                scriptSQL = "SELECT * FROM sc_alunas.tb_anotacao WHERE id_alunas ="+ ID;
                RespostaSQL = banco.SelecionaDados(scriptSQL);
                if (RespostaSQL.Rows.Count > 0)
                {
                    var anotacoes = RespostaSQL.Rows[0].Field<String>("anotacao");
                    ViewData["Anotacao"] = anotacoes;
                }
            return View("Anotacao");

            }
            else { ModelState.AddModelError("", "Login Inválido."); }
            return View("Index");
        }
    }
}