function Search() {
    var x = document.getElementById("search");
    document.getElementById("wordList").style.display = "none";
    document.getElementById("about").style.display = "none";
    if (x.style.display === "flex") {
        x.style.display = "none";
    } else {
        x.style.display = "flex";
    }
}

function WordList() {
    var x = document.getElementById("wordList");
    document.getElementById("search").style.display = "none";
    document.getElementById("about").style.display = "none";
    if (x.style.display === "flex") {
        x.style.display = "none";
    } else {
        x.style.display = "flex";
    }
}

function AddForm() {
    var x = document.getElementById("add-form");
    document.getElementById("update-form").style.display = "none";
    document.getElementById("remove-form").style.display = "none";
    if (x.style.display === "flex") {
        x.style.display = "none";
    } else {
        x.style.display = "flex";
    }
}

function UpdateForm() {
    var x = document.getElementById("update-form");
    document.getElementById("add-form").style.display = "none";
    document.getElementById("remove-form").style.display = "none";
    if (x.style.display === "flex") {
        x.style.display = "none";
    } else {
        x.style.display = "flex";
    }
}

function RemoveForm() {
    var x = document.getElementById("remove-form");
    document.getElementById("add-form").style.display = "none";
    document.getElementById("update-form").style.display = "none";
    if (x.style.display === "flex") {
        x.style.display = "none";
    } else {
        x.style.display = "flex";
    }
}

function RemovalConfirmation() {
    confirm("Are you sure you want to remove this word?");
}


function About() {
    var x = document.getElementById("about");
    document.getElementById("wordList").style.display = "none";
    document.getElementById("search").style.display = "none";
    if (x.style.display === "flex") {
        x.style.display = "none";
    } else {
        x.style.display = "flex";
    }
}
