var name = "";
var page;
var number;

function getAlbums(name, page, number) {
    $.ajax({
        type: "POST",
        url: "/Artist/GetTopAlbum",
        data: { name: name, page: page, count: number },
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
               
                <div class="col-md-2" style="height: 350px">
                    <img src="${data[i].link}" style="width: 100%" />
                    <h4 class="text-center" style="margin-bottom: 15px">${data[i].name}</h4>
                </div> 
            `;
            container.append(markup);
        }
    }
}


$(document).ready(function () {
    name = document.getElementById('name').innerText;
    console.log(name);
    var div = document.getElementById('page');
    console.log(page);
    if (!(page - 1 != 0)) {
        div.innerHTML = page + "";
    }

    $('.next_a').click(function () {
        console.log(name);
        page++;
        getAlbums(name, page, number);
        var div = document.getElementById('page');

        div.innerHTML = page + "";


        console.log(page);
    })


    $('.prev_a').click(function () {
        if (page != 1) {
            page--;
            getAlbums(name, page, number);
            var div = document.getElementById('page');
            div.innerHTML = page + "";
            console.log(page);
        }
    })

    $('.top_albums').click(function () {
        page = 1;
        number = 24;
        getAlbums(name, page, number);
        console.log(name);
    })


    $('.24a').click(function () {
        page = 1;
        number = 24;
        getAlbums(name, page, number);
    })



    $('.12a').click(function () {
        page = 1;
        number = 12;
        getAlbums(name, page, number);
    })

    $('.36a').click(function () {
        page = 1;
        number = 36;
        getAlbums(name, page, number);
    })


});
