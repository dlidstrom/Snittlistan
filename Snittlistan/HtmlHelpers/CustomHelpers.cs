using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Snittlistan.ViewModels;

namespace Snittlistan.HtmlHelpers
{
	public static class CustomHtmlHelpers
	{
		/// <summary>
		/// Returns HTML with edit and back links. Only if request is authenticated,
		/// is the edit link included.
		/// </summary>
		/// <param name="html">The HTML helper instance that this method extends.</param>
		/// <param name="model">Match view model.</param>
		/// <returns>The HTML markup with edit and back links.</returns>
		public static MvcHtmlString GenerateEditBackLinks(this HtmlHelper html, MatchViewModel.MatchDetails model)
		{
			var builder = new TagBuilder("p");

			if (html.ViewContext.HttpContext.Request.IsAuthenticated)
			{
				builder.InnerHtml = html.ActionLink("Redigera", "Edit", new { id = model.Id }).ToString() + " | ";
			}

			builder.InnerHtml += html.ActionLink("Tillbaka", "Index");

			return new MvcHtmlString(builder.ToString());
		}

		/// <summary>
		/// Returns HTML with an anchor link to BITS for the match.
		/// </summary>
		/// <param name="html">The HTML helper instance that this method extends.</param>
		/// <param name="model">Match view model.</param>
		/// <returns>The HTML markup with an anchor link to BITS.</returns>
		public static MvcHtmlString GenerateBitsLink(this HtmlHelper html, MatchViewModel.MatchDetails model)
		{
			var builder = new TagBuilder("a");
			builder.MergeAttribute("href", string.Format("http://bits.swebowl.se/MatchFact.aspx?MatchId={0}", model.BitsMatchId));
			builder.SetInnerText("Matchfakta");

			return new MvcHtmlString(builder.ToString());
		}
	}
}