function test(obj) {
    var _json = obj.replace(/&quot;/g, "\"");
    var jeste = JSON.parse(_json);
    console.log(jeste[0].Ime);
}