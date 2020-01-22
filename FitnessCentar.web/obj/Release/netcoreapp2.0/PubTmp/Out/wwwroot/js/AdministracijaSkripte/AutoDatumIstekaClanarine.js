$(document).ready(function () {
    autoDatumIsteka();
    $('#tipClanarine').change(function () { autoDatumIsteka(); });
    $('#datumPocetka').change(function () { autoDatumIsteka(); });
    function autoDatumIsteka() {
        var tipClanarine = document.getElementById('tipClanarine').value;   
        var url = '/Ajax/TrajanjeClanarine?id=' + tipClanarine;
        $.ajax({
            url: url,
            success: function (result) {                
                var datum = $('#datumPocetka').val();            
                var date = new Date(datum);
                date.setDate(date.getDate() + result);
                var godina = date.getFullYear(),
                    mjesec = date.getMonth()+ 1,
                    dan = date.getDate();            
                if (mjesec / 10 < 1) {
                    mjesec = '0' + mjesec;
                }
                if (dan / 10 < 1) {
                    dan = '0' + dan;
                }
                var datumIsteka = godina + '-' + mjesec + '-' + dan;
                $('#datumIsteka').val(datumIsteka);
            }
        });    
    }
    
});


