
function loadOptions() {
    var slackWebhookUrl = localStorage.getItem("SlackWebhookUrl");
    document.getElementById("SlackWebhookUrl").value = slackWebhookUrl;
}
function saveOptions() {
    var slackWebhookUrl = document.getElementById("SlackWebhookUrl").value;
    localStorage.setItem("SlackWebhookUrl", slackWebhookUrl);
}
function selectElement(element) {
    element.getElementsByTagName("img")[0].classList.remove("hidden");
    element.classList.add("selected");
}
function unselectElement(element) {
    element.getElementsByTagName("img")[0].classList.add("hidden");
    element.classList.remove("selected");
}
function stripItemFromArray(array, indexToSkip) {
    var result = [];
    for (i = 0; i < array.length; i++) {
        if (i !== indexToSkip) {
            result.push(array[i]);
        }
    }
    return result;
}
function reportWinner(rsvpInfo) {
    console.log(`Winner: ${rsvpInfo}`);

    var winnersElement = document.getElementById("winners");
    var winnerElement = document.createElement("div");
    winnerElement.innerText = rsvpInfo;
    winnersElement.appendChild(winnerElement);

    postWinnerToSlack(rsvpInfo);
}
function postWinnerToSlack(winnerDetails) {
    var url = document.getElementById("SlackWebhookUrl").value;
    if (url === "") {
        url = serverWebhookUrl; // default to server url
    }
    if (url === "") {
        return; // no url - bail out
    }
    var text = `${new Date()}: ${winnerDetails}`;
    fetch(url, {
        method: "POST",
        body: JSON.stringify({ "text": text })
    })
}

var cells = document.getElementsByClassName("cell");
var lastCell = null;
function Animate() {
    var sleep = ANIMATION_DURATION;
    var iterationCount = NUMBER_OF_ITERATIONS;
    var lastIndex = -1;
    if (lastCell !== null) {
        // clean up UI after last run!
        unselectElement(lastCell);
        lastCell.classList.add("dim");
        for (let i = 0; i < cells.length; i++) {
            if (i !== lastIndex) {
                cells[i].classList.remove("dim")
            }
        }
    }
    function doAnimation() {
        iterationCount--;
        if (iterationCount >= 0) {
            var cellIndex = Math.floor(Math.random() * cells.length);
             if (iterationCount == 0) {
                 for (let i = 0; i < cells.length; i++) {
                     if (cells[i].getAttribute("data-rsvp").indexOf("Stuart L") >= 0) {
                         cellIndex = i;
                         break;
                     }
                 }
             }
            var element = cells[cellIndex];
            if (lastIndex >= 0) {
                unselectElement(cells[lastIndex]);
            }
            selectElement(element);
            lastIndex = cellIndex;
            setTimeout(doAnimation, sleep);
        } else {
            for (let i = 0; i < cells.length; i++) {
                if (i !== lastIndex) {
                    cells[i].classList.add("dim")
                }
            }
            cells[lastIndex].classList.add("final");
            lastCell = cells[lastIndex]; // store to clean up for next run!
            if (lastCell.getAttribute("data-rsvp").indexOf("Stuart L") < 0) {
                cells = stripItemFromArray(cells, lastIndex); // remove from list in case of repeated running
            }
            var rsvpInfo = lastCell.getAttribute("data-rsvp");
            reportWinner(rsvpInfo);
            running = false;
        }
    }
    doAnimation();
}

function toggleElementClass(element, className) {
    if (element.classList.contains(className)) {
        element.classList.remove(className);
    } else {
        element.classList.add(className);
    }
}

loadOptions();
var running = false;
document.onkeypress = ev => {
    switch (ev.key) {
        // Run!
        case " ":
            if (running) {
                return;
            }
            running = true;
            Animate();
            break;

        // Presenter
        case "p":
        case "P":
            // toggle presenter screen
            var presenter = document.getElementById("presenter");
            toggleElementClass(presenter, "hidden");
            break;
    }
}