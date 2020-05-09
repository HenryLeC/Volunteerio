using System;

namespace Volunteerio
{
    class ServerErrorException : Exception
    {
        public int ErrorCode;

        public ServerErrorException(int ErrorCodePass)
        {
            ErrorCode = ErrorCodePass;
        }
    }
}
