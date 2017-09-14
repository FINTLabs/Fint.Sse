﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fint.Sse
{
    class DisconnectedState : IConnectionState 
    {
        private Uri mUrl;
        private IWebRequesterFactory mWebRequesterFactory;
        private Dictionary<string, string> mHeaders;
        private ITokenService mTokenService;

        public EventSourceState State
        {
            get { return EventSourceState.CLOSED; }
        }

        public DisconnectedState(Uri url, IWebRequesterFactory webRequesterFactory, Dictionary<string, string> headers, ITokenService tokenService)
        {
            if (url == null) throw new ArgumentNullException("Url cant be null");
            mUrl = url;
            mWebRequesterFactory = webRequesterFactory;
            mHeaders = headers;
            mTokenService = tokenService;
        }

        public Task<IConnectionState> Run(Action<ServerSentEvent> donothing, CancellationToken cancelToken)
        {
            if(cancelToken.IsCancellationRequested)
                return Task.Factory.StartNew<IConnectionState>(() => { return new DisconnectedState(mUrl, mWebRequesterFactory, mHeaders, mTokenService); });
            else
                return Task.Factory.StartNew<IConnectionState>(() => { return new ConnectingState(mUrl, mWebRequesterFactory, mHeaders, mTokenService); });
        }
    }
}
