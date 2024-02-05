// Bu kodu modal içindeki mesajý güncellemek için kullanabilirsiniz
function showSuccessMessage(message) {
    // Modal içindeki body kýsmýný bul
    var modalBody = $("#successModal .modal-body");

    // Eðer modal body bulunamazsa iþlemi sonlandýr
    if (!modalBody) {
        return;
    }

    // Modal içindeki body kýsmýnýn içeriðini güncelle
    modalBody.html(message);

    // Modal'ý göster
    $("#successModal").modal("show");
}

// Bu kodu hata mesajýný modal içinde göstermek için kullanabilirsiniz
function showErrorMessage(message) {
    // Hata mesajýný modal içinde göster
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
            console.log("Hata oluþtu: " + error);
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
                // Ürün baþarýyla sepete eklendiðinde modal'ý göster
                showSuccessMessage(result);
                InfoBasket();
            },
            error: function (error) {
                // Hata durumunda modal'ý göster
                showErrorMessage("Ürün sepete eklenirken bir hata oluþtu.");
                console.log("Hata oluþtu: " + error);
            }
        });
    });
});
