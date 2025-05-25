using System.Net;

namespace Quizmaster.Datatypes {
    public struct ReturnValue<T> 
    {
        public HttpStatusCode Code;
        public T Value;
        public bool IsError;
        public string Message;
    }
}