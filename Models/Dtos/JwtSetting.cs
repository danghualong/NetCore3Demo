﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Models.Dtos
{
    public class JwtSetting
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public int ExpireSeconds { get; set; } = 300;
        public int RefreshExpireSeconds { get; set; } = 86400;
    }
}
