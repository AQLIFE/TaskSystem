using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.IServices.SystemServices
{
    interface IComon{
        public string GetMD5(string info);
        public string GetAES(string info);
        public AliasAccount ToAlias(Object obj);
        public AliasAccount ToAlias(Object obj,char cr);
    }
}