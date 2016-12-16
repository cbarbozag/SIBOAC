﻿using System.Web;
using System.Web.Optimization;

namespace Cosevi.SIBOAC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/listgroup.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                      "~/Scripts/Toastr-2.1.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                     "~/Scripts/app.js"));

            // Report: reportePorUsuario bundles
            bundles.Add(new ScriptBundle("~/bundles/reportePorUsuario").Include(
                     "~/Scripts/reportePorUsuario.js"));

            // Report: ReportePorEstadoActualDelPlano bundles
            bundles.Add(new ScriptBundle("~/bundles/ReportePorEstadoActualDelPlano").Include(
                     "~/Scripts/reportePorEstadoActualDelPlano.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datepicker.min.css",
                      "~/Content/Toastr-2.1.3.min.css",
                      "~/Content/site.css"));


            
        }
    }
}
