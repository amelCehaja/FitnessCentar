$(document).ready(function () {
    IzmjenaPodkategorija();
    $('#kategorija').change(function () { IzmjenaPodkategorija(); });
    function IzmjenaPodkategorija() {
        var kategorijaId = $('#kategorija').val();
        var _url = '/Ajax/PodkategorijeList?KategorijaID=' + kategorijaId;
        $.ajax({
            url : _url,
            success: function (result) {
                $('#podkategorije').html(result);
            }
        });
    }
});