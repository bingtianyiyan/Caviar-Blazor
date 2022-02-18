﻿using AntDesign;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caviar.SharedKernel.Entities.View;
using System.Net;
using Caviar.SharedKernel.Entities;

namespace Caviar.AntDesignUI.Pages.Menu
{
    public partial class DataTemplate
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            SysMenus = await GetMenus();
            CheckMenuType();
        }

        private List<SysMenuView> SysMenus = new List<SysMenuView>();
        

        async Task<List<SysMenuView>> GetMenus()
        {

            var result = await HttpService.GetJson<PageData<SysMenuView>>($"{Url[CurrencyConstant.SysMenuKey]}?pageSize=100");
            if (result.Status != HttpStatusCode.OK) return null;
            if (DataSource.ParentId > 0)
            {
                List<SysMenuView> listData = new List<SysMenuView>();
                result.Data.Rows.TreeToList(listData);
                var parent = listData.SingleOrDefault(u => u.Id == DataSource.ParentId);
                if (parent != null)
                {
                    ParentMenuName = parent.Entity.Key;
                }
            }
            return result.Data.Rows;
        }


        string ParentMenuName { get; set; } = "无上层目录";
        void EventRecord(TreeEventArgs<SysMenuView> args)
        {
            ParentMenuName = args.Node.Title;
            DataSource.Entity.ParentId = int.Parse(args.Node.Key);
            DataSource.Entity.ControllerName = args.Node.Key;
        }

        void RemoveRecord()
        {
            ParentMenuName = "无上层目录";
            DataSource.Entity.ParentId = 0;
        }
    }
}
