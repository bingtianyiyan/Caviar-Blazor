﻿using AntDesign;
using Caviar.SharedKernel.View;
using Caviar.AntDesignUI.Helper;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;

namespace Caviar.AntDesignUI.Pages.CodeGeneration
{
    public partial class CodeFileGenerate
    {
        List<ViewFields> Entitys { get; set; }

        protected override async Task OnInitializedAsync()
        {
            #if DEBUG
            ControllerList.Add("Permission");
            await base.OnInitializedAsync();
            await GetModels();
            #else
            Message.Error("代码生成只能在debug模式下进行！");
            #endif
        }


        public async Task GetModels()
        {
            var result = await HttpService.GetJson<List<ViewFields>>(UrlList["GetEntitys"]);
            if (result.Status != StatusCodes.Status200OK) return;
            Entitys = result.Data;
        }


        CodeGenerateOptions GenerateData = new CodeGenerateOptions();
        Form<CodeGenerateOptions> GenerateFrom;
        void OnPreClick()
        {
            current--;
        }

        async void OnNextClick()
        {
            if (current == 1)
            {
                if (!GenerateFrom.Validate())
                {
                    return;
                }
                var result = await HttpService.PostJson<CodeGenerateOptions,List<PreviewCode>>($"{UrlList["CodeFileGenerate"]}?isPerview=true", GenerateData);
                if (result.Status == StatusCodes.Status200OK)
                {
                    lstTabs = result.Data;
                }
            }
            current++;
            StateHasChanged();
        }

        string ResultStatus = "";
        string ReusltTitle = "";
        string ResultSubTitle = "";
        async void OnGenerateClick()
        {
            var result = await HttpService.PostJson<CodeGenerateOptions, string>($"{UrlList["CodeFileGenerate"]}?isPerview=false", GenerateData);
            if (result.Status == StatusCodes.Status200OK)
            {
                ResultStatus = "success";
                ReusltTitle = result.Title;
                ResultSubTitle = "代码生成完毕,代码生效需要关闭程序重新编译运行";
            }
            else
            {
                ResultStatus = "error";
                ReusltTitle = result.Title;
                ResultSubTitle = result.Detail;
            }
            OnNextClick();
        }


        List<PreviewCode> lstTabs { get; set; } = new List<PreviewCode>();
        string nKey { get; set; } = "1";
    }
}
