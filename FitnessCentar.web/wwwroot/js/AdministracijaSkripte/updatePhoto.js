//--Konfiguracija web kamere
Webcam.set({
    width: 320,
    height: 240,
    image_format: 'jpeg',
    jpeg_quality: 90
});
Webcam.attach('#my_camera');

function take_snapshot() {
    // uslikaj  i preuzmi podatke o slici
    Webcam.snap(function (data_uri) {
        // prikazi sliku
        document.getElementById('results').innerHTML =
            '<img id="photo" src="' +
            data_uri +
            '"/>';
    });
}

function save() {
    var data_uri = document.getElementById('photo').src;
    //spremi sliku
    Webcam.upload(data_uri, '/AdministracijaClan/Capture');
    setTimeout(function () { window.location.href = ("/AdministracijaClan/PrikazClanova" ); }, 1000);
}
