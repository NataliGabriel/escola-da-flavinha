using GestaoMusical_Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data;
using System.Security.Claims;
using GestaoMusical_Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenAI_API;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// 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
/// </summary>
namespace GestaoMusical_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static int _id = 0;
        public static string _name = null;
        private const string ChatGptApiKey = "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";
        private const string ChatGptBaseUrl = "https://api.openai.com/v1/completions";


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
            if (usuario == "André Rodrigues" && senha == "encarregadovilalaurea")
            {
                Banco banco = new Banco();
                var scriptSQL = "SELECT * FROM sc_alunas.tb_alunas WHERE nome_completo = 'Ana Beatriz Ortune Rodrigues'";


                DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);

                var ID = RespostaSQL.Rows[0].Field<Int64>("id_alunas").ToString();
                ListaNomeAluna(Convert.ToInt32(ID));
                _name = "Irmão André";
                ViewData["Nome"] = _name;
                return View("Encarregado");
            }
            else if (usuario == "Flavia" && senha == "melhorinstrutora")
            {

                Banco banco = new Banco();
                var scriptSQL = "SELECT * FROM sc_alunas.tb_alunas WHERE nome_completo = 'Ana Beatriz Ortune Rodrigues'";


                DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);

                var ID = RespostaSQL.Rows[0].Field<Int64>("id_alunas").ToString();
                ListaNomeAluna(Convert.ToInt32(ID));
                _name = "Flávia";
                ViewData["Nome"] = _name;
                return View("Encarregado");

            }
            else
            {
                var CPF = senha.Substring(0, 3) + "," + senha.Substring(3, 3) + "," + senha.Substring(6, 3) + "-" + senha.Substring(9, 2);

                Banco banco = new Banco();
                var scriptSQL = "SELECT * FROM sc_alunas.tb_alunas WHERE nome_completo = '" + usuario + "' AND cpf_aluna = '" + CPF + "' ";


                DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);

                _id = Convert.ToInt32(RespostaSQL.Rows[0].Field<Int64>("id_alunas"));
                if (RespostaSQL.Rows.Count > 0)
                {
                    ListaDataAnotacao(Convert.ToInt32(_id));
                    var user = usuario.Split(" ");
                    _name = user[0];
                    ViewData["Nome"] = _name;

                    scriptSQL = "SELECT * FROM sc_alunas.tb_anotacao WHERE id_alunas =" + _id + " ORDER BY id_anotacao DESC limit 1";
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
        }
        [HttpPost]
        public IActionResult AdicionaAnotacao(string anotacao)
        {
            try
            {
                #region Variáveis
                Banco banco = new Banco();
                string id = Convert.ToString(_id);
                DateTime data = DateTime.Now;
                string dataCurta = data.ToShortDateString();
                string dataFormatada = data.ToString("MM/dd/YYYY");
                #endregion
                //string SelectSQL = $"SELECT * FROM sc_alunas.tb_alunas WHERE id_alunas = '{id}' ";
                //var resposta = banco.SelecionaDados(SelectSQL);

                string insertSQL = "INSERT INTO " +
                "sc_alunas.tb_anotacao(anotacao, id_alunas, data_anotacao) " +
                "VALUES(" +
                "'" + anotacao + "', " +
                " " + id + ", '" + dataFormatada + "');";

                int respostaSql = banco.InsereDados(insertSQL);

                if (respostaSql != 0)
                {
                    ViewBag.Sucesso = true;
                }
                else
                {
                    ViewBag.Sucesso = false;
                }

                ListaNomeAluna(Convert.ToInt32(id));
                ViewData["Nome"] = _name;
                return View("Encarregado");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult FiltroNome(string aluna)
        {
            Banco banco = new Banco();

            ListaNomeAluna(Convert.ToInt32(aluna));
            ViewData["Nome"] = _name;

            _id = Convert.ToInt32(aluna);
            var scriptSQL = "SELECT * FROM sc_alunas.tb_anotacao WHERE id_alunas =" + aluna + "ORDER BY id_anotacao DESC limit 1";
            DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);
            if (RespostaSQL.Rows.Count > 0)
            {
                var anotacoes = RespostaSQL.Rows[0].Field<String>("anotacao");
                ViewData["Anotacao"] = anotacoes;
            }
            return View("Encarregado");

        }
        [HttpPost]
        public IActionResult FiltraData(int dataid)
        {
            Banco banco = new Banco();
            ViewData["Nome"] = _name;
            var scriptSQL = "SELECT * FROM sc_alunas.tb_anotacao WHERE id_anotacao =" + dataid + " AND id_alunas = "+_id+" ORDER BY id_anotacao DESC limit 1";
            DataTable RespostaSQL = banco.SelecionaDados(scriptSQL);
            if (RespostaSQL.Rows.Count > 0)
            {
                var anotacoes = RespostaSQL.Rows[0].Field<String>("anotacao");
                ViewData["Anotacao"] = anotacoes;
            }
            ListaDataAnotacao(_id);
            return View("Anotacao");
        }
        private void ListaNomeAluna(int alunaID)
        {
            List<SelectListItem> ListagemAluna = new List<SelectListItem>();
            Login dt = new Login();
            List<Login> ListagemAlunas = dt.ListaAlunas();

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
        private void ListaDataAnotacao(int alunaID)
        {
            List<SelectListItem> ListagemAluna = new List<SelectListItem>();
            Login dt = new Login();
            Banco banco = new Banco();
            List<Login> ListagemAlunas = dt.ListaDatas(alunaID);
            string sql = "SELECT DISTINCT on (data_anotacao) data_anotacao FROM sc_alunas.tb_anotacao WHERE id_alunas =" + alunaID + " ORDER BY data_anotacao DESC limit 1";
            DataTable data = banco.SelecionaDados(sql);
            if (ListagemAlunas.Count != 0)
            {
                for (int i = 0; i < ListagemAlunas.Count; i++)
                {
                    ListagemAluna.Add(new SelectListItem// dd/mm/yyyy 00:00:00
                    {
                        Value = ListagemAlunas[i].id_anotacao.ToString(),
                        Text = ListagemAlunas[i].data_anotacao.ToString(),
                    });
                    if (data.Rows[0]["data_anotacao"].ToString() == ListagemAlunas[i].data_anotacao)
                        ListagemAluna[i].Selected = true;
                }
            }

            ViewBag.Lista = ListagemAluna;

        }
        [HttpPost]
        public ActionResult GetChatGptResponse(string message)
        {
            var client = new RestClient(ChatGptBaseUrl);
            var request = new RestRequest(ChatGptBaseUrl, Method.Post);
            request.AddHeader("Authorization", $"Bearer {ChatGptApiKey}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(new { model = "text-davinci-003", prompt = message + " (escreva com no máximo 100 caracteres)", max_tokens = 100 }), ParameterType.RequestBody);

            RestResponse response = client.Execute(request);
            var content = JObject.Parse(response.Content);
            var choices = content["choices"];
            var chatGptResponse = choices.First?["text"]?.ToString();

            return Json(new { message = chatGptResponse });
        }

    }
}