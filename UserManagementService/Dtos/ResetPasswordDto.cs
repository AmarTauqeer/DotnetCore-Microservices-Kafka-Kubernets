﻿namespace UserManagementService.Dtos
{
    public class ResetPasswordDto
    {
        public string? Email { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }

    }
}
