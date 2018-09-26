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

        public void Add(int bitsMatchId, string rosterId, DateTime date, int turn, MatchSerie matchSerie)
        {
            foreach (var matchTable in new[] { matchSerie.Table1, matchSerie.Table2, matchSerie.Table3, matchSerie.Table4 })
            {
                var firstPlayerResult = new PlayerResult(
                    bitsMatchId,
                    rosterId,
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
                    rosterId,
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

        public void Add(int bitsMatchId, string rosterId, DateTime date, int turn, MatchSerie4 matchSerie)
        {
            var games = new[]
            {
                (tableNumber: 1, game: matchSerie.Game1),
                (tableNumber: 2, game: matchSerie.Game2),
                (tableNumber: 3, game: matchSerie.Game3),
                (tableNumber: 4, game: matchSerie.Game4)
            };
            foreach (var (tableNumber, game) in games)
            {
                var playerResult = new PlayerResult(
                    bitsMatchId,
                    rosterId,
                    date,
                    turn,
                    matchSerie.SerieNumber,
                    tableNumber,
                    game.Player,
                    game.Score,
                    game.Pins);
                PlayerResults.Add(playerResult);
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

        public void RemoveWhere(int bitsMatchId)
        {
            PlayerResults.RemoveWhere(x => x.BitsMatchId == bitsMatchId);
        }

        [DebuggerDisplay("BitsMatchId={BitsMatchId} Date={Date} Turn={Turn} SerieNumber={SerieNumber} TableNumber={TableNumber} PlayerId={PlayerId} Score={Score} Pins={Pins}")]
        public class PlayerResult
        {
            public PlayerResult(int bitsMatchId, string rosterId, DateTime date, int turn, int serieNumber, int tableNumber, string playerId, int score, int pins)
            {
                BitsMatchId = bitsMatchId;
                RosterId = rosterId;
                Date = date;
                Turn = turn;
                SerieNumber = serieNumber;
                TableNumber = tableNumber;
                PlayerId = playerId;
                Score = score;
                Pins = pins;
            }

            public int BitsMatchId { get; }

            public string RosterId { get; }

            public DateTime Date { get; }

            public int Turn { get; }

            public int SerieNumber { get; }

            public int TableNumber { get; }

            public string PlayerId { get; }

            public int Score { get; }

            public int Pins { get; }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = BitsMatchId;
                    hashCode = (hashCode * 397) ^ (RosterId != null ? RosterId.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ SerieNumber;
                    hashCode = (hashCode * 397) ^ TableNumber;
                    hashCode = (hashCode * 397) ^ (PlayerId != null ? PlayerId.GetHashCode() : 0);
                    return hashCode;
                }
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as PlayerResult);
            }

            public bool Equals(PlayerResult playerResult)
            {
                var eq = playerResult.BitsMatchId == BitsMatchId
                    && playerResult.SerieNumber == SerieNumber
                    && playerResult.TableNumber == TableNumber
                    && string.Equals(playerResult.PlayerId, PlayerId);
                return eq;
            }
        }
    }
}