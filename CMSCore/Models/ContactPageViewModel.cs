﻿using CMSCore.Application.ViewModels;

namespace CMSCore.Models
{
    public class ContactPageViewModel
    {
        public ContactViewModel Contact { set; get; }

        public FeedbackViewModel Feedback { set; get; }
    }
}