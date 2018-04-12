var pageNum = 1;
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
            <a ` + similar + ` href="http://172.19.0.251:45457/Home/GetArtist?name=${data[i].name}">
                <div class="col-md-2">
                    <img src="${data[i].photo}" style="width: 100%" />
                    <h4 class="text-center">${data[i].name}</h4>
                </div>
            </a>
            `;
            container.append(markup);
        }
    }
    window.history.replaceState("http://172.19.0.251:45457/", "Index", "http://172.19.0.251:45457/" + pageNum);
}

function getSimilar(name) {
    $.ajax({
        type: "POST",
        url: "/Home/GetSimilar",
        data: { name: name },
        dataType: "json",
        success: function (data) { loadData(data, isSimilar); }
    });
}

$(document).ready(function () {
    var div = document.getElementById('page');
    console.log(pageNum);
    div.innerHTML = pageNum + "";


    $('.next').click(function () {
        isSimilar = false;
        pageNum++;
        getJson(pageNum, count);
        var div = document.getElementById('page');
        div.innerHTML = pageNum + "";
        console.log(pageNum);
    })


    $('.previous').click(function () {
        isSimilar = false;
        if (pageNum != 1) {
            pageNum--;
            getJson(pageNum, count);
            var div = document.getElementById('page');
            if (pageNum - 1 != 0) {
                div.innerHTML = pageNum + "";
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


    $('.12').click(function () {
        isSimilar = false;
        pageNum = 1;
        count = 12;
        getJson(pageNum, count);
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

    $('.sim').click(function () {
        $('.nava').addClass('visible');
    })

    $('.alb').click(function () {
        $('.nava').addClass('visible');
    })

    $('.tr').click(function () {
        $('.nava').addClass('visible');
    })
});
