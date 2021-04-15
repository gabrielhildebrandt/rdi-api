using System;
using System.Collections.Generic;
using System.Linq;

namespace RDI.Domain.Kernel
{
    public sealed class Environment : IEnvironment
    {
        private const string DevelopmentEnvironmentName = "Development";
        private const string TestingEnvironmentName = "Testing";
        private const string ProductionEnvironmentName = "Production";

        public Environment(string name, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            Name = name.Trim();
            _parameters = parameters;
        }

        private readonly IEnumerable<KeyValuePair<string, string>> _parameters;

        public bool IsDevelopment()
        {
            return string.Equals(Name.Trim(), DevelopmentEnvironmentName, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsTesting()
        {
            return string.Equals(Name.Trim(), TestingEnvironmentName, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool IsDevelopmentOrTesting()
        {
            return IsDevelopment() || IsTesting();
        }

        public bool IsProduction()
        {
            return string.Equals(Name.Trim(), ProductionEnvironmentName, StringComparison.CurrentCultureIgnoreCase);
        }

        public string Name { get; }

        public string this[string key]
        {
            get { return _parameters.FirstOrDefault(x => x.Key == key).Value; }
        }
    }
}