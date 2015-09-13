using causetolife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using causetolife.Helpers;

namespace causetolife.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }


    public class CauseController : Controller
    {
        public ActionResult Index()
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Causes";
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Get,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<List<Cause>>(response);
            return View(result);
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Causes";
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Get,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<List<Cause>>(response);
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(x => x.Name.Contains(name) || x.Geographies.Exists(y=>y.Contains(name))).ToList();
            }
            return View(result);
        }
        public ActionResult Create()
        {
            var countries = new List<Country> { new Country { Name = "India" }, new Country { Name = "USA" }, new Country { Name = "UK" } };
            var selectList = new SelectList(countries, "Name", "Name", null);
            ViewBag.CountryList = selectList;
            return View();
        }

        public ActionResult Details(string id)
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Causes/" + id;
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Get,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<Cause>(response);
            return View(result);
        }

        [HttpPost]
        public ActionResult Create(Cause user)
        {
            var webRequestManager = new HttpWebRequestManager();
            var json = JsonConvert.Serialize(user);
            var url = Constants.ServerApi + "Causes";
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Post,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, json);
            var result = JsonConvert.Deserialize<Cause>(response);
            return RedirectToAction("Index");
        }

        public ActionResult Donation(string id)
        {
            ViewBag.CauseId = id;
            return View();
        }

        [HttpPost]
        public ActionResult Donate(string causeId, double amount)
        {
            var id = causeId;
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Causes/" + id;
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Get,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<Cause>(response);
            var donator = new Donator
            {
                Username = Session["UserProfile"] != null && ((User)Session["UserProfile"])!= null && ((User)Session["UserProfile"]).Username != null ? ((User)Session["UserProfile"]).Username : "suman",
                Amount = amount
            };
            if (result.Sponsers == null)
                result.Sponsers = new List<Donator>();
            result.Sponsers.Add(donator);
            var json = JsonConvert.Serialize(result);
            requestConfig = new RequestSettings
            {
                Method = HttpMethod.Put,
            };
            headers = ServiceHelper.AddHeaders("application/json");
            response = webRequestManager.GetResponse(url, requestConfig, headers, null, json);
            result = JsonConvert.Deserialize<Cause>(response);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Causes/" + id;
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Delete,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<Cause>(response);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Like(string id)
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Causes/" + id;
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Get,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<Cause>(response);

            result.Votes++;
            var json = JsonConvert.Serialize(result);
            var requestConfig1 = new RequestSettings
            {
                Method = HttpMethod.Put,
            };
            var headers1 = ServiceHelper.AddHeaders("application/json");
            var response1 = webRequestManager.GetResponse(url, requestConfig1, headers1, null, json);
            var result1 = JsonConvert.Deserialize<Cause>(response1);
            return RedirectToAction("Index");
        }
    }

    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Users";
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Get,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<List<User>>(response);
            return View(result);
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Causes";
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Get,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<List<Cause>>(response);
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(x => x.Name.Contains(name) || x.Geographies.Contains(name)).ToList();
            }
            return View(result);
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Users";
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Get,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<List<User>>(response);
            var result1 = result.Where(x=>x.Username ==username).FirstOrDefault();
            if (result1 != null)
            {
                Session["UserProfile"] = result1;
                return RedirectToAction("Index", "Cause");
            }
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logoff()
        {
            Session["UserProfile"] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            var webRequestManager = new HttpWebRequestManager();
            var json = JsonConvert.Serialize(user);
            var url = Constants.ServerApi + "Users";
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Post,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, json);
            var result = JsonConvert.Deserialize<User>(response);
            return RedirectToAction("Index");
            //return View();
        }
        public ActionResult Edit(string id)
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Users/" + id;
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Get,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<User>(response);
            return View();
        }

        [HttpPost]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var webRequestManager = new HttpWebRequestManager();
            var url = Constants.ServerApi + "Users/" + id ;
            var requestConfig = new RequestSettings
            {
                Method = HttpMethod.Delete,
            };
            var headers = ServiceHelper.AddHeaders("application/json");
            var response = webRequestManager.GetResponse(url, requestConfig, headers, null, null);
            var result = JsonConvert.Deserialize<User>(response);
            return RedirectToAction("Index");
        }
    }

    public class Country
    {
        public string Name { get; set; }
    }

    public class Constants
    {
        public const string ServerApi = "http://localhost:3000/";
    }
}
