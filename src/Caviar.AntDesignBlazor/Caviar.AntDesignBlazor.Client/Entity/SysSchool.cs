// Copyright (c) BeiYinZhiNian (1031622947@qq.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: http://www.caviar.wang/ or https://gitee.com/Cherryblossoms/caviar.

using Caviar.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations;

namespace Caviar.AntDesignBlazor.Client.Entity
{
    public class SysSchool : SysUseEntity
    {
        /// <summary>
        /// 学校名称
        /// </summary>
        [StringLength(50)]
        [Required]
        public string SchoolName { get; set; } = null!;
        /// <summary>
        /// 校长
        /// </summary>
        [StringLength(10)]
        public string? Principal { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = CurrencyConstant.EmailRuleErrorMsg)]
        public string? Email { get; set; } = null!;
        /// <summary>
        /// 学校类别
        /// </summary>
        [Required]
        public SchoolTypeEnum SchoolType { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [Required]
        [RegularExpression("^[0-9]{3,3}$", ErrorMessage = "{0}必须是3位数字")]
        [StringLength(3)]
        public override string Number { get; set; } = null!;
        /// <summary>
        /// 学校地址
        /// </summary>
        [StringLength(300)]
        public string? Address { get; set; }
    }

    public enum SchoolTypeEnum
    {
        /// <summary>
        /// 公立
        /// </summary>
        PUBLIC,
        /// <summary>
        /// 私立
        /// </summary>
        PRIVATE
    }
}
