using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RosCottedge.Models;
using PagedList;

namespace RosCottedge.ViewModels
{
    public class HouseIndexViewModel
    {
        public PagedList.IPagedList<Review> Reviews { get; set; }
        public House House { get; set; }
        public bool AllowComments { get; set; }
        public List<Picture> Pictures { get; set; }
    }
}