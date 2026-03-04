using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsApp.Domain.Singleton;


namespace CarsApp.Infrastructure.Singleton
{
    public sealed class ServiceLogger : IServiceLogger
    {
        // === SINGLETON MECHANICS ===

        // Câmpul static privat — echivalent cu:
        // private static field instance: Database
        private static ServiceLogger? _instance;

        // Obiectul de lock pentru thread-safety — echivalent cu:
        // acquireThreadLock()
        private static readonly object _lock = new();

        // Constructorul PRIVAT — nimeni nu poate face `new ServiceLogger()`
        // Echivalent cu: private constructor Database()
        private ServiceLogger()
        {
            _history = new List<string>();
            Console.WriteLine("  [ServiceLogger] Instanta creata.");
        }

        // Proprietatea statică publică — echivalent cu getInstance()
        // Folosește Double-Check Locking, exact ca în exemplul abstract:
        //   if (instance == null) then
        //     acquireThreadLock() and then
        //       if (instance == null) then
        //         instance = new Database()
        public static ServiceLogger Instance
        {
            get
            {
                if (_instance == null)                    // primul check (fara lock)
                {
                    lock (_lock)                          // acquireThreadLock()
                    {
                        if (_instance == null)            // al doilea check (cu lock)
                        {
                            _instance = new ServiceLogger();
                        }
                    }
                }
                return _instance;
            }
        }

        // === BUSINESS LOGIC ===
        // Echivalent cu metoda query(sql) din exemplul abstract

        private readonly List<string> _history;

        public void Log(string vehicleInfo, string serviceName, string technicianName, decimal cost)
        {
            var entry = $"[{DateTime.Now:HH:mm:ss}] {vehicleInfo} | {serviceName} | " +
                        $"Tehnician: {technicianName} | Cost: {cost:C}";
            _history.Add(entry);
        }

        public IReadOnlyList<string> GetHistory() => _history.AsReadOnly();

        public void PrintHistory()
        {
            if (_history.Count == 0)
            {
                Console.WriteLine("    Nu exista inregistrari.");
                return;
            }
            foreach (var entry in _history)
                Console.WriteLine($"    {entry}");
        }
    }
}
