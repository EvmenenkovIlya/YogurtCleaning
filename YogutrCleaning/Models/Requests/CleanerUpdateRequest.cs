﻿using System.ComponentModel.DataAnnotations;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.Infrastructure;

namespace YogurtCleaning.Models;

public class CleanerUpdateRequest
{
    public int Id { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.NameIsRequired)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.NameMaxLength)]
    public string FirstName { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.LastNameIsRequired)]
    [MaxLength(50, ErrorMessage = ApiErrorMessages.LastNameMaxLength)]
    public string LastName { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.BirthDateIsRequired)]
    public DateTime BirthDate { get; set; }
    [Required(ErrorMessage = ApiErrorMessages.PhoneIsRequired)]
    [MaxLength(15, ErrorMessage = ApiErrorMessages.PhoneMaxLength)]
    public string Phone { get; set; }
    public List<int> ServicesIds { get; set; }
    public List<DistrictEnum> Districts { get; set; }
}

