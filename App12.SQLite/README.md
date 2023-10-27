# App12.SQLite

You don't need to install any database software. You can use SQLite.

你不必像MySQl或SQL Server一样先安装数据库软件才能使用数据库，你可以使用SQLite。

## [Hotel System](https://github.com/mentapro/HotelSystem)

This is a hotel system that can be used to book a room for a certain period of time.

- MVVM Example
- Entity Framework Example
- WPF

This application (WPF) works with database SQLite through Entity Framework.
I tried to write clean code. I used MVVM pattern and followed SOLID principles.

Current application represents database for hotel. There are rooms and clients. Each room can contain many clients but
one client can live only in one room.
User interface is written in XAML. XAML elements use binding to show data.

#### Clients tab:

![HotelSystem_ClientTab](https://farm5.staticflickr.com/4249/34079855013_73588d5604_b.jpg)

#### Rooms tab:

![HotelSystem_RoomsTab](https://farm5.staticflickr.com/4223/34890145765_b72db14657_b.jpg)

## 附录一: [如何实现数据库自动升级](https://juejin.cn/post/7294072846128021545)

### 0 预置条件

#### 0.1 Nuget

透过`Nuget`安装`Microsoft.EntityFrameworkCore.Sqlite`和`Microsoft.EntityFrameworkCore.Tools`两个库。

#### 0.2 Rider插件

Rider打开插件市场，安装Entity Framework Core UI，这个插件平替实现Visual
Studio的NuGet包管理器控制台的功能。`Microsoft.EntityFrameworkCore.Tools`中：

- Add-Migration 将基于自上次迁移创建以来对模型所做的更改来构建下一次迁移
- Remove-Migration
- Update-Database 将对数据库应用任意挂起的迁移
- Drop-Database
- ...

### 1 最终材料清单

- Migrations目录
- 表__EFMigrationsHistory
- .db数据库文件

![最终材料清单](https://raw.githubusercontent.com/zhongwcool/WpfApps/main/App12.SQLite/Assets/142514.png)

#### 1.1 Migrations目录

**首次**使用工具Rider->Tools->Entity Framework Core->`Add Migration`将在工程里面生成Migrations目录，向*Migrations*
目录下的项目添加以下三个文件：

- *XXXXXXXXXXXXXX_AddCreatedTimestamp.cs*- 主迁移文件。 包含应用迁移所需的操作（在`Up`中）和还原迁移所需的操作（在`Down`中）。
- *XXXXXXXXXXXXXX_AddCreatedTimestamp.Designer.cs*- 迁移元数据文件。 包含 EF 所用的信息。
- *MyContextModelSnapshot.cs*--当前模型的快照。 用于确定添加下一迁移时的更改内容。

#### 1.2 表__EFMigrationsHistory

**首次**使用工具Rider->Tools->Entity Framework Core->`Update Database`将在数据库文件增加一张记录迁移历史的表：__
EFMigrationsHistory。

![表__EFMigrationsHistory](https://raw.githubusercontent.com/zhongwcool/WpfApps/main/App12.SQLite/Assets/150152.png)

#### 1.3 .db数据库文件

基于以上操作生成的.db数据库文件才具备自动迁移的前提。当在App.xaml.cs添加如下代码：

```c#
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    using var dbContext = new IaContext();
    // 检查数据模型是否发生了变化
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        // 应用未应用的迁移
        dbContext.Database.Migrate();
    }
}
```

会在程序启动时完成数据库升级。

### 2 升级完成

当升级完成，数据中的数据记录将和Migrations中的迁移脚本的数量一致。未更新的数据库文件的迁移历史缺少最新一条。