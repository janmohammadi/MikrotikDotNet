using System;
using System.Collections.Generic;

namespace MikrotikDotNet
{
    public interface IMKConnection
    {
        Guid GUID { get; }
        string Host { get; set; }
        bool IsOpen { get; }
        string Password { get; set; }
        int Port { get; set; }
        string UserName { get; set; }

        void Close();
        IMKCommand CreateCommand();
        IMKCommand CreateCommand(string commandText);
        void Dispose();
        string EncodePassword(string Password, string hash);
        bool Equals(MKConnection other);
        void Open();
        void Push();
        List<string> Read();
        void Send(string co);
    }
}