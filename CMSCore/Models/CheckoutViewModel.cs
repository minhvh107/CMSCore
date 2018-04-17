using CMSCore.Application.ViewModels;
using CMSCore.Data.Enums;
using CMSCore.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMSCore.Models
{
    public class CheckoutViewModel : BillViewModel
    {
        public List<ShoppingCartViewModel> Carts { get; set; }

        public List<EnumViewModel> PaymentMethods
        {
            get
            {
                return ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                    .Select(c => new EnumViewModel
                    {
                        Value = (int)c,
                        Text = c.GetDescription()
                    }).ToList();
            }
        }
    }
}