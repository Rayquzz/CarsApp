using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Services;

namespace CarsApp.Infrastructure.Factories.FactoryMethod
{
    public abstract class ServiceFactory
    {
        public abstract IServiceOperation CreateService();


        // metoda optionala, ca in exemplul oficial de la Microsoft, 
        //care poate fi folosita pentru a executa serviciul creat, 
        //dar nu este obligatorie in pattern-ul Factory Method
        public void ExecuteService(IServiceOperation service)
        {
            Console.WriteLine("Service created successfully.");
        }
    }
}
