﻿using LightlessSync.API.Data.Enum;
using LightlessSync.PlayerData.Handlers;
using LightlessSync.Services;
using LightlessSync.Services.Mediator;
using Microsoft.Extensions.Logging;

namespace LightlessSync.PlayerData.Factories;

public class GameObjectHandlerFactory
{
    private readonly DalamudUtilService _dalamudUtilService;
    private readonly ILoggerFactory _loggerFactory;
    private readonly LightlessMediator _lightlessMediator;
    private readonly PerformanceCollectorService _performanceCollectorService;

    public GameObjectHandlerFactory(ILoggerFactory loggerFactory, PerformanceCollectorService performanceCollectorService, LightlessMediator lightlessMediator,
        DalamudUtilService dalamudUtilService)
    {
        _loggerFactory = loggerFactory;
        _performanceCollectorService = performanceCollectorService;
        _lightlessMediator = lightlessMediator;
        _dalamudUtilService = dalamudUtilService;
    }

    public async Task<GameObjectHandler> Create(ObjectKind objectKind, Func<nint> getAddressFunc, bool isWatched = false)
    {
        return await _dalamudUtilService.RunOnFrameworkThread(() => new GameObjectHandler(_loggerFactory.CreateLogger<GameObjectHandler>(),
            _performanceCollectorService, _lightlessMediator, _dalamudUtilService, objectKind, getAddressFunc, isWatched)).ConfigureAwait(false);
    }
}