using System;
using System.Collections.Generic;
using System.Linq;

namespace Example4
{
    public class Program
    {

        public class PaymentRequest
        {
            public string PaymentMethodChoice { get; set; }
        }

        public class PaymentResult
        {

        }

        public class CancelRequest : IPaymentMethod
        {

            public string Name => "CANCEL";

            public PaymentResult Process(PaymentRequest request)
            {
                Console.WriteLine("Request cancelled!");
                return null;
            }
        }

        public interface IPaymentMethod
        {
            string Name { get; }

            PaymentResult Process(PaymentRequest request);
        }

        public class CashOnDelivery : IPaymentMethod
        {

            public string Name => "CASH ON DELIVERY";

            public PaymentResult Process(PaymentRequest request)
            {
                Console.WriteLine("Paid with cash on delivery");
                return null;
            }
        }

        public class CreditCardPayment : IPaymentMethod
        {

            public string Name => "PAY WITH CREDIT CARD";

            public PaymentResult Process(PaymentRequest request)
            {
                Console.WriteLine("Paid with credit card");
                return null;
            }
        }

        public class WireTransferPayment : IPaymentMethod
        {

            public string Name => "PAY WITH WIRE TRANSFER";

            public PaymentResult Process(PaymentRequest request)
            {
                Console.WriteLine("Paid with wire transfer");
                return null;
            }
        }

        public interface IPaymentService
        {
            PaymentResult Pay(PaymentRequest request);
        }

        public class PaymentService : IPaymentService
        {
            private readonly PaymentResolver paymentResolver;

            public PaymentService(PaymentResolver paymentResolver)
            {
                this.paymentResolver = paymentResolver;
            }

            public PaymentResult Pay(PaymentRequest request)
            {

                IPaymentMethod paymentMethod = paymentResolver.Resolve(request.PaymentMethodChoice);
                return paymentMethod.Process(request);
            }
        }

        static void Main(string[] args)
        {
            var paymentRequest = new PaymentRequest();
            var paymentMethods = new List<IPaymentMethod>()
            {
                new CashOnDelivery(),
                new CreditCardPayment(),
                new WireTransferPayment(),
                new CancelRequest()
            };


            int choice;
            string paymentMethodChoice = null;
            do
            {
                Console.WriteLine("Please select mode of payment:");
                var i = 1;
                foreach (var paymentMethod in paymentMethods)
                {
                    Console.WriteLine($"{i} {paymentMethod.Name}");
                    i++;
                }
                var input = Console.ReadLine();
                try
                {
                    choice = Convert.ToInt32(input);
                    choice--;
                    paymentMethodChoice = paymentMethods.ElementAtOrDefault(choice)?.Name;
                }
                catch (Exception) { }

            }
            while (paymentMethodChoice == null);

            paymentRequest.PaymentMethodChoice = paymentMethodChoice;
            var resolver = new PaymentResolver(paymentMethods);
            IPaymentService paymentService = new PaymentService(resolver);
            var result = paymentService.Pay(paymentRequest);

            Console.ReadLine();

        }

        public class PaymentResolver
        {
            private IEnumerable<IPaymentMethod> paymentMethods;

            public PaymentResolver(IEnumerable<IPaymentMethod> paymentMethods)
            {
                this.paymentMethods = paymentMethods;
            }

            public IPaymentMethod Resolve(string paymentMethodChoice)
            {
                return paymentMethods.FirstOrDefault(p => p.Name == paymentMethodChoice);
            }
        }
    }
}
