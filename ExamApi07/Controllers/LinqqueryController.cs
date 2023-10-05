using ExamApi07.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamApi07.Controllers
{
    public class LinqqueryController : Controller
    {
        private APEntities db = new APEntities();

        public ActionResult Index()
        {

            var maxs = db.Invoices.Max(m => m.InvoiceTotal);
            ViewData["maxs"] = maxs;
            var min = db.Invoices.Min(m => m.InvoiceTotal);
            @ViewData["mins"] = min;
            var AVG = db.Invoices.Average(m => m.InvoiceTotal);
            @ViewData["AVG"] = AVG;
            var sum = db.Invoices.Sum(m => m.InvoiceTotal);
            @ViewData["sum"] = sum;
            var count = db.Invoices.Count(m => m.InvoiceTotal > 0);
            @ViewData["count"] = count;
            return View(db.Invoices.ToList());

        }




    }

}