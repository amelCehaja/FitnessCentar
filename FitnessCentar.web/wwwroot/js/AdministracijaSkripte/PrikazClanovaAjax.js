$(document).ready(function () {
    updateClanovi();
    $('#imePrezime').on('input propertychange paste', function () { updateClanovi(); });
    $('#tipClanarine').change(function () {
        console.log('tipClanarine -> ' + $('#tipClanarine').val());
        updateClanovi();
    });
    $('#clanarinaAN').change(function () {
        console.log('clanarinaAN -> ' + $('#clanarinaAN').val());
        updateClanovi();
    });
    function updateClanovi() {
        var imePrezime = $('#imePrezime').val();
        var tipClanarine = $('#tipClanarine').val();
        var clanarinaAN = $('#clanarinaAN').val();
        var url = '/Ajax/PrikazClanova?imePrezime=' + imePrezime + "&&tipClanarine=" + tipClanarine + "&&aktivnaDN=" + clanarinaAN;
        $.ajax({
            url: url,
            success: function (result) {
                $('#tabela').html(result);
            }
        })
    }
})