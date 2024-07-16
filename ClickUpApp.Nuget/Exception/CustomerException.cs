namespace ClickUpApp.Nuget.CustomException
{
    [Serializable]
    public class CustomerException : Exception
    {
        public CustomerException() { }
        public CustomerException(Exception innerException) { }
        public CustomerException(string message) : base(message) { }
        public CustomerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
