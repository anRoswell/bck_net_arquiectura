using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.QueryFilters.QueryFiltersSCRWeb
{
    public class QueryOp360ServerSideV2
    {
        public int? first { get; set; }
        public int? rows { get; set; }
        public string? sortField { get; set; }
        public int? sortOrder { get; set; }
        public string? sortString
        {
            get
            {
                var sortOrderStr = "";
                if (!string.IsNullOrWhiteSpace(this.sortField))
                {
                    sortOrderStr = sortOrder == 1 ? $"{this.sortField?.ToLower()} desc" : $"{this.sortField?.ToLower()} asc";
                }
                return sortOrderStr;
            }
        }

        public Dictionary<string, List<Filterv2>>? filters { get; set; }
        public string? filtersString
        {
            get
            {
                if (filters == null)
                {
                    return null;
                }

                var filtersString = new List<string>();
                var matchModes = new List<string> { "equals", "contains" };
                foreach (var filterType in filters.Where(y => y.Value.Where(x => matchModes.Contains(x.matchMode) && !string.IsNullOrWhiteSpace(x.value)).Any()))
                {
                    var filterTypeString = new List<string>();
                    foreach (var filter in filterType.Value.Where(x => matchModes.Contains(x.matchMode) && !string.IsNullOrWhiteSpace(x.value)))
                    {
                        if (filter.matchMode.ToLower() == "contains")
                        {
                            string mivalor = filter.value.ToUpper().Trim().Replace(" ", "%");
                            string mikey = $"UPPER({filterType.Key.Trim()})";
                            filterTypeString.Add($"{mikey} like #x#{mivalor}#y#");
                        }
                        else if (filter.matchMode.ToLower() == "equals")
                        {
                            string mivalor;
                            string mikey;
                            if (filter.TipoDato.Item2 == "numero")
                            {
                                mivalor = filter.value.ToUpper();
                                mikey = filterType.Key.ToUpper();
                            }
                            else if (filter.TipoDato.Item2 == "texto")
                            {
                                mivalor = $"'{filter.value.ToUpper()}'";
                                mikey = filterType.Key.ToUpper();
                            }
                            else if (filter.TipoDato.Item2 == "fecha")
                            {
                                mivalor = $"TO_DATE('{DateTime.Parse(filter.value.ToUpper()).ToString("yyyy-MM-dd")}','yyyy/mm/dd')";
                                mikey = $"TRUNC({filterType.Key.ToLower()})";
                            }
                            else
                            {
                                mivalor = filter.value.ToUpper();
                                mikey = filterType.Key.ToUpper();
                            }

                            filterTypeString.Add($"{mikey} = {mivalor}");
                        }

                    }
                    filtersString.Add(string.Join(" and ", filterTypeString));
                }
                var mifiltro = string.Join(" and ", filtersString);
                mifiltro = string.IsNullOrWhiteSpace(mifiltro) ? "" : string.Concat(" (", mifiltro, ")");
                return mifiltro;
            }
        }

        public object? globalFilter { get; set; }
    }

    public class Filterv2
    {
        public string? value { get; set; }
        public string? matchMode { get; set; }
        [JsonPropertyName("operator")]
        public string? Operator { get; set; }
        public (string, string) TipoDato
        {
            get
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return (null, null);
                }
                return Tools.Funciones.GetTipoDato(this.value);
            }
        }
    }
}
