﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

using AKSongs.Models;

namespace AKSongs.Controllers
{
    public class MainController : Controller
    {
        private Context db = new Context();

        //[OutputCache(Duration = 1, Location = OutputCacheLocation.Client)]
        public ActionResult Index(string song)
        {
          return View();
        }

        [Route("cachemanifest")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Manifest()
        {
          var manifestResult = new ManifestResult("000000008 " + db.Songs.OrderByDescending(s => s.Modified).Select(s => s.Modified).First())
          {
            CacheResources = new [] { 
              "Content/style.css",
              "Content/cherub.png",
              "Scripts/script.js",
              "Scripts/lodash.js",
              "Scripts/lunr.js",
              "Scripts/knockout-3.2.0.js",
              "Scripts/jquery-2.1.1.js",
              "api/songs",
            },
            NetworkResources = new [] { "*" },
            FallbackResources = new Dictionary<string, string> { { "/", "/" } }
          };
          Response.Expires = 0;
          return manifestResult;
        }

        [Route("test")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TestPassword(string password)
        {
          if (password != ConfigurationManager.AppSettings["ApiKey"])
          {
            return HttpNotFound();
          }
          return Content("ok");
        }
    }
}