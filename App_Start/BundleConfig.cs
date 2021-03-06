﻿using System.Web;
using System.Web.Optimization;

namespace ThrowdownAttire
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.11.4.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/shirt").Include(
                      "~/Scripts/OwlCarousel/owl.carousel.js",
                      "~/Scripts/shirt.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/series").Include(
                      "~/Scripts/series.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/floats").Include(
                      "~/Scripts/floats.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/edit").Include(
                      "~/Scripts/edit.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                      "~/Scripts/TableSorter/jquery.tablesorter.js",
                      "~/Scripts/admin.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/OwlCarousel/owl.carousel.css",
                      "~/Content/OwlCarousel/owl.theme.css",
                      "~/Content/OwlCarousel/owl.transitions.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/edit").Include(
                          "~/Content/themes/base/all.css"));
        }
    }
}
