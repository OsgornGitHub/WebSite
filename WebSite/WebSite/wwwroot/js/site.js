var pageNum = 1;
var count = 24;
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
    container.html('');
    if (data != -1) {
        for (var i = 0; i < data.length; i++) {
            var markup =
                `
            <a href="http://localhost:54638/Artist/GetArtist?name=" + ${data[i].name}>
                <div class="col-md-2">
                    <img src="${data[i].photo}" style="width: 100%" />
                    <h4 class="text-center">${data[i].name}</h4>
                </div>
            </a>
            `;
            container.append(markup);
        }

        $.each(data, function (index, value) {

        });
    }
}

$(document).ready(function () {
    var div = document.getElementById('page');
    console.log(pageNum);
    div.innerHTML = pageNum + "";
    $('.next').click(function () {
        pageNum++;
        getJson(pageNum, count);
        var div = document.getElementById('page');
        console.log(pageNum);
        div.innerHTML = pageNum + "";
    })


    $('.previous').click(function () {
        if (pageNum != 1) {
            pageNum--;
            getJson(pageNum, count);
            var div = document.getElementById('page');
            console.log(pageNum);
            div.innerHTML = pageNum + "";
        }
    })


    $('.24').click(function () {
        count = 24;
    })

    $('.48').click(function () {
        count = 48;
    })
});

