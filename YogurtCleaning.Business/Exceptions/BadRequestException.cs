﻿namespace YogurtCleaning.Business.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}