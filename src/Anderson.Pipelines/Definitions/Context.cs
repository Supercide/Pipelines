using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Andersoft.Pipelines.Definitions
{

    public class Context : Dictionary<string, object>
    {
        private readonly Dictionary<Type, object> _response;
        private readonly Dictionary<Type, object> _errors;

        public Context()
        {
            _response = new Dictionary<Type, object>();
            _errors = new Dictionary<Type, object>();
        }

        public bool HasError => _errors.Any();

        public void SetResponse<TResponse>(TResponse response)
        {
            _response[typeof(TResponse)] = response;
        }

        public void SetError<TError>(TError error)
        {
            _errors[typeof(TError)] = error;
        }

        public bool ContainsError<TError>()
        {
            return _errors.ContainsKey(typeof(TError));
        }

        public IEnumerable GetAllErrors()
        {
            return _errors.Select(x => x.Value);
        }

        public bool ContainsResponse<TResponse>()
        {
            return _response.ContainsKey(typeof(TResponse));
        }

        public IEnumerable GetAllResponses()
        {
            return _response.Select(x => x.Value);
        }

        public TError GetError<TError>()
        {
            return (TError) _errors[typeof(TError)];
        }

        public TResponse GetResponse<TResponse>()
        {
            return (TResponse) _response[typeof(TResponse)];
        }
    }
}