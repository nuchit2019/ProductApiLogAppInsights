using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Text.Json;

namespace ProductApi.Logging
{
    /// <summary>
    /// Helper class for logging processes to both console and Application Insights.
    /// </summary>
    /// <author>Nuchit Atjanawat</author>
    /// <date>Created on: December 3, 2024</date>
    public static class TelemetryHelper
    {
        /// <summary>
        /// Logs the process details to both the console and Application Insights.
        /// </summary>
        /// <param name="logger">The logger instance for console logging.</param>
        /// <param name="telemetryClient">The telemetry client for Application Insights logging.</param>
        /// <param name="processName">The name of the process being logged.</param>
        /// <param name="logType">The type of log (e.g., start, success, warning, exception).</param>
        /// <param name="detail">Optional details to include in the log.</param>
        /// <param name="ex">Optional exception to include in the log.</param>
        public static void LogProcess(ILogger logger, TelemetryClient telemetryClient, string processName, string logType, object context = null, Exception ex = null)
        {
            // Format the log message template
            string messageTemplate = string.Format(logType, processName);
            var traceTelemetry = new TraceTelemetry(messageTemplate, GetSeverityLevel(logType));

            // Add detail information to the telemetry properties if provided
            if (context != null)
            {
                traceTelemetry.Properties.Add("Context", JsonSerializer.Serialize(context));
            }

            // Add exception information to the telemetry properties if provided
            if (ex != null)
            {
                traceTelemetry.Properties.Add("ExceptionMessage", ex.Message);
                traceTelemetry.Properties.Add("FileName", new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileName());
                traceTelemetry.Properties.Add("LineNumber", new System.Diagnostics.StackTrace(ex, true).GetFrame(0)?.GetFileLineNumber().ToString());
                telemetryClient.TrackException(ex);
            }

            // Track the trace telemetry
            // Send Logs to Application Insights
            telemetryClient.TrackTrace(traceTelemetry);

            // Log to the console based on the log type
            switch (logType)
            {
                case LoggingConstants.START_PROCESS:
                case LoggingConstants.SUCCESS_PROCESS:
                    logger.LogInformation(messageTemplate);
                    break;
                case LoggingConstants.WARNING_PROCESS:
                    logger.LogWarning(messageTemplate);
                    break;
                case LoggingConstants.EXCEPTION_PROCESS:
                    logger.LogError(ex, messageTemplate);
                    break;
            }
        }

        /// <summary>
        /// Determines the severity level based on the log type.
        /// </summary>
        /// <param name="logType">The type of log (e.g., start, success, warning, exception).</param>
        /// <returns>The corresponding severity level.</returns>
        private static SeverityLevel GetSeverityLevel(string logType)
        {
            return logType switch
            {
                LoggingConstants.START_PROCESS => SeverityLevel.Information,
                LoggingConstants.SUCCESS_PROCESS => SeverityLevel.Information,
                LoggingConstants.WARNING_PROCESS => SeverityLevel.Warning,
                LoggingConstants.EXCEPTION_PROCESS => SeverityLevel.Error,
                _ => SeverityLevel.Information,
            };
        }

    }
}
