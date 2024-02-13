﻿using Database.Entities;
using System.Diagnostics;

namespace EventPublisher.Contracts;

/// <summary>
/// Event Contract
/// </summary>
public class FileContract
{
    public string FileGuid { get; set; }

    public static FileContract ConvertToEventContract(InputHtmlFileEntity entity)
    {
        var contract = new FileContract()
        {
            FileGuid = entity.FileGuid,
        };

        return contract;
    }
}