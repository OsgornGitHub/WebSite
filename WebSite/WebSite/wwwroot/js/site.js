var pageNum = 2;
var count = 24;
var isSimilar = false;
console.log(pageNum);
function getJson(page, count) {
    $.ajax({
        type: "POST",
        url: "Home/GetJson",
        data: { page: pageNum, count: count },
        dataType: "json",
        success: function (data) { loadData(data); }
    });
}

function loadData(data) {
    console.log(data)
    var container = $('div.artist');

    if (isSimilar) {
        similar = `target="_blank"`;
    }
    else {
        similar = "";
    }

    container.html('');
    if (data != -1) {
        for (var i = 0; i < data.length; i++) {
            var markup =
                `
            <a ` + similar +  ` href="http://localhost:54638/Artist/GetArtist?name=${data[i].name}">
                <div class="col-md-2">
                    <img src="${data[i].photo}" style="width: 100%" />
                    <h4 class="text-center">${data[i].name}</h4>
                </div>
            </a>
            `;
            container.append(markup);
        }
    }
}

function getSimilar(name) {
    $.ajax({
        type: "POST",
        url: "/Artist/GetSimilar",
        data: { name: name },
        dataType: "json",
        success: function (data) { loadData(data, isSimilar); }
    });
}

$(document).ready(function () {
    var div = document.getElementById('page');
    console.log(pageNum);
    if (pageNum - 1 != 0 && div!=null) {
        div.innerHTML = (pageNum - 1) + "";
    }

    $('.next').click(function () {
        isSimilar = false;
        pageNum++;
        getJson(pageNum, count);
        var div = document.getElementById('page');
        div.innerHTML = (pageNum - 1) + "";
        console.log(pageNum);
    })


    $('.previous').click(function () {
        isSimilar = false;
        if (pageNum != 1) {
            pageNum--;
            getJson(pageNum, count);
            var div = document.getElementById('page');
            if (pageNum - 1 != 0) {
                div.innerHTML = (pageNum - 1) + "";
            }
            console.log(pageNum);
        }
    })

    $('.similar').click(function () {
        var name = document.getElementById('name').innerText;
        isSimilar = true;
        getSimilar(name)
        console.log(name);
    })


    $('.24').click(function () {
        isSimilar = false;
        pageNum = 1;
        count = 24;
        getJson(pageNum, count);
    })

    $('.36').click(function () {
        isSimilar = false;
        pageNum = 1;
        count = 36;
        getJson(pageNum, count);
    })

    $('.12').click(function () {
        isSimilar = false;
        pageNum = 1;
        count = 12;
        getJson(pageNum, count);
    })
});
