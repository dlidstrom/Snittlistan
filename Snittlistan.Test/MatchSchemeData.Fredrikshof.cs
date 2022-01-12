using NUnit.Framework;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Test;
public partial class MatchSchemeData
{
    private static readonly TestCaseData Fif = new TestCaseData(
        "Snittlistan.Test.BitsResult.FredrikshofIF-matchScheme.html",
        new[]
        {
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 1,
                    Date = new DateTime(2017, 9, 3, 10, 0, 0),
                    BitsMatchId = 3138419,
                    Teams = "BK Runan B - KK Strike",
                    MatchResult = "14 - 5",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=1"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 1,
                    Date = new DateTime(2017, 9, 3, 10, 0, 0),
                    BitsMatchId = 3138420,
                    Teams = "BK Scott - DN-Expressen B",
                    MatchResult = "14 - 6",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=1"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 1,
                    Date = new DateTime(2017, 9, 3, 10, 0, 0),
                    BitsMatchId = 3138523,
                    Teams = "Fredrikshof IF BK B - Djurgårdens IF C",
                    MatchResult = "6 - 14",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=1"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 1,
                    Date = new DateTime(2017, 9, 3, 12, 0, 0),
                    BitsMatchId = 3138418,
                    Teams = "BK Mercur - Jakobsbergs BS",
                    MatchResult = "7 - 13",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=1"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 1,
                    Date = new DateTime(2017, 9, 3, 14, 0, 0),
                    BitsMatchId = 3138417,
                    Teams = "AIK DB - Matteus-Pojkarna BK B",
                    MatchResult = "8 - 12",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=1"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 10, 10, 0, 0),
                    BitsMatchId = 3138422,
                    Teams = "DN-Expressen B - Djurgårdens IF C",
                    MatchResult = "10 - 9",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 10, 10, 0, 0),
                    BitsMatchId = 3138423,
                    Teams = "Fredrikshof IF BK B - BK Scott",
                    MatchResult = "12 - 8",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 10, 10, 0, 0),
                    BitsMatchId = 3138426,
                    Teams = "Matteus-Pojkarna BK B - BajenFans BF B",
                    MatchResult = "13 - 6",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 10, 10, 0, 0),
                    BitsMatchId = 3138518,
                    Teams = "BK Mercur - KK Strike",
                    MatchResult = "8 - 12",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 2,
                    Date = new DateTime(2017, 9, 10, 11, 0, 0),
                    BitsMatchId = 3138424,
                    Teams = "Jakobsbergs BS - BK Runan B",
                    MatchResult = "9 - 11",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=2"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 17, 10, 0, 0),
                    BitsMatchId = 3138427,
                    Teams = "BK Scott - Matteus-Pojkarna BK B",
                    MatchResult = "10 - 10",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 17, 10, 0, 0),
                    BitsMatchId = 3138428,
                    Teams = "DN-Expressen B - BK Mercur",
                    MatchResult = "12 - 8",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 17, 10, 0, 0),
                    BitsMatchId = 3138513,
                    Teams = "BajenFans BF B - KK Strike",
                    MatchResult = "7 - 13",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 17, 11, 0, 0),
                    BitsMatchId = 3138430,
                    Teams = "Jakobsbergs BS - AIK DB",
                    MatchResult = "10 - 10",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 3,
                    Date = new DateTime(2017, 9, 17, 12, 0, 0),
                    BitsMatchId = 3138429,
                    Teams = "Fredrikshof IF BK B - BK Runan B",
                    MatchResult = "16 - 2",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=3"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 24, 10, 0, 0),
                    BitsMatchId = 3138436,
                    Teams = "Matteus-Pojkarna BK B - Jakobsbergs BS",
                    MatchResult = "11 - 9",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 24, 11, 20, 0),
                    BitsMatchId = 3138435,
                    Teams = "BK Runan B - Djurgårdens IF C",
                    MatchResult = "7 - 13",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 24, 11, 40, 0),
                    BitsMatchId = 3138434,
                    Teams = "BK Mercur - BK Scott",
                    MatchResult = "15 - 5",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 24, 14, 0, 0),
                    BitsMatchId = 3138432,
                    Teams = "AIK DB - DN-Expressen B",
                    MatchResult = "10 - 10",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 4,
                    Date = new DateTime(2017, 9, 24, 16, 0, 0),
                    BitsMatchId = 3138433,
                    Teams = "BajenFans BF B - Fredrikshof IF BK B",
                    MatchResult = "7 - 13",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=4"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 5,
                    Date = new DateTime(2017, 10, 1, 10, 0, 0),
                    BitsMatchId = 3138437,
                    Teams = "AIK DB - Fredrikshof IF BK B",
                    MatchResult = "13 - 7",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=5"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 5,
                    Date = new DateTime(2017, 10, 1, 10, 0, 0),
                    BitsMatchId = 3138439,
                    Teams = "BK Mercur - Djurgårdens IF C",
                    MatchResult = "10 - 9",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=5"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 5,
                    Date = new DateTime(2017, 10, 1, 10, 0, 0),
                    BitsMatchId = 3138441,
                    Teams = "Matteus-Pojkarna BK B - KK Strike",
                    MatchResult = "17 - 3",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=5"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 5,
                    Date = new DateTime(2017, 10, 1, 14, 0, 0),
                    BitsMatchId = 3138438,
                    Teams = "BajenFans BF B - DN-Expressen B",
                    MatchResult = "14 - 6",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=5"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 15, 10, 0, 0),
                    BitsMatchId = 3138443,
                    Teams = "DN-Expressen B - BK Runan B",
                    MatchResult = "11 - 8",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 15, 10, 0, 0),
                    BitsMatchId = 3138446,
                    Teams = "KK Strike - AIK DB",
                    MatchResult = "18 - 2",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 15, 11, 0, 0),
                    BitsMatchId = 3138442,
                    Teams = "Djurgårdens IF C - Matteus-Pojkarna BK B",
                    MatchResult = "12 - 8",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 15, 11, 0, 0),
                    BitsMatchId = 3138445,
                    Teams = "Jakobsbergs BS - BajenFans BF B",
                    MatchResult = "4 - 16",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 6,
                    Date = new DateTime(2017, 10, 15, 13, 40, 0),
                    BitsMatchId = 3138444,
                    Teams = "Fredrikshof IF BK B - BK Mercur",
                    MatchResult = "16 - 4",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=6"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 21, 11, 0, 0),
                    BitsMatchId = 3138449,
                    Teams = "Jakobsbergs BS - BK Scott",
                    MatchResult = "8 - 12",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 22, 10, 0, 0),
                    BitsMatchId = 3138447,
                    Teams = "AIK DB - BK Mercur",
                    MatchResult = "11 - 9",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 22, 10, 0, 0),
                    BitsMatchId = 3138450,
                    Teams = "KK Strike - Djurgårdens IF C",
                    MatchResult = "6 - 14",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 22, 13, 0, 0),
                    BitsMatchId = 3138451,
                    Teams = "Matteus-Pojkarna BK B - DN-Expressen B",
                    MatchResult = "11 - 9",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 7,
                    Date = new DateTime(2017, 10, 22, 14, 0, 0),
                    BitsMatchId = 3138448,
                    Teams = "BajenFans BF B - BK Runan B",
                    MatchResult = "11 - 9",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=7"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 29, 10, 0, 0),
                    BitsMatchId = 3138453,
                    Teams = "BK Scott - AIK DB",
                    MatchResult = "16 - 4",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 29, 10, 0, 0),
                    BitsMatchId = 3138455,
                    Teams = "DN-Expressen B - Jakobsbergs BS",
                    MatchResult = "10 - 10",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 29, 11, 0, 0),
                    BitsMatchId = 3138454,
                    Teams = "Djurgårdens IF C - BajenFans BF B",
                    MatchResult = "12 - 8",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 29, 11, 40, 0),
                    BitsMatchId = 3138456,
                    Teams = "Fredrikshof IF BK B - KK Strike",
                    MatchResult = "10 - 10",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 8,
                    Date = new DateTime(2017, 10, 29, 12, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3138452,
                    Teams = "BK Mercur - Matteus-Pojkarna BK B",
                    MatchResult = "6 - 14",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=8"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 19, 10, 0, 0),
                    BitsMatchId = 3138457,
                    Teams = "BK Runan B - Matteus-Pojkarna BK B",
                    MatchResult = "16 - 4",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 19, 10, 0, 0),
                    BitsMatchId = 3138458,
                    Teams = "BK Scott - BajenFans BF B",
                    MatchResult = "8 - 12",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 19, 10, 0, 0),
                    BitsMatchId = 3138460,
                    Teams = "DN-Expressen B - KK Strike",
                    MatchResult = "9 - 11",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 19, 10, 0, 0),
                    BitsMatchId = 3138461,
                    Teams = "Fredrikshof IF BK B - Jakobsbergs BS",
                    MatchResult = "12 - 8",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 9,
                    Date = new DateTime(2017, 11, 19, 11, 0, 0),
                    BitsMatchId = 3138459,
                    Teams = "Djurgårdens IF C - AIK DB",
                    MatchResult = "13 - 7",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=9"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 25, 13, 40, 0),
                    BitsMatchId = 3138465,
                    Teams = "KK Strike - BK Scott",
                    MatchResult = "16 - 4",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 26, 10, 0, 0),
                    BitsMatchId = 3138466,
                    Teams = "Matteus-Pojkarna BK B - Fredrikshof IF BK B",
                    MatchResult = "15 - 5",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 26, 14, 0, 0),
                    BitsMatchId = 3138462,
                    Teams = "AIK DB - BK Runan B",
                    MatchResult = "10 - 10",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 26, 14, 20, 0),
                    BitsMatchId = 3138464,
                    Teams = "Jakobsbergs BS - Djurgårdens IF C",
                    MatchResult = "13 - 7",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 10,
                    Date = new DateTime(2017, 11, 26, 16, 0, 0),
                    BitsMatchId = 3138463,
                    Teams = "BajenFans BF B - BK Mercur",
                    MatchResult = "11 - 9",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=10"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 10, 0, 0),
                    BitsMatchId = 3138467,
                    Teams = "AIK DB - BajenFans BF B",
                    MatchResult = "6 - 14",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 2, 13, 0, 0),
                    BitsMatchId = 3138471,
                    Teams = "Jakobsbergs BS - KK Strike",
                    MatchResult = "8 - 12",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 3, 10, 0, 0),
                    BitsMatchId = 3138468,
                    Teams = "BK Mercur - BK Runan B",
                    MatchResult = "12 - 8",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 3, 10, 0, 0),
                    BitsMatchId = 3138470,
                    Teams = "DN-Expressen B - Fredrikshof IF BK B",
                    MatchResult = "9 - 11",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 3, 11, 40, 0),
                    BitsMatchId = 3138469,
                    Teams = "BK Scott - Djurgårdens IF C",
                    MatchResult = "12 - 8",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 9, 10, 0, 0),
                    BitsMatchId = 3138500,
                    Teams = "BK Runan B - DN-Expressen B",
                    MatchResult = "11 - 9",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 10, 10, 0, 0),
                    BitsMatchId = 3138497,
                    Teams = "AIK DB - KK Strike",
                    MatchResult = "16 - 4",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 10, 10, 0, 0),
                    BitsMatchId = 3138499,
                    Teams = "BK Mercur - Fredrikshof IF BK B",
                    MatchResult = "4 - 16",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 10, 10, 0, 0),
                    BitsMatchId = 3138501,
                    Teams = "Matteus-Pojkarna BK B - Djurgårdens IF C",
                    MatchResult = "10 - 10",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 11,
                    Date = new DateTime(2017, 12, 10, 14, 0, 0),
                    BitsMatchId = 3138498,
                    Teams = "BajenFans BF B - Jakobsbergs BS",
                    MatchResult = "16 - 4",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=11"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 14, 10, 0, 0),
                    BitsMatchId = 3138473,
                    Teams = "BK Runan B - BK Mercur",
                    MatchResult = "11 - 9",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 14, 10, 0, 0),
                    BitsMatchId = 3138475,
                    Teams = "Fredrikshof IF BK B - DN-Expressen B",
                    MatchResult = "14 - 6",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 14, 10, 0, 0),
                    BitsMatchId = 3138476,
                    Teams = "KK Strike - Jakobsbergs BS",
                    MatchResult = "8 - 12",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 14, 11, 0, 0),
                    BitsMatchId = 3138474,
                    Teams = "Djurgårdens IF C - BK Scott",
                    MatchResult = "17 - 3",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 12,
                    Date = new DateTime(2018, 1, 14, 12, 0, 0),
                    BitsMatchId = 3138472,
                    Teams = "BajenFans BF B - AIK DB",
                    MatchResult = "14 - 6",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=12"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 10, 0, 0),
                    BitsMatchId = 3138477,
                    Teams = "BK Mercur - BajenFans BF B",
                    MatchResult = "4 - 16",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 20, 13, 20, 0),
                    BitsMatchId = 3138479,
                    Teams = "BK Scott - KK Strike",
                    MatchResult = "17 - 2",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 21, 10, 0, 0),
                    BitsMatchId = 3138478,
                    Teams = "BK Runan B - AIK DB",
                    MatchResult = "12 - 8",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 21, 10, 0, 0),
                    BitsMatchId = 3138481,
                    Teams = "Fredrikshof IF BK B - Matteus-Pojkarna BK B",
                    MatchResult = "15 - 5",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 13,
                    Date = new DateTime(2018, 1, 21, 11, 0, 0),
                    BitsMatchId = 3138480,
                    Teams = "Djurgårdens IF C - Jakobsbergs BS",
                    MatchResult = "13 - 6",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=13"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 14,
                    Date = new DateTime(2018, 1, 28, 10, 0, 0),
                    BitsMatchId = 3138482,
                    Teams = "AIK DB - Djurgårdens IF C",
                    MatchResult = "11 - 9",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=14"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 14,
                    Date = new DateTime(2018, 1, 28, 10, 0, 0),
                    BitsMatchId = 3138486,
                    Teams = "Matteus-Pojkarna BK B - BK Runan B",
                    MatchResult = "12 - 8",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=14"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 14,
                    Date = new DateTime(2018, 1, 28, 11, 0, 0),
                    BitsMatchId = 3138484,
                    Teams = "Jakobsbergs BS - Fredrikshof IF BK B",
                    MatchResult = "11 - 9",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=14"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 14,
                    Date = new DateTime(2018, 1, 28, 11, 40, 0),
                    BitsMatchId = 3138485,
                    Teams = "KK Strike - DN-Expressen B",
                    MatchResult = "6 - 13",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=14"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 14,
                    Date = new DateTime(2018, 1, 28, 14, 0, 0),
                    BitsMatchId = 3138483,
                    Teams = "BajenFans BF B - BK Scott",
                    MatchResult = "16 - 4",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=14"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 11, 10, 0, 0),
                    BitsMatchId = 3138487,
                    Teams = "AIK DB - BK Scott",
                    MatchResult = "3 - 17",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 11, 10, 0, 0),
                    BitsMatchId = 3138490,
                    Teams = "KK Strike - Fredrikshof IF BK B",
                    MatchResult = "8 - 12",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 11, 10, 0, 0),
                    BitsMatchId = 3138491,
                    Teams = "Matteus-Pojkarna BK B - BK Mercur",
                    MatchResult = "15 - 5",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 11, 11, 0, 0),
                    BitsMatchId = 3138489,
                    Teams = "Jakobsbergs BS - DN-Expressen B",
                    MatchResult = "13 - 7",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 11, 14, 0, 0),
                    BitsMatchId = 3138488,
                    Teams = "BajenFans BF B - Djurgårdens IF C",
                    MatchResult = "15 - 5",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 15,
                    Date = new DateTime(2018, 2, 11, 14, 20, 0),
                    DateChanged = true,
                    BitsMatchId = 3138440,
                    Teams = "BK Runan B - BK Scott",
                    MatchResult = "12 - 7",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=15"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 18, 10, 0, 0),
                    BitsMatchId = 3138493,
                    Teams = "BK Runan B - BajenFans BF B",
                    MatchResult = "9 - 10",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 18, 10, 0, 0),
                    BitsMatchId = 3138494,
                    Teams = "BK Scott - Jakobsbergs BS",
                    MatchResult = "14 - 6",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 18, 10, 0, 0),
                    BitsMatchId = 3138496,
                    Teams = "DN-Expressen B - Matteus-Pojkarna BK B",
                    MatchResult = "7 - 13",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 18, 11, 0, 0),
                    BitsMatchId = 3138495,
                    Teams = "Djurgårdens IF C - KK Strike",
                    MatchResult = "12 - 8",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 16,
                    Date = new DateTime(2018, 2, 18, 12, 0, 0),
                    BitsMatchId = 3138492,
                    Teams = "BK Mercur - AIK DB",
                    MatchResult = "18 - 1",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=16"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 10, 10, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3138503,
                    Teams = "Djurgårdens IF C - BK Mercur",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 11, 10, 0, 0),
                    BitsMatchId = 3138502,
                    Teams = "BK Scott - BK Runan B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 11, 10, 0, 0),
                    BitsMatchId = 3138504,
                    Teams = "DN-Expressen B - BajenFans BF B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 11, 10, 0, 0),
                    BitsMatchId = 3138506,
                    Teams = "KK Strike - Matteus-Pojkarna BK B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 18,
                    Date = new DateTime(2018, 3, 11, 13, 40, 0),
                    BitsMatchId = 3138505,
                    Teams = "Fredrikshof IF BK B - AIK DB",
                    MatchResult = "0 - 0",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=18"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 18, 10, 0, 0),
                    BitsMatchId = 3138507,
                    Teams = "BK Scott - BK Mercur",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 18, 10, 0, 0),
                    BitsMatchId = 3138509,
                    Teams = "DN-Expressen B - AIK DB",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 18, 13, 20, 0),
                    BitsMatchId = 3138510,
                    Teams = "Fredrikshof IF BK B - BajenFans BF B",
                    MatchResult = "0 - 0",
                    OilPatternName = "ABT#2",
                    OilPatternId = 61,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 18, 13, 40, 0),
                    BitsMatchId = 3138511,
                    Teams = "Jakobsbergs BS - Matteus-Pojkarna BK B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 19,
                    Date = new DateTime(2018, 3, 25, 11, 0, 0),
                    DateChanged = true,
                    BitsMatchId = 3138508,
                    Teams = "Djurgårdens IF C - BK Runan B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=19"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 8, 10, 0, 0),
                    BitsMatchId = 3138431,
                    Teams = "KK Strike - BajenFans BF B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 8, 10, 0, 0),
                    BitsMatchId = 3138512,
                    Teams = "AIK DB - Jakobsbergs BS",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 8, 10, 0, 0),
                    BitsMatchId = 3138514,
                    Teams = "BK Mercur - DN-Expressen B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 8, 10, 0, 0),
                    BitsMatchId = 3138516,
                    Teams = "Matteus-Pojkarna BK B - BK Scott",
                    MatchResult = "0 - 0",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 20,
                    Date = new DateTime(2018, 4, 8, 11, 20, 0),
                    BitsMatchId = 3138515,
                    Teams = "BK Runan B - Fredrikshof IF BK B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=20"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 15, 10, 0, 0),
                    BitsMatchId = 3138519,
                    Teams = "BK Runan B - Jakobsbergs BS",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Täby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=814&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 15, 10, 0, 0),
                    BitsMatchId = 3138520,
                    Teams = "BK Scott - Fredrikshof IF BK B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 15, 11, 0, 0),
                    BitsMatchId = 3138521,
                    Teams = "Djurgårdens IF C - DN-Expressen B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 15, 11, 40, 0),
                    BitsMatchId = 3138425,
                    Teams = "KK Strike - BK Mercur",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 21,
                    Date = new DateTime(2018, 4, 15, 13, 40, 0),
                    BitsMatchId = 3138517,
                    Teams = "BajenFans BF B - Matteus-Pojkarna BK B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Broadway",
                    OilPatternId = 32,
                    Location = "Stockholm - Bowl-O-Rama",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=780&SeasonId=0&RoundId=21"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 22,
                    Date = new DateTime(2018, 4, 22, 10, 0, 0),
                    BitsMatchId = 3138522,
                    Teams = "DN-Expressen B - BK Scott",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Gullmarsplan",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=777&SeasonId=0&RoundId=22"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 22,
                    Date = new DateTime(2018, 4, 22, 10, 0, 0),
                    BitsMatchId = 3138526,
                    Teams = "Matteus-Pojkarna BK B - AIK DB",
                    MatchResult = "0 - 0",
                    OilPatternName = "43-44",
                    OilPatternId = 47,
                    Location = "Stockholm - Birka",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=774&SeasonId=0&RoundId=22"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 22,
                    Date = new DateTime(2018, 4, 22, 11, 0, 0),
                    BitsMatchId = 3138421,
                    Teams = "Djurgårdens IF C - Fredrikshof IF BK B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Vårby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=785&SeasonId=0&RoundId=22"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 22,
                    Date = new DateTime(2018, 4, 22, 11, 0, 0),
                    BitsMatchId = 3138524,
                    Teams = "Jakobsbergs BS - BK Mercur",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Rinkeby",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=781&SeasonId=0&RoundId=22"
                },
                new ParseMatchSchemeResult.MatchItem
                {
                    Turn = 22,
                    Date = new DateTime(2018, 4, 22, 11, 40, 0),
                    BitsMatchId = 3138525,
                    Teams = "KK Strike - BK Runan B",
                    MatchResult = "0 - 0",
                    OilPatternName = "Ingen OljeProfil",
                    OilPatternId = 0,
                    Location = "Stockholm - Åkeshov",
                    LocationUrl = "http://bits.swebowl.se/Matches/HallSchemeAdminList.aspx?HallId=786&SeasonId=0&RoundId=22"
                }
        });
}
