using Core.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.DTOs
{
    public class PagingRequest
    {
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }
        [JsonProperty(PropertyName = "columns")]
        public IList<Column> Columns { get; set; }
        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }
        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }
        [JsonProperty(PropertyName = "search")]
        public Search Search { get; set; }
        [JsonProperty(PropertyName = "searchCriteria")]
        public SearchCriteria SearchCriteria { get; set; }
    }

    public class Column
    {
        [JsonProperty(PropertyName = "data")]
        public string Data {get;set;}
        [JsonProperty(PropertyName = "name")]
        public string NameData { get; set; }
        [JsonProperty(PropertyName = "searchable")]
        public bool SearchableData { get; set; }
        [JsonProperty(PropertyName = "orderable")]
        public bool OrderableData { get; set; }
        [JsonProperty(PropertyName = "search")]
        public Search SearchData { get; set; }
    }

    public class PagingResponse
    {
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }
        [JsonProperty(PropertyName = "recordsFiltered")]
        public int RecordsFiltered { get; set; }
        [JsonProperty(PropertyName = "recordsTotal")]
        public int RecordsTotal { get; set; }
        [JsonProperty(PropertyName = "data")]
        public Usuario[] Usuarios { get; set; }
    }

    public class Search
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "regex")]
        public bool Regex { get; set; }
    }

    public class SearchCriteria
    {
        [JsonProperty(PropertyName = "filter")]
        public string Filter { get; set; }
        [JsonProperty(PropertyName = "isPageLoad")]
        public bool IsPageLoad { get; set; }
    }
}
