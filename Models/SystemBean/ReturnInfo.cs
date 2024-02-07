using System.Text.Json.Serialization;

namespace TaskManangerSystem.Models.SystemBean
{

    public class Result<T>
    {
        [JsonInclude]
        public  bool status = false;
        [JsonInclude]
        public  T? data;

        // public ReturnInfo(bool bl,T obj){
        //     status = bl;
        //     data = obj;
        // }
        public Result(){}

        public Result(bool bl,T obj){
            this.status = bl;
            this.data = obj;
        }
    }

}
