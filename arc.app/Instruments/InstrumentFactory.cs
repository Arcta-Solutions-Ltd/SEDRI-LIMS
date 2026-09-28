using arc.app.Instruments.Processors;
using System;

namespace arc.app.Instruments
{
    public class InstrumentFactory : IInstrumentFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public InstrumentFactory (IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public IRespond Get(string type)
        {
            switch (type.ToLowerInvariant())
            {
                case "ast":
                    return new AstProcessor(_serviceProvider);
                case "directtestinstrument":
                    return new DirectTestInstrumentProcessor(_serviceProvider);
                case "custominstrument":
                    return new CustomInstrumentProcessor(_serviceProvider);
                default:
                    break;
            }

            return null;
        }
    }
}
