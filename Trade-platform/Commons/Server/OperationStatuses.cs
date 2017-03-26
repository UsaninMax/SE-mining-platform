namespace TradePlatform.Commons.Server
{
    //TODO: from property file
    class OperationStatuses
    {
        public static string FAIL_TO_UPDATE_SECURITIES_INFO
        {
            get
            {
                return "Fail to update securities info";
            }
        }

        public static string SECURITIES_INFO_UPDATED
        {
            get
            {
                return "Securities info - updated";
            }
        }

        public static string SECURITIES_INFO_UPDATE_IN_PROGRESS
        {
            get
            {
                return "Securities info update - in progress";
            }
        }
    }
}
