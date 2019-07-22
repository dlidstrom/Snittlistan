namespace Snittlistan.Web.Areas.V2.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using ViewModels;

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
            if (AwardedMedals.TryGetValue(playerId, out var existingMedal) == false)
                existingMedal = EliteMedal.None;
            return existingMedal;
        }

        public FormattedMedal GetFormattedExistingMedal(string playerId)
        {
            if (AwardedMedals.TryGetValue(playerId, out var existingMedal) == false)
                existingMedal = EliteMedal.None;
            return existingMedal.GetDescription();
        }

        public FormattedMedal GetNextMedal(string playerId)
        {
            if (AwardedMedals.TryGetValue(playerId, out var existingMedal) == false)
                existingMedal = EliteMedal.None;
            var nextMedal = string.Empty;
            var text = string.Empty;
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
                    text = "KLAR";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new FormattedMedal(nextMedal, text);
        }

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

            public FormattedMedal GetDescription()
            {
                if (Value == EliteMedalValue.None) return new FormattedMedal(string.Empty, "Saknar medalj");
                return new FormattedMedal(GetDescription(Value), $"({CapturedSeason} - {CapturedSeason + 1})");
            }

            public static string GetDescription(Enum value)
            {
                var type = value.GetType();
                var name = Enum.GetName(type, value);
                if (name == null) return value.ToString();

                var field = type.GetField(name);
                if (field == null) return value.ToString();
                var description = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr
                    ? attr.Description
                    : value.ToString();
                return description;
            }
        }
    }
}