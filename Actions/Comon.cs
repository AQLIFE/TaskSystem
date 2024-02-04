// using TaskManangerSystem.Models;

using System.Collections.Generic;
using System.Security.Cryptography;
using TaskManangerSystem.Models;

namespace TaskManangerSystem.Actions
{
    public static class Comon
    {
        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }


        // public static AliasEmployeeSystemAccount ToAlias(EncryptEmployeeSystemAccount obj) => new(obj);
        // public static AliasEmployeeSystemAccount ToAlias(EmployeeSystemAccount obj) => new(obj);

        public static AliasEmployeeSystemAccount ToAlias<T>(T obj)where T:EncryptEmployeeSystemAccount,EmployeeSystemAccount =>new (obj);
        public static AliasEmployeeSystemAccount ToAlias(EncryptEmployeeSystemAccount obj, char cr = '*') => new(obj, cr);
        public static AliasEmployeeSystemAccount ToAlias(EmployeeSystemAccount obj, char cr = '*') => new(obj, cr);

        // public static List<AliasEmployeeSystemAccount> ToAlias<T>(List<T> obj) where T : EncryptEmployeeSystemAccount => obj.Select(item => ToAlias(item)).ToList();
        // public static List<AliasEmployeeSystemAccount> ToAlias(List<EncryptEmployeeSystemAccount> obj) => obj.Select(item => ToAlias(item)).ToList();
        public static List<AliasEmployeeSystemAccount> ToListAlias<T>(List<T> obj, char cr = '*') where T : class
        {
            List<AliasEmployeeSystemAccount> values = new();
            // obj.Select(item =>(item is EncryptEmployeeSy stemAccount objs) ? objs : (item is EmployeeSystemAccount objy) ? objy : new EmployeeSystemAccount()).ToList();
            foreach (var item in obj)
                if (item is EncryptEmployeeSystemAccount encryptSystemAccount )
                    {
                        // encryptSystemAccount = item as EncryptEmployeeSystemAccount;
                        Console.WriteLine(encryptSystemAccount.EmployeeAlias);
                    }
                else if( item is EmployeeSystemAccount employeeSystemAccount)
                {
                    
                }
            return values; // 返回转换后的列表  
        }

    }
}