async function SearchButton(button) {
    var inputValue = button.previousElementSibling.value;

    var response = await fetch('/User/SeacrhImages', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ LikedImageUrl: inputValue })
    });
    if (response.ok) {
        var data = await response.json();
        updatePageContent(data);
    } else {
        console.error('Fetch request failed with status:', response.status);
    }
}

function updatePageContent(data) {
    var content = '<div class="divv">';

    for (var i = 0; i < data.length; i++) {
        var url = data[i];
        content += '<div class="page-image">';
        content += '<img class="img" src="' + url.Url + '">';
        content += '<button data-state="' + (url.State ? "true" : "false") + '" style="background-color:' + (url.State ? "green" : "red") + '" onclick="toggleLikeState(this,\'' + url.Url + '\')" class="btn btn-secondary my-2 my-sm-0">' + (url.State ? "Like" : "Dislike") + '</button>';
        content += '</div>';
    }

    content += '</div>';
    var resultContainer = document.getElementById('resultContainer');
    console.log(resultContainer);
    if (resultContainer) {
        resultContainer.innerHTML = content;
    }
}