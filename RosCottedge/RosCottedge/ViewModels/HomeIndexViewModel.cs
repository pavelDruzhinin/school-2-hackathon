﻿using RosCottedge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace RosCottedge.ViewModels
{
    public class HomeIndexViewModel
    {
        public PagedList<House> Houses { get; set; }
        public List<string> Regions { get; set; }
    }
}