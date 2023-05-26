$(document).ready(function () {
    // Ao clicar no botão Enviar, envia a pergunta para o servidor
    $('#send-button').click(function () {
        var message = $('#message').val();

        if (message.trim() !== '') {
            sendMessage(message);
            $('#message').val('');
        }
    });

    // Ao pressionar Enter no campo de texto, envia a pergunta para o servidor
    $('#message').keypress(function (e) {
        if (e.which === 13) {
            var message = $(this).val();

            if (message.trim() !== '') {
                sendMessage(message);
                $(this).val('');
            }
        }
    });

    // Envia a pergunta para o servidor e exibe a resposta na interface
    function sendMessage(message) {
        $.ajax({
            type: 'POST',
            url: '/Home/GetChatGptResponse',
            data: { message: message },
            success: function (response) {
                appendMessage(message, 'Aluna: ');
                appendMessage(response.message, 'Professora Flávia: ');
            }
        });
    }


    // Adiciona a mensagem na interface
    function appendMessage(message, sender) {
        var $messageElement = $('<p></p>').text(sender + message);
        $messageElement.addClass(sender);

        $('#chat-body').append($messageElement);
        $('#chat-body').scrollTop($('#chat-body')[0].scrollHeight);
    }
});