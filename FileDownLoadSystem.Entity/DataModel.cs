namespace FileDownLoadSystem.Entity
{
    public class DataModel
    {
        public class PageDataOptions
        {
            public int PageIndex { get; set; }

            public int PageCount { get; set; }

            public int TotalPages { get; set; }

            public string TableName { get; set; }

            public string Sort { get; set; }

            public string Order { get; set; }

            public string Wheres { get; set; }

            public string Values { get; set; }

        }
        public class SearchParametes
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public string DisplayType { get; set; }
        }
    }
}
