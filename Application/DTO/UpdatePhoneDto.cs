﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UpdatePhoneDto
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
    }
}
