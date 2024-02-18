using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using BestHTTP;
using BestHTTP.WebSocket;
using System.Security.Claims;


namespace LG
{
    /// <summary>
    /// AppServiceModule 模块
    /// 支持 http 请求 以及 websocket协议
    /// </summary>
    public class AppServiceModule<C> : ModuleBase<C> where C : ModuleBase<C>, new()
    {
        protected bool RemoteServer;
        public string AppServiceAddr;
        protected WebSocketComp webSocketComp;
        public override void LGLoad(params object[] agrs)
        {
            if (agrs.Length == 2)
            {
                RemoteServer = (bool)agrs[0];
                AppServiceAddr = (string)agrs[1];
                if (RemoteServer)
                    webSocketComp = AddComp<WebSocketComp>(AppServiceAddr);
                base.LGLoad(agrs);
            }
            else
            {
                Log.Error("ViewManagerModule 启动参数错误，请检查代码");
            }
        }

        public virtual void OnOpen(WebSocket ws)
        {

        }

        public virtual void OnMessageReceived(WebSocket ws, byte[] data)
        {

        }
        public virtual void OnMessageReceived(WebSocket ws, string message)
        {

        }
        public virtual void OnClosed(WebSocket ws, ushort code, string message)
        {

        }

        public virtual void OnError(WebSocket ws, Exception ex)
        {

        }

        public virtual void Send(byte[] buff)
        {
            this.webSocketComp.Send(buff);
        }
    }
}