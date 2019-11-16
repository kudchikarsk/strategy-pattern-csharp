using System;
using System.Collections.Generic;
using System.Linq;
using static Example4.Program;

namespace Example4
{
    public class PaymentResolver
    {
        private IEnumerable<IPaymentMethod> paymentMethods;

        public PaymentResolver(IEnumerable<IPaymentMethod> paymentMethods)
        {
            this.paymentMethods = paymentMethods;
        }

        public IPaymentMethod Resolve(string paymentMethodChoice)
        {
            return paymentMethods.FirstOrDefault(p=>p.Name == paymentMethodChoice);
        }
    }
}