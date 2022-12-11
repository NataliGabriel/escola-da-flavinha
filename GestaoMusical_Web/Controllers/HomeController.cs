using GestaoMusical_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data;
using System.Security.Claims;
using GestaoMusical_Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            if (usuario != "André Rodrigues" && senha != "encarregadovilalaurea")
            {
                var CPF = senha.Substring(0, 3) + "," + senha.Substring(3, 3) + "," + senha.Substring(6, 3) + "-" + senha.Substring(9, 2);

                Banco banco = new Banco();
                var scriptSQL = "SELECT * FROM sc_alunas.tb_alunas WHERE nome_completo = '" + usuario + "' AND cpf_aluna = '" + CPF + "' ";


                DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);

                var ID = RespostaSQL.Rows[0].Field<Int64>("id_alunas").ToString();
                if (RespostaSQL.Rows.Count > 0)
                {
                    ListaNomeAluna(Convert.ToInt32(ID));
                    var user_menor = usuario.Split(" ");
                    ViewData["Nome"] = user_menor[0];

                    scriptSQL = "SELECT * FROM sc_alunas.tb_anotacao WHERE id_alunas =" + ID + " ORDER BY id_anotacao DESC limit 1";
                    RespostaSQL = banco.SelecionaDados(scriptSQL);
                    if (RespostaSQL.Rows.Count > 0)
                    {
                        var anotacoes = RespostaSQL.Rows[0].Field<String>("anotacao");
                        ViewData["Anotacao"] = anotacoes;
                    }
                    return View("Anotacao");

                }
                else { ModelState.AddModelError("", "Login Inválido."); return View("Index"); }
            }
            else
            {


                Banco banco = new Banco();
                var scriptSQL = "SELECT * FROM sc_alunas.tb_alunas WHERE nome_completo = 'Ana Beatriz Ortune Rodrigues'";


                DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);

                var ID = RespostaSQL.Rows[0].Field<Int64>("id_alunas").ToString();
                ListaNomeAluna(Convert.ToInt32(ID));
                ViewData["Nome"] = "Irmão André";
                return View("Encarregado");
            }
        }

        [HttpPost]
        public IActionResult FiltroNome (string aluna)
        {
            Banco banco = new Banco();
            
                ListaNomeAluna(Convert.ToInt32(aluna));
                ViewData["Nome"] = "Irmão André";
                
                
                var scriptSQL = "SELECT * FROM sc_alunas.tb_anotacao WHERE id_alunas =" + aluna + "ORDER BY id_anotacao DESC limit 1";
                DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);
                if (RespostaSQL.Rows.Count > 0)
                {
                    var anotacoes = RespostaSQL.Rows[0].Field<String>("anotacao");
                    ViewData["Anotacao"] = anotacoes;
                }
                return View("Encarregado");

        }

        private void ListaNomeAluna(int alunaID)
        {
            List<SelectListItem> ListagemAluna= new List<SelectListItem>();
            Login dt= new Login();
            List<Login> ListagemAlunas= dt.ListaAlunas();

            if (ListagemAlunas.Count != 0)
            {
                for (int i = 0; i < ListagemAlunas.Count; i++)
                {
                    ListagemAluna.Add(new SelectListItem
                    {
                        Value = ListagemAlunas[i].IdAlunas.ToString(),
                        Text = ListagemAlunas[i].usuario
                    });
                    if (alunaID == ListagemAlunas[i].IdAlunas)
                        ListagemAluna[i].Selected = true;
                }
            }
            ViewBag.Lista = ListagemAluna;

        }
    }
}