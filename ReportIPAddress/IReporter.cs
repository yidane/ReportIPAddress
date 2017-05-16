namespace ReportIPAddress
{
    interface IReporter
    {
        void Report(string taskId, string message);
    }
}
