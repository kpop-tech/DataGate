﻿namespace Pharus.Services.Roles.Contracts
{
    using System.Collections.Generic;

    using Pharus.Domain.Models.Users;

    public interface IRolesService
    {
        List<PharusRole> GetAllRoles();

        PharusRole GetRole(string roleName);
    }
}