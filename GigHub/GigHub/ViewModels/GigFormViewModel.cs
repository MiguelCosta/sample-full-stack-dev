using GigHub.Controllers;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GigHub.ViewModels
{
    public class GigFormViewModel
    {
        public string Action
        {
            get
            {
                Expression<Func<GigsController, Task<ActionResult>>> update =
                    (c => c.Update(this));

                Expression<Func<GigsController, Task<ActionResult>>> create =
                    (c => c.Create(this));

                var action = (Id != 0) ? update : create;
                var actionName = (action.Body as MethodCallExpression).Method.Name;

                return actionName;
            }
        }

        [Required]
        [FutureDate]
        public string Date { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public string Heading { get; set; }

        public int Id { get; set; }

        [Required]
        [ValidTime]
        public string Time { get; set; }

        [Required]
        public string Venue { get; set; }

        public DateTime GetDateTime()
        {
            return DateTime.Parse($"{Date} {Time}");
        }
    }
}
