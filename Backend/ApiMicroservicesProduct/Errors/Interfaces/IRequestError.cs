﻿using System.Net;

namespace ApiMicroservicesProduct.Errors.Interfaces;

public interface IRequestError
{
    string Name { get; set; }
    string Message { get; set; }
    string Severity { get; set; }
    bool Response { get; set; }
    HttpStatusCode HttpStatus { get; set; }
}
