INSERT INTO dbo.CMS_WidgetTemplate
        ( Title ,
          GroupName ,
          PartialView ,
          AssemblyName ,
          ServiceTypeName ,
          ViewModelTypeName ,
          Thumbnail ,
          [Order]
        )
VALUES  ( N'ËÑË÷' ,
          N'1.Í¨ÓÃ' , 
          N'Widget.Search' , 
          N'ZKEACMS.Search' , 
          N'ZKEACMS.Search.Service.SearchWidgetService' , 
          N'ZKEACMS.Search.Models.SearchWidget' ,
          N'~/Plugins/ZKEACMS.Search/Content/Image/Widget.Search.png' ,
          12
        )