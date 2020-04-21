namespace Snittlistan.Web.Areas.V2.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Configuration;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Web.Mvc;
    using Domain;
    using Helpers;
    using Indexes;
    using Infrastructure.Attributes;
    using Models;
    using PdfSharp.Pdf;
    using PdfSharp.Pdf.IO;
    using Raven.Abstractions;
    using ReadModels;
    using ViewModels;
    using Web.Controllers;

    [Authorize(Roles = WebsiteRoles.EliteMedals.EditMedals)]
    public class EliteMedalsPrintController : AbstractController
    {
        [HttpPost]
        [SetTempModelState]
        public ActionResult GeneratePdf(PostModel postModel)
        {
            // find out current season
            var season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
            var seasonResults = DocumentSession.Load<SeasonResults>(SeasonResults.GetId(season));
            if (seasonResults == null)
            {
                ModelState.AddModelError("resultat", "Det finns inga resultat för säsongen.");
            }

            if (ModelState.IsValid == false)
            {
                return RedirectToAction("EliteMedals", "MatchResult");
            }

            var eliteMedals = DocumentSession.Load<EliteMedals>(EliteMedals.TheId);
            var playersDict = DocumentSession.Query<Player, PlayerSearch>()
                                             .Where(p => p.PlayerStatus == Player.Status.Active)
                                             .ToDictionary(x => x.Id);
            var viewModel = new EliteMedalsViewModel(season, playersDict, eliteMedals, seasonResults);

            var templateFilename = ConfigurationManager.AppSettings["ElitemedalsTemplateFilename"];
            var stream = new MemoryStream();
            var archiveFileName = $"Elitmedaljer_{TenantConfiguration.FullTeamName}_{season}-{season + 1}.zip";
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                var playersThatHaveMedalsToAchieve =
                    viewModel.Players
                             .Where(x => x.ExistingMedal != EliteMedals.EliteMedal.EliteMedalValue.Gold5)
                             .OrderBy(x => x.Name)
                             .ToArray();
                var listOfMissingPersonalNumbers = new List<string>();
                foreach (var player in playersThatHaveMedalsToAchieve)
                {
                    var playerMedalInfo = new PlayerMedalInfo(
                        player.Name,
                        player.PersonalNumber,
                        player.FormattedExistingMedal(),
                        player.FormattedNextMedal(),
                        player.TopThreeResults);
                    var result = CreateFileEntry(
                        zip,
                        templateFilename,
                        playerMedalInfo,
                        TenantConfiguration,
                        postModel);
                    if (result == CreateFileEntryResult.MissingPersonalNumber)
                    {
                        listOfMissingPersonalNumbers.Add(player.Name);
                    }
                }

                if (listOfMissingPersonalNumbers.Any())
                {
                    foreach (var playerName in listOfMissingPersonalNumbers)
                    {
                        ModelState.AddModelError(playerName, $"{playerName} saknar personnummer i fliken Medlemmar.");
                    }

                    return RedirectToAction("EliteMedals", "MatchResult");
                }
            }

            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/zip", archiveFileName);
        }

        private enum CreateFileEntryResult
        {
            NotAchievedThreeResults,
            MissingPersonalNumber,
            CreatedDocument
        }

        private static CreateFileEntryResult CreateFileEntry(
            ZipArchive zip,
            string templateFilename,
            PlayerMedalInfo playerMedalInfo,
            TenantConfiguration tenantConfiguration,
            PostModel postModel)
        {
            var top3ValidResults = playerMedalInfo.PlayerTopThreeResults
                                                  .Where(x => x.Item2)
                                                  .GroupBy(x => new { x.Item1.BitsMatchId, x.Item1.Turn, x.Item1.Date })
                                                  .OrderBy(x => x.Key.Turn)
                                                  .ThenBy(x => x.Key.Date)
                                                  .ToArray();
            if (top3ValidResults.Length < 3)
            {
                return CreateFileEntryResult.NotAchievedThreeResults;
            }

            if (playerMedalInfo.PlayerPersonalNumber == 0)
            {
                return CreateFileEntryResult.MissingPersonalNumber;
            }

            var entry = zip.CreateEntry($"{playerMedalInfo.PlayerName}.pdf", CompressionLevel.Fastest);
            using (var entryStream = entry.Open())
            using (var document = PdfReader.Open(templateFilename, PdfDocumentOpenMode.Modify))
            {
                if (document.AcroForm.Elements.ContainsKey("/NeedAppearances"))
                {
                    document.AcroForm.Elements["/NeedAppearances"] = new PdfBoolean(true);
                }
                else
                {
                    document.AcroForm.Elements.Add("/NeedAppearances", new PdfBoolean(true));
                }

                document.AcroForm.Fields["Text1"].Value = new PdfString(playerMedalInfo.PlayerName);
                document.AcroForm.Fields["Text2"].Value = new PdfString(playerMedalInfo.PlayerPersonalNumber.ToString());
                document.AcroForm.Fields["Text3"].Value = new PdfString(playerMedalInfo.FormattedExistingMedal.Description);
                //document.AcroForm.Fields["Text4"].Value = new PdfString("Märkets nummer");
                document.AcroForm.Fields["Text5"].Value = new PdfString(playerMedalInfo.FormattedNextMedal.Description);

                var firstResult = top3ValidResults[0];
                var secondResult = top3ValidResults[1];
                var thirdResult = top3ValidResults[2];
                document.AcroForm.Fields["Text6"].Value = new PdfString(firstResult.Key.Date.ToString("yyyy-MM-dd"));
                document.AcroForm.Fields["Text7"].Value = new PdfString($"Omgång {firstResult.Key.Turn}");
                document.AcroForm.Fields["Text8"].Value = new PdfString(firstResult.Count().ToString());
                document.AcroForm.Fields["Text9"].Value = new PdfString(firstResult.Sum(x => x.Item1.Pins).ToString());

                document.AcroForm.Fields["1"].Value = new PdfString(secondResult.Key.Date.ToString("yyyy-MM-dd"));
                document.AcroForm.Fields["2"].Value = new PdfString($"Omgång {secondResult.Key.Turn}");
                document.AcroForm.Fields["3"].Value = new PdfString(secondResult.Count().ToString());
                document.AcroForm.Fields["4"].Value = new PdfString(secondResult.Sum(x => x.Item1.Pins).ToString());

                document.AcroForm.Fields["5"].Value = new PdfString(thirdResult.Key.Date.ToString("yyyy-MM-dd"));
                document.AcroForm.Fields["6"].Value = new PdfString($"Omgång {thirdResult.Key.Turn}");
                document.AcroForm.Fields["7"].Value = new PdfString(thirdResult.Count().ToString());
                document.AcroForm.Fields["8"].Value = new PdfString(thirdResult.Sum(x => x.Item1.Pins).ToString());

                //document.AcroForm.Fields["9"].Value = new PdfString("Datum fjärde matchen");
                //document.AcroForm.Fields["10"].Value = new PdfString("Tävling eller omgång fjärde matchen");
                //document.AcroForm.Fields["11"].Value = new PdfString("Antal serier fjärde matchen");
                //document.AcroForm.Fields["12"].Value = new PdfString("Poäng fjärde matchen");
                document.AcroForm.Fields["Text10"].Value = new PdfString(postModel.Location);
                document.AcroForm.Fields["Text11"].Value = new PdfString(DateTime.Now.Date.ToString("yyyy-MM-dd"));
                document.AcroForm.Fields["Text12"].Value = new PdfString(tenantConfiguration.FullTeamName);
                //document.AcroForm.Fields["Text14"].Value = new PdfString("OrtBestyrkes");
                //document.AcroForm.Fields["Text15"].Value = new PdfString("DatumBestyrkes");
                //document.AcroForm.Fields["Text16"].Value = new PdfString("Distriktförbund");
                document.Save(entryStream);
            }

            return CreateFileEntryResult.CreatedDocument;
        }

        public class PostModel
        {
            [Required, MaxLength(40)]
            public string Location { get; set; }
        }

        private class PlayerMedalInfo
        {
            public PlayerMedalInfo(
                string playerName,
                int playerPersonalNumber,
                FormattedMedal formattedExistingMedal,
                FormattedMedal formattedNextMedal,
                HashSet<Tuple<SeasonResults.PlayerResult, bool>> playerTopThreeResults)
            {
                PlayerName = playerName;
                PlayerPersonalNumber = playerPersonalNumber;
                FormattedExistingMedal = formattedExistingMedal;
                FormattedNextMedal = formattedNextMedal;
                PlayerTopThreeResults = playerTopThreeResults;
            }

            public string PlayerName { get; }
            public int PlayerPersonalNumber { get; }
            public FormattedMedal FormattedExistingMedal { get; }
            public FormattedMedal FormattedNextMedal { get; }
            public HashSet<Tuple<SeasonResults.PlayerResult, bool>> PlayerTopThreeResults { get; }
        }
    }
}
