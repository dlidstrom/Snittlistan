namespace Snittlistan.Web.Infrastructure
{
    using System.IO;
    using Cassette;
    using Cassette.BundleProcessing;
    using Cassette.Scripts;
    using Cassette.Stylesheets;

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

            // these files are using newer JS syntax that the minifier doesn't support
            bundles.AddPerIndividualFile<ScriptBundle>(
                "Content/js",
                new FileSearch { SearchOption = SearchOption.TopDirectoryOnly },
                x =>
                {
                    var i = x.Pipeline.IndexOf<MinifyAssets>();
                    if (i >= 0)
                    {
                        x.Pipeline.RemoveAt(i);
                    }
                });
            bundles.AddPerIndividualFile<ScriptBundle>("Content/js/helpers");
            bundles.AddPerIndividualFile<ScriptBundle>("Content/external");
            bundles.AddPerIndividualFile<StylesheetBundle>("Content/external");

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