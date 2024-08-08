namespace TaskManangerSystem.Services.Tool
{
    public static class ConditionalCheckOperators
    {
        /// <summary>
        /// 方法A
        /// 支持Lambda 表达式的方式去检查对象<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">检查对象</param>
        /// <param name="condition">检查条件</param>
        /// <returns>符合条件则返回obj，反之返回default<T></returns>
        public static T? ConditionalCheck<T>(this T obj, Func<T, bool> condition) where T : class
            => obj is not null && condition(obj) ? obj : default;



        /// <summary>
        /// 方法B
        /// 支持Lambda 表达式的方式去检查对象<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj">检查对象</param>
        /// <param name="condition">检查条件</param>
        /// <param name="succeed">成功时的操作</param>
        /// <returns>符合条件则返回succeed(obj)，反之返回default</returns>
        public static TResult? ConditionalCheck<T, TResult>(this T obj, Func<T, bool> condition, Func<T, TResult> succeed) where T : class
            => obj is not null && condition(obj) ? succeed(obj) : default;


        /// <summary>
        /// 方法B 的异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <param name="condition"></param>
        /// <param name="succeed"></param>
        /// <returns></returns>
        public static async Task<TResult?> ConditionalCheckAsync<T, TResult>(this T obj, Func<T, Task<bool>> condition, Func<T, Task<TResult>> succeed) where T : class
            => obj is not null && await condition(obj) ? await succeed(obj) : default;

        public static async Task<TResult?> ConditionalCheckAsync<T, TResult>(this T obj, Func<T, bool> condition, Func<T, Task<TResult>> succeed, TResult fail) where T : class
            => obj is not null && condition(obj) ? await succeed(obj) : fail;

        public static async Task<TResult?> ConditionalCheckAsync<T, TResult>(this T obj, bool condition, Func<T, Task<TResult>> succeed, TResult fail) where T : class
            => obj is not null && condition ? await succeed(obj) : fail;


        /// <summary>
        /// 方法C
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj">检查对象</param>
        /// <param name="condition">检查条件</param>
        /// <param name="succeed">成功时的操作</param>
        /// <param name="fail">失败时的操作</param>
        /// <returns>succeed(obj) Or fail(obj)</returns>
        /// <exception cref="Exception">若检查对象为null，则会产生报错</exception>
        public static TResult ConditionalCheck<T, TResult>(this T obj, Func<T, bool> condition, Func<T, TResult> succeed, Func<T, TResult> fail) where T : class
            => obj is not null ? condition(obj) ? succeed(obj) : fail(obj) : throw new Exception("obj is null,Cannot execute.");

        /// <summary>
        /// 方法C 的变体，在失败时返回一个自定义值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">自定义值</typeparam>
        /// <param name="obj">检查对象</param>
        /// <param name="condition">检查条件</param>
        /// <param name="succeed">成功时的操作</param>
        /// <param name="fail">失败的值</param>
        /// <returns>succeed(obj) Or fail</returns>
        public static TResult ConditionalCheck<T, TResult>(this T? obj, Func<T?, bool> condition, Func<T?, TResult> succeed, TResult fail) where T : class
        => condition(obj) ? succeed(obj) : fail;

        //不返回值,但可能会修改obj
        public static void ConditionalCheck<T>(this T obj, Func<T, bool> condition, Action<T> succeed) where T : class
        { if (obj is not null && condition(obj)) succeed(obj); }
        //不返回值，但可能会修改obj
        public static void ConditionalCheck<T>(this T obj, Func<T, bool> condition, Action<T> succeed, Action<T> fail) where T : class
        { if (obj is not null) { if (condition(obj)) succeed(obj); else fail(obj); } else throw new Exception("obj is null,Cannot execute."); }
    }
}
