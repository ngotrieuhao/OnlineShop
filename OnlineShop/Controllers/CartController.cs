using OnlineShop.ApiIntegration;
using OnlineShop.Utilities.Constants;
using OnlineShop.ViewModels.Sales;
using OnlineShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Stripe;
using System.Net.Http;
using System.Text;
using Stripe.Checkout;
using PaymentMethod = OnlineShop.ViewModels.Utilities.Enums.PaymentMethod;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderApiClient;
        private readonly IUserApiClient _userApiClient;
        private readonly ICouponApiClient _couponApiClient;

        public CartController(IProductApiClient productApiClient, IOrderApiClient orderApiClient, IUserApiClient userApiClient, ICouponApiClient couponApiClient)
        {
            _productApiClient = productApiClient;
            _orderApiClient = orderApiClient;
            _userApiClient = userApiClient;
            _couponApiClient = couponApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Checkout()
        {
            return View(GetCheckoutViewModel());
        }

        [HttpPost]
        [Authorize]
        //[Consumes("multipart/form-data")]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            var currentCart = new CartViewModel();
            currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            long price = 0;
            float sub_price = 0;

            if (currentCart.Promotion != 0)
            {
                var promotion = currentCart.Promotion;
                sub_price = (float)(currentCart.CartItems.Sum(x => x.Price * x.Quantity));
                price = (long)((long)sub_price * (100f - promotion) / 100f);
            }
            else
            {
                price = (long)currentCart.CartItems.Sum(x => x.Price * x.Quantity);
            }

            // Tìm Guid của người mua để gán vào order
            var claims = User.Claims.ToList();
            var userId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var users = await _userApiClient.GetAll();
            var x = users.FirstOrDefault(x => x.Id.ToString() == userId);

            // Order detail là lấy từ session chứ không lấy qua CheckoutViewModel, vì model binding không có bind cái danh sách sản phẩm
            var model = GetCheckoutViewModel();
            var orderDetails = new List<OrderDetailViewModel>();

            foreach (var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailViewModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            var checkoutRequest = new CheckoutRequest()
            {
                UserID = x.Id,
                Address = request.CheckoutModel.Address,
                Name = request.CheckoutModel.Name,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                OrderDetails = orderDetails,
                PaymentMethod = PaymentMethod.COD,
                Total = price,
            };

            if (model.CouponCode != null)
            {
                var coupons = await _couponApiClient.GetAll();
                var coupon = coupons.FirstOrDefault(x => x.Code == model.CouponCode);
                checkoutRequest.CouponId = coupon.Id;
            }

            var result = await _orderApiClient.CreateOrder(checkoutRequest);

            if (result != "Failed")
            {
                // mail admin when have new email
                var email1 = new EmailService.EmailService();
                email1.Send("ngotrieuhao258@gmail.com", "ngotrieuhao258@gmail.com",
                    "NEW ORDERS", $"Code orders is <strong style='font-size: 20px;'>{result}</strong>, click <a href='" + "https://localhost:5002/Order/Detail?orderId=" + result + "'>HERE</a> to go to this order management page.");

                var orderSummaryHtml = "<table border='1' style='border-collapse:collapse'>"
                        + "<thead>"
                        + "<tr>"
                        + "<th style='width:15rem; '>Product's name</th>"
                        + "<th style='width:4rem; '>Price</th>"
                        + "<th style='width:4rem;'>Quantity</th>"
                        + "<th style='width:6rem; '>Total Price</th>"
                        + "</tr>"
                        + "</thead>"
                        + "<tbody>";
                decimal total = 0;
                decimal amount = 0;
                // mail client when placed order successfully
                foreach (var product in currentCart.CartItems)
                {
                    amount = product.Price * product.Quantity;
                    orderSummaryHtml +=
                        "<tr style='text-align: center; '>"
                        + "<td>" + product.Name + "</td>"
                        + "<td>" + " <span>&#36;</span>" + product.Price.ToString("N0")  + "</td>"
                        + "<td>" + product.Quantity
                        + "</td>"
                        + "<td>"+ " <span>&#36;</span>" + amount.ToString("N0")  + "</td>"
                        + "</tr>"
                        + "</tbody>";

                    total += amount;
                };

                if (currentCart.Promotion != 0)
                {
                    
                    orderSummaryHtml +=
                        "<tfoot>"
                        + "<tr style='text-align: center; '>"
                        + "<td><strong>Total Price</strong></td>"
                        + $"<td><strong> <span>&#36;</span>{sub_price:N0} </strong></td>"
                        + "</tr>"
                        + "<tr style='text-align: center; '>"
                        + "<td><strong>Price has decreased</strong></td>"
                        + $"<td><strong>  <span>&#36;</span>{sub_price - (sub_price * ((100f - model.Promotion) / 100f)):N0} </strong></td>"
                        + "</tr>"
                        + "<tr style='text-align: center; '>"
                        + "<td><strong>Total order price has been decreased</strong></td>"
                        + $"<td><strong>  <span>&#36;</span>{price:N0} </strong></td>"
                        + "</tr>"
                        + "</tfoot>"
                        + "</table>"
                        + $"<p>Name: {request.CheckoutModel.Name} </p> \r\n <p>Address: {request.CheckoutModel.Address} </p> \r\n <p>Phone Number: {request.CheckoutModel.PhoneNumber} </p>";
                }
                else
                {
                    orderSummaryHtml +=
                        "<tfoot>"
                        + "<tr style='text-align: center; '>"
                        + "<td><strong>Total Price</strong></td>"
                        + $"<td><strong>  <span>&#36;</span>{price:N0} </strong></td>"
                        + "</tr>"
                        + "</tfoot>"
                        + "</table>"
                        + $"<p>Name: {request.CheckoutModel.Name} </p> \r\n <p>Address: {request.CheckoutModel.Address} </p> \r\n <p>Phone Number: {request.CheckoutModel.PhoneNumber} </p>";
                }

                var templateHtml = "<h1>HaoHao Store</h1>" + "<br>"
                            + $"<h2>Your order has been successfully placed! Your order will be approved soon."
                            + "<br>"
                            + $"ID Order is {result}"
                            + "<br>"
                            + "<h3>List of ordered products</h3>"
                            + "<br>";

                var userMail = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var email2 = new EmailService.EmailService();
                email2.Send("ngotrieuhao258@gmail.com", userMail,
                                "ORDER SUCCESSFULLY",
                                templateHtml
                                + orderSummaryHtml
                                );

                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
                currentCart.CartItems.Clear();
                currentCart.Promotion = 0;
                HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
                TempData["SuccessMsg"] = "Order purchased successful";
                return View(request);
            }

            ModelState.AddModelError("", "ORDER FAILED");
            return View(request);
        }

        [TempData]
        public string TotalAmount { get; set; }

        [HttpGet]
        public IActionResult Payment()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            ViewBag.cart = currentCart;
            ViewBag.DollarAmount = currentCart.Sum(x => x.Price * x.Quantity);
            ViewBag.total = Math.Round(ViewBag.DollarAmount, 2) * 100 ;
            ViewBag.total = Convert.ToInt64(ViewBag.total);
            long total = ViewBag.total;
            TotalAmount = total.ToString();
            return View();
        }

        // Thanh toán online
        [HttpPost]
        public async Task<IActionResult> Processing(string stripeToken, string stripeEmail, CheckoutViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            var currentCart = new CartViewModel();
            currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            long price = 0;
            float sub_price = 0f;

            if (currentCart.Promotion != 0)
            {
                var promotion = currentCart.Promotion;
                sub_price = (float)(currentCart.CartItems.Sum(x => x.Price * x.Quantity));
                price = (long)((long)sub_price * (100f - promotion) );
            }
            else
            {
                price = (long)currentCart.CartItems.Sum(x => x.Price * x.Quantity);
            }

            var claims = User.Claims.ToList();
            var userEmail = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            var address = new AddressOptions()
            {
                Line1 = request.CheckoutModel.Address
            };

            var optionsCust = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Name = request.CheckoutModel.Name,
                Phone = request.CheckoutModel.PhoneNumber,
                Address = address
            };

            var serviceCust = new CustomerService();
            Customer customer = serviceCust.Create(optionsCust);

            var shipping = new ChargeShippingOptions()
            {
                Name = request.CheckoutModel.Name,
                Address = new AddressOptions()
                {
                    Line1 = request.CheckoutModel.Address
                }
            };

            var optionsCharge = new ChargeCreateOptions
            {
                /*Amount = HttpContext.Session.GetLong("Amount")*/
                //Amount = Convert.ToInt64(TempData["TotalAmount"]),
                Amount = price,
                Currency = "USD",
                Description = "Order Shoes at HaoHao Store",
                Source = stripeToken,
                Shipping = shipping,
                ReceiptEmail = stripeEmail,
            };

            var service = new ChargeService();
            Charge charge = service.Create(optionsCharge);
            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                ViewBag.AmountPaid = Convert.ToDecimal(charge.Amount) % 100  + (charge.Amount) / 100;
                ViewBag.BalanceTxId = BalanceTransactionId;
                ViewBag.Customer = customer.Name;
                //return View();
            }

            // Tìm Guid của người mua để gán vào order
            var users = await _userApiClient.GetAll();
            var x = users.FirstOrDefault(x => x.Email == userEmail);

            // Order detail là lấy từ session chứ không lấy qua CheckoutViewModel, vì model binding không có bind cái danh sách sản phẩm
            var model = GetCheckoutViewModel();
            var orderDetails = new List<OrderDetailViewModel>();

            foreach (var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailViewModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            var checkoutRequest = new CheckoutRequest()
            {
                UserID = x.Id,
                Address = request.CheckoutModel.Address,
                Name = request.CheckoutModel.Name,
                PhoneNumber = request.CheckoutModel.PhoneNumber,
                OrderDetails = orderDetails,
                PaymentMethod = PaymentMethod.CreditCard,
                Total = price,
            };

            if (model.CouponCode != null)
            {
                var coupons = await _couponApiClient.GetAll();
                var coupon = coupons.FirstOrDefault(x => x.Code == model.CouponCode);
                checkoutRequest.CouponId = coupon.Id;
            }

            var result = await _orderApiClient.CreateOrder(checkoutRequest);

            if (result != "Failed")
            {
                // mail admin when have new email
                var email1 = new EmailService.EmailService();
                email1.Send("ngotrieuhao258@gmail.com", "ngotrieuhao258@gmail.com",
                    "NEW ORDERS", $"Code orders is <strong>{result}</strong>, click <a href='" + "https://localhost:5002/Order/Detail?orderId=" + result + "'>HERE</a> to go to this order management page.");

                var orderSummaryHtml = "<table border='1' style='border-collapse:collapse'>"
                        + "<thead>"
                        + "<tr>"
                        + "<th style='width:15rem; '>Product's Name</th>"
                        + "<th style='width:4rem; '>Price</th>"
                        + "<th>Quantity</th>"
                        + "<th style='width:6rem; '>Total Price</th>"
                        + "</tr>"
                        + "</thead>"
                        + "<tbody>";
                decimal total = 0;
                decimal amount = 0;
                // mail client when placed order successfully
                foreach (var product in currentCart.CartItems)
                {
                    amount = product.Price * product.Quantity;
                    orderSummaryHtml +=
                        "<tr style='text-align: center; '>"
                        + "<td>" + product.Name + "</td>"
                        + "<td>" + "<span>&#36;</span>"+product.Price.ToString("N0") + "</td>"
                        + "<td>" + product.Quantity
                        + "</td>"
                        + "<td>" + "<span>&#36;</span>" + amount.ToString("N0") + "</td>"
                        + "</tr>"
                        + "</tbody>";

                    total += amount;
                };

                if (currentCart.Promotion != 0)
                {
                    orderSummaryHtml +=
                        "<tfoot>"
                        + "<tr style='text-align: center; '>"
                        + "<td><strong>Total Price</strong></td>"
                        + $"<td><strong> <span>&#36;</span>{sub_price:N0} </strong></td>"
                        + "</tr>"
                        + "<tr style='text-align: center; '>"
                        + "<td><strong>Price has decreased</strong></td>"
                        + $"<td><strong> <span>&#36;</span>{sub_price - (sub_price * ((100f - model.Promotion) / 100f)):N0} </strong></td>"
                        + "</tr>"
                        + "<tr style='text-align: center; '>"
                        + "<td><strong>Total order price has been decreased</strong></td>"
                        + $"<td><strong> <span>&#36;</span>{price:N0} </strong></td>"
                        + "</tr>"
                        + "</tfoot>"
                        + "</table>";
                }
                else
                {
                    orderSummaryHtml +=
                        "<tfoot>"
                        + "<tr style='text-align: center; '>"
                        + "<td><strong>Total Price</strong></td>"
                        + $"<td><strong> <span>&#36;</span>{price:N0} </strong></td>"
                        + "</tr>"
                        + "</tfoot>"
                        + "</table>";
                }

                var templateHtml = "<h1>HaoHao Store</h1>" + "<br>"
                            + $"<h2>Your order has been successfully placed! Your order will be approved soon."
                            + "<br>"
                            + $"Code orders is {result}"
                            + "<br>"
                            + "<h3>List of ordered products</h3>"
                            + "<br>";

                var userMail = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var email2 = new EmailService.EmailService();
                email2.Send("ngotrieuhao258@gmail.com", userMail,
                                "ORDER SUCCESSFULLY",
                                templateHtml
                                + orderSummaryHtml
                                );

                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
                currentCart.CartItems.Clear();
                currentCart.Promotion = 0;
                HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));
                TempData["SuccessMsg"] = "Order purchased successful";
                return View(request);
            }

            ModelState.AddModelError("", "ORDER FAILED");
            return View(request);

            //return View("Index", "Home");
        }

        private CheckoutViewModel GetCheckoutViewModel()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            //var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var claims = User.Claims.ToList();

            var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var address = claims.FirstOrDefault(x => x.Type == ClaimTypes.StreetAddress).Value;
            var phoneNumber = claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone).Value;

            var currentCart = new CartViewModel();
            currentCart.CartItems = new List<CartItemViewModel>();

            if (session != null)
            {
                //currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            }

            var checkoutVm = new CheckoutViewModel()
            {
                CartItems = currentCart.CartItems,
                CheckoutModel = new CheckoutRequest(),
                Name = name.ToString(),
                Address = address.ToString(),
                PhoneNumber = phoneNumber.ToString(),
                Promotion = currentCart.Promotion,
                CouponCode = currentCart.CouponCode
            };

            return checkoutVm;
        }

        [HttpGet]
        public IActionResult GetListItems()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            var currentCart = new CartViewModel();
            currentCart.CartItems = new List<CartItemViewModel>();

            if (session != null)
            {
                //currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            }

            return Ok(currentCart);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _productApiClient.GetById(id);

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            //List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            var currentCart = new CartViewModel();
            currentCart.CartItems = new List<CartItemViewModel>();

            if (session != null)
            {
                //currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            }

            int quantity = 1;

            if (currentCart.CartItems.Any(x => x.ProductId == id))
            {
                if (currentCart.CartItems.First(x => x.ProductId == id).Quantity == product.Stock)
                {
                    return Ok(currentCart.CartItems);
                }

                quantity = currentCart.CartItems.First(x => x.ProductId == id).Quantity + quantity;
                currentCart.CartItems.First(x => x.ProductId == id).Quantity = quantity;
            }
            else
            {
                var cartItem = new CartItemViewModel()
                {
                    ProductId = id,
                    Image = product.ThumbnailImage,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                };

                currentCart.CartItems.Add(cartItem);
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

            return Ok(currentCart);
        }

        public async Task<IActionResult> UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);

            var currentCart = new CartViewModel();
            currentCart.CartItems = new List<CartItemViewModel>();

            if (session != null)
            {
                //currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
                currentCart = JsonConvert.DeserializeObject<CartViewModel>(session);
            }

            foreach (var item in currentCart.CartItems)
            {
                if (item.ProductId == id)
                {
                    var product = await _productApiClient.GetById(item.ProductId);
                    var productStock = product.Stock;

                    if (quantity == 0 && currentCart.CartItems.Count > 1)
                    {
                        currentCart.CartItems.Remove(item);
                        break;
                    }
                    else if (quantity == 0 && currentCart.CartItems.Count == 1) // for what ?
                    {
                        currentCart.CartItems.Remove(item);
                        currentCart.Promotion = 0;
                        break;
                    }
                    else if (quantity > productStock)
                    {
                        return Content("quantity is greater than stock");
                    }
                    item.Quantity = quantity;
                }
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

            return Ok(currentCart);
        }
    }
}