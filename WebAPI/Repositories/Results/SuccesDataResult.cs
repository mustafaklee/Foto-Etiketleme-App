using WebAPI.Repositories.Results;

namespace WebAPI.Repositories.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        // Mesaj Data ve Success birlikte döner
        public SuccessDataResult(T data, bool success, string message) : base(data, true, message)
        {

        }
        // Data Mesaj ile birlikte döner
        public SuccessDataResult(T data, string message) : base(data, true, message)
        {

        }

        // Data Sadece Success ile birlikte döner
        public SuccessDataResult(T data) : base(data, true)
        {

        }

        // Sadece Mesaj ve Success döner
        public SuccessDataResult(string message) : base(default, true, message)
        {

        }

        // Sadece Success döner
        public SuccessDataResult() : base(default, true)
        {

        }

    }
}
