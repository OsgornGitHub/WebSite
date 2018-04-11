var name = "";
var pageAlbum;
var countAlbum;


function getAlbums(name, pageAlbum, countAlbum) {
    $.ajax({
        type: "POST",
        url: "/Home/GetTopAlbum",
        data: { name: name, page: pageAlbum, count: countAlbum },
        dataType: "json",
        success: function (data) { loadAlbums(data); }
    });
}

function loadAlbums(data) {
    console.log(data)
    var container = $('div.albums');
    container.html('');
    if (data != -1) {
        for (var i = 0; i < data.length; i++) {
            var markup = `
               <a  href="http://172.19.0.251:45455/Home/GetAlbum?nameAlbum=${data[i].nameAlbum}&nameArtist=${data[i].nameArtist}">
                    <div class="col-md-2" style="height: 350px">
                        <img src="${data[i].cover}" style="width: 100%" />
                        <h4 class="text-center" style="margin-bottom: 15px">${data[i].nameAlbum}</h4>
                    </div> 
               </a>
            `;
            container.append(markup);
        }
    }
}


$(document).ready(function () {
    name = document.getElementById('name').innerText;
    console.log(name);
    var div = document.getElementById('page');
    console.log(pageAlbum);
    div.innerHTML = 1 + "";

    $('.next_a').click(function () {
        console.log(name);
        pageAlbum++;
        getAlbums(name, pageAlbum, countAlbum);
        var div = document.getElementById('page');
        div.innerHTML = pageAlbum + "";
        console.log(pageAlbum);
    })


    $('.prev_a').click(function () {
        if (pageAlbum != 1) {
            pageAlbum--;
            getAlbums(name, pageAlbum, countAlbum);
            var div = document.getElementById('page');
            div.innerHTML = pageAlbum + "";
            console.log(pageAlbum);
        }
    })

    $('.top_albums').click(function () {
        var div = document.getElementById('page');
        pageAlbum = 1;
        countAlbum = 24;
        div.innerHTML = pageAlbum + "";
        getAlbums(name, pageAlbum, countAlbum);
        console.log(name);
    })

});
