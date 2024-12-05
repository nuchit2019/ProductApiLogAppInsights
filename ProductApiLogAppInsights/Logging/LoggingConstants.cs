namespace ProductApi.Logging
{
   
    public static class LoggingConstants
    { 
        private const string ApplicationName = "ProductApi";
        public const string START_PROCESS = ApplicationName + " Start Process: {0}"; 
        public const string WARNING_PROCESS = ApplicationName + " Warning Process: {0}";
        public const string SUCCESS_PROCESS = ApplicationName + " Success Process: {0}";
        public const string EXCEPTION_PROCESS = ApplicationName + " Exception Process: {0}";
    }
}
