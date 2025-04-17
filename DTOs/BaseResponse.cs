namespace SitoDeiSiti.Models
{
    public record Response<T> where T : class
    {
        public bool success { get; set; }
        public T Data { get; set; }
        public Error Error { get; set; }

        public Response(bool _succes, T _data) 
        {
            success = _succes;
            Data = _data;
        }

        public Response(bool _success, Error _error)
        {
            success = _success;
            Error = _error;
        }
    }

    public enum ErrorCode
    {
        EmailInUso,
        IscrizioneEventoGiaEffettuata
    }

    public record Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public Error(string _message)
        {
            Message = _message;
        }

        public Error(ErrorCode _code, string _message)
        {
            Code = (int)_code;
            Message = _message;
        }
    }
}
