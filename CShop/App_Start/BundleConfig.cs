using System.Web;
using System.Web.Optimization;

namespace WebApplication1
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/script/jquery").Include(
                        "~/Scripts/jquery-1.11.1.js"));

            bundles.Add(new ScriptBundle("~/script/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/script/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/script/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/custom_bootstrap.css"
                        ));

            //my script
            bundles.Add(new ScriptBundle("~/script/myscripts").Include(
                "~/Scripts/default.js"
            ));

            //my style
            bundles.Add(new StyleBundle("~/css/mystyles").Include(
                "~/Content/default.css"
                
            ));
        }
    }
}
