const url = 'https://localhost:44306/api/';

function show(anagrams) {
    let tab =
        `<ul>
            <li>---Anagramos---</li>
        </ul>`;

    for (let r of anagrams) {
        tab += `<ul>
            <li>${r}</li>
        </ul>`;
    }

    document.getElementById("anagrams").innerHTML = tab;
}

async function getAnagrams(url, id) {
    const response = await fetch(`${url}/${id}`);
    console.log(id)
    var anagrams = await response.json();
    console.log(anagrams);
    show(anagrams);
}

getAnagrams(url);




//const controller = new APIController();
//const api = new APIfunctions();

//class APIController {
//    constructor() {
//        this.apiFunctions = new APIfunctions();
//    }

//    appendAnagramsData(data) {
//        var table = document.getElementById("anagrams-list");
//        table.innerHTML = "";
//        for (var i = 0; i < data.length; i++) {
//            var row = table.insertRow(0);
//            var cell1 = row.insertCell(0);
//            cell1.innerHTML = data[i];
//        }
//        //heading 
//        var heading = document.getElementById("anagrams-heading");
//        heading.innerHTML = "Anagrams found:";
//    }

//    getAnagrams(id) {
//        this.apiFunctions.getWordAnagrams(id)
//            .then(result => {
//                console.log("getAnagrams-controller", result);
//                this.appendAnagramsData(result);
//            });
//    }
//}

//class APIfunctions {
//    constructor() {
//        this.url = 'https://localhost:44306/api/';
//    }

//    async getWordAnagrams(id) {
//        const anagramList = await fetch(`${this.url}/${id}`)
//        console.log("getWordAnagrams", anagramList);
//        return await anagramList.json();
//    }
//}

//// Action functions
//function GetAnagrams() {
//    var id = document.getElementsByName("word").value;
//    controller.getAnagrams(id);
//    // document.getElementById('search-input').value = '';  
//}