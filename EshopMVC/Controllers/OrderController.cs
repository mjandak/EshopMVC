﻿using EshopMVC.Models;
using EshopMVC.Models.Cart;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EshopMVC.Models.Order;

namespace EshopMVC.Controllers
{
    public class OrderController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Create(IEnumerable<CartItemViewModel> items)
        {
            ApplicationUser user = UserManager.FindByName(User.Identity.Name);
            using (var db = new DB_9FCCB1_eshopEntities())
            {
                var order = new Order();
                order.CreateDate = DateTime.Now;
                order.UserId = user.Id;
                order.OrderProduct = items.Select(
                    i => new OrderProduct()
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    }).ToArray();

                db.Order.Add(order);
                db.SaveChanges();
            }
            return View("Created");
        }

        [Authorize]
        public ActionResult List()
        {
            ApplicationUser user = UserManager.FindByName(User.Identity.Name);
            using (var db = new DB_9FCCB1_eshopEntities())
            {
                var orders = db.Order.Where(o => o.UserId == user.Id).ToArray();
                var model = orders.Select(o =>
                    new OrderSummaryViewModel(o)).ToArray();
                return View(model);
            }
        }
    }
}