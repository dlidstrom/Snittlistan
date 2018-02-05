// Usage: phantomjs getMatches.js 'Fredrikshof IF BK' 'Fredrikshof IF BK F' my-file

"use strict";

// require
var sys = require('system');
var webpage = require('webpage');
var fs = require('fs');

if (sys.args.length != 5) {
    console.log('Usage:', sys.args[0], 'TeamGeneralName TeamSpecificName standings-file match-scheme-file')
    phantom.exit();
}

var teamGeneralName = sys.args[1];
var teamSpecificName = sys.args[2];
var standingsFile = sys.args[3];
var matchSchemeFile = sys.args[4];

console.log(teamGeneralName, teamSpecificName, standingsFile, matchSchemeFile);

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
        console.log('searching for', teamGeneralName);
        page.evaluate(function(teamGeneralName) {
            var textboxSearchClub = document.getElementById('MainContentPlaceHolder_Standings1_TextBoxSearchClub');
            textboxSearchClub.value = teamGeneralName;
        }, teamGeneralName);
        click('MainContentPlaceHolder_Standings1_ButtonSearchClub');
    },
    function() {
        console.log('looking for', teamSpecificName);
        page.evaluate(function(teamSpecificName) {
            var options = document.querySelectorAll('#MainContentPlaceHolder_Standings1_DropDownListTeams option');
            console.log('options.length', options.length);
            for (var i = 0; i < options.length; i++) {
                var option = options[i];
                console.log('option', i, option.text);
                if (option.text === teamSpecificName) {
                    console.log('found team');
                    var sel = document.getElementById('MainContentPlaceHolder_Standings1_DropDownListTeams');
                    sel.selectedIndex = i;
                    var event = document.createEvent("UIEvents");
                    event.initUIEvent("change", true, true);
                    sel.dispatchEvent(event);
                    break;
                }
            }
        }, teamSpecificName);
    },
    function() {
        console.log('writing standings.png');
        page.render('standings.png');
    },
    function() {
        console.log('writing', standingsFile);
        fs.write(standingsFile, page.content);
    },
    function() {
        console.log('clicking MatchScheme');
        click('MainContentPlaceHolder_Standings1_HyperLinkDivision');
    },
    function() {
        console.log('writing matchScheme.png');
        page.render('matchScheme.png');
    },
    function() {
        console.log('writing', matchSchemeFile);
        fs.write(matchSchemeFile, page.content);
    }
];

var interval = setInterval(function() {
    if (!loadInProgress && typeof steps[testindex] == "function") {
        console.log("step " + (testindex + 1));
        try {
            steps[testindex]();
        } catch (e) {
            console.error(e)
        }

        testindex++;
    }
    if (typeof steps[testindex] != "function") {
        console.log("test complete!");
        phantom.exit();
    }
}, 500);
