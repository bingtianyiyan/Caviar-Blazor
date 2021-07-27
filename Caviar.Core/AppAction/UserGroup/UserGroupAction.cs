﻿using Caviar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caviar.Core.UserGroup
{
    public partial class UserGroupAction
    {
        public async Task<ResultMsg<SysUserGroup>> GetUserGroup(int userId)
        {
            var user = await BC.DbContext.GetSingleEntityAsync<SysUser>(u => u.Id == userId);
            if (user == null) return Error<SysUserGroup>("不存在该角色");
            var group = await BC.DbContext.GetSingleEntityAsync<SysUserGroup>(u => u.Id == user.UserGroupId);
            return Ok(group);
        }
    }
}
