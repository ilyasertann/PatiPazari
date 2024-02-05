// Bu kodu modal i�indeki mesaj� g�ncellemek i�in kullanabilirsiniz
function showSuccessMessage(message) {
    // Modal i�indeki body k�sm�n� bul
    var modalBody = $("#successModal .modal-body");

    // E�er modal body bulunamazsa i�lemi sonland�r
    if (!modalBody) {
        return;
    }

    // Modal i�indeki body k�sm�n�n i�eri�ini g�ncelle
    modalBody.html(message);

    // Modal'� g�ster
    $("#successModal").modal("show");
}

// Bu kodu hata mesaj�n� modal i�inde g�stermek i�in kullanabilirsiniz
function showErrorMessage(message) {
    // Hata mesaj�n� modal i�inde g�ster
    $("#errorModal .modal-body").html(message);
    $("#errorModal").modal("show");
}
function InfoBasket() {
    $.ajax({
        type: "Post",
        url: "/Home/GetInfoBasket",
        success: function (result) {
            $(".cart-count").html(result);
        },
        error: function (error) {
            console.log("Hata olu�tu: " + error);
        }
    });
};
$(document).ready(function () {

    $(document).on("click", ".add-basket", function (e) {
        e.preventDefault();
        var formData = $(this).closest("form").serialize();

        $.ajax({
            type: "GET",
            url: $(this).closest("form").attr("action"),
            data: formData,
            success: function (result) {
                // �r�n ba�ar�yla sepete eklendi�inde modal'� g�ster
                showSuccessMessage(result);
                InfoBasket();
            },
            error: function (error) {
                // Hata durumunda modal'� g�ster
                showErrorMessage("�r�n sepete eklenirken bir hata olu�tu.");
                console.log("Hata olu�tu: " + error);
            }
        });
    });
});
