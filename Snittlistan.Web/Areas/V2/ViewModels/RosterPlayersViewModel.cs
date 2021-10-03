namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class RosterPlayersViewModel : IValidatableObject
    {
        public string Table1Player1 { get; set; }

        public string Table1Player2 { get; set; }

        public string Table2Player1 { get; set; }

        public string Table2Player2 { get; set; }

        public string Table3Player1 { get; set; }

        public string Table3Player2 { get; set; }

        public string Table4Player1 { get; set; }

        public string Table4Player2 { get; set; }

        public string Player1 { get; set; }

        public string Player2 { get; set; }

        public string Player3 { get; set; }

        public string Player4 { get; set; }

        public string Reserve1 { get; set; }

        public string Reserve2 { get; set; }

        public bool IsFourPlayer { get; set; }

        public bool Preliminary { get; set; }

        public string TeamLeader { get; set; }

        public bool SendUpdateMail { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            HashSet<string> hash;
            if (IsFourPlayer)
            {
                if (Player1 == null) yield return new ValidationResult("Ange namn på spelare 1");
                if (Player2 == null) yield return new ValidationResult("Ange namn på spelare 2");
                if (Player3 == null) yield return new ValidationResult("Ange namn på spelare 3");
                if (Player4 == null) yield return new ValidationResult("Ange namn på spelare 4");
                hash = new HashSet<string>(new[] { Player1, Player2, Player3, Player4 }.Where(x => x != null));
                if (hash.Count != 4)
                {
                    yield return new ValidationResult("Någon spelare vald flera gånger");
                }
            }
            else
            {
                if (Table1Player1 == null) yield return new ValidationResult("Ange namn på spelare 1");
                if (Table1Player2 == null) yield return new ValidationResult("Ange namn på spelare 2");
                if (Table2Player1 == null) yield return new ValidationResult("Ange namn på spelare 3");
                if (Table2Player2 == null) yield return new ValidationResult("Ange namn på spelare 4");
                if (Table3Player1 == null) yield return new ValidationResult("Ange namn på spelare 5");
                if (Table3Player2 == null) yield return new ValidationResult("Ange namn på spelare 6");
                if (Table4Player1 == null) yield return new ValidationResult("Ange namn på spelare 7");
                if (Table4Player2 == null) yield return new ValidationResult("Ange namn på spelare 8");
                hash = new HashSet<string>(new[]
                {
                    Table1Player1,
                    Table1Player2,
                    Table2Player1,
                    Table2Player2,
                    Table3Player1,
                    Table3Player2,
                    Table4Player1,
                    Table4Player2
                }.Where(x => x != null));
                if (hash.Count != 8)
                {
                    yield return new ValidationResult("Någon spelare vald flera gånger");
                }
            }

            if (Reserve1 != null && hash.Contains(Reserve1))
                yield return new ValidationResult("Felaktig reserv 1");

            if (Reserve2 != null && (hash.Contains(Reserve2) || Reserve1 == Reserve2))
                yield return new ValidationResult("Felaktig reserv 2");
        }
    }
}
