using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class EliteMedals
    {
        public EliteMedals()
        {
            Id = TheId;
            AwardedMedals = new Dictionary<string, EliteMedal>();
        }

        public const string TheId = "EliteMedals";

        public string Id { get; private set; }

        public Dictionary<string, EliteMedal> AwardedMedals { get; private set; }

        public EliteMedal GetExistingMedal(string playerId)
        {
            EliteMedal existingMedal;
            if (AwardedMedals.TryGetValue(playerId, out existingMedal) == false)
                existingMedal = EliteMedal.None;
            return existingMedal;
        }

        public string GetFormattedExistingMedal(string playerId)
        {
            EliteMedal existingMedal;
            if (AwardedMedals.TryGetValue(playerId, out existingMedal) == false)
                existingMedal = EliteMedal.None;
            return existingMedal.GetDescription();
        }

        public string GetNextMedal(string playerId)
        {
            EliteMedal existingMedal;
            if (AwardedMedals.TryGetValue(playerId, out existingMedal) == false)
                existingMedal = EliteMedal.None;
            string nextMedal;
            switch (existingMedal.Value)
            {
                case EliteMedal.EliteMedalValue.None:
                    nextMedal = EliteMedal.GetDescription(EliteMedal.EliteMedalValue.Bronze);
                    break;
                case EliteMedal.EliteMedalValue.Bronze:
                    nextMedal = EliteMedal.GetDescription(EliteMedal.EliteMedalValue.Silver);
                    break;
                case EliteMedal.EliteMedalValue.Silver:
                    nextMedal = EliteMedal.GetDescription(EliteMedal.EliteMedalValue.Gold1);
                    break;
                case EliteMedal.EliteMedalValue.Gold1:
                    nextMedal = EliteMedal.GetDescription(EliteMedal.EliteMedalValue.Gold2);
                    break;
                case EliteMedal.EliteMedalValue.Gold2:
                    nextMedal = EliteMedal.GetDescription(EliteMedal.EliteMedalValue.Gold3);
                    break;
                case EliteMedal.EliteMedalValue.Gold3:
                    nextMedal = EliteMedal.GetDescription(EliteMedal.EliteMedalValue.Gold4);
                    break;
                case EliteMedal.EliteMedalValue.Gold4:
                    nextMedal = EliteMedal.GetDescription(EliteMedal.EliteMedalValue.Gold5);
                    break;
                case EliteMedal.EliteMedalValue.Gold5:
                    nextMedal = "KLAR";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return nextMedal;
        }

        //public void AddResults(Dictionary<string, Tuple<int, int>> playerIdToTotalAndSeries, DateTime date, int bitsMatchId, int turn)
        //{
        //    foreach (var playerId in playerIdToTotalAndSeries.Keys)
        //    {
        //        var totalAndSeries = playerIdToTotalAndSeries[playerId];
        //        EliteMedal existingMedal;
        //        if (AwardedMedals.TryGetValue(playerId, out existingMedal) == false)
        //            existingMedal = EliteMedal.None;
        //        var isCandidateResult = false;
        //        switch (existingMedal.Value)
        //        {
        //            case EliteMedal.EliteMedalValue.None:
        //                {
        //                    isCandidateResult = totalAndSeries.Item1 > 4 * 190 && totalAndSeries.Item2 == 4
        //                        || totalAndSeries.Item1 > 3 * 200 && totalAndSeries.Item2 == 3;
        //                    break;
        //                }
        //            case EliteMedal.EliteMedalValue.Bronze:
        //                {
        //                    isCandidateResult = totalAndSeries.Item1 > 4 * 200 && totalAndSeries.Item2 == 4
        //                        || totalAndSeries.Item1 > 3 * 210 && totalAndSeries.Item2 == 3;
        //                    break;
        //                }
        //            case EliteMedal.EliteMedalValue.Silver:
        //            case EliteMedal.EliteMedalValue.Gold1:
        //            case EliteMedal.EliteMedalValue.Gold2:
        //            case EliteMedal.EliteMedalValue.Gold3:
        //            case EliteMedal.EliteMedalValue.Gold4:
        //                {
        //                    isCandidateResult = totalAndSeries.Item1 > 4 * 210 && totalAndSeries.Item2 == 4
        //                        || totalAndSeries.Item1 > 3 * 220 && totalAndSeries.Item2 == 3;
        //                    break;
        //                }
        //            case EliteMedal.EliteMedalValue.Gold5:
        //                break;
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }

        //        if (isCandidateResult)
        //        {
        //            //List<CandidateResult> candidateResults;
        //            //if (CandidateResults.TryGetValue(playerId, out candidateResults) == false)
        //            //{
        //            //    candidateResults = new List<CandidateResult>();
        //            //    CandidateResults.Add(playerId, candidateResults);
        //            //}

        //            //if (candidateResults.SingleOrDefault(x => x.BitsMatchId == bitsMatchId) == null)
        //            //    candidateResults.Add(new CandidateResult(date, bitsMatchId, turn, totalAndSeries.Item1));
        //        }
        //    }
        //}

        public void AwardMedal(string playerId, EliteMedal.EliteMedalValue eliteMedalValue, int? capturedSeason)
        {
            AwardedMedals[playerId] = new EliteMedal(eliteMedalValue, capturedSeason);
        }

        public class EliteMedal
        {
            public EliteMedal(EliteMedalValue eliteMedalValue, int? capturedSeason)
            {
                Value = eliteMedalValue;
                CapturedSeason = capturedSeason;
            }

            public EliteMedalValue Value { get; private set; }

            public int? CapturedSeason { get; private set; }

            public static EliteMedal None
            {
                get { return new EliteMedal(EliteMedalValue.None, 0); }
            }

            public enum EliteMedalValue
            {
                [Description("")]
                None,
                [Description("Brons")]
                Bronze,
                [Description("Silver")]
                Silver,
                [Description("Guld")]
                Gold1,
                [Description("Guld 2")]
                Gold2,
                [Description("Guld 3")]
                Gold3,
                [Description("Guld 4")]
                Gold4,
                [Description("ELITPLAKETT")]
                Gold5
            }

            public string GetDescription()
            {
                if (Value == EliteMedalValue.None) return "Saknar medalj";
                return string.Format("{0} ({1} - {2})", GetDescription(Value), CapturedSeason, CapturedSeason + 1);
            }

            public static string GetDescription(Enum value)
            {
                var type = value.GetType();
                var name = Enum.GetName(type, value);
                if (name == null) return value.ToString();

                var field = type.GetField(name);
                if (field == null) return value.ToString();
                var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                return attr != null ? attr.Description : value.ToString();
            }
        }
    }
}