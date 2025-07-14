using WebAPI.Repositories.Results;

namespace WebAPI.Repositories.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        // Mesaj Data ve false birlikte döner
        public ErrorDataResult(T data, bool success, string message) : base(data, false, message)
        {

        }
        // Data Mesaj ile birlikte döner
        public ErrorDataResult(T data, string message) : base(data, false, message)
        {

        }

        // Data Sadece false ile birlikte döner
        public ErrorDataResult(T data) : base(data, false)
        {

        }

        // Sadece Mesaj ve Success döner
        public ErrorDataResult(string message) : base(default, false, message)
        {

        }

        // Sadece Success döner
        public ErrorDataResult() : base(default, false)
        {

        }

    }
}