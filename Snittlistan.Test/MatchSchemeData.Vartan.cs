using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Test;
public partial class MatchSchemeData
{
    public static TestCaseData Vartan = new TestCaseData(
        "Snittlistan.Test.BitsResult.VärtansIK-matchScheme.html",
        new[]
        {
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 1,
                    Date = new DateTime(2017, 9, 3, 10, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3151972,
                    Teams = "Bålsta BC - BK Glam F1",
                    MatchResult = "12 - 8",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=1"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 9, 10, 0, 0),
                    BitsMatchId = 3151971,
                    Teams = "Värtans IK - BK Glam F1",
                    MatchResult = "6 - 14",
                    OilPatternName = "Abbey road",
                    OilPatternId = 34,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 9, 10, 40, 0),
                    BitsMatchId = 3151979,
                    Teams = "BK Trol - BK Rättvik",
                    MatchResult = "14 - 6",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 9, 11, 0, 0),
                    BitsMatchId = 3151976,
                    Teams = "Domnarvets BS - Sollentuna BwK",
                    MatchResult = "13 - 7",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 9, 13, 40, 0),
                    BitsMatchId = 3151973,
                    Teams = "BK Klossen - Turebergs IF",
                    MatchResult = "17 - 3",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 9, 13, 40, 0),
                    BitsMatchId = 3151977,
                    Teams = "Wåxnäs BC - Gävle KK",
                    MatchResult = "8 - 12",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 9, 14, 20, 0),
                    BitsMatchId = 3151975,
                    Teams = "Falu BK - Sollentuna BwK",
                    MatchResult = "7 - 13",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 9, 15, 20, 0),
                    BitsMatchId = 3151978,
                    Teams = "BK Taifun - Gävle KK",
                    MatchResult = "15 - 5",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 9, 16, 0, 0),
                    BitsMatchId = 3151980,
                    Teams = "Hammarby IF - BK Rättvik",
                    MatchResult = "12 - 8",
                    OilPatternName = "Elitserien 39 2016",
                    OilPatternId = 96,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 10, 10, 0, 0),
                    BitsMatchId = 3151969,
                    Teams = "Sandvikens AIK - BK Kasi",
                    MatchResult = "10 - 10",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 10, 11, 0, 0),
                    BitsMatchId = 3151974,
                    Teams = "BK Enjoy - Turebergs IF",
                    MatchResult = "7 - 13",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 10, 15, 0, 0),
                    BitsMatchId = 3151970,
                    Teams = "Uppsala BC 90 - BK Kasi",
                    MatchResult = "8 - 12",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 16, 10, 0, 0),
                    BitsMatchId = 3151981,
                    Teams = "Värtans IK - Sandvikens AIK",
                    MatchResult = "5 - 15",
                    OilPatternName = "Abbey road",
                    OilPatternId = 34,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 16, 10, 0, 0),
                    BitsMatchId = 3151983,
                    Teams = "Turebergs IF - Uppsala BC 90",
                    MatchResult = "10 - 9",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 16, 10, 0, 0),
                    BitsMatchId = 3151988,
                    Teams = "Domnarvets BS - BK Klossen",
                    MatchResult = "13 - 6",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 16, 10, 0, 0),
                    BitsMatchId = 3151992,
                    Teams = "BK Glam F1 - Hammarby IF",
                    MatchResult = "6 - 14",
                    OilPatternName = "Elitserien 37 2016",
                    OilPatternId = 95,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 16, 11, 40, 0),
                    BitsMatchId = 3151984,
                    Teams = "Sollentuna BwK - Uppsala BC 90",
                    MatchResult = "4 - 16",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 16, 13, 40, 0),
                    BitsMatchId = 3151987,
                    Teams = "Falu BK - BK Klossen",
                    MatchResult = "3 - 17",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 16, 13, 40, 0),
                    BitsMatchId = 3151991,
                    Teams = "BK Kasi - Hammarby IF",
                    MatchResult = "7 - 11",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 16, 14, 0, 0),
                    BitsMatchId = 3151982,
                    Teams = "Bålsta BC - Sandvikens AIK",
                    MatchResult = "5 - 15",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 17, 11, 0, 0),
                    BitsMatchId = 3151986,
                    Teams = "Gävle KK - BK Enjoy",
                    MatchResult = "18 - 2",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 17, 16, 0, 0),
                    BitsMatchId = 3151985,
                    Teams = "BK Rättvik - BK Enjoy",
                    MatchResult = "16 - 4",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 10, 0, 0),
                    BitsMatchId = 3151994,
                    Teams = "Uppsala BC 90 - BK Taifun",
                    MatchResult = "8 - 11",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 10, 0, 0),
                    BitsMatchId = 3151998,
                    Teams = "Gävle KK - Värtans IK",
                    MatchResult = "14 - 6",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 10, 20, 0),
                    BitsMatchId = 3152004,
                    Teams = "Hammarby IF - Falu BK",
                    MatchResult = "16 - 4",
                    OilPatternName = "Elitserien 39 2016",
                    OilPatternId = 96,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 11, 40, 0),
                    BitsMatchId = 3152001,
                    Teams = "BK Kasi - Domnarvets BS",
                    MatchResult = "10 - 10",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 12, 0, 0),
                    BitsMatchId = 3151995,
                    Teams = "Turebergs IF - Wåxnäs BC",
                    MatchResult = "14 - 6",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 13, 40, 0),
                    BitsMatchId = 3151996,
                    Teams = "Sollentuna BwK - Wåxnäs BC",
                    MatchResult = "7 - 12",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 13, 40, 0),
                    BitsMatchId = 3151999,
                    Teams = "BK Klossen - Bålsta BC",
                    MatchResult = "16 - 4",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 14, 0, 0),
                    BitsMatchId = 3152003,
                    Teams = "BK Trol - Falu BK",
                    MatchResult = "15 - 5",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 14, 40, 0),
                    BitsMatchId = 3151997,
                    Teams = "BK Rättvik - Värtans IK",
                    MatchResult = "8 - 12",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 15, 0, 0),
                    BitsMatchId = 3151993,
                    Teams = "Sandvikens AIK - BK Taifun",
                    MatchResult = "10 - 10",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 23, 15, 20, 0),
                    BitsMatchId = 3152002,
                    Teams = "BK Glam F1 - Domnarvets BS",
                    MatchResult = "7 - 13",
                    OilPatternName = "Elitserien 37 2016",
                    OilPatternId = 95,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 24, 10, 0, 0),
                    BitsMatchId = 3152000,
                    Teams = "BK Enjoy - Bålsta BC",
                    MatchResult = "12 - 8",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 14, 10, 0, 0),
                    BitsMatchId = 3152011,
                    Teams = "Falu BK - BK Rättvik",
                    MatchResult = "9 - 11",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 14, 12, 20, 0),
                    BitsMatchId = 3152008,
                    Teams = "Sollentuna BwK - Sandvikens AIK",
                    MatchResult = "8 - 12",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 14, 12, 40, 0),
                    BitsMatchId = 3152015,
                    Teams = "BK Trol - BK Glam F1",
                    MatchResult = "15 - 5",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 14, 13, 40, 0),
                    BitsMatchId = 3152009,
                    Teams = "BK Klossen - Gävle KK",
                    MatchResult = "12 - 8",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 14, 13, 40, 0),
                    BitsMatchId = 3152012,
                    Teams = "Domnarvets BS - BK Rättvik",
                    MatchResult = "11 - 9",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 14, 13, 40, 0),
                    BitsMatchId = 3152013,
                    Teams = "Wåxnäs BC - BK Kasi",
                    MatchResult = "13 - 7",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 14, 14, 0, 0),
                    BitsMatchId = 3152007,
                    Teams = "Turebergs IF - Sandvikens AIK",
                    MatchResult = "5 - 15",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 14, 15, 20, 0),
                    BitsMatchId = 3152014,
                    Teams = "BK Taifun - BK Kasi",
                    MatchResult = "18 - 2",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 14, 16, 20, 0),
                    BitsMatchId = 3152016,
                    Teams = "Hammarby IF - BK Glam F1",
                    MatchResult = "6 - 13",
                    OilPatternName = "Elitserien 39 2016",
                    OilPatternId = 96,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 15, 10, 0, 0),
                    BitsMatchId = 3152006,
                    Teams = "Bålsta BC - Uppsala BC 90",
                    MatchResult = "9 - 11",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 15, 11, 0, 0),
                    BitsMatchId = 3152010,
                    Teams = "BK Enjoy - Gävle KK",
                    MatchResult = "10 - 10",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 15, 15, 20, 0),
                    BitsMatchId = 3152005,
                    Teams = "Värtans IK - Uppsala BC 90",
                    MatchResult = "8 - 12",
                    OilPatternName = "Big Ben",
                    OilPatternId = 147,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 21, 10, 0, 0),
                    BitsMatchId = 3152017,
                    Teams = "Sandvikens AIK - Wåxnäs BC",
                    MatchResult = "10 - 10",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 21, 10, 0, 0),
                    BitsMatchId = 3152019,
                    Teams = "Värtans IK - BK Taifun",
                    MatchResult = "13 - 7",
                    OilPatternName = "Big Ben",
                    OilPatternId = 147,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 21, 11, 0, 0),
                    BitsMatchId = 3152024,
                    Teams = "Domnarvets BS - Turebergs IF",
                    MatchResult = "13 - 7",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 21, 11, 40, 0),
                    BitsMatchId = 3152021,
                    Teams = "BK Rättvik - Sollentuna BwK",
                    MatchResult = "13 - 7",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 21, 14, 0, 0),
                    BitsMatchId = 3152020,
                    Teams = "Bålsta BC - BK Taifun",
                    MatchResult = "9 - 11",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 21, 14, 40, 0),
                    BitsMatchId = 3152023,
                    Teams = "Falu BK - Turebergs IF",
                    MatchResult = "3 - 17",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 21, 15, 0, 0),
                    BitsMatchId = 3152018,
                    Teams = "Uppsala BC 90 - Wåxnäs BC",
                    MatchResult = "12 - 8",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 21, 16, 20, 0),
                    BitsMatchId = 3152022,
                    Teams = "Gävle KK - Sollentuna BwK",
                    MatchResult = "10 - 9",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 22, 11, 0, 0),
                    BitsMatchId = 3152025,
                    Teams = "BK Kasi - BK Klossen",
                    MatchResult = "11 - 9",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 22, 12, 0, 0),
                    BitsMatchId = 3152028,
                    Teams = "Hammarby IF - BK Enjoy",
                    MatchResult = "14 - 6",
                    OilPatternName = "Elitserien 39 2016",
                    OilPatternId = 96,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 22, 14, 40, 0),
                    BitsMatchId = 3152026,
                    Teams = "BK Glam F1 - BK Klossen",
                    MatchResult = "13 - 6",
                    OilPatternName = "Elitserien 37 2016",
                    OilPatternId = 95,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 22, 15, 0, 0),
                    BitsMatchId = 3152027,
                    Teams = "BK Trol - BK Enjoy",
                    MatchResult = "17 - 3",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 25, 19, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3152035,
                    Teams = "Wåxnäs BC - BK Taifun",
                    MatchResult = "10 - 10",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 28, 10, 0, 0),
                    BitsMatchId = 3152030,
                    Teams = "Värtans IK - Bålsta BC",
                    MatchResult = "8 - 11",
                    OilPatternName = "Big Ben",
                    OilPatternId = 147,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 28, 10, 0, 0),
                    BitsMatchId = 3152031,
                    Teams = "Turebergs IF - Sollentuna BwK",
                    MatchResult = "10 - 9",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 28, 10, 0, 0),
                    BitsMatchId = 3152034,
                    Teams = "Falu BK - Domnarvets BS",
                    MatchResult = "4 - 16",
                    OilPatternName = "36 Fot",
                    OilPatternId = 43,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 28, 10, 0, 0),
                    BitsMatchId = 3152037,
                    Teams = "BK Trol - Hammarby IF",
                    MatchResult = "12 - 8",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 28, 11, 40, 0),
                    BitsMatchId = 3152033,
                    Teams = "BK Klossen - BK Enjoy",
                    MatchResult = "15 - 5",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 28, 11, 40, 0),
                    BitsMatchId = 3152036,
                    Teams = "BK Kasi - BK Glam F1",
                    MatchResult = "10 - 10",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 28, 12, 0, 0),
                    BitsMatchId = 3152032,
                    Teams = "BK Rättvik - Gävle KK",
                    MatchResult = "6 - 14",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 28, 13, 40, 0),
                    BitsMatchId = 3152029,
                    Teams = "Sandvikens AIK - Uppsala BC 90",
                    MatchResult = "12 - 8",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 29, 14, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3151990,
                    Teams = "BK Taifun - BK Trol",
                    MatchResult = "11 - 9",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 29, 15, 40, 0),
                    DateChanged = true,
                    BitsMatchId = 3151989,
                    Teams = "Wåxnäs BC - BK Trol",
                    MatchResult = "12 - 8",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 18, 10, 0, 0),
                    BitsMatchId = 3152039,
                    Teams = "Uppsala BC 90 - BK Trol",
                    MatchResult = "14 - 4",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 18, 10, 0, 0),
                    BitsMatchId = 3152041,
                    Teams = "Bålsta BC - Hammarby IF",
                    MatchResult = "7 - 13",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 18, 11, 0, 0),
                    BitsMatchId = 3152043,
                    Teams = "Gävle KK - Turebergs IF",
                    MatchResult = "16 - 4",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 18, 12, 0, 0),
                    BitsMatchId = 3152047,
                    Teams = "BK Taifun - Domnarvets BS",
                    MatchResult = "15 - 5",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 18, 13, 40, 0),
                    BitsMatchId = 3152044,
                    Teams = "BK Klossen - Sollentuna BwK",
                    MatchResult = "8 - 12",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 18, 13, 40, 0),
                    BitsMatchId = 3152046,
                    Teams = "Wåxnäs BC - Domnarvets BS",
                    MatchResult = "8 - 11",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 18, 15, 20, 0),
                    BitsMatchId = 3152038,
                    Teams = "Sandvikens AIK - BK Trol",
                    MatchResult = "13 - 7",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 18, 16, 0, 0),
                    BitsMatchId = 3152042,
                    Teams = "BK Rättvik - Turebergs IF",
                    MatchResult = "16 - 4",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 19, 10, 0, 0),
                    BitsMatchId = 3152045,
                    Teams = "BK Enjoy - Sollentuna BwK",
                    MatchResult = "9 - 11",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 19, 10, 40, 0),
                    BitsMatchId = 3152040,
                    Teams = "Värtans IK - Hammarby IF",
                    MatchResult = "10 - 10",
                    OilPatternName = "Big Ben",
                    OilPatternId = 147,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 10, 0, 0),
                    BitsMatchId = 3152051,
                    Teams = "Uppsala BC 90 - Bålsta BC",
                    MatchResult = "16 - 4",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 10, 0, 0),
                    BitsMatchId = 3152052,
                    Teams = "Turebergs IF - Värtans IK",
                    MatchResult = "10 - 10",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 10, 0, 0),
                    BitsMatchId = 3152055,
                    Teams = "Gävle KK - Falu BK",
                    MatchResult = "16 - 4",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 10, 0, 0),
                    BitsMatchId = 3152057,
                    Teams = "BK Enjoy - Domnarvets BS",
                    MatchResult = "13 - 7",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 10, 0, 0),
                    BitsMatchId = 3152059,
                    Teams = "BK Glam F1 - Wåxnäs BC",
                    MatchResult = "17 - 3",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 10, 0, 0),
                    BitsMatchId = 3152060,
                    Teams = "BK Trol - BK Taifun",
                    MatchResult = "11 - 9",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 11, 40, 0),
                    BitsMatchId = 3152053,
                    Teams = "Sollentuna BwK - Värtans IK",
                    MatchResult = "9 - 11",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 13, 40, 0),
                    BitsMatchId = 3152058,
                    Teams = "BK Kasi - Wåxnäs BC",
                    MatchResult = "9 - 11",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 14, 0, 0),
                    BitsMatchId = 3152061,
                    Teams = "Hammarby IF - BK Taifun",
                    MatchResult = "8 - 12",
                    OilPatternName = "Elitserien 39 2016",
                    OilPatternId = 96,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 14, 40, 0),
                    BitsMatchId = 3152054,
                    Teams = "BK Rättvik - Falu BK",
                    MatchResult = "16 - 4",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 14, 40, 0),
                    BitsMatchId = 3152056,
                    Teams = "BK Klossen - Domnarvets BS",
                    MatchResult = "14 - 6",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 15, 40, 0),
                    BitsMatchId = 3152050,
                    Teams = "Sandvikens AIK - Bålsta BC",
                    MatchResult = "16 - 4",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 10, 0, 0),
                    BitsMatchId = 3152062,
                    Teams = "Sandvikens AIK - BK Glam F1",
                    MatchResult = "17 - 3",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 11, 0, 0),
                    BitsMatchId = 3152066,
                    Teams = "BK Rättvik - Bålsta BC",
                    MatchResult = "8 - 12",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 11, 0, 0),
                    BitsMatchId = 3152068,
                    Teams = "Falu BK - Värtans IK",
                    MatchResult = "9 - 11",
                    OilPatternName = "Phase 1",
                    OilPatternId = 39,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 12, 20, 0),
                    BitsMatchId = 3152065,
                    Teams = "Sollentuna BwK - BK Kasi",
                    MatchResult = "8 - 12",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 14, 0, 0),
                    BitsMatchId = 3152064,
                    Teams = "Turebergs IF - BK Kasi",
                    MatchResult = "12 - 8",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 14, 40, 0),
                    BitsMatchId = 3152069,
                    Teams = "Domnarvets BS - Värtans IK",
                    MatchResult = "12 - 8",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 15, 40, 0),
                    BitsMatchId = 3152063,
                    Teams = "Uppsala BC 90 - BK Glam F1",
                    MatchResult = "9 - 11",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 15, 40, 0),
                    BitsMatchId = 3152067,
                    Teams = "Gävle KK - Bålsta BC",
                    MatchResult = "17 - 3",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 3, 10, 0, 0),
                    BitsMatchId = 3152072,
                    Teams = "BK Trol - BK Klossen",
                    MatchResult = "17 - 3",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 3, 12, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3152070,
                    Teams = "Wåxnäs BC - BK Enjoy",
                    MatchResult = "14 - 6",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 3, 13, 20, 0),
                    BitsMatchId = 3152073,
                    Teams = "Hammarby IF - BK Klossen",
                    MatchResult = "13 - 7",
                    OilPatternName = "Elitserien 39 2016",
                    OilPatternId = 96,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 3, 13, 40, 0),
                    DateChanged = true,
                    BitsMatchId = 3152071,
                    Teams = "BK Taifun - BK Enjoy",
                    MatchResult = "15 - 4",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 10, 0, 0),
                    BitsMatchId = 3152078,
                    Teams = "BK Klossen - Sandvikens AIK",
                    MatchResult = "11 - 8",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 10, 0, 0),
                    BitsMatchId = 3152084,
                    Teams = "BK Trol - Gävle KK",
                    MatchResult = "13 - 7",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 11, 0, 0),
                    BitsMatchId = 3152081,
                    Teams = "Domnarvets BS - Uppsala BC 90",
                    MatchResult = "9 - 11",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 11, 40, 0),
                    BitsMatchId = 3152082,
                    Teams = "BK Kasi - BK Rättvik",
                    MatchResult = "6 - 14",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 12, 0, 0),
                    BitsMatchId = 3152077,
                    Teams = "Sollentuna BwK - BK Taifun",
                    MatchResult = "17 - 3",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 13, 40, 0),
                    BitsMatchId = 3152076,
                    Teams = "Turebergs IF - BK Taifun",
                    MatchResult = "7 - 13",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 13, 40, 0),
                    BitsMatchId = 3152085,
                    Teams = "Hammarby IF - Gävle KK",
                    MatchResult = "10 - 10",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 14, 40, 0),
                    BitsMatchId = 3152080,
                    Teams = "Falu BK - Uppsala BC 90",
                    MatchResult = "4 - 16",
                    OilPatternName = "Phase 1",
                    OilPatternId = 39,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 15, 0, 0),
                    BitsMatchId = 3152079,
                    Teams = "BK Enjoy - Sandvikens AIK",
                    MatchResult = "6 - 14",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 13, 15, 20, 0),
                    BitsMatchId = 3152083,
                    Teams = "BK Glam F1 - BK Rättvik",
                    MatchResult = "15 - 5",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 14, 10, 0, 0),
                    BitsMatchId = 3152074,
                    Teams = "Värtans IK - Wåxnäs BC",
                    MatchResult = "12 - 8",
                    OilPatternName = "Big Ben",
                    OilPatternId = 147,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 14, 14, 0, 0),
                    BitsMatchId = 3152075,
                    Teams = "Bålsta BC - Wåxnäs BC",
                    MatchResult = "9 - 11",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 10, 0, 0),
                    BitsMatchId = 3152087,
                    Teams = "Uppsala BC 90 - Sollentuna BwK",
                    MatchResult = "15 - 5",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 10, 0, 0),
                    BitsMatchId = 3152089,
                    Teams = "Bålsta BC - Turebergs IF",
                    MatchResult = "15 - 5",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 10, 0, 0),
                    BitsMatchId = 3152090,
                    Teams = "BK Rättvik - Domnarvets BS",
                    MatchResult = "15 - 5",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 10, 0, 0),
                    BitsMatchId = 3152097,
                    Teams = "BK Glam F1 - BK Trol",
                    MatchResult = "12 - 8",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 13, 40, 0),
                    BitsMatchId = 3152095,
                    Teams = "BK Taifun - Hammarby IF",
                    MatchResult = "13 - 7",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 13, 40, 0),
                    BitsMatchId = 3152096,
                    Teams = "BK Kasi - BK Trol",
                    MatchResult = "13 - 7",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 15, 20, 0),
                    BitsMatchId = 3152086,
                    Teams = "Sandvikens AIK - Sollentuna BwK",
                    MatchResult = "12 - 8",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 15, 20, 0),
                    BitsMatchId = 3152094,
                    Teams = "Wåxnäs BC - Hammarby IF",
                    MatchResult = "12 - 8",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 15, 40, 0),
                    BitsMatchId = 3152091,
                    Teams = "Gävle KK - Domnarvets BS",
                    MatchResult = "18 - 2",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 15, 40, 0),
                    BitsMatchId = 3152092,
                    Teams = "BK Klossen - Falu BK",
                    MatchResult = "19 - 1",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 21, 10, 0, 0),
                    BitsMatchId = 3152088,
                    Teams = "Värtans IK - Turebergs IF",
                    MatchResult = "6 - 14",
                    OilPatternName = "Big Ben",
                    OilPatternId = 147,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 21, 10, 0, 0),
                    BitsMatchId = 3152093,
                    Teams = "BK Enjoy - Falu BK",
                    MatchResult = "17 - 3",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 10, 11, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3152104,
                    Teams = "BK Taifun - Wåxnäs BC",
                    MatchResult = "6 - 14",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 10, 11, 40, 0),
                    BitsMatchId = 3152098,
                    Teams = "Uppsala BC 90 - Sandvikens AIK",
                    MatchResult = "13 - 7",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 10, 11, 40, 0),
                    BitsMatchId = 3152100,
                    Teams = "Sollentuna BwK - Turebergs IF",
                    MatchResult = "9 - 11",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 10, 11, 40, 0),
                    BitsMatchId = 3152101,
                    Teams = "Gävle KK - BK Rättvik",
                    MatchResult = "11 - 9",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 10, 11, 40, 0),
                    BitsMatchId = 3152103,
                    Teams = "Domnarvets BS - Falu BK",
                    MatchResult = "14 - 6",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 10, 11, 40, 0),
                    BitsMatchId = 3152105,
                    Teams = "BK Glam F1 - BK Kasi",
                    MatchResult = "12 - 8",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 10, 12, 0, 0),
                    BitsMatchId = 3152102,
                    Teams = "BK Enjoy - BK Klossen",
                    MatchResult = "10 - 10",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 10, 12, 20, 0),
                    BitsMatchId = 3152106,
                    Teams = "Hammarby IF - BK Trol",
                    MatchResult = "12 - 8",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 10, 13, 0, 0),
                    BitsMatchId = 3152099,
                    Teams = "Bålsta BC - Värtans IK",
                    MatchResult = "17 - 3",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 9, 20, 0),
                    BitsMatchId = 3152109,
                    Teams = "Turebergs IF - Hammarby IF",
                    MatchResult = "11 - 9",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 10, 0, 0),
                    BitsMatchId = 3152107,
                    Teams = "Värtans IK - BK Trol",
                    MatchResult = "7 - 13",
                    OilPatternName = "Big Ben",
                    OilPatternId = 147,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 10, 0, 0),
                    BitsMatchId = 3152112,
                    Teams = "Gävle KK - Uppsala BC 90",
                    MatchResult = "15 - 5",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 10, 0, 0),
                    BitsMatchId = 3152113,
                    Teams = "Falu BK - Sandvikens AIK",
                    MatchResult = "7 - 13",
                    OilPatternName = "Phase 1",
                    OilPatternId = 39,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 11, 0, 0),
                    BitsMatchId = 3152110,
                    Teams = "Sollentuna BwK - Hammarby IF",
                    MatchResult = "9 - 11",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 13, 40, 0),
                    BitsMatchId = 3152114,
                    Teams = "Domnarvets BS - Sandvikens AIK",
                    MatchResult = "14 - 6",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 13, 40, 0),
                    BitsMatchId = 3152115,
                    Teams = "Wåxnäs BC - BK Klossen",
                    MatchResult = "12 - 8",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 15, 20, 0),
                    BitsMatchId = 3152116,
                    Teams = "BK Taifun - BK Klossen",
                    MatchResult = "6 - 14",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 16, 0, 0),
                    BitsMatchId = 3152111,
                    Teams = "BK Rättvik - Uppsala BC 90",
                    MatchResult = "16 - 4",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 17, 16, 0, 0),
                    BitsMatchId = 3152118,
                    Teams = "BK Glam F1 - BK Enjoy",
                    MatchResult = "14 - 6",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 18, 10, 0, 0),
                    BitsMatchId = 3152108,
                    Teams = "Bålsta BC - BK Trol",
                    MatchResult = "9 - 11",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 18, 11, 0, 0),
                    BitsMatchId = 3152117,
                    Teams = "BK Kasi - BK Enjoy",
                    MatchResult = "16 - 4",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 17,
                    Date = new DateTime(2018, 2, 24, 11, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3152049,
                    Teams = "BK Glam F1 - Falu BK",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=17"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 17,
                    Date = new DateTime(2018, 2, 24, 15, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3152048,
                    Teams = "BK Kasi - Falu BK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=17"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 10, 0, 0),
                    BitsMatchId = 3152119,
                    Teams = "Sandvikens AIK - Turebergs IF",
                    MatchResult = "0 - 0",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 10, 0, 0),
                    BitsMatchId = 3152122,
                    Teams = "Bålsta BC - Sollentuna BwK",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 10, 0, 0),
                    BitsMatchId = 3152126,
                    Teams = "Domnarvets BS - Gävle KK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 10, 0, 0),
                    BitsMatchId = 3152128,
                    Teams = "BK Taifun - BK Glam F1",
                    MatchResult = "0 - 0",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 11, 40, 0),
                    BitsMatchId = 3152127,
                    Teams = "Wåxnäs BC - BK Glam F1",
                    MatchResult = "0 - 0",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 12, 0, 0),
                    BitsMatchId = 3152123,
                    Teams = "BK Klossen - BK Rättvik",
                    MatchResult = "0 - 0",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 12, 0, 0),
                    BitsMatchId = 3152129,
                    Teams = "BK Trol - BK Kasi",
                    MatchResult = "0 - 0",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 13, 40, 0),
                    BitsMatchId = 3152125,
                    Teams = "Falu BK - Gävle KK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Phase 1",
                    OilPatternId = 39,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 15, 20, 0),
                    BitsMatchId = 3152120,
                    Teams = "Uppsala BC 90 - Turebergs IF",
                    MatchResult = "0 - 0",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 16, 20, 0),
                    BitsMatchId = 3152130,
                    Teams = "Hammarby IF - BK Kasi",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 11, 10, 0, 0),
                    BitsMatchId = 3152121,
                    Teams = "Värtans IK - Sollentuna BwK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Big Ben",
                    OilPatternId = 147,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 11, 11, 0, 0),
                    BitsMatchId = 3152124,
                    Teams = "BK Enjoy - BK Rättvik",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 10, 0, 0),
                    BitsMatchId = 3152132,
                    Teams = "Uppsala BC 90 - Hammarby IF",
                    MatchResult = "0 - 0",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 10, 0, 0),
                    BitsMatchId = 3152136,
                    Teams = "BK Enjoy - Värtans IK",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 10, 20, 0),
                    BitsMatchId = 3152133,
                    Teams = "Turebergs IF - BK Trol",
                    MatchResult = "0 - 0",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 11, 0, 0),
                    BitsMatchId = 3152137,
                    Teams = "Falu BK - Bålsta BC",
                    MatchResult = "0 - 0",
                    OilPatternName = "Phase 1",
                    OilPatternId = 39,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 12, 0, 0),
                    BitsMatchId = 3152134,
                    Teams = "Sollentuna BwK - BK Trol",
                    MatchResult = "0 - 0",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 13, 40, 0),
                    BitsMatchId = 3152139,
                    Teams = "Wåxnäs BC - BK Rättvik",
                    MatchResult = "0 - 0",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 14, 20, 0),
                    BitsMatchId = 3152138,
                    Teams = "Domnarvets BS - Bålsta BC",
                    MatchResult = "0 - 0",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 14, 40, 0),
                    BitsMatchId = 3152135,
                    Teams = "BK Klossen - Värtans IK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 15, 20, 0),
                    BitsMatchId = 3152131,
                    Teams = "Sandvikens AIK - Hammarby IF",
                    MatchResult = "0 - 0",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 17, 15, 20, 0),
                    BitsMatchId = 3152140,
                    Teams = "BK Taifun - BK Rättvik",
                    MatchResult = "0 - 0",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 18, 10, 0, 0),
                    BitsMatchId = 3152141,
                    Teams = "BK Kasi - Gävle KK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 18, 13, 40, 0),
                    BitsMatchId = 3152142,
                    Teams = "BK Glam F1 - Gävle KK",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 10, 0, 0),
                    BitsMatchId = 3152144,
                    Teams = "Uppsala BC 90 - Värtans IK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Beaten path",
                    OilPatternId = 30,
                    Location = "Uppsala - Fyrishovs bowling",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=820&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 10, 0, 0),
                    BitsMatchId = 3152147,
                    Teams = "BK Rättvik - BK Klossen",
                    MatchResult = "0 - 0",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 10, 0, 0),
                    BitsMatchId = 3152152,
                    Teams = "BK Glam F1 - BK Taifun",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Örebro - Strike & Co",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=844&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 10, 0, 0),
                    BitsMatchId = 3152153,
                    Teams = "BK Trol - Wåxnäs BC",
                    MatchResult = "0 - 0",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 12, 0, 0),
                    BitsMatchId = 3152146,
                    Teams = "Sollentuna BwK - Bålsta BC",
                    MatchResult = "0 - 0",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 13, 40, 0),
                    BitsMatchId = 3152145,
                    Teams = "Turebergs IF - Bålsta BC",
                    MatchResult = "0 - 0",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 13, 40, 0),
                    BitsMatchId = 3152151,
                    Teams = "BK Kasi - BK Taifun",
                    MatchResult = "0 - 0",
                    OilPatternName = "Allsvenskan 39 2016",
                    OilPatternId = 91,
                    Location = "Karlskoga Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=700&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 14, 0, 0),
                    BitsMatchId = 3152154,
                    Teams = "Hammarby IF - Wåxnäs BC",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 14, 40, 0),
                    BitsMatchId = 3152148,
                    Teams = "Gävle KK - BK Klossen",
                    MatchResult = "0 - 0",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 7, 15, 20, 0),
                    BitsMatchId = 3152143,
                    Teams = "Sandvikens AIK - Värtans IK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Sandvikens Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=763&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 8, 12, 0, 0),
                    BitsMatchId = 3152149,
                    Teams = "Falu BK - BK Enjoy",
                    MatchResult = "0 - 0",
                    OilPatternName = "Phase 1",
                    OilPatternId = 39,
                    Location = "Faluns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=658&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 8, 16, 0, 0),
                    BitsMatchId = 3152150,
                    Teams = "Domnarvets BS - BK Enjoy",
                    MatchResult = "0 - 0",
                    OilPatternName = "Allsvenskan 41 2016",
                    OilPatternId = 92,
                    Location = "Borlänge - Maserhallen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=856&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 10, 0, 0),
                    BitsMatchId = 3152155,
                    Teams = "Värtans IK - BK Kasi",
                    MatchResult = "0 - 0",
                    OilPatternName = "Big Ben",
                    OilPatternId = 147,
                    Location = "Stockholm - Kungsholmen",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=779&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 10, 0, 0),
                    BitsMatchId = 3152160,
                    Teams = "Gävle KK - Sandvikens AIK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Bocken 2016-2017",
                    OilPatternId = 122,
                    Location = "Gävle Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=665&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 11, 0, 0),
                    BitsMatchId = 3152161,
                    Teams = "BK Klossen - Uppsala BC 90",
                    MatchResult = "0 - 0",
                    OilPatternName = "Bourbon Street",
                    OilPatternId = 59,
                    Location = "Söderhamns Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=798&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 11, 0, 0),
                    BitsMatchId = 3152163,
                    Teams = "Wåxnäs BC - Falu BK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 12, 0, 0),
                    BitsMatchId = 3152165,
                    Teams = "BK Trol - Domnarvets BS",
                    MatchResult = "0 - 0",
                    OilPatternName = "Birkagatan 44",
                    OilPatternId = 139,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 12, 20, 0),
                    BitsMatchId = 3152158,
                    Teams = "Sollentuna BwK - BK Glam F1",
                    MatchResult = "0 - 0",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 12, 40, 0),
                    BitsMatchId = 3152164,
                    Teams = "BK Taifun - Falu BK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Middle road",
                    OilPatternId = 31,
                    Location = "Karlstad - Nöjesfabriken",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=872&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 14, 0, 0),
                    BitsMatchId = 3152156,
                    Teams = "Bålsta BC - BK Kasi",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Bålsta Bowlingcenter",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=645&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 14, 0, 0),
                    BitsMatchId = 3152157,
                    Teams = "Turebergs IF - BK Glam F1",
                    MatchResult = "0 - 0",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Sollentuna Bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=771&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 14, 40, 0),
                    BitsMatchId = 3152159,
                    Teams = "BK Rättvik - Sandvikens AIK",
                    MatchResult = "0 - 0",
                    OilPatternName = "Rättvik liga",
                    OilPatternId = 81,
                    Location = "Rättviks bowlinghall",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=879&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 15, 20, 0),
                    BitsMatchId = 3152166,
                    Teams = "Hammarby IF - Domnarvets BS",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Stockholm - Brännkyrka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=776&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 14, 16, 0, 0),
                    BitsMatchId = 3152162,
                    Teams = "BK Enjoy - Uppsala BC 90",
                    MatchResult = "0 - 0",
                    OilPatternName = "41-42",
                    OilPatternId = 46,
                    Location = "Sundsvall - Birsta",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=791&SeasonId=0&RoundId=21"
                }
        });
}
