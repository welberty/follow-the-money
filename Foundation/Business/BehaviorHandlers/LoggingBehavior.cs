﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace Foundation.Business.BehaviorHandlers
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");
            TResponse response;
            try
            {
                response = await next();
            }
            finally
            {
                _logger.LogInformation($"Handled {typeof(TResponse).Name}");
            }
            return response;
        }
    }
}
