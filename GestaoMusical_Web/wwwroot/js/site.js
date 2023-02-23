const toggleButton = document.getElementById("escondeMostra");
const text = document.getElementById("senha");

toggleButton.addEventListener("click", function () {
    if (text.type === "password") {
        text.type = "text";
    } else {
        text.type = "password";
    }
});


