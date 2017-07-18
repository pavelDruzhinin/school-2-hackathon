using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RosCottedge.ViewModels
{
    public class EditMyHouseViewModel
    {
        public IQueryable<ReviewsNotices> ReviewsNotices { get; set; }
        public IQueryable<ReservationNotices> ReservationNotices { get; set; }
        public IQueryable<Picture> Pictures { get; set; }
        public House House { get; set; }
        public User User { get; set; }
    }
}