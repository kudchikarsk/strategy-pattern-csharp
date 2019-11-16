using System;

namespace Example2
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

        // This becomes our IStrategy interface
        public interface IPaymentMethod
        {
            PaymentResult Process(PaymentRequest request);
        }


        //A Strategy that implements IStrategy interface
        public class CashOnDelivery : IPaymentMethod
        {
            public PaymentResult Process(PaymentRequest request)
            {
                Console.WriteLine("Paid with cash on delivery");
                return null;
            }
        }

        //Another Strategy that implements IStrategy interface
        public class CreditCardPayment : IPaymentMethod
        {
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

        //The context class that executes the strategy applicable based on user choice
        public class PaymentService : IPaymentService
        {
            public PaymentResult Pay(PaymentRequest request)
            {

                IPaymentMethod paymentMethod = null;
                switch (request.PaymentMethodChoice)
                {
                    case PaymentMethodChoice.CashOnDelivery:
                        paymentMethod = new CashOnDelivery();
                        break;
                    case PaymentMethodChoice.CreditCardPayment:
                        paymentMethod = new CreditCardPayment();
                        break;
                }

                return paymentMethod.Process(request);
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

            IPaymentService paymentService = new PaymentService();
            var result = paymentService.Pay(paymentRequest);

            Console.ReadLine();
        }
    }
}
