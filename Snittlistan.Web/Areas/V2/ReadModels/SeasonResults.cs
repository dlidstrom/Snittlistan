using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Domain.Match;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class SeasonResults : IReadModel
    {
        public SeasonResults(int season)
        {
            Id = GetId(season);
            PlayerResults = new HashSet<PlayerResult>();
        }

        public string Id { get; private set; }

        public HashSet<PlayerResult> PlayerResults { get; private set; }

        public static string GetId(int season)
        {
            return "SeasonResults-" + season;
        }

        public void Add(int bitsMatchId, DateTime date, int turn, MatchSerie matchSerie)
        {
            foreach (var matchTable in new[] { matchSerie.Table1, matchSerie.Table2, matchSerie.Table3, matchSerie.Table4 })
            {
                var firstPlayerResult = new PlayerResult(
                    bitsMatchId,
                    date,
                    turn,
                    matchSerie.SerieNumber,
                    matchTable.TableNumber,
                    matchTable.Game1.Player,
                    matchTable.Score,
                    matchTable.Game1.Pins);
                PlayerResults.Add(firstPlayerResult);
                var secondPlayerResult = new PlayerResult(
                    bitsMatchId,
                    date,
                    turn,
                    matchSerie.SerieNumber,
                    matchTable.TableNumber,
                    matchTable.Game2.Player,
                    matchTable.Score,
                    matchTable.Game2.Pins);
                PlayerResults.Add(secondPlayerResult);
            }
        }

        public HashSet<Tuple<PlayerResult, bool>> GetTopThreeResults(string playerId, EliteMedals.EliteMedal.EliteMedalValue existingMedal)
        {
            var query =
                from playerResult in PlayerResults.Select(x => x)
                where playerResult.PlayerId == playerId
                group playerResult by new
                {
                    playerResult.BitsMatchId
                }
                into grouping
                where grouping.Count() == 4
                let totalPins = grouping.Sum(x => x.Pins)
                let validResult = existingMedal == EliteMedals.EliteMedal.EliteMedalValue.None && totalPins >= 760
                    || existingMedal == EliteMedals.EliteMedal.EliteMedalValue.Bronze && totalPins >= 800
                    || totalPins >= 840
                orderby totalPins descending
                select new
                {
                    grouping.Key.BitsMatchId,
                    PlayerResults = grouping.ToArray(),
                    ValidResult = validResult
                };
            var topThree = query.Take(3).ToArray();
            var playerResults = topThree.SelectMany(x => x.PlayerResults.Select(y => Tuple.Create(y, x.ValidResult))).ToArray();
            var topThreeResults = new HashSet<Tuple<PlayerResult, bool>>(playerResults);
            return topThreeResults;
        }

        [DebuggerDisplay("BitsMatchId={BitsMatchId} Date={Date} Turn={Turn} SerieNumber={SerieNumber} TableNumber={TableNumber} PlayerId={PlayerId} Score={Score} Pins={Pins}")]
        public class PlayerResult
        {
            public PlayerResult(int bitsMatchId, DateTime date, int turn, int serieNumber, int tableNumber, string playerId, int score, int pins)
            {
                BitsMatchId = bitsMatchId;
                Date = date;
                Turn = turn;
                SerieNumber = serieNumber;
                TableNumber = tableNumber;
                PlayerId = playerId;
                Score = score;
                Pins = pins;
            }

            public int BitsMatchId { get; private set; }

            public DateTime Date { get; private set; }

            public int Turn { get; private set; }

            public int SerieNumber { get; private set; }

            public int TableNumber { get; private set; }

            public string PlayerId { get; private set; }

            public int Score { get; private set; }

            public int Pins { get; private set; }

            public override int GetHashCode()
            {
                return ToString().GetHashCode();
            }

            public override string ToString()
            {
                return string.Format("{0}{1}{2}{3}{4}{5}{6}", BitsMatchId, Date, Turn, SerieNumber, PlayerId, Score, Pins);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as PlayerResult);
            }

            public bool Equals(PlayerResult playerResult)
            {
                var @equals = playerResult != null
                              && playerResult.PlayerId == PlayerId
                              && playerResult.Date == Date
                              && playerResult.BitsMatchId == BitsMatchId
                              && playerResult.SerieNumber == SerieNumber;
                return @equals;
            }
        }
    }
}