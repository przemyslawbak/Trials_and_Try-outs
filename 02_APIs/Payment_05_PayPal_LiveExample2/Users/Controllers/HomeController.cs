using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System;
using System.Collections.Generic;
using Users.Infrastructure;

namespace Users.Controllers
{
    //https://www.codeproject.com/Articles/870870/Using-Paypal-Rest-API-with-ASP-NET-MVC
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PaymentWithCreditCard()
        {
            //create and item for which you are taking payment
            //if you need to add more items in the list
            //Then you will need to create multiple item objects or use some loop to instantiate object
            Item item = new Item
            {
                name = "Demo Item",
                currency = "USD",
                price = "0.1",
                quantity = "1",
                sku = "sku"
            };

            //Now make a List of Item and add the above item to it
            //you can create as many items as you want and add to this list
            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;

            //Address for the payment
            Address billingAddress = new Address
            {
                city = "NewYork",
                country_code = "US",
                line1 = "23rd street kew gardens",
                postal_code = "43210",
                state = "NY"
            };


            //Now Create an object of credit card and add above details to it
            //Please replace your credit card details over here which you got from paypal
            CreditCard crdtCard = new CreditCard
            {
                billing_address = billingAddress,
                cvv2 = "874",  //card cvv2 number
                expire_month = 1, //card expire date
                expire_year = 2020, //card expire year
                first_name = "Aman",
                last_name = "Thakur",
                number = "1234567890123456", //enter your credit card number here
                type = "visa" //credit card type here paypal allows 4 types
            };

            // Specify details of your payment amount.
            Details details = new Details
            {
                shipping = "0.1",
                subtotal = "0.1",
                tax = "0.1"
            };

            // Specify your total payment amount and assign the details object
            Amount amnt = new Amount
            {
                currency = "USD",
                // Total = shipping tax + subtotal.
                total = "0.3",
                details = details
            };

            // Now make a transaction object and assign the Amount object
            Transaction tran = new Transaction
            {
                amount = amnt,
                description = "Description about the payment amount.",
                item_list = itemList,
                invoice_number = "your invoice number which you are generating"
            };

            // Now, we have to make a list of transaction and add the transactions object
            // to this list. You can create one or more object as per your requirements

            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);

            // Now we need to specify the FundingInstrument of the Payer
            // for credit card payments, set the CreditCard which we made above

            FundingInstrument fundInstrument = new FundingInstrument
            {
                credit_card = crdtCard
            };

            // The Payment creation API requires a list of FundingIntrument

            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);

            // Now create Payer object and assign the fundinginstrument list to the object
            Payer payr = new Payer
            {
                funding_instruments = fundingInstrumentList,
                payment_method = "credit_card"
            };

            // finally create the payment object and assign the payer object & transaction list to it
            Payment payment = new Payment
            {
                intent = "sale",
                payer = payr,
                transactions = transactions
            };

            try
            {
                //getting context from the paypal
                //basically we are sending the clientID and clientSecret key in this function
                //to the get the context from the paypal API to make the payment
                //for which we have created the object above.

                //Basically, apiContext object has a accesstoken which is sent by the paypal
                //to authenticate the payment to facilitator account.
                //An access token could be an alphanumeric string

                APIContext apiContext = PaypalConfiguration.GetAPIContext();

                //Create is a Payment class function which actually sends the payment details
                //to the paypal API for the payment. The function is passed with the ApiContext
                //which we received above.

                Payment createdPayment = payment.Create(apiContext); //bad request

                //if the createdPayment.state is "approved" it means the payment was successful else not

                if (createdPayment.state.ToLower() != "approved")
                {
                    return View("FailureView");
                }
            }
            catch (PayPal.PayPalException ex)
            {
                return View("FailureView");
            }

            return View("SuccessView");
        }

        public IActionResult PaymentWithPaypal()
        {
            //https://localhost:44339//Paypal/PaymentWithPayPal?guid=82683&paymentId=PAYID-LYKLVDA58799230CY830741G&token=EC-6XC85037HU899221S&PayerID=VZKW9KXWH4R38
            //getting the apiContext as earlier
            APIContext apiContext = PaypalConfiguration.GetAPIContext();


            try
            {
                string payerId = Request.Query["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = "https://localhost:44339//Paypal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    string guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    Payment createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    List<Links>.Enumerator links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    //https://benjii.me/2016/07/using-sessions-and-httpcontext-in-aspnetcore-and-mvc-core/
                    HttpContext.Session.SetString(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters
                    // from the previous call to the function Create
                    // Executing a payment

                    string guid = Request.Query["guid"];

                    string payer = HttpContext.Session.GetString(guid);

                    var executedPayment = ExecutePayment(apiContext, payerId, payer);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
                //z kartą: https://localhost:44339//Paypal/PaymentWithPayPal?guid=9140&paymentId=PAYID-LYKHJCQ3RK12273RS689210W&token=EC-9XH215117A886852A&PayerID=KV
            }

            return View("SuccessView");
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            itemList.items.Add(new Item()
            {
                name = "Item Name",
                currency = "USD",
                price = "0.1",
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "0.1",
                shipping = "0.1",
                subtotal = "0.1"
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = "0.3", // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "your invoice number",
                amount = amount,
                item_list = itemList
            });

            var payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return payment.Create(apiContext);
        }
    }
}










//- nie działa - NA POCZĄTEK: install i test SDK core https://github.com/paypal/PayPal-NET-SDK/tree/2.0-beta
//- payment config problem - przyklad: https://www.c-sharpcorner.com/article/paypal-payment-gateway-integration-in-asp-net-mvc/
//inny przykład: https://www.codeproject.com/Articles/870870/Using-Paypal-Rest-API-with-ASP-NET-MVC
//tutorial: https://www.ittutorialswithexample.com/2018/03/paypal-payment-gateway-integration-in-aspnet.html
//sandbox test: https://stackoverflow.com/questions/51124822/sandbox-test-second-payment-failed-paypal-integration-asp-net-mvc
//https://medium.com/@pmareke/using-paypal-sdk-with-net-core-full-explanation-66aab76cef66
//https://stackoverflow.com/questions/50588921/asp-net-core-paypal-implementation