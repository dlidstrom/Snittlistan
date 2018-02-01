"use strict";

// require
var webpage = require('webpage');
var fs = require('fs');

// start
var page = webpage.create();
var loadInProgress = false;
var testindex = 0;

// external event listeners
page.onConsoleMessage = function(msg) {
    console.log(msg);
};

page.onLoadStarted = function() {
    loadInProgress = true;
    console.log("load started");
};

page.onLoadFinished = function(status) {
    loadInProgress = false;
    if (status !== 'success') {
        console.log('Unable to access network');
        phantom.exit();
    } else {
        console.log("load finished");
    }
};

function click(id) {
    page.evaluate(function(id) {
        var a = document.getElementById(id);
        var e = document.createEvent('MouseEvents');
        e.initMouseEvent('click', true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
        a.dispatchEvent(e);
    }, id);
}

var steps = [
    function() {
        console.log('opening start page');
        page.open('http://bits.swebowl.se');
    },
    function() {
        console.log('clicking Seriespelet');
        click('ButtonMatches');
    },
    function() {
        // här är man tvungen att söka efter lagets namn => konfiguration alltså
        console.log('searching for Fredrikshof IF BK');
        page.evaluate(function() {
            var textboxSearchClub = document.getElementById('MainContentPlaceHolder_Standings1_TextBoxSearchClub');
            textboxSearchClub.value = "Fredrikshof IF BK";
        });
        click('MainContentPlaceHolder_Standings1_ButtonSearchClub');
    },
    function() {
        page.evaluate(function() {
            console.log('looking for team');
            var options = document.querySelectorAll('#MainContentPlaceHolder_Standings1_DropDownListTeams option');
            console.log('options.length', options.length);
            for (var i = 0; i < options.length; i++) {
                var option = options[i];
                console.log('option', i, option.text);
                if (option.text === 'Fredrikshof IF BK A') {
                    console.log('found team');
                    var sel = document.getElementById('MainContentPlaceHolder_Standings1_DropDownListTeams');
                    sel.selectedIndex = i;
                    var event = document.createEvent("UIEvents");
                    event.initUIEvent("change", true, true);
                    sel.dispatchEvent(event);
                    break;
                }
            }
        });
    },
    function() {
        page.render('page.png');
    },
    function() {
        page.evaluate(function() {
            // time to extract contents

            // direct link to page
            var directLinkElement = document.getElementById('MainContentPlaceHolder_Standings1_LabelDirectLink');

            // table standings
            var tableStandingsRowElements = document.querySelectorAll('#MainContentPlaceHolder_Standings1_TableStandings tr');

            var standingsLink = directLinkElement ? directLinkElement.textContent : '';
            var team = {
                standingsLink: standingsLink
            };
            console.log(JSON.stringify(team, null, 2));
            return team;
        }, function(team) {
            fs.write("page.json", JSON.stringify(team, null, 2));
        });
    }
];

// page.open("http://bits.swebowl.se/", function(status) {
//     if (status === "success") {
//         page.evaluate(function() {
//             console.log($("span.version").text());
//             try {
//                 //fs.write("file.txt", "Hello World", 'a');
//                 //fs.write("file.txt", "Hello World", 'a');
//             } catch (e) {
//                 //console.log(e);
//             }
//         });
//         phantom.exit(0);
//     } else {
//         phantom.exit(1);
//     }
// });

var interval = setInterval(function() {
    if (!loadInProgress && typeof steps[testindex] == "function") {
        console.log("step " + (testindex + 1));
        steps[testindex]();
        testindex++;
    }
    if (typeof steps[testindex] != "function") {
        console.log("test complete!");
        phantom.exit();
    }
}, 500);
