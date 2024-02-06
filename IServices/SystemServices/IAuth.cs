using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.IServices.SystemServices{
    public interface IAuth{
        public IActionResult AuthLogin(AliasAccount account);
        //登录
        
        public IActionResult AuthRegist(AliasAccount account);
        // 注册
        
        public IActionResult AuthPostpone(AliasAccount account);
        // 延期
    }
}