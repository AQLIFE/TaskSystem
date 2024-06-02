// using AutoMapper;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using TaskManangerSystem.Actions;
// using TaskManangerSystem.IServices.BeanServices;
// using TaskManangerSystem.Models.DataBean;
// using TaskManangerSystem.Models.SystemBean;
// using TaskManangerSystem.Services;

// namespace TaskManangerSystem.Controllers
// {

//     [ApiController, Authorize]
//     [Route("api/[controller]")]
//     public class EmployeeController(ManagementSystemContext storage, IMapper mapper) : ControllerBase
//     {

//         private EmployeeActions action = new(storage,mapper);
//         private Repository<EmployeeAccount> commonAction = new(storage);

//         // [HttpGet]
//         // [Authorize(Policy = "admin")]
//         // public async Task<ActionResult<IEnumerable<IPartInfo>>> GetEmployeeSystemAccounts(int page = 1, int pageSize = SystemInfo.pageSize)
//         //     => mapper.Map<List<EmployeeAccountForSelectOrUpdate>>( await commonAction.SearchFor(page, pageSize,e=>e.EmployeeAlias));

//         // [HttpGet("info")]
//         // public IPartInfo GetEmployeeSystemAccount(string hashId) => mapper.Map<EmployeeAccountForSelectOrUpdate>(action.GetEmployeeByHashId(hashId));

//         // [HttpPut("add")]
//         // public async Task<string?> PutEmployeeSystemAccount(string hashId, EmployeeAccountForSelectOrUpdate info)
//         // {
//         //     EmployeeAccount? objs = await action.GetEmployeeByHashId(hashId);
//         //     objs!.AccountPermission = info.AccountPermission;
//         //     objs!.EmployeeAlias = info.EmployeeAlias;

//         //     storage.Entry<EmployeeAccount>(objs).State = EntityState.Modified;
//         //     await storage.SaveChangesAsync();
//         //     return info.EmployeeAlias;
//         // }

//         // [HttpDelete("delete")]
//         // public async Task<string> DeleteEmployeeSystemAccount(string hashId)
//         // {
//         //     EmployeeAccount? employeeAccount = await action.GetEmployeeByHashId(hashId);
//         //     if (employeeAccount == null) return "不存在这个账户";

//         //     string str = employeeAccount.EmployeeAlias;
//         //     employeeAccount.EmployeeAlias= SystemInfo.GenerateUniqueRandomName(32);
//         //     employeeAccount.AccountPermission = 0;
//         //     return str;
//         // }
//     }
// }