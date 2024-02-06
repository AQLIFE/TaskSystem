using System.Reflection;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Actions
{
    public static class Comon
    {
        [Obsolete("该加密可信度偏低")]
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
    }

    public class AppFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext action){}
        
        public void OnActionExecuted(ActionExecutedContext action){
            HttpRequest obj = action.HttpContext.Request;
            ObjectResult item = action.Result as ObjectResult ?? throw new Exception("没有这个类型");
            // if(action.Exception!=null || item.Value==null ){
            //     action.Result = new ObjectResult(new Result<string>(false,"请求失败"));
            // }
            // else {
            //     action.Result =  new ObjectResult(new Result<object>(true,item.Value));
            // }

            action.Result = new ObjectResult(new Result<Object?>(!(action.Exception!=null || item.Value==null),item.Value));
        }
    }


    [Obsolete("尚未开发完成")]
    public class ActionTypeExtension : Exception{
        public string message {set;get;}

        public ActionTypeExtension(string str){
            this.message = str;
        }        
    }
}