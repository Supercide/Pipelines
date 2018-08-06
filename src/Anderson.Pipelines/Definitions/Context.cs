using System.Collections.Generic;
using Anderson.Pipelines.Responses;

namespace Anderson.Pipelines.Definitions
{

    public class Context : Dictionary<string, object>
    {
        public bool HasError => this.ContainsKey(WellKnownContextKeys.Error);

        public void SetResponse(object response)
        {
            this[WellKnownContextKeys.Response] = response;
        }

        public void SetError(object error)
        {
            this[WellKnownContextKeys.Error] = error;
        }

        public TError GetError<TError>()
        {
            return (TError) this[WellKnownContextKeys.Error];
        }

        public TResponse GetResponse<TResponse>()
        {
            return (TResponse)this[WellKnownContextKeys.Response];
        }
    }

    public class WellKnownContextKeys
    {
        public const string Response = "Response";
        public static string Error = "Error";
    }
}