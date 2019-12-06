CodeFirst如何生成数据库?
1.创建如Models.Entities.Activity实体
2.在MyContext.cs文件中添加该实体集合
3.在appsettings.json中配置数据库地址：
"ConnectionStrings": {
    "db": "data source=./reception.db"
  },
4.在包管理器控制台中输入如下命令：
add-migration init
5.在包管理器控制台中输入如下命令:
Update-Database