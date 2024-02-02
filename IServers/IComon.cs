using TaskManangerSystem.Models;

namespace TaskManangerSystem.IServer{
    interface IComon{
        public string GetMD5(string info);
        public string GetAES(string info);
        public AliasEmployeeSystemAccount ToAlias(Object obj);
        public AliasEmployeeSystemAccount ToAlias(Object obj,char cr);
    }
}