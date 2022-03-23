using Domain.Configurations;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCrontab;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 
namespace Persistence.BackgroundTasks
{
    public class SetModuleToTaken : BackgroundService
    {
        private readonly SetModuleToTakenConfig _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        //private readonly IModuleService _moduleService;
        //private readonly ITopicService _topicService;
        private CrontabSchedule _schedule;
        private DateTime _nextRun;

        private readonly ILogger<SetModuleToTaken> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public SetModuleToTaken(IServiceScopeFactory serviceScopeFactory, IOptions<SetModuleToTakenConfig> configuration, ILogger<SetModuleToTaken> logger, IHttpContextAccessor contextAccessor)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration.Value;
            _logger = logger;
            _schedule = CrontabSchedule.Parse(_configuration.CronExpression);
            _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
            _contextAccessor = contextAccessor;
            //_moduleService = moduleService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Background Hosted Service for {nameof(SetModuleToTaken)}  is starting");
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<IModuleService>();
                    var context2 = scope.ServiceProvider.GetRequiredService<ITopicService>();
                    var context3 = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    var context4 = scope.ServiceProvider.GetRequiredService<IAssessmentService>();

                    if(_contextAccessor.HttpContext?.User == null)
                    {
                        _logger.LogInformation($"No Module is due for returning");
                    }
                    else
                    {
                        var signedInUserId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var learnerId = Guid.Parse(signedInUserId.ToString());
                        var modules = await context.GetAllModulesByLearner(learnerId);
                        if (modules == null)
                        {
                            _logger.LogInformation($"No Module is due for ");
                        }
                        foreach (var module in modules)
                        {
                            var isModuletaken = context2.IsAllModuleTopicsTaken(module.Id);
                            if (isModuletaken)
                            {
                                module.IsTaken = true;
                                context3.Modules.Update(module);
                            }
                        }
                        await context3.SaveChangesAsync(stoppingToken);
                        await context4.GenerateAssessment(learnerId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occured reading Module table  in database. {ex.Message}");
                    _logger.LogError(ex, ex.Message);
                }
                _logger.LogInformation($"Background Hosted Service for {nameof(SetModuleToTaken)}  is stopping");
                var timeSpan = _nextRun - now;
                await Task.Delay(timeSpan, stoppingToken);
                _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
            }
        }
    }
}
