﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ace.Core.Page;
using Microsoft.AspNetCore.Mvc;

namespace Ace.Boss
{
    /// <summary>
    /// jqGrid分页与标准分页转换
    /// </summary>
    public static class PageConvert
    {

        public static PageOption ToPageOption(this Pagination pagination)
        {
            var orderList = new string[] { };
            var orderColumns = new List<ColumnInfo>();
            if (!string.IsNullOrEmpty(pagination.sidx))
                orderList = (pagination.sidx + " " + pagination.sord).Split(',');
            return new PageOption(pagination.page, pagination.rows, orderList.Select(s =>
            {
                var order = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return new ColumnInfo
                {
                    Name = order[0],
                    SortOrder = order[1] == "asc" ? ColumnSortOrder.Asc : ColumnSortOrder.Desc
                };
            }).ToList());
        }
        public static IActionResult ToPageData<T>(this PageResult<T> pageResult)
        {
            var pageData = new PageData()
            {
                page = pageResult.PageOption.PageIndex,
                total = pageResult.TotalPages,
                records = pageResult.TotalRows,
                rows = pageResult.DataList,
            };
            if (pageData.page > pageData.total)
                pageData.page = pageData.total;
            if (pageData.total > 0)
            {
                if (pageData.page == 0)
                    pageData.page = 1;
            }
            return new JsonResult(pageData);
        }
    }
}