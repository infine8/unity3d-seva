using System;
using UniKid.Core.Service;
using UniKid.Core.Service.DbServiceType;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace UniKid.Core.Controller.DbCommand
{
    public abstract class DbCommandBase<T>  : EventCommand where T : IDbType<T>
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject ContextView { get; set; }

        public IDbService DbService { get; set; }

        protected abstract string DbCommandName { get; }

        protected virtual void PrepareCommand() { }

        public override void Execute()
        {
            PrepareCommand();

            Retain();

            DbService.dispatcher.AddListener(DbCommandName, OnComplete);
        }

        protected virtual void OnComplete(IEvent evt)
        {
            DbService.dispatcher.RemoveListener(DbCommandName, OnComplete);

            var co = evt.data as CallbackObject;
            if (co == null) throw new Exception("Callback object is null in " + typeof(T).Name);

            OnCommandExecuted((T)co.Object, co.Error);

            Release();
        }

        protected virtual void OnCommandExecuted(T obj, string error) { }

    }
}