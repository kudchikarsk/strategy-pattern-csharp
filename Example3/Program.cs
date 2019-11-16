using System;
using System.Collections.Generic;
using System.Linq;

namespace Example3
{
    public class Program
    {
        public enum PaymentMethodChoice
        {
            CashOnDelivery,
            CreditCardPayment,
            CancelRequest
        }

        public class PaymentRequest
        {
            public PaymentMethodChoice PaymentMethodChoice { get; set; }
        }

        public class PaymentResult
        {

        }

        public class CancelRequest : IPaymentMethod
        {
            public PaymentMethodChoice PaymentMethod => PaymentMethodChoice.CancelRequest;

            public string Name => "CANCEL";

            public PaymentResult Process(PaymentRequest request)
            {
                Console.WriteLine("Request cancelled!");
                return null;
            }
        }

        public interface IPaymentMethod
        {
            PaymentMethodChoice PaymentMethod { get; }
            string Name { get; }

            PaymentResult Process(PaymentRequest request);
        }

        public class CashOnDelivery : IPaymentMethod
        {
            public PaymentMethodChoice PaymentMethod => PaymentMethodChoice.CashOnDelivery;

            public string Name => "CASH ON DELIVERY";

            public PaymentResult Process(PaymentRequest request)
            {
                Console.WriteLine("Paid with cash on delivery");
                return null;
            }
        }

        public class CreditCardPayment : IPaymentMethod
        {
            public PaymentMethodChoice PaymentMethod => PaymentMethodChoice.CreditCardPayment;

            public string Name => "PAY WITH CREDIT CARD";

            public PaymentResult Process(PaymentRequest request)
            {
                Console.WriteLine("Paid with credit card");
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

                IPaymentMethod paymentMethod
                    = paymentResolver.Resolve(request.PaymentMethodChoice);
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
            new CancelRequest()
        };


            var choice = paymentMethods.Count;
            PaymentMethodChoice paymentMethodChoice;
            do
            {
                Console.WriteLine("Please select mode of payment:");
                foreach (var paymentMethod in paymentMethods)
                {
                    Console.WriteLine(
                        $"{(int)paymentMethod.PaymentMethod + 1} {paymentMethod.Name}");
                }
                var input = Console.ReadLine();
                choice = Convert.ToInt32(input);
                choice--;
            }
            while (!Enum.TryParse(choice.ToString(), out paymentMethodChoice)
                || !Enum.IsDefined(typeof(PaymentMethodChoice), paymentMethodChoice));


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

            public IPaymentMethod Resolve(PaymentMethodChoice paymentMethodChoice)
            {
                return paymentMethods
                    .FirstOrDefault(p => p.PaymentMethod == paymentMethodChoice);
            }
        }
    }
}
