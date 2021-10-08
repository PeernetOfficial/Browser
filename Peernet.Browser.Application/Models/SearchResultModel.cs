﻿using Peernet.Browser.Application.Enums;
using System;
using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class SearchResultModel
    {
        public string Id { get; set; }
        public SearchResultRowModel[] Rows { get; set; } = new SearchResultRowModel[0];

        public SearchFilterResultModel Filters { get; set; }

        public IDictionary<FiltersType, int> Stats { get; set; }

        public Tuple<int, int> Size { get; set; }
    }
}