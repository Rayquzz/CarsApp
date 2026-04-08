using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Lab4.Adapter
{
    // Simuleaza un API bancar vechi — interfata incompatibila
    public class ExternalBankApi
    {
        public int InitiateTransfer(string beneficiary, float amount, string currency)
        {
            return new Random().Next(100000, 999999);
        }

        public string GetTransferStatus(int transferCode)
        {
            return transferCode > 0 ? "COMPLETED" : "FAILED";
        }
    }
}