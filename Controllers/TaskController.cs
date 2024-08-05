using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Services;

namespace TaskManangerSystem.Controllers
{

    [ApiController, Authorize, Route("api/[controller]")]
    public class TaskSystemController(ManagementSystemContext storage, IMapper mapper) : ControllerBase
    {
        private TaskAffairRepositoryAsync TRA = new(storage);

        [HttpGet]
        public async Task<PageContent<TaskAffairForSelect>> Gettasks(int index = 1, int size = 100)
        => mapper.Map<PageContent<TaskAffairForSelect>>(await TRA.SearchAsync(index, size));

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<TaskAffairForSelect> GetTaskAffair(int serial)
        => mapper.Map<TaskAffairForSelect>(await TRA.GetTaskAffairBySerial(serial));

        // POST: api/tasks
        [HttpPost]
        public async Task<bool> CreateTaskAffair(TaskAffairForAdd taskAffair)
        => await TRA.AddAsync(mapper.Map<TaskAffair>(taskAffair, opt => opt.Items["ManagementSystemContext"] = storage));


        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<bool> UpdateTaskAffair(int serial, TaskAffairForUpdate updatedTaskAffair)
        {
            TaskAffair? af = await TRA.GetTaskAffairBySerial(serial);

            return af is TaskAffair ? await TRA.UpdateAsync(af, updatedTaskAffair) : false;
            // ���߿��Է���Ok(updatedTaskAffair)��ʾ�ɹ����²����ظ��º������
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public async Task<bool> DeleteTaskAffair(int serial)
        {
            TaskAffair? taskAffair = await TRA.GetTaskAffairBySerial(serial);
            return taskAffair is TaskAffair ag ? await TRA.DeleteAsync(ag) : false;

        }
    }
}