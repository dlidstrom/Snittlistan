using System.IO;
using Cassette;
using Cassette.HtmlTemplates;
using Cassette.Scripts;
using Cassette.Stylesheets;

namespace Snittlistan.Web.Infrastructure
{
    /// <summary>
    /// Configures the Cassette asset bundles for the web application.
    /// </summary>
    public class CassetteBundleConfiguration : IConfiguration<BundleCollection>
    {
        public void Configure(BundleCollection bundles)
        {
            // Please read http://getcassette.net/documentation/configuration

            // This default configuration treats each file as a separate 'bundle'.
            // In production the content will be minified, but the files are not combined.
            // So you probably want to tweak these defaults!
            // bundles.AddPerIndividualFile<StylesheetBundle>("Content");
            // bundles.AddPerIndividualFile<ScriptBundle>("Scripts");
            bundles.AddPerIndividualFile<StylesheetBundle>("Content/css");
            bundles.AddPerIndividualFile<ScriptBundle>(
                "Content/js",
                new FileSearch { SearchOption = SearchOption.TopDirectoryOnly });
            bundles.AddPerIndividualFile<ScriptBundle>("Content/js/helpers");
            bundles.AddPerSubDirectory<ScriptBundle>("Content/js/app");
            bundles.AddPerIndividualFile<ScriptBundle>("Content/external/hogan");
            bundles.Add<HtmlTemplateBundle>("Content/templates");

            // To combine files, try something like this instead:
            //   bundles.Add<StylesheetBundle>("Content");
            // In production mode, all of ~/Content will be combined into a single bundle.

            // If you want a bundle per folder, try this:
            //   bundles.AddPerSubDirectory<ScriptBundle>("Scripts");
            // Each immediate sub-directory of ~/Scripts will be combined into its own bundle.
            // This is useful when there are lots of scripts for different areas of the website.
        }
    }
}