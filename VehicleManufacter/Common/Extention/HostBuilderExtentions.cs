using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Common.Serilog;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Net;

namespace Common.Extention
{
    public static class HostBuilderExtentions
    {
        public static IHostBuilder ConfigureLogging(this IHostBuilder builder, string appName = null)
        {
            builder.UseSerilog((hostingContext, services, loggerConfiguration) =>
            {
                var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                telemetryConfiguration.InstrumentationKey = hostingContext.Configuration.GetValue<string>("ApplicationInsights:APPINSIGHTS_INSTRUMENTATIONKEY");

                loggerConfiguration
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.With(new OperationIdEnricher())
                    .Enrich.WithProperty("ApplicationName", appName)
                    .WriteTo.Async(a => a.Console(theme: AnsiConsoleTheme.Code,
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"))
                    .WriteTo.Logger(lc => lc
                        .Filter.ByExcluding(e => e.Level == LogEventLevel.Debug)
                        //.WriteTo.ApplicationInsights(telemetryConfiguration, new CustomTelemetryConverter())serviceProvider.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces
                        //.WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces)
                        //.WriteTo.ApplicationInsights(services.GetRequiredService<TelemetryConfiguration>(), new RedaTraceTelemetryConverter())
                        );

            });

            builder.ConfigureServices((hostContext, services) =>
            {
                var applicationInsightsIK = hostContext.Configuration.GetValue<string>("ApplicationInsights:APPINSIGHTS_INSTRUMENTATIONKEY");
                var applicationInsightsCRN = hostContext.Configuration.GetValue<string>("ApplicationInsights:APPINSIGHTS_CLOUDROLENAME");

                services.AddLogging(e => e.AddSerilog());


                if (builder.Properties.Any(e => e.Value is WebHostBuilderContext))
                    services.AddApplicationInsightsTelemetry(opt =>
                    {
                        opt.InstrumentationKey = applicationInsightsIK;
                        opt.EnableAdaptiveSampling = false;
                        opt.EnableDependencyTrackingTelemetryModule = false;
                    });
                else
                    services.AddApplicationInsightsTelemetryWorkerService(opt =>
                    {
                        opt.InstrumentationKey = applicationInsightsIK;
                        opt.EnableAdaptiveSampling = false;
                        opt.EnableDependencyTrackingTelemetryModule = false;
                    });

                //Fix per memory leaks nei worker
                var sp = services.BuildServiceProvider();

                services.AddSingleton<Common.LoggerFactoryWrapper>(new Common.LoggerFactoryWrapper(sp.GetService<ILoggerFactory>()));

            });

            return builder;
        }

        public static IHostBuilder ConfigureRedaWorkstationLogging(this IHostBuilder builder, string appName = null)
        {
            builder.UseSerilog((hostingContext, services, loggerConfiguration) =>
            {
                var configuration = services.GetService<IConfiguration>();

                Enum.TryParse<LogEventLevel>(configuration.GetSection("Serilog")["MinimumLevel"], out LogEventLevel logLevel);

                var logLevelSwitch = new LoggingLevelSwitch();
                logLevelSwitch.MinimumLevel = logLevel;

                loggerConfiguration
                    .MinimumLevel.ControlledBy(logLevelSwitch)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithThreadId()
                    .Enrich.With(new OperationIdEnricher())
                    .Enrich.WithProperty("ApplicationName", appName)
                    .WriteTo.Async(a => a.File($"{configuration.GetSection("logsPath").Value}/{Dns.GetHostName()}/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30))
                    .WriteTo.Async(a => a.Console(theme: AnsiConsoleTheme.Code,
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
                    ));

            });

            builder.ConfigureServices((hostContext, services) =>
            {
                services.AddLogging(e => e.AddSerilog());
            });

            return builder;
        }
    }
}
