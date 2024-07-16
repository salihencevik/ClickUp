namespace ClickUpApp.Nuget.CustomException
{
    [Serializable]
    public class BusinessException : Exception
    {
        public BusinessException() { }
        public BusinessException(Exception innerException) { }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception innerException) : base(message, innerException) { }
    }
}
