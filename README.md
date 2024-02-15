# 项目介绍

这是一个预期设计为任务管理系统和库存管理系统集成的项目，使用NET 8 SDK 开发

 项目机密信息，如数据库连接字符串和密钥使用`dotnet user-secrets` 保存于开发机。
数据库使用Mysql 8.x 版本。

预期设计接口：

- 系统账户
    - CURD
    - Certification/Authentication

- 任务
    - 客户
    - 任务状态
    - 任务材料清单
    - 任务列表

- 库存
    - 库存登记
    - 出入库

- 分类
    - 客户分类
    - 库存分类


## 备忘录

下一个目标：

- 重写所有api逻辑

- 实现全局异常捕获

- 加入操作日志

- 对鉴权进行优化，目前无法识别是否为有效用户

已完成目标：

- 加入环境变量读取

- 统一数据返回

- 身份验证和授权方案[Bearer Token]

- 取消MD5加解密，转为使用SHA-256加密

