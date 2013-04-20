using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace UnoTV.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/html5shiv").Include(
                "~/Scripts/html5shiv.js"
                ));

            bundles.Add(new ScriptBundle("~/js").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.signalR-{version}.js",
                "~/Scripts/knockout-{version}.js"
                ));

            bundles.Add(new LessBundle("~/content/less").Include(
                "~/Content/style.less" 
                ));

            bundles.Add(new ScriptBundle("~/table").Include(
                "~/Scripts/table.js"
                ));

            bundles.Add(new ScriptBundle("~/player").Include(
                "~/Scripts/player.js"
                ));
        }
    }
}