﻿using System.Net;

namespace ApiMicroservicesProduct.Errors.Interfaces;

public interface IRequestException
{
    string Name { get; set; }
    string Message { get; }
    string Severity { get; set; }
    bool Response { get; set; }
    HttpStatusCode HttpStatus { get; set; }
}
