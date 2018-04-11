var name = "";
var pageTrack;
var countTrack;


function getTracks(name, pageTrack, countTrack) {
    $.ajax({
        type: "POST",
        url: "/Home/GetTopTracks",
        data: { name: nameArtist, page: pageTrack, count: countTrack },
        dataType: "json",
        success: function (data) { loadTracks(data); }
    });
}

function loadTracks(data) {
    console.log(data)
    var container = $('div.tracks');
    container.html('');
    if (data != -1) {
        for (var i = 0; i < data.length; i++) {
            var markup = ` 
                <div class="col-md-2">
                    <img src="${data[i].cover}" style="width: 100%" />
                    <h4 class="text-center">${data[i].name}</h4>
                    <a><h5 class="text-center" style="cursor: pointer">Download<span  class="glyphicon glyphicon-floppy-disk"></span></h5></a>
                </div>
            `;
            container.append(markup);

        }
    }
}
$(document).ready(function () {
    nameArtist = document.getElementById('name').innerText;
    console.log(name);
    pageTrack = 1;



    $('.next_a').click(function () {
        console.log(name);
        pageTrack++;
        getTracks(name, pageTrack, countTrack);
        var div = document.getElementById('page');
        div.innerHTML = pageTrack + "";
        console.log(pageTrack);
    })


    $('.prev_a').click(function () {
        if (pageTrack != 1) {
            pageTrack--;
            getTracks(name, pageTrack, countTrack);
            var div = document.getElementById('page');
            div.innerHTML = pageTrack + "";
            console.log(pageTrack);
        }
    })

    $('.top_tracks').click(function () {
        pageTrack = 1;
        countTrack = 24;
        var div = document.getElementById('page');
        div.innerHTML = pageTrack + "";
        getTracks(name, pageTrack, countTrack);
        console.log(name);
    })

});
