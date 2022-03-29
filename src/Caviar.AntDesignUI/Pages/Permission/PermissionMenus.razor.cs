﻿using AntDesign;
using Caviar.AntDesignUI.Core;
using Caviar.SharedKernel.Entities;
using Caviar.SharedKernel.Entities.View;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Caviar.AntDesignUI.Pages.Permission
{
    public partial class PermissionMenus: ITableTemplate
    {
        [Parameter]
        public string CurrentUrl { get; set; }

        [Parameter]
        public ApplicationRoleView DataSource { get; set; }
        [Inject]
        UserConfig UserConfig { get; set; }
        [Inject]
        HttpService HttpService { get; set; }
        [Inject]
        MessageService MessageService { get; set; }
        List<SysMenuView> Menus { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetSelectMenus();//获取已选择数据
        }

        async Task GetSelectMenus()
        {
            var result = await HttpService.GetJson<List<SysMenuView>>($"{UrlConfig.GetPermissionMenus}?roleName={DataSource.Entity.Name}");
            if (result.Status != HttpStatusCode.OK) return;
            if (result.Data != null)
            {
                Menus = result.Data;
            }
        }

        public async Task<bool> Validate()
        {
            List<SysMenuView> menus = new List<SysMenuView>();
            Menus.TreeToList(menus);
            var urls = menus.Where(u => u.IsPermission && !string.IsNullOrEmpty(u.Entity.Url)).Select(u => u.Entity.Url).ToList();
            // 当权限菜单没有url时，以id为权限
            urls.AddRange(menus.Where(u => u.IsPermission && string.IsNullOrEmpty(u.Entity.Url)).Select(u => u.Entity.Id.ToString()));
            var result = await HttpService.PostJson($"{UrlConfig.SavePermissionMenus}?roleName={DataSource.Entity.Name}", urls);
            if (result.Status != HttpStatusCode.OK) return false;
            _ = MessageService.Success(result.Title);
            UserConfig.RefreshMenuAction.Invoke();//更新菜单
            return true;
        }
    }
}
