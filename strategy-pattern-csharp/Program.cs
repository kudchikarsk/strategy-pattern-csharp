﻿using System;

namespace strategy_pattern_csharp
{
    class Program
    {
        public enum PaymentMethodChoice { CashOnDelivery, CreditCardPayment }

        public class PaymentRequest
        {
            public PaymentMethodChoice PaymentMethodChoice { get; set; }
        }

        public class PaymentResult
        {

        }

        public class PaymentService
        {
            public PaymentResult Pay(PaymentRequest request)
            {
                switch (request.PaymentMethodChoice)
                {
                    case PaymentMethodChoice.CashOnDelivery:
                        //Here our payment operation will get execute
                        Console.WriteLine("Paid with cash on delivery");
                        return null;
                    case PaymentMethodChoice.CreditCardPayment:
                        //Here our payment operation will get execute
                        Console.WriteLine("Paid with credit card");
                        return null;
                }

                throw new Exception("Failed to process payment request!");
            }
        }

        static void Main(string[] args)
        {
            var paymentRequest = new PaymentRequest();
            var choice = String.Empty;
            do
            {
                Console.WriteLine("Please select mode of payment:");
                Console.WriteLine("1. CASH ON DELIVERY");
                Console.WriteLine("2. PAY WITH CREDIT CARD");
                Console.WriteLine("3. CANCEL");
                choice = Console.ReadLine();
            }
            while (choice != "1" && choice != "2" && choice != "3");

            switch (choice)
            {
                case "1":
                    paymentRequest.PaymentMethodChoice
                        = PaymentMethodChoice.CashOnDelivery;
                    break;

                case "2":
                    paymentRequest.PaymentMethodChoice
                        = PaymentMethodChoice.CreditCardPayment;
                    break;

                default:
                    Console.WriteLine("Payment request canceled");
                    return;
            }

            var paymentService = new PaymentService();
            var result = paymentService.Pay(paymentRequest);

            Console.ReadLine();
        }
    }
}
 