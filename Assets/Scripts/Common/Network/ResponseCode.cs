namespace Common
{
    namespace Network
    {
        public enum ResponseCode
        {
            Success = 200,
            Created = 201,
            Accepted = 202,
            NoContent = 204,
            BadRequest = 400,
            NotFound = 404,
            RequestTimeout = 408,
            InternalServerError = 500,
            Unknown
        }
    }    
}
