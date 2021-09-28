﻿using Peernet.Browser.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class SearchFilterResultModel
    {
        public TimePeriods? Time { get; set; }

        public HealthType? HealthType { get; set; }

        public int? SizeFrom { get; set; }
        public int? SizeTo { get; set; }

        public int SizeMin { get; set; }
        public int SizeMax { get; set; }

        public SortOrders? Order { get; set; }

        public FileFormats[] FileFormats { get; set; }

        public IEnumerable<string> Get()
        {
            var res = new List<string>();
            if (Time.HasValue) res.Add(Time.Value.GetDescription());
            if (HealthType.HasValue) res.Add(HealthType.Value.GetDescription());
            if (!FileFormats.IsNullOrEmpty()) res.AddRange(FileFormats.Select(x => x.GetDescription()));
            if (Order.HasValue) res.Add(Order.Value.GetDescription());
            if (SizeFrom.HasValue && SizeTo.HasValue) res.Add($"{SizeFrom}GB - {SizeTo}GB");
            return res;
        }
    }
}