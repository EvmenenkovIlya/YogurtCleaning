﻿using System.ComponentModel.DataAnnotations;
using YogurtCleaning.Infrastructure;
namespace YogurtCleaning.Models;

public class ClientRegisterRequest
{
    [Required(ErrorMessage = ApiErrorMessages.NameIsRequired)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.NameMaxLength)]
    public string Name { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.LastNameIsRequired)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.LastNameMaxLength)]
    public string LastName { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.BirthDateIsRequired)]
    public DateTime? BirthDate { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.PasswordIsRequired)]
    [MinLength(8, ErrorMessage = ApiErrorMessages.PasswordMinLength)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.PasswordMaxLength)]
    public string Password { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.ConfirmPasswordIsRequired)]
    [MinLength(8, ErrorMessage = ApiErrorMessages.ConfirmPasswordMinLength)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.ConfirmPasswordMaxLength)]
    public string ConfirmPassword { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.EmailIsRequired)]
    [EmailAddress(ErrorMessage = ApiErrorMessages.EmailNotValid)]
    public string Email { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.PhoneIsRequired)]
    public string Phone { get; set; }
}
