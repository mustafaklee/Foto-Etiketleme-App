﻿namespace UI.Repositories.Results;

public interface IResult
{
    bool Success { get; }
    string Message { get; }
}
