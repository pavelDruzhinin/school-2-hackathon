using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RosCottedge.Models;
using Webdiyer.WebControls.Mvc;

namespace RosCottedge.ViewModels
{
    public class HouseIndexViewModel
    {
        public PagedList<Review> Reviews { get; set; }
        public House House { get; set; }
        public bool AllowComments { get; set; }
        public List<Picture> Pictures { get; set; }
        public User User { get; set; }
    }
}