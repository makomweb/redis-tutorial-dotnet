﻿using StackExchange.Redis;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Redis.Tutorial.Dotnet
{
    public class RedisMessages
    {
        readonly ISubscriber _sub;
        private readonly string _channel;

        public RedisMessages(ConnectionMultiplexer redis, string channel)
        {
            _sub = redis.GetSubscriber();
            _channel = channel;
        }

        public IObservable<string> Messages
        {
            get
            {
                var subscription = _sub.Subscribe(_channel);

                var observable = Observable.Create<ChannelMessage>(observer =>
                {
                    subscription.OnMessage(msg =>
                    {
                        observer.OnNext(msg);
                        //observer.OnCompleted();
                    });
                    return Disposable.Empty;
                });

#if false
                var p = observable.Publish();
                p.Connect();
                return p;
#else
                return observable.Select(msg => (string)msg.Message);
#endif
            }
        }
    }
}
