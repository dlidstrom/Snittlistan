var  items = [];
for (var row = 0; row < 1000; row++) {
    var turnNode = $x(`//*[@id="MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenRoundFormatted_${row}"]`);
    if (!turnNode) break;
    var bitsMatchIdLabelNode = $x(`//*[@id="MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LblMatchId_${row}"]`);
    if (!bitsMatchIdLabelNode.length) break;
    if (!bitsMatchIdLabelNode[0].innerText) continue;
    var dateNode = $x(`//*[@id="MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenFieldMatchDate_${row}"]`);
    var bitsMatchIdNode = $x(`//*[@id="MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenFieldMatchId_${row}"]`);
    var timeNode = $x(`//*[@id="MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HiddenFieldMatchTime_${row}"]`)
    var teamsNode = $x(`//*[@id="MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HyperLinkMatchFakta_${row}"]`);
    var matchResultNode = $x(`//*[@id="MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LabelMatchResult_${row}"]`);
    var oilPatternNameNode = $x(`//*[@id="MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_LabelMatchOilPattern_${row}"]`);
    var locationNode = $x(`//*[@id="MainContentPlaceHolder_MatchScheme1_ListViewMatchScheme_HyperLinkHall_${row}"]`);
    //
    var turn = parseInt(turnNode[0].defaultValue.replace("Omgång ", ""));
    var year = dateNode[0].value.substring(0, 4);
    var month = parseInt(dateNode[0].value.substring(5, 7));
    var day = parseInt(dateNode[0].value.substring(9, 11));
    var hour = parseInt(timeNode[0].value.substring(0, 2));
    var minute = parseInt(timeNode[0].value.substring(2, 4));
    var bitsMatchId = parseInt(bitsMatchIdNode[0].value);
    var re = /OilPatternId=(\d+)/;
    var oilPatternId = parseInt(re.exec(oilPatternNameNode[0].href)[1]);
    items.push(`new ParseMatchSchemeResult.MatchItem {\nTurn = ${turn},\nDate = new DateTime(${year}, ${month}, ${day}, ${hour}, ${minute}, 0),\nBitsMatchId = ${bitsMatchId},\nTeams = "${teamsNode[0].innerText}",\nMatchResult = "${matchResultNode[0].innerText}",\nOilPatternName = "${oilPatternNameNode[0].innerText}",\nOilPatternId = ${oilPatternId},\nLocation = "${locationNode[0].innerText}",\nLocationUrl = "${locationNode[0].href}"\n}`);
}

console.log(items.join(",\n"));
