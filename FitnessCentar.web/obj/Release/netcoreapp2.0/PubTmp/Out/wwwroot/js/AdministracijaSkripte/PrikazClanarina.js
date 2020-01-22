$(document).ready(function () {
    getClanarine("sve", "sve");
    $("#status").change(function () { getClanarine();})
    $("#tipClanarine").change(function () { getClanarine(); })

    function getClanarine() {
        var tipClanarine = document.getElementById('tipClanarine').value;
        var status = document.getElementById('status').value;
        $.ajax({
            url: "/ajax/prikazclanarina?tipClanarine=" + tipClanarine + "&&status=" + status,
            success: function (result) {
                $("#clanarine").html(result);
            }
        });
    }
});