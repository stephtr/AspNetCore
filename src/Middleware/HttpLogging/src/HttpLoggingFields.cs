// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Microsoft.AspNetCore.HttpLogging
{
    /// <summary>
    /// Flags used to control which parts of the
    /// request and response are logged.
    /// </summary>
    [Flags]
    public enum HttpLoggingFields : long
    {
        /// <summary>
        /// No logging.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Flag for logging the HTTP Request Path, which includes both the <see cref="HttpRequest.Path"/>
        /// and <see cref="HttpRequest.PathBase"/>.
        /// <p>
        /// For example:
        /// Path: /index
        /// PathBase: /app
        /// </p>
        /// </summary>
        RequestPath = 0x1,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.QueryString"/>.
        /// <p>
        /// For example:
        /// Query: ?index=1
        /// </p>
        /// </summary>
        RequestQueryString = 0x2,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Protocol"/>.
        /// <p>
        /// For example:
        /// Protocol: HTTP/1.1
        /// </p>
        /// </summary>
        RequestProtocol = 0x4,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Method"/>.
        /// <p>
        /// For example:
        /// Method: GET
        /// </p>
        /// </summary>
        RequestMethod = 0x8,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Scheme"/>.
        /// <p>
        /// For example:
        /// Scheme: https
        /// </p>
        /// </summary>
        RequestScheme = 0x10,

        /// <summary>
        /// Flag for logging the HTTP Response <see cref="HttpResponse.StatusCode"/>.
        /// <p>
        /// For example:
        /// StatusCode: 200
        /// </p>
        /// </summary>
        ResponseStatusCode = 0x20,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Headers"/>.
        /// Request Headers are logged as soon as the middleware is invoked.
        /// Headers are redacted by default with the character '[Redacted]' unless specified in
        /// the <see cref="HttpLoggingOptions.RequestHeaders"/>.
        /// <p>
        /// For example:
        /// Connection: keep-alive
        /// My-Custom-Request-Header: [Redacted]
        /// </p>
        /// </summary>
        RequestHeaders = 0x40,

        /// <summary>
        /// Flag for logging the HTTP Response <see cref="HttpResponse.Headers"/>.
        /// Response Headers are logged when the <see cref="HttpResponse.Body"/> is written to
        /// or when <see cref="IHttpResponseBodyFeature.StartAsync(System.Threading.CancellationToken)"/>
        /// is called.
        /// Headers are redacted by default with the character '[Redacted]' unless specified in
        /// the <see cref="HttpLoggingOptions.ResponseHeaders"/>.
        /// <p>
        /// For example:
        /// Content-Length: 16
        /// My-Custom-Response-Header: [Redacted]
        /// </p>
        /// </summary>
        ResponseHeaders = 0x80,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="IHttpRequestTrailersFeature.Trailers"/>.
        /// Request Trailers are currently not logged.
        /// </summary>
        RequestTrailers = 0x100,

        /// <summary>
        /// Flag for logging the HTTP Response <see cref="IHttpResponseTrailersFeature.Trailers"/>.
        /// Response Trailers are currently not logged.
        /// </summary>
        ResponseTrailers = 0x200,

        /// <summary>
        /// Flag for logging the HTTP Request <see cref="HttpRequest.Body"/>.
        /// Logging the request body has performance implications, as it requires buffering
        /// the entire request body up to <see cref="HttpLoggingOptions.RequestBodyLogLimit"/>.
        /// </summary>
        RequestBody = 0x400,

        /// <summary>
        /// Flag for logging the HTTP Response <see cref="HttpResponse.Body"/>.
        /// Logging the response body has performance implications, as it requires buffering
        /// the entire response body up to <see cref="HttpLoggingOptions.ResponseBodyLogLimit"/>.
        /// </summary>
        ResponseBody = 0x800,

        /// <summary>
        /// Flag for logging the date and time
        /// that the activity occurred.
        /// </summary>
        DateTime = 0x1000,

        /// <summary>
        /// Flag for logging the IP address
        /// of the client sending the <see cref="HttpRequest"/>.
        /// </summary>
        ClientIpAddress = 0x2000,

        /// <summary>
        /// Flag for logging the IP address
        /// of the server sending the <see cref="HttpResponse"/>.
        /// </summary>
        ServerIpAddress = 0x4000,

        /// <summary>
        /// Flag for logging the port number
        /// the client is connected to.
        /// </summary>
        ServerPort = 0x8000,

        /// <summary>
        /// Flag for logging the content of the cookie
        /// sent by the client, if any.
        /// </summary>
        RequestCookie = 0x10000,

        /// <summary>
        /// Flag for logging the name of the
        /// authenticated user that accessed the server.
        /// </summary>
        UserName = 0x20000,

        /// <summary>
        /// Flag for logging properties that are part of the <see cref="HttpContext"/>
        /// Includes <see cref="ClientIpAddress"/>, <see cref="ServerIpAddress"/> and <see cref="ServerPort"/>.
        /// </summary>
        ConnectionInfoFields = ClientIpAddress | ServerIpAddress | ServerPort,

        /// <summary>
        /// Flag for logging the default collection of properties that are included in the
        /// W3C Server Logs, including <see cref="DateTime"/>, <see cref="ConnectionInfoFields"/>, <see cref="RequestPath"/>,
        /// <see cref="RequestHeaders"/>, <see cref="RequestProtocol"/>, <see cref="RequestMethod"/>, <see cref="RequestQueryString"/>,
        /// <see cref="ResponseStatusCode"/>, and <see cref="ResponseHeaders"/>.
        /// </summary>
        W3CDefaultFields = DateTime | ConnectionInfoFields | RequestPath | RequestHeaders | RequestProtocol | RequestMethod | RequestQueryString | ResponseStatusCode | ResponseHeaders,

        /// <summary>
        /// Flag for logging properties which are considered optional for
        /// W3C Server Logs, including <see cref="RequestCookie"/> and <see cref="UserName"/>.
        /// These fields contain information which could expose
        /// identifiable information about the client user.
        /// </summary>
        W3COptionalFields = RequestCookie | UserName,

        /// <summary>
        /// Flag for logging all properties that can be included in the
        /// W3C Server Logs, including <see cref="W3CDefaultFields"/> and <see cref="W3COptionalFields"/>.
        /// </summary>
        W3CAllFields = W3CDefaultFields | W3COptionalFields,

        /// <summary>
        /// Flag for logging a collection of HTTP Request properties,
        /// including <see cref="RequestPath"/>, <see cref="RequestQueryString"/>, <see cref="RequestProtocol"/>,
        /// <see cref="RequestMethod"/>, <see cref="RequestScheme"/>, and <see cref="RequestCookie"/>.
        /// </summary>
        RequestProperties = RequestPath | RequestQueryString | RequestProtocol | RequestMethod | RequestScheme | RequestCookie,

        /// <summary>
        /// Flag for logging HTTP Request properties and headers.
        /// Includes <see cref="RequestProperties"/> and <see cref="RequestHeaders"/>
        /// </summary>
        RequestPropertiesAndHeaders = RequestProperties | RequestHeaders,

        /// <summary>
        /// Flag for logging HTTP Response properties and headers.
        /// Includes <see cref="ResponseStatusCode"/> and <see cref="ResponseHeaders"/>
        /// </summary>
        ResponsePropertiesAndHeaders = ResponseStatusCode | ResponseHeaders,

        /// <summary>
        /// Flag for logging the entire HTTP Request.
        /// Includes <see cref="RequestPropertiesAndHeaders"/> and <see cref="RequestBody"/>.
        /// Logging the request body has performance implications, as it requires buffering
        /// the entire request body up to <see cref="HttpLoggingOptions.RequestBodyLogLimit"/>.
        /// </summary>
        Request = RequestPropertiesAndHeaders | RequestBody,

        /// <summary>
        /// Flag for logging the entire HTTP Response.
        /// Includes <see cref="ResponsePropertiesAndHeaders"/> and <see cref="ResponseBody"/>.
        /// Logging the response body has performance implications, as it requires buffering
        /// the entire response body up to <see cref="HttpLoggingOptions.ResponseBodyLogLimit"/>.
        /// </summary>
        Response = ResponseStatusCode | ResponseHeaders | ResponseBody,

        /// <summary>
        /// Flag for logging both the HTTP Request and Response,
        /// in addition to all W3C Logging fields.
        /// Includes <see cref="Request"/>, <see cref="Response"/>, and <see cref="W3CAllFields"/>.
        /// Logging the request and response body has performance implications, as it requires buffering
        /// the entire request and response body up to the <see cref="HttpLoggingOptions.RequestBodyLogLimit"/>
        /// and <see cref="HttpLoggingOptions.ResponseBodyLogLimit"/>.
        /// </summary>
        All = Request | Response | W3CAllFields
    }
}
