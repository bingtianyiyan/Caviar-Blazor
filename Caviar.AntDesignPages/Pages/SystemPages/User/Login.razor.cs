﻿using AntDesign;
using Caviar.Models.SystemData;
using Caviar.AntDesignPages.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Caviar.AntDesignPages.Pages.SystemPages.User
{
    partial class Login
    {
        bool Loading { get; set; }
        public SysUserLogin SysLoginUserData { get; set; } = new SysUserLogin();
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject]
        MessageService _message { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }

        [CascadingParameter]
        public EventCallback LayoutStyleCallBack { get; set; }
        [Inject]
        HttpHelper http { get; set; }
        [Inject]
        UserToken UserToken { get; set; }
        [Inject]
        IJSRuntime JsRuntime { get; set; }
        public async void SubmitLogin()
        {
            Loading = true;
            SysLoginUserData.Password = CommonHelper.SHA256EncryptString(SysLoginUserData.Password);
            var result = await http.PostJson<SysUserLogin,UserToken>("UserLogin/Login", SysLoginUserData);
            SysLoginUserData.Password = "";
            Loading = false;
            if (result.Status==200)
            {
                UserToken.AutoAssign(result.Data);
                var json = JsonConvert.SerializeObject(UserToken);
                var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                await JsRuntime.InvokeVoidAsync("setCookie", Config.CookieName, base64, UserToken.Duration);
                http.IsSetCookie = false;//更新cookies
                NavigationManager.NavigateTo("/");
                _message.Success(result.Title);
                return;
            }
            this.StateHasChanged();
        }

        protected override void OnInitialized()
        {
            string backgroundImage = Configuration["Background:Image"];
            var style = $"min-height:100vh;background-image: url({backgroundImage});";
            LayoutStyleCallBack.InvokeAsync(style);
            base.OnInitialized();
        }

        
    }
}
