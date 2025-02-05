﻿using System;
using System.Net;

namespace Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public BadRequestException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}