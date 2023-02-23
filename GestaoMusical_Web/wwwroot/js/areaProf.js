////const { data } = require('jquery');

// Seleciona o elemento <p> com o id "nome"
const nome = document.querySelector("#nome");

// Verifica se o conteúdo do elemento <p> é igual a "Flavia"
if (nome.textContent === "Olá Flávia!") {
    // Seleciona o elemento <textarea> com o id "texto"
    const texto = document.querySelector("#anotacao");
    // Remove o atributo "readonly" do <textarea>
    texto.removeAttribute("readonly");
    // Seleciona o elemento <button> com o id "botao"
    const botao = document.querySelector("#btnSalva");
    // Mostra o botão, definindo o estilo "display" como "block"
    botao.style.display = "block";
}

setTimeout(function () {
    var mensagem = document.getElementById('mensagem');
    mensagem.classList.add('d-none');
}, 5000);