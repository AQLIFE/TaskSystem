using AutoMapper;
using TaskManangerSystem.Actions;

// using TaskManangerSystem.Controllers;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Services
{
    public class ModelMap
    {
        public static readonly APIModel[] apiModelGroup = [
           new ( typeof(EmployeeAccount), typeof(EmployeeActions) ),
           new ( typeof(Category), typeof(CategoryActions) )
       ];

        public static APIModel? GetApiModel(string e) => apiModelGroup.FirstOrDefault(c => c.modelName == e);
        public static bool ExistApiModel(string e) => apiModelGroup.Any(c => c.modelName == e);
        public static APIModel? TryGetApiModel(string e) => ExistApiModel(e) ? GetApiModel(e) : default;
    }

    public class APIModel(Type model, object apiModel)
    {
        public string modelName = model.Name;

        public Type? modelType = model;

        public Object? modelActionType = apiModel;
    }
}
