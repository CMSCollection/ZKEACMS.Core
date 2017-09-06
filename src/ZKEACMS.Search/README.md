# ZKEACMS 全站搜索插件
让ZKEACMS支持搜索功能
![全站搜索插件](https://user-images.githubusercontent.com/6006218/30117859-0fbf8ca6-9354-11e7-89b0-5e6fbc002a99.png)
## 数据库 Microstft Sql Server 2008R2 以上
页面的索引主要是运用了MsSql的
[全文索引](https://docs.microsoft.com/zh-cn/sql/relational-databases/search/full-text-search)
### 初始化数据库
数据库脚本在目录DbScripts下。全文索引数据库，可以直接创建在CMS数据下，或者另外建一个数据库用于存储页面的索引数据。
#### dbo.WebPages.Table.sql
这个脚本用于创建WebPages表并对该表创建一个中文的全文索引。如果不是用独立索引数据库，这个脚本可直接在CMS数据库中执行。如果要用独立的数据库，手动创建数据后，在新的数据库中执行该脚本即可。
#### dbo.CMS_WidgetTemplate.sql
这个脚本在**CMS数据库**中执行。用于添加一个搜索的组件。
## 配置 appsettings.json
这个是搜索插件的配置文件

**ConnectionString** 
索引数据库的连接字符串

**Command** 
dotnet命令，如果没有设置Path或是Linux，可能需要设置完整的dotnet路径

**Host**
域名或者是某个页面的地址，搜索插件中的爬虫将会爬该域名下或页面下的所有链接都索引起来。
## 索引页面
在搜索插件开始使用之前，需要先索引全部的页面。
### 方式一
在后台的 **全站搜索** 目录下点击 **开始索引** 按钮。注意先给角色添加权限。
### 方式二（推荐）
由于页面的内容是在不断变化的，所以需要定时更新索引的页面内容。搜索插件，其实也是一个完整的.net core程序，可以使用dotnet命令来运行。
`dotnet ZKEACMS.Search.dll`
。这样一来，就可以添加一个计划任务，来定时启动这个搜索索引程序。注意配置文件（appsettings.json）要在同一目录下。如果是开发环境，可以直接鼠标右键->调式->启动新的实例来启动爬虫程序。
## 开始搜索
编辑你的任意页面，或者添加一个页面。然后往页面中添加**搜索**插件。

![搜索](https://raw.githubusercontent.com/SeriaWei/ZKEACMS.Core/search/src/ZKEACMS.Search/Content/Image/Widget.Search.png)