﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Web.Models
{
    public class Home
    {
        public int HomeId { get; set; }
        public string WelcomeMessage { get; set; }
        public string FooterMessage { get; set; } = "Footer by @MiguelMorales63";

    }
}