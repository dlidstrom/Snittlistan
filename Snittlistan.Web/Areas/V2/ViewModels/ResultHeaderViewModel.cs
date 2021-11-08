#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using System.Web;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class ResultHeaderViewModel
    {
        public ResultHeaderViewModel(ResultHeaderReadModel readModel, Roster roster)
        {
            RosterId = roster.Id!;
            AggregateId = readModel.AggregateId!;
            Date = roster.Date;
            TeamLevel = roster.TeamLevel!;
            Turn = roster.Turn;
            Team = roster.Team;
            Opponent = roster.Opponent;
            BitsMatchId = roster.BitsMatchId;
            FormattedResult = new HtmlString($"{readModel.TeamScore}&minus;{readModel.OpponentScore}");
            Location = roster.Location;
            MatchCommentaryHtml = new HtmlString(readModel.MatchCommentaryHtml);
            BodyText = readModel.BodyText;
        }

        public string RosterId { get; }

        public string AggregateId { get; }

        public DateTime Date { get; }

        public string TeamLevel { get; }

        public int Turn { get; }

        public string Team { get; }

        public string Opponent { get; }

        public int BitsMatchId { get; }

        public IHtmlString FormattedResult { get; }

        public string Location { get; }

        public IHtmlString MatchCommentaryHtml { get; }

        public string[] BodyText { get; }
    }
}
